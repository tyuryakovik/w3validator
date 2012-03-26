using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  interface LogFilter
  {
    bool Filter(LogItem logItem);

    void SetFilter(int type, object[] items);

  }
}
