using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KBProject.Common
{
  public class GlobalVars : SettingsAccess<GlobalVars>, StringIndexed
  {
    private DictionaryFacade<string, string> options = new DictionaryFacade<string,string>(string.Empty);

    private GlobalVars()
    {
    }

    public string this[string key]
    {
      get
      {
        return options[key];
      }

      set
      {
        options[key] = value;
      }
    }
  }
}
