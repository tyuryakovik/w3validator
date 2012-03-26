using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.OptionsResources
{
  class OptionsAccessXml : OptionsAccess
  {
    const string Option = "option";
    const string Name = "name";
    const string Value = "value";
    const char Separator = ',';

    XmlFacade xml;
    public bool CanRead(string name)
    {
      xml = new XmlFacade();
      return xml.Load(name + ".xml");
    }

    string[] GetOption()
    {
      string[] option = xml[Value].Split(Separator);
      string[] output = new string[option.Length + 1];
      output[0] = xml[Name];
      int n = 1;
      foreach (string opt in option)
        output[n++] = opt;
      return output;
    }

    void SaveValue(string[] option)
    {
      if (option[0] != string.Empty)
      {
        xml.Add(Option);
        xml[Name] = option[0];
        string name = option[1];
        for (int n = 2; n < option.Length; n++)
          name += Separator + option[n];
        xml[Value] = name;
        xml.CdUp();
      } 
    }

    public string[][] ReadOptions()
    {
      List<string[]> list = new List<string[]>();
      foreach (string name in xml)
        if (name == Option)        
          list.Add(GetOption());        
      string[][] output = new string[list.Count][];
      int n = 0;
      foreach (string[] option in list)
        output[n++] = option;
      return output;
    }

    public void WriteOptions(string[][] options)
    {
      xml.RemoveChilds();
      foreach (string[] option in options)
        if (option.Length > 1)
          SaveValue(option);
      xml.Save();
    }
  }
}
