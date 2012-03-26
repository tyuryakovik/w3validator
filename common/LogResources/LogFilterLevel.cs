using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogFilterLevel : LogFilterBase
  {
    int level = Log.INFO;

    protected override bool DoFilter(LogItem logItem)
    {
      return logItem.Level <= level;
    }

    protected override void DoSetFilter(object[] items)
    {
      if (items == null || items.Length == 0)
        level = Log.INFO;
      else
        if (items[0] is int)
          level = (int)items[0];
    }
  }
}
