using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KBProject.Common;

namespace KBProject.Common.LogResources
{
  class CLCLevelAppender : ConfigurableLogConstructor
  {
    DictionaryFacade<int, string> levels = new DictionaryFacade<int, string>(string.Empty);

    public CLCLevelAppender()
    {
      levels.Add(Log.MESSAGE,Options.Get("LogMessageString"));
      levels.Add(Log.DEBUG,Options.Get("LogDebugString"));
      levels.Add(Log.INFO,Options.Get("LogInfoString"));
      levels.Add(Log.WARNING, Options.Get("LogWarningString"));
      levels.Add(Log.ERROR, Options.Get("LogErrorString"));
    }

    public override string ConstructLine(LogItem item)
    {
      sb.Append(levels[item.Level]);
      return clc.ConstructLine(item);
    }
  }
}
