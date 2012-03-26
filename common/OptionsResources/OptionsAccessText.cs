using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KBProject.Common.OptionsResources
{
  class OptionsAccessText : OptionsAccess
  {
    const int BufSize = 1000000;
    string[] LineSeparators = new string[] { "\r\n", "\n" };
    const char MainSeparator = '=';
    const char OptionSeparator = ',';

    string filename;
    public bool CanRead(string name)
    {
        filename = name + ".cfg";
        return File.Exists(filename);      
    }
    

    public string[][] ReadOptions()
    {
      string optFile = string.Empty;
      try
      { 
        FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        char[] buf = new char[BufSize];
        int len = (new StreamReader(file)).Read(buf, 0, BufSize);
        optFile = new string(buf, 0, len);
        file.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);        
      }
      return ParseOptions(ref optFile);
    }    

    string[][] ParseOptions(ref string optFile)
    {
      string[] optlines = optFile.Split(LineSeparators, StringSplitOptions.RemoveEmptyEntries);
      
      List<string[]> list = new List<string[]>();
      foreach(string line in optlines)
      {
        string[] parsedLine = null;
        if(ParseLine(line, ref parsedLine))
          list.Add(parsedLine);
      }
      string[][] output = new string[list.Count][];
      int n = 0;
      foreach (string[] option in list)
        output[n++] = option;
      return output;
    }

    bool ParseLine(string line, ref string[] parsed)
    {

      int sep = line.IndexOf(MainSeparator);
      if (sep == -1)
        return false;
      string[] values =  line.Substring(sep+1).Split(OptionSeparator);
      if (values.Length == 0)
        return false;
      parsed = new string[values.Length + 1];
      parsed[0] = line.Substring(0,sep);
      int n = 1;
      foreach (string val in values)
        parsed[n++] = val;
      return true;
    }
    

    public void WriteOptions(string[][] options)
    {

    }

  }
}
