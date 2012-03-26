using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogConstructorFactory 
  {
    string logFormat;
    string dateFormat;
   string msFormat;
    string idSplit;
    ConfigurableLogConstructor current;
    List<ConfigurableLogConstructor> appenders = new List<ConfigurableLogConstructor>();

    public LogConstructorFactory()
    {
      logFormat = Options.Get("LogFormat").ToLower();
      dateFormat = Options.Get("LogDateTimeFormat");
      msFormat = Options.Get("LogMilisecondsFormat");
      idSplit = Options.Get("LogIDSplitChar");
      if (idSplit == string.Empty)
        idSplit = " ";
    }

    List<int> FindSlashes()
    {
      List<int> slashes = new List<int>();
      int index = -1;
      while ((index = logFormat.IndexOf('\\',index+1)) != -1)
      {
        if(index < logFormat.Length - 1)
          slashes.Add(index);
        if (logFormat[index + 1] == '\\')
          index++;
      }
      return slashes;
    }

    void AppendText(string line)
    {
      if(line==string.Empty)
        return;
      CLCStringAppender stringAppender;
      if (appenders.Count == 0 || (stringAppender = appenders.Last() as CLCStringAppender) == null)
        appenders.Add(new CLCStringAppender(line));        
      else
        stringAppender.Append(line);
    }

    void AppendSlashed(char val)
    {
      switch (val)
      {
        case 'd':
          appenders.Add(new CLCDateTimeAppender(dateFormat, msFormat));
          break;
        case 'l':
          appenders.Add(new CLCLevelAppender());
          break;
        case 'i':
          appenders.Add(new CLCIdAppender(idSplit[0]));
          break;
        case 'm':
          appenders.Add(new CLCModuleAppender());
          break;
        case 's':
          appenders.Add(new CLCMessageAppender());
          break;
        case 'e':
          appenders.Add(new CLCExceptionAppender());
          break;
        case 't':
          appenders.Add(new CLCStackTraceAppender());
          break;
        case '\\':
          AppendText("\\");
          break;
        case 'n':
          AppendText("\n");
          break;
        default:
          break;
      }
    }

    List<ConfigurableLogConstructor> CreateAppenders(List<int> slashes)
    {
      appenders.Clear();
      int iprev = 0;      
      foreach (int i in slashes)
      {
        AppendText(logFormat.Substring(iprev, i - iprev));
        AppendSlashed(logFormat[i + 1]);
        iprev = i + 2;
      }
      AppendText(logFormat.Substring(slashes.Last() + 2));
      return appenders;
    }


    public LogConstructor CreateLogConstructor()
    {
      if (logFormat == "basic" || logFormat.Length < 5)
        return new BasicLogConstructor();
      current = new CLCStringReturner();
      /* d - dateTime, l - level, i- ID, e - exception m - module , s - message
       */
          
      List<ConfigurableLogConstructor> appenders= CreateAppenders(FindSlashes());
      for (int n = appenders.Count - 1; n >= 0; n--)
      {
        appenders[n].AddToChain(current);
        current = appenders[n];
      }
      return current;
    }
  }
}
