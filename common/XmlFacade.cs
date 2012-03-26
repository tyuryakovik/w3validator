using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;


namespace KBProject.Common
{
  public class XmlFacade
  {
    XmlDocument doc;
    XmlNode cur;
    string filename = string.Empty;

    public XmlFacade()
    {
      doc = new XmlDocument();
      CreateTopElement("default");
    }

    public XmlFacade(string filename)
    {
      doc = new XmlDocument();
      Load(filename);
    }

    #region manage
    public bool Load(string filename)
    {
      this.filename = filename;
      try
      {
        doc.Load(filename);
        cur = doc.DocumentElement;        
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }

    public void Save()
    {
      if (doc.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
      {
        XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.InsertBefore(dec, doc.DocumentElement);
      }
      if (filename != string.Empty)
        try
        {
          doc.Save(filename);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
    }

    public void Save(string filename)
    {
      this.filename = filename;
      Save();
    }

    public bool Set(string inputXml)
    {
      try
      {
        doc.LoadXml(inputXml);
        cur = doc.DocumentElement;
      }
      catch (Exception)
      {
        return true;
      }
      return false;    
    }
    #endregion    

    #region traverse
    public void CdTop()
    {
      cur = doc.DocumentElement;
    }

    public void CdUp()
    {
      if (cur.ParentNode != doc)
        cur = cur.ParentNode;
    }

    private bool Shift(XmlNode shift)
    {  
      if (shift != null)
      {
        cur = shift;
        return true;
      }
      return false;
    }

    public bool CdFirst()
    {
      return Shift(cur.FirstChild);
    }

    public bool CdLast()
    {
      return Shift(cur.LastChild);
    }

    public bool CdNext()
    {
      return Shift(cur.NextSibling);
    }

    public bool CdPrev()
    {
      return Shift(cur.PreviousSibling);
    }

    public IEnumerator GetEnumerator()
    {
      return new xmsgEnumerator(this);
    }

    private class xmsgEnumerator : IEnumerator
    {
      private bool first = true;
      XmlFacade xmsg;

      public xmsgEnumerator(XmlFacade xmsg)
      {
        this.xmsg = xmsg;
      }

      public bool MoveNext()
      {
        bool ok;
        if (first)
          ok = xmsg.CdFirst();
        else
          ok = xmsg.CdNext();
        
        if (!first&&!ok)
          xmsg.CdUp();
        first = false;
        return ok;
      }

      public void Reset()
      {
        first = true;
      }

      public object Current
      {
        get
        {
          return xmsg.curName;
        }
      }

    }

    public bool Cd(string name)
    {
      XmlNode node = cur;
      if (!CdFirst() || !ByName(name))
      {
        cur = node;
        return false;
      }
      return true;
    }

    public bool NextByName()
    {
      string name = curName;
      XmlNode node = cur;
      if(!CdNext()||!ByName(name))
      {
        cur = node;
        return false;
      }
      return true;
    }

    private bool ByName(string name)
    {
      XmlNode traverse = cur;
      while (traverse != null)
      {
        if (traverse.Name == name)
        {
          cur = traverse;
          return true;
        }
        traverse = traverse.NextSibling;
      }
      return false;
    }
    #endregion

    #region edit
    void CreateTopElement(string name)
    {
      cur = doc.CreateElement(name);
      doc.AppendChild(cur);
    }

    public void Add(string name, bool cd = true)
    {
      XmlNode node;
      try
      {
        node = doc.CreateElement(name);
        cur.AppendChild(node);
        if (cd)
          cur = node;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    public void Replace(string name)
    {
      XmlNode replace = cur;
      cur = cur.ParentNode;
      cur.RemoveChild(replace);
      replace = doc.CreateElement(name);
      cur.AppendChild(replace);
      cur = replace;
    }

    public void Remove()
    {
      XmlNode removal = cur;
      cur = cur.ParentNode;
      cur.RemoveChild(removal);
      if (cur == doc)      
        CreateTopElement("default");
      
    }

    public void RemoveChilds()
    {
      while (CdFirst())
        Remove();
    }

    public string this[string name]
    {
      get
      {
        XmlAttribute xma = cur.Attributes[name];
        if (xma == null)
          return string.Empty;
        else
          return xma.Value;
      }
      set
      {
        XmlAttribute xma = cur.Attributes[name];
        if (xma == null)
        {
          xma = doc.CreateAttribute(name);
          cur.Attributes.Append(xma);
        }
        xma.Value = value;
      }
    }

    public string curName
    {
      get
      {
        if (cur != null)
          return cur.Name;
        else return string.Empty;
      }
    }

    public string InnerText
    {
      get
      {
          return cur.InnerText;
      }
      set
      {
          cur.InnerText = value;
      }
    }
    #endregion

    
  }
}
