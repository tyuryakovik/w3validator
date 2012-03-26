using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class BasicLogConstructor : LogConstructor
  {
    StringBuilder sb = new StringBuilder();
    String[] levelString = {"Ошибка:    ",
                            "Внимание:  ",
                            "Инфо:      ",
                            "Отладка:   ",
                            "Сообщение: "};

    private void AppendIdString(int[] id)
    {
      foreach (int i in id)
        sb.Append(String.Format(":{0}", (int)i));
      if (id.Length > 0)
        sb.Append(':');
    }

    public string ConstructLine(LogItem item)
    {
      sb.Clear();
      sb.Append("\r\n");
      sb.Append(item.Time.ToString("dd-MM-yyyy HH.mm.ss"));
      sb.Append(String.Format(":{0:000} ", item.Time.Millisecond));
      AppendIdString(item.Id);
      sb.Append(' ');
      sb.Append(item.Module);
      sb.Append("\r\n");
      sb.Append(levelString[item.Level]);
      sb.Append(item.Line);
      if (item.Ex != null)
      {
        sb.Append("\r\n");
        sb.Append(item.Ex.Message);
        sb.Append(item.Ex.StackTrace);
      }
      return sb.ToString();
    }
  }
}
