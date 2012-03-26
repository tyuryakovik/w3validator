using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace KBProject.Common
{
  public abstract class DoubleCheckedLock<T>
  {
    static T instance;
    static Mutex staticMutex = new Mutex();

    protected DoubleCheckedLock()
    {
    }

    protected static T Instance
    {
      get
      {
        if (instance == null)
        {
          if (staticMutex.WaitOne())
          {
            Type type = typeof(T);
            if (instance == null)
              instance = (T)typeof(T).GetConstructor(
                         BindingFlags.Instance | BindingFlags.NonPublic,
                         null,
                         new Type[0],
                         new ParameterModifier[0]).Invoke(null);
            staticMutex.ReleaseMutex();
          }
        }
        return instance;
      }
    }

  }
}
