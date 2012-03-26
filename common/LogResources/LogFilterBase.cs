using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  abstract class LogFilterBase : LogFilter
  {
    LogFilter chained;
    int filterType;

    public LogFilter Chained
    {
      get { return chained; }
      set { chained = value; }
    }  

    public int FilterType
    {
      get { return filterType; }
      set { filterType = value; }
    }

    protected abstract void DoSetFilter(object[] items);

    protected abstract bool DoFilter(LogItem logItem);

    public bool Filter(LogItem logItem)
    {
      return (DoFilter(logItem)&&chained.Filter(logItem));
    }

    public void SetFilter(int type, object[] items)
    {
      if (type == filterType)
      {
        DoSetFilter(items);
      }
      else
        chained.SetFilter(type, items);
    }
  }
}
