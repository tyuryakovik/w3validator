using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KBProject.Common;
using System.Windows.Controls;
using System.Web;
using System.Windows.Media;
using System.Windows.Documents;

namespace W3Validity
{
  class Error
  {
    public int line;
    public int column;
    public string message= string.Empty;
    public string source1=string.Empty, source2=string.Empty;
    public string strong=string.Empty;
  }

  class ValidityParser
  {
    string origUrl;
    bool valid = false;
    XmlFacade validity;
    readonly string[] strongSplit = new string[2] { "<strong", "</strong>" };

    int errorcount;
    List<Error> errors = new List<Error>();
    int warningcount;
    List<Error> warnings = new List<Error>();

    public Expander Exp
    {
      get
      {
        Expander exp = new Expander();
        string header = origUrl+ "   ";
        if (errorcount == 0)
          header += "Passed";
        else
          header += "Errors: " + errorcount;
        header += " Warnings: " + warningcount;
        exp.Header = header;
        if (errorcount > 0)
          exp.Foreground = Brushes.Red;
        else
        {
          if (warningcount > 1)
            exp.Foreground = Brushes.Yellow;
          else
            exp.Foreground = Brushes.Green;
        }          


        exp.ExpandDirection = ExpandDirection.Down;
        StackPanel stp;
        exp.Content = stp= new StackPanel();
        foreach (Error err in errors)
          AddErr("Error: ", err, stp);
        foreach (Error warn in warnings)
          AddErr("Warning: ", warn, stp);        
        return exp;
      }
    }
    void AddTb(StackPanel stp, string text)
    {
      TextBlock tb = new TextBlock();
      tb.Foreground = Brushes.Black;
      tb.Text = text;
      stp.Children.Add(tb);
    }

    

    private void AddErr(String start, Error err, StackPanel stp)
    {
      AddTb(stp, start+"Line "+err.line+" Column "+err.column+"   "+err.message);
      FlowDocument doc = new FlowDocument();
      RichTextBox rich = new RichTextBox(doc);
      stp.Children.Add(rich);
      rich.IsReadOnly = true;
      Paragraph para = new Paragraph();
      doc.Blocks.Add(para);
      TextBlock strong = new TextBlock();
      para.Inlines.Add(err.source1);
      para.Inlines.Add(strong);
      para.Inlines.Add(err.source2);
      strong.Text = err.strong;
      strong.Foreground = new SolidColorBrush(Colors.Red);
      
      AddTb(stp, " ");
    }

    public bool Valid
    {
      get { return valid; }
    }

    public ValidityParser(string url)
    {
      origUrl = url;
      if (url.StartsWith(ValidityCollector.httpString))
        url = url.Substring(ValidityCollector.httpString.Length);
      url = Options.Get("ValidatorSite") + url + Options.Get("ValidatorParms");
      string content = (new PageDownloader()).DownloadPage(url);
      if (content != string.Empty)
      {

        validity = new XmlFacade();
        validity.Set(content);
        Parse();
      }
    }

    string GetValue(string Key)
    {
      string val = string.Empty;
      if (validity.Cd(Key))
      {
        val = validity.InnerText;
        validity.CdUp();
      }
      return val;
    }

    string Decode(string input)
    {
      byte[] bytes = Encoding.GetEncoding(1251).GetBytes(input);
      string ret = UTF8Encoding.UTF8.GetString(bytes);
      return ret;
    }

    Error GetError()
    {
      Error err = new Error();
      err.message = GetValue("m:message");
      int.TryParse(GetValue("m:line"), out err.line);
      int.TryParse(GetValue("m:col"), out err.column);
      if (validity.Cd("m:source"))
      {
        string[] errtxt = validity.InnerText.Split(strongSplit, StringSplitOptions.None);
        err.source1 = Decode(HttpUtility.HtmlDecode(errtxt.First()));
        if (errtxt.Length > 1)
        {
          err.source2 = Decode(HttpUtility.HtmlDecode(errtxt.Last()));
          err.strong = Decode(HttpUtility.HtmlDecode(errtxt[1].Substring(errtxt[1].IndexOf('>') + 1)));
        }
        validity.CdUp();
      }
      return err;
    }

    int GetErrorList(string type, List<Error> outlist)
    {
      string err;
      err = "m:" + type;
      int errCount;
      if (!validity.Cd(err + "s"))
        return 0;
      int.TryParse(GetValue(err + "count"), out errCount);

      if (validity.Cd(err + "list"))
      {
        foreach (string name in validity)
          if (name == err)
          {
            Error e = GetError();
            if (err != null)
              outlist.Add(e);
          }
        validity.CdUp();
      }
      validity.CdUp();
      return errCount;
    }

    void Parse()
    {
      while (validity.curName.StartsWith("env"))
        validity.CdFirst();
      if (validity.curName == "m:markupvalidationresponse")
      {
        errorcount = GetErrorList("error", errors);
        warningcount = GetErrorList("warning", warnings);
        valid = true;
      }
    }

  }
}
