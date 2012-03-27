using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using KBProject.Common;
using System.Threading;

namespace W3Validity
{
  class PageDownloader
  {
    HttpWebRequest req;
    string url;
    Decoder coder = Encoding.GetEncoding(1251).GetDecoder();
    StringBuilder content = new StringBuilder();
    bool proxied;

    void RevertProxy()
    {   
      proxied = !proxied;
      SetProxy();
    }

    void AddCookie()
    {
      //stub
      /*  if (name != "msk_cl" && name != null)
        {
          req.CookieContainer = new CookieContainer();
          req.CookieContainer.Add(new Cookie("citilink_space", name, "/", "www.citilink.ru"));
        }*/
    }

    void SetProxy()
    {
      if (!proxied)
      {
        req.Proxy = null;
        return;
      }
      string proxyName = Options.Get("ProxyName");
      int proxyPort = Options.GetInt("ProxyPort", 8080);
      req.Proxy = new WebProxy(proxyName, proxyPort);
      string login = Options.Get("ProxyLogin");
      string pass = Options.Get("ProxyPassword");
      req.Proxy.Credentials = new NetworkCredential(login, pass);
      req.KeepAlive = true;
      req.Timeout = Options.GetInt("HttpTimeout", 5000);
    }

    void CreateReq()
    {
      try
      {
        req = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        req.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";       
        SetProxy();  
      }
      catch (Exception ex)
      {
        Log.Error(ToString(), "Create Request Error", ex);
      }
    }

    bool GetResponse()
    {      
      byte[] buf = new byte[1000];
      char[] dec = new char[1000];
      try
      {
        WebResponse resp = req.GetResponse();
        Stream stream = resp.GetResponseStream();
        bool ok = true;
        do
        {
          int a = stream.Read(buf, 0, 1000); 
          if (a == 0)
            ok = false;
          else
          {
            coder.GetChars(buf, 0, a, dec, 0);
            content.Append(dec, 0, a);
          }
        } while (ok);
        stream.Close();
        resp.Close();        
      }
     catch (Exception ex)
      {
        Log.Error(ToString(), "Error getting a responce", ex);
        return false;
      }
      return true;
    }

    public String DownloadPage(String url)
    {
      Log.Info(ToString(), "Downloading " + url);
      this.url = url;
      int retry = Options.GetInt("HttpRetryCount",5);
      string proxyTest = GlobalVars.Get("ProxyTest");
      if (proxyTest == "Enabled")
        proxied = true;
      bool success = false;
      int delay = Options.GetInt("RetryDelay", 1000);
      while (retry-- > 0 && !success)
      {
       CreateReq();
       success=GetResponse();
       
       if (proxyTest == string.Empty)
       {
         if (success)
           GlobalVars.Set("ProxyTest", proxied ? "Enabled" : "Disabled");
         else
           proxied = !proxied;
       }
       if (!success)
         Thread.Sleep(delay);
      }
      string output=content.ToString();
      if (output == string.Empty)
        Log.Error(ToString(), "Download failed");
      else
        Log.Info(ToString(), "Downloaded: \n" + output);
      return output;
    }
  }
}
