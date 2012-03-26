using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class CLCStringReturner : ConfigurableLogConstructor
  {
    public CLCStringReturner()
    {
      sb = new StringBuilder();
    }

    public override string ConstructLine(LogItem item)
    {
      String output = sb.ToString();
      sb.Clear();
      if (output != string.Empty && output.Last() != '\n')
        output += '\n';
      return output;
    }
  }
}
