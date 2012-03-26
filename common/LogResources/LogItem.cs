using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common.LogResources
{
  class LogItem
  {     
    String module;
    String line;
    int[] id ;
    int level;
    Exception ex;
    DateTime time;

    public LogItem(String module, int level, String line)
    {
      time = DateTime.Now;
      this.module = module;
      this.level = level;
      this.line = line;
      id = new int[0];
      ex = null;
    }

    public String Module
    {
      get
      {
        return module;
      }
    }

    public String Line
    {
      get
      {
        return line;
      }
    }

    public void setId(int[] id)
    {
      this.id = id;
    }

    public int[] Id
    {
      get
      {
        return id;
      }
      set
      {
        if (value != null)
          id = value;
      }
    }

    public Exception Ex
    {
      get
      {
        return ex;
      }
      set
      {
        ex = value;
      }
    }

    public int Level
    {
      get
      {
        return level;
      }
    }

    public DateTime Time
    {
      get
      {
        return time;
      }
    }

  }
}
