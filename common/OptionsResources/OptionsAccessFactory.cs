using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.OptionsResources
{
  class OptionsAccessFactory
  {
    public OptionsAccess CreateOptionsAccess(string programDir)
    {
      OptionsAccess options;
      string filename = programDir + "options";
      options = new OptionsAccessXml();
      if (options.CanRead(filename))
        return options;
      options = new OptionsAccessText();
      if (options.CanRead(filename))
        return options;
      return new OptionsAccessRegistry();
    }
  }
}
