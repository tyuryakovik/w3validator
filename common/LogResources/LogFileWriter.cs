using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KBProject.Common.LogResources
{
  class LogFileWriter
  {
    FileStream fileStream;
    long fileSize;
    int sizeLimit;
    String[] fileName;

    public LogFileWriter()
    {
      string dirname = Options.Get("ProgramDir") + "logs/";
      Directory.CreateDirectory(dirname);
      sizeLimit = (1 << 20) * Options.GetUInt("LogRotationSizeMb", 5);
      fileName = new string[Options.GetUInt("LogRotationFiles", 5)];
      fileName[0] = dirname + "main.log";
      for (int n = 1; n < fileName.Length; n++)
        fileName[n] = dirname + "old" + n + ".log";      
    }

    public void Write(String line)
    {
      try
      {

        byte[] lineBytes = UTF8Encoding.UTF8.GetBytes(line);
        if (fileSize + lineBytes.Length > sizeLimit)
        {
          FileClose();
          Rotate();
        }
        FileOpen();
        fileStream.Write(lineBytes, 0, lineBytes.Length);
        fileSize += lineBytes.Length;
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }      
    }

    public void onTimeout()
    {
      if (fileStream != null)
        fileStream.Flush();
    }

    void FileOpen()
    {
      if (fileStream == null)
      {
        if (File.Exists(fileName[0]))
        {
          fileStream = new FileStream(fileName[0], FileMode.Append, FileAccess.Write, FileShare.Read);
          fileSize = fileStream.Length;
        }
        else
        {
          fileStream = new FileStream(fileName[0], FileMode.Create, FileAccess.Write, FileShare.Read);
          byte[] preamble = UTF8Encoding.UTF8.GetPreamble();
          fileStream.Write(preamble, 0, preamble.Length);//,new byte[] { 0xef, 0xbb, 0xbf }, 0, 3);
        }
        fileSize = 3;
      }
    }

    void FileClose()
    {
      fileStream.Flush();
      fileStream.Close();
      fileStream.Dispose();
      fileStream = null;
    }

    void Rotate()
    {
      if (File.Exists(fileName.Last()))
        File.Delete(fileName.Last());
      for (int n = fileName.Length - 2; n >= 0; n--)
        if (File.Exists(fileName[n]))
          File.Move(fileName[n], fileName[n + 1]);
    }

  }
}
