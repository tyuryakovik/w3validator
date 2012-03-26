using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.Logger
{
  class LogFilterManager : LogFilter
  {
    DictionaryFacade<string, LogFilter> filters = new DictionaryFacade<string, LogFilter>();
    LogFilter[] filtersArray;
    string 

    public LogFilterManager()
    {
      filtersArray = filters.ToArray();
    }

    public void setFilter(string type, object[] items)
    {
      LogFilter filter = filters[type];
      if (filter == null)
      {
        switch(type)
      }
    }

    public bool Filter(LogItem logItem)
    {
      bool result = true;
      for (int n = 0; n < filtersArray.Length && result; n++)      
        result = filtersArray[n].Filter(logItem);
      return result;
    }
  }
}
