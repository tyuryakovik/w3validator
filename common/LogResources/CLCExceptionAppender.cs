using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCExceptionAppender : ConfigurableLogConstructor
  {

    private void AppendException(Exception ex)
    {
      if (ex != null)
      {
        sb.Append('\n');
        sb.Append(ex.Message);
        AppendException(ex.InnerException);
      }
    }

    public override string ConstructLine(LogItem item)
    {
      AppendException(item.Ex);
      return clc.ConstructLine(item);
    }
  }
}
