using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCStackTraceAppender : ConfigurableLogConstructor
  {
    public override string ConstructLine(LogItem item)
    {
      if(item.Ex!=null)
        sb.Append(item.Ex.StackTrace);
      return clc.ConstructLine(item);
    }
  }
}
