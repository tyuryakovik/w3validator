using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCModuleAppender : ConfigurableLogConstructor
  {
    public override string ConstructLine(LogItem item)
    {
      sb.Append(item.Module);
      return clc.ConstructLine(item);
    }
  }
}
