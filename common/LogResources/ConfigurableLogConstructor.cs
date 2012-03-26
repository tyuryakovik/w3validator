using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  abstract class ConfigurableLogConstructor : LogConstructor
  {
    protected StringBuilder sb;
    protected ConfigurableLogConstructor clc;
    

    public void AddToChain(ConfigurableLogConstructor next)
    {
      clc = next;
      sb = clc.GetBuilder();
    }

    protected StringBuilder GetBuilder()
    {
      return sb;
    }

    public abstract string ConstructLine(LogItem item);

  }
}
