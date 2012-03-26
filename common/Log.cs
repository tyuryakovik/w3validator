using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KBProject.Common.LogResources;

namespace KBProject.Common
{

  public class Log : DoubleCheckedLock<Log>
  {

    #region level constants
    public const int ERROR = 0;
    public const int WARNING = 1;
    public const int INFO = 2;
    public const int DEBUG = 3;
    public const int MESSAGE = 4;
    #endregion

    #region filtering id constants
    public const int LEVEL = 1;
    public const int MODULE = 2;
    public const int ID = 3;
    #endregion

    #region instance fields
    private LogFilter filter;
    private LogConstructor constructor;
    private QueuedThread<LogItem> thread;
    private LogFileWriter writer;
    #endregion

    #region static methods

    public static void Info(String module, int id, String line)
    {
      LogItem msg= new LogItem(module, INFO, line);
      msg.setId(new int[] { id });
      Instance.Send(msg);
    }

    public static void Info(String module, int[] id, String line)
    {
      LogItem msg = new LogItem(module, INFO, line);
      msg.setId(id);
      Instance.Send(msg);
    }

    public static void Info(String module, String line)
    {
      Instance.Send(new LogItem(module, INFO, line));
    }

    public static void Error(String module, int id, String line, Exception ex)
    {
      LogItem msg = new LogItem(module, ERROR, line);
      msg.setId(new int[] { id });
      msg.Ex = ex;
      Instance.Send(msg);
    }

    public static void Error(String module, String line, Exception ex)
    {
      LogItem msg = new LogItem(module, ERROR, line);
      msg.Ex = ex;
      Instance.Send(msg);
    }
    
    public static void Error(String module, String line)
    {
      Instance.Send(new LogItem(module, ERROR, line));
    }

    public static void Terminate()
    {
      Instance.TerminateLogger();
    }
    #endregion

    #region instance methods
    private void Init()
    {   
      filter = (new LogFilterFactory()).createFilter();
      constructor = (new LogConstructorFactory()).CreateLogConstructor();
      writer = new LogFileWriter();
      
    }

    private Log()
    {      
      thread = new QueuedThread<LogItem>(onReceive);
      Init();
    }

    private Log(ManualResetEvent centralizedTerminate)
    {      
      thread = new QueuedThread<LogItem>(onReceive,centralizedTerminate);
      Init();
    }

    private void Send(LogItem item)
    {
      thread.Send(item);
    }

    private void onReceive(LogItem[] msgs)
    {
      foreach (LogItem msg in msgs)
      {
        if (filter.Filter(msg))
          writer.Write(constructor.ConstructLine(msg));
      }
      thread.SetTimeout(onTimeout, 1000);
    }

    private void onTimeout()
    {
      writer.onTimeout();
      thread.ResetTimeout();
    }

    private void addFilter()
    {

    }

    private void TerminateLogger()
    {
      thread.Terminate();
    }
    #endregion
  }



}
