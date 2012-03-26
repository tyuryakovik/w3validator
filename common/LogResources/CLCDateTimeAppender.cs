using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCDateTimeAppender :ConfigurableLogConstructor
  {
    string dateTimeFormat;
    string milisecondsFormat;

    public CLCDateTimeAppender(string dateTimeFormat,string milisecondsFormat)
    {
      this.dateTimeFormat = dateTimeFormat;
      this.milisecondsFormat = milisecondsFormat;
    }

    public override string ConstructLine(LogItem item)
    {
      sb.Append(item.Time.ToString(dateTimeFormat));
      sb.Append(String.Format(milisecondsFormat,item.Time.Millisecond));
      return clc.ConstructLine(item);
    }
  }
}
