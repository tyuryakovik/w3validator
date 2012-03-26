using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCMessageAppender : ConfigurableLogConstructor
  {
    public override string ConstructLine(LogItem item)
    {
      sb.Append(item.Line);
      return clc.ConstructLine(item);
    }
  }
}
