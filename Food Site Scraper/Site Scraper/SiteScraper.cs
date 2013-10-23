using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Site_Scraper
{
	class SiteScraper
	{
		private static HtmlDocument htmlDoc;

		static void Main(string[] args)
		{
            //String url = "http://www.yelp.com/biz/bacchanalia-atlanta";
			//String url = "http://www.zagat.com/r/bacchanalia-atlanta";
			String url = "http://www.urbanspoon.com/r/9/120002/restaurant/Westside/Bacchanalia-Atlanta";

			htmlDoc = new HtmlDocument();
			using (System.Net.WebClient webClient = new System.Net.WebClient())
			{
				using (var stream = webClient.OpenRead(url))
				{
					htmlDoc.Load(stream);
				}
			}

			if (url.Contains("dine"))
			{
				DineScraper dine = new DineScraper(url, htmlDoc, "dine");
			}
			else if (url.Contains("google"))
			{
				GoogleScraper google = new GoogleScraper(url, htmlDoc, "google");
			}
			else if (url.Contains("urbanspoon"))
			{
				UrbanspoonScraper urbanspoon = new UrbanspoonScraper(url, htmlDoc, "urbanspoon");
			}
			else if (url.Contains("yelp"))
			{
				YelpScraper yelp = new YelpScraper(url, htmlDoc, "yelp");
			}
			else if (url.Contains("zagat"))
			{
				ZagatScraper zagat = new ZagatScraper(url, htmlDoc, "zagat");
			}
		}
	}
}
