using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.OptionsResources
{
  class OptionsAccessRegistry : OptionsAccess
  {
    public bool CanRead(string name)
    {
      return false;
    }

    public string[][] ReadOptions()
    {
      return null;
    }

    public void WriteOptions(string[][] options)
    {
    }
  }
}
