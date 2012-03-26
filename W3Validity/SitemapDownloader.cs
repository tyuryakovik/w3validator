using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KBProject.Common;

namespace W3Validity
{
  class SitemapDownloader
  {
    public delegate void AddUrl(string url);

    XmlFacade sitemap;
    AddUrl addUrl;
    public void Download(string url, AddUrl addUrl)
    {      
      this.addUrl=addUrl;
      string map = (new PageDownloader()).DownloadPage(url);
      if (map == string.Empty)
        return ;
      sitemap = new XmlFacade();
      sitemap.Set(map);
     // if (sitemap.curName.Equals("urlset",StringComparison.OrdinalIgnoreCase)
     //     || sitemap.curName.Equals("sitemapindex", StringComparison.OrdinalIgnoreCase))
        GetUrlList();
    }

    void GetUrlList()
    {
      foreach (string urlEl in sitemap)
      {
        if (urlEl.Equals("url", StringComparison.OrdinalIgnoreCase))
        {
          if (sitemap.Cd("loc"))
          {
            addUrl(sitemap.InnerText);
            sitemap.CdUp();
          }
        }
        if (urlEl.Equals("sitemap", StringComparison.OrdinalIgnoreCase))
        {
          if (sitemap.Cd("loc"))
          {
            (new SitemapDownloader()).Download(sitemap.InnerText,addUrl);
            sitemap.CdUp();
          }
        }
      }
    }
  }
}
