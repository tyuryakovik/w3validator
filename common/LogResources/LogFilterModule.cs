using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogFilterModule : LogFilterBase
  {
    HashSet<string> modules = new HashSet<string>();

    protected override void DoSetFilter(object[] items)
    {
      modules.Clear();
      if (items == null)
        return;
      foreach (string item in items)
        if(!modules.Contains(item))
          modules.Add(item);
    }

    protected override bool DoFilter(LogItem logItem)
    {
      if (modules.Count == 0)
        return true;
      return modules.Contains(logItem.Module);
    }
  }
}
