using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KBProject.Common
{
  public abstract class SettingsAccess<T> : DoubleCheckedLock<T> where T:StringIndexed
  {
    SettingsAccess<T> settingsInstance;

    protected SettingsAccess()
    {
    }

    public static string Get(string optionName)
    {
      return Instance[optionName];
    }

    public static string Get(string optionName, string defVal)
    {
      string val = Instance[optionName];
      if (val == string.Empty)
        return defVal;
      return val;
    }

    public static void Set(string optionName, string value)
    {
      Instance[optionName] = value;
    }

    public static bool GetBool(string optionName)
    {
      string option = Instance[optionName].ToLower();
      if (option == "true" || option == "1")
        return true;
      else
        return false;
    }

    public static void SetBool(string optionName, bool value)
    {
      Instance[optionName] = (value ? "true" : "false");
    }

    public static int GetInt(string optionName, int defVal)
    {
      int parsed = 0;
      if (int.TryParse(Instance[optionName], out parsed))
        return parsed;
      return defVal;
    }

    public static int GetUInt(string optionName, int defVal)
    {
      uint parsed = 0;
      if (uint.TryParse(Instance[optionName], out parsed))
        return (int)parsed;
      return defVal;
    }

  }
}
