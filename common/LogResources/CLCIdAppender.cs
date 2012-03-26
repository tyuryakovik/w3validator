using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCIdAppender : ConfigurableLogConstructor
  {
    char idSplitChar;

    public CLCIdAppender( char idSplitChar)
    {
      this.idSplitChar = idSplitChar;
    }

    public override string ConstructLine(LogItem item)
    {
      bool first = true;
      foreach (int i in item.Id)
      {
        if(!first) 
          sb.Append(idSplitChar);
        first = false;
        sb.Append(i.ToString());
      }
      return clc.ConstructLine(item);
    }
  }
}
