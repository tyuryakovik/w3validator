using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common
{
  public interface StringIndexed
  {
    string this[string key]
    {
      get;
      set;
    }
  }
}
