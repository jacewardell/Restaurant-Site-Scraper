using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Site_Scraper
{
	class ZagatScraper : Scraper
	{
		String overallScore;

		public ZagatScraper(string url, HtmlDocument htmlDoc, String source)
			: base(url, htmlDoc, source) { parseSite(); }

		public override void parseSite()
		{
			extractRestaurant();

			extractReviews();
		}

		public override void extractRestaurant()
		{
			//Extract restaurant name
			String restName;
			if ((restName = htmlDoc.DocumentNode.SelectNodes("//h1[@itemprop='name']")[0].InnerHtml) == null)
				restName = "N/A";

			restName = removeChars(restName);

			//Extract cuisine type
			String cuisine;
			if ((cuisine = htmlDoc.DocumentNode.SelectNodes("//span[@class='date']")[0].InnerHtml) == null)
				cuisine = "N/A";
			else
				cuisine = cuisine.Substring(0, cuisine.IndexOf('|') - 1);

			cuisine = removeChars(cuisine);

			//Extract restaurant address
			String address;
			if ((address = htmlDoc.DocumentNode.SelectNodes("//span[@itemprop='streetAddress']")[0].InnerHtml) == null)
				address = "N/A";

			address = removeChars(address);

			//Extract restaurant city
			String city;
			if ((city = htmlDoc.DocumentNode.SelectNodes("//span[@itemprop='addressLocality']")[0].InnerHtml) == null)
				city = "N/A";

			city = removeChars(city);

			//Extract restaurant state
			String state;
			if ((state = htmlDoc.DocumentNode.SelectNodes("//span[@itemprop='addressRegion']")[0].InnerHtml) == null)
				state = "N/A";

			state = removeChars(state);

			//Extract restaurant zip code
			String zipCode;
			if ((zipCode = htmlDoc.DocumentNode.SelectNodes("//span[@itemprop='postalCode']")[0].InnerHtml) == null)
				zipCode = "N/A";

			zipCode = removeChars(zipCode);

			//Extract restaurant phone number
			// Text node query //p[itemprop='address']/text()    \d{3}-\d{3}-\d{4}
			String phone = "";
			HtmlNodeCollection test = (htmlDoc.DocumentNode.SelectNodes("//p[@itemprop='address']/text()"));
			foreach (HtmlNode node in test)
			{
				string s = node.OuterHtml.Trim();
				MatchCollection matches = Regex.Matches(s, @"(\(\d{3}\) ?\d{3}( |-)?\d{4}|\d{3}( |-)?\d{3}( |-)?\d{4})",
					RegexOptions.Multiline);

				foreach (Match match in matches)
				{
					phone = match.Captures[0].Value;
				}
			}

			phone = removeChars(phone);

			//Extract restaurant website
			String website;
			if ((website = htmlDoc.DocumentNode.SelectNodes("//a[@target='_blank']")[1].InnerHtml) == null)
				website = "N/A";

			website = removeChars(website);

			restaurant = new Restaurant(restName, overallScore, cuisine, address, city, state, zipCode, phone, website);
		}

		public override void extractReviews()
		{

		}
	}
}
