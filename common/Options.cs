using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using KBProject.Common;
using KBProject.Common.OptionsResources;

namespace KBProject.Common
{
  public class Options : SettingsAccess<Options> , StringIndexed
  {
    private DictionaryFacade<string, string[]> options;
    private OptionsAccess optionsAccess;

    private Options()
    {
      options = new DictionaryFacade<string, string[]>(new string[] { string.Empty });
      string progDir = Directory.GetCurrentDirectory().Replace('\\', '/');
      if(progDir.Last()!='/')
        progDir += '/';
      options.Add("ProgramDir", new string[] { progDir });
      optionsAccess = new OptionsAccessFactory().CreateOptionsAccess(progDir);
      ReadOptions();     
    }

    void ReadOptions()
    {
      string[][] optStrings = optionsAccess.ReadOptions();
      foreach (string[] option in optStrings)
        if (option.Length >= 2)
        {
          string[] opVal = new string[option.Length - 1];
          for (int n = 0; n < opVal.Length; n++)
            opVal[n] = option[n + 1];
          options.Add(option[0], opVal);
          //this works 10 times slower ->  options.Add(option[0], option.Where((a, i) => i > 0).ToArray());
        }        
    }

    public string this[string key]
    {
      get
      {
        return options[key][0];
      }

      set
      {
        options[key] = new string[]{value};
      }
    }
  }
}
