using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  /// <summary>
  /// summary
  /// </summary>
  class CLCStringAppender : ConfigurableLogConstructor
  {
    String statString;

    public CLCStringAppender(String statString)
    {
      this.statString = statString;
    }

    public void Append(String line)
    {
      statString += line;
    }

    public override string ConstructLine(LogItem item)
    {
      sb.Append(statString);
      return clc.ConstructLine(item);
    }

    public override string ToString()
    {
      return base.ToString() + ".Value=\"" + statString+"\"";
    }
  }
}
