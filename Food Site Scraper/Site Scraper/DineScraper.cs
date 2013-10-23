using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Site_Scraper
{
	class DineScraper : Scraper
	{
		public DineScraper(string url, HtmlDocument htmlDoc, String source)
			: base(url, htmlDoc, source) { parseSite(); }
		public override void parseSite()
		{
			
		}

		public override void extractRestaurant()
		{
			throw new NotImplementedException();
		}

		public override void extractReviews()
		{
			throw new NotImplementedException();
		}
	}
}
