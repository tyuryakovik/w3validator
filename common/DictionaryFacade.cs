using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBProject.Common
{

  public class DictionaryFacade<TKey, TValue>
  {
    Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();
    TValue defaultValue;

    public DictionaryFacade(TValue defaultValue)
    {
      this.defaultValue = defaultValue;
    }

    public DictionaryFacade()
    {
      defaultValue = default(TValue);
    }

    public TValue this[TKey key]
    {
      get
      {
        if (key == null || !items.ContainsKey(key))
          return defaultValue;
        else
          return items[key];
      }
      set
      {
        if (key != null)
        {
          if (items.ContainsKey(key))
            items[key] = value;
          else
            items.Add(key, value);
        }    

      }
    }

    public int Count
    {
      get
      {
        return items.Count;
      }
    }
    
    public bool Remove(TKey key)
    {
      if(key!=null)
        return items.Remove(key);
      return false;
    }

    public void Add(TKey key, TValue value)
    {
      this[key] = value;
    }
   
    public void Clear()
    {
      items.Clear();
    }

    public TValue[] ToArray()
    {
      return items.Values.ToArray();
    }

    public IEnumerator GetEnumerator()
    {
      return new dictEnumerator(items);
    }

    private class dictEnumerator : IEnumerator
    {
      private int pos = -1;
      private TValue[] items;

      public dictEnumerator(Dictionary<TKey,TValue> dict)
      {
        this.items = dict.Values.ToArray();
      }

      public bool MoveNext()
      {
        if (pos < items.Length - 1)
        {
          pos++;
          return true;
        }
        return false;
      }

      public void Reset()
      {
        pos = -1;
      }

      public object Current
      {
        get
        {
          return items[pos];
        }
      }

    }

  }
}
