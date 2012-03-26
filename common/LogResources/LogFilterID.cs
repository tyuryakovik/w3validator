using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogFilterID : LogFilterBase
  {
    HashSet<int> idSet = new HashSet<int>();

    protected override void DoSetFilter(object[] items)
    {
      idSet.Clear();
      if (items == null)
        return;
      foreach (int item in items)
        idSet.Add(item);
    }

    protected override bool DoFilter(LogItem logItem)
    {
      if(idSet.Count == 0)
        return true;
      foreach (int id in logItem.Id)
        if (idSet.Contains(id))
          return true;
      return false;
    }
  }
}
