using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.OptionsResources
{
  interface OptionsAccess
  {
    bool CanRead (string name);

    string[][] ReadOptions();

    void WriteOptions(string[][] options);
  }
}
