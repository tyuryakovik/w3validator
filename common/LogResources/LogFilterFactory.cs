using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogFilterFactory
  {
    List<LogFilterBase> filters = new List<LogFilterBase>();

    void Add(LogFilterBase newFilter, int type)
    {
      newFilter.FilterType = type;
      filters.Add(newFilter);
    }

    LogFilter Output()
    {
      if(filters.Count>1)
      for (int n = 0; n < filters.Count - 1; n++)
        filters[n].Chained = filters[n+1];
      filters.Last().Chained = new LogFilterTerminal();
      return filters.First();
    }

    public LogFilter createFilter()
    {
      Add(new LogFilterLevel(),Log.LEVEL);
      Add(new LogFilterModule(),Log.MODULE);
      Add(new LogFilterID(),Log.ID);

      return Output();

    }
  }
}
