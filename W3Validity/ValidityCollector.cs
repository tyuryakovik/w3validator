using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using KBProject.Common;
using System.Threading;
namespace W3Validity
{
  class ValidityCollector
  {
    public delegate void Finished(ValidityParser[] validators);
    public delegate void Progress(int max, int cur);
    const string smString = "Sitemap:";
    public const string httpString = "http://";

    Finished finished;
    Progress progress;

    List<string> urllist = new List<string>();
    List<ValidityParser> validators = new List<ValidityParser>();
    string site;

    public ValidityCollector(string analizedSite)
    {
      site = CheckURL(analizedSite);
    }

    public void Collect(Finished finished, Progress progress)
    {
      this.finished = finished;
      this.progress = progress;
      ThreadPool.QueueUserWorkItem(DoCollect);
    }

    void DoCollect(Object state)
    {
      if (GetSiteMap())
      {        
          MakeValidators();        
      }
      finished(validators.ToArray());
      Log.Info(ToString(), "Finished");
    }

    private void MakeValidators()
    {
      int n = 1;
      foreach (string url in urllist)
      {
        validators.Add(new ValidityParser(url));
        progress(urllist.Count, n++);
      }
    }

    string GetRobots()
    {
      string robots = (new PageDownloader()).DownloadPage(site + "/robots.txt");
      return robots;
    }

    string GetSMUrl(string robots)
    {
      int sm = robots.IndexOf(smString,StringComparison.OrdinalIgnoreCase);
      if (sm == -1)
        return string.Empty;
      int lf = robots.IndexOf('\n', sm);
      int rm = robots.IndexOf('\r', sm);
      if (lf < rm || rm == -1)
        rm = lf;
      if (rm == -1)
        return string.Empty;
      sm += smString.Length;
      return CheckURL(robots.Substring(sm, rm - sm).Trim());
    }

    bool GetSiteMap()
    {
      string url = GetSMUrl(GetRobots());
      if (url == string.Empty)
        url = site + "/sitemap.xml";
      (new SitemapDownloader()).Download(url, AddUrl);
      Log.Info(ToString(), "List of " + urllist.Count + " urls created");
      return true;
    }

    string CheckURL(string url)
    {
      if (!url.StartsWith(httpString, StringComparison.Ordinal))
      {
        url = httpString + url;
      }
      while(url.EndsWith("/"))
        url=url.Substring(0, url.Length - 1);
      return url;
    }

    void AddUrl(string url)
    {
      urllist.Add(url);
    }

  }
}
