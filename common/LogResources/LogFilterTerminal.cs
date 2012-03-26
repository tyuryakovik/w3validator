using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogFilterTerminal: LogFilter
  {
    public bool Filter(LogItem logItem)
    {
      return true;
    }

    public void SetFilter(int type, object[] items)
    {
    }
  }
}
