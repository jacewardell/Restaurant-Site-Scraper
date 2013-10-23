using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Site_Scraper
{
	class UrbanspoonScraper : Scraper
	{
		public UrbanspoonScraper(string url, HtmlDocument htmlDoc, string source)
			: base(url, htmlDoc, source) { parseSite(); }

		public override void parseSite()
		{
            extractRestaurant();

            extractReviews();
		}

        /**
         * Extracts restaurant info from saved html code
         */
        public override void extractRestaurant()
        {
            //Extract restaurant name
            string restName;
            if ((restName = htmlDoc.DocumentNode.SelectNodes("//h1[@itemprop='name']")[0].InnerHtml) == null)
                restName = "N/A";

            removeChars(restName);

			
            //Extract cuisine type
            string cuisine;
            if ((cuisine = htmlDoc.DocumentNode.SelectNodes("//a[@data-ga_action='explore-resto-cuisine']")[0].InnerHtml) == null)
                cuisine = "N/A";

            cuisine = removeChars(cuisine);

            //Extract restaurant address
            string address;
            if ((address = htmlDoc.DocumentNode.SelectNodes("//span[@class='street-address']")[0].InnerHtml) == null)
                address = "N/A";

            address = removeChars(address);

            //Extract restaurant city
            string city;
            if ((city = htmlDoc.DocumentNode.SelectNodes("//span[@class='locality']")[0].InnerHtml) == null)
                city = "N/A";

            city = removeChars(city);

            //Extract restaurant state
            string state;
            if ((state = htmlDoc.DocumentNode.SelectNodes("//span[@class='region']")[0].InnerHtml) == null)
                state = "N/A";

            state = removeChars(state);

            //Extract restaurant zip code
			IEnumerable<string> zip = Enumerable.Empty<string>();
            string zipCode;
			HtmlAttributeCollection attributes = htmlDoc.DocumentNode.SelectNodes("//a[@class='ga_event cs_track']")[0].Attributes;

			zip = from att in attributes
					  where att.Name == "title"
					  select att.Value;

			int startZipIndex = zip.ElementAt(0).IndexOf(state) + state.Length;
			zipCode = zip.ElementAt(0).Substring(startZipIndex);

            zipCode = removeChars(zipCode);

            //Extract restaurant phone number
            string phone;
            if ((phone = htmlDoc.DocumentNode.SelectNodes("//div[@class='phone tel']")[0].InnerHtml) == null)
                phone = "N/A";

            phone = removeChars(phone);

            //Extract restaurant website
			IEnumerable<string> site = Enumerable.Empty<string>();
            string website;
            HtmlAttributeCollection siteAttributes = htmlDoc.DocumentNode.SelectNodes("//a[@data-ga_action='explore-resto-website']")[0].Attributes;

			site = from att in siteAttributes
				   where att.Name == "title"
				   select att.Value;

			website = removeChars(site.ElementAt(0));

            restaurant = new Restaurant(restName, overallScore, cuisine, address, city, state, zipCode, phone, website);
        }

        /**
		* Extracts restaurant info from saved html code
		*/
        public override void extractReviews()
        {
            //Extracts names of review authors
            IEnumerable<string> authors = Enumerable.Empty<string>();
            if ((names = htmlDoc.DocumentNode.SelectNodes("//a[@data-ga_action='explore-user-profile-page']")) == null)
                names = null;
            else
            {
                authors = from name in names
						  where !name.InnerHtml.Contains("img")
                          select name.InnerHtml;
            }

            //Extracts locations of review authors
            IEnumerable<string> locations = Enumerable.Empty<string>();
            if ((locales = htmlDoc.DocumentNode.SelectNodes("//p[@class='reviewer_info']")) == null)
                locales = null;
            else
            {
                locations = from locale in locales
                            select locale.InnerHtml;
            }

            //Extracts ratings from review authors
            IEnumerable<string> ratings = Enumerable.Empty<string>();
            if ((scores = htmlDoc.DocumentNode.SelectNodes("//meta[@itemprop='ratingValue']")) == null)
                scores = null;
            else
            {
                ratings = from score in scores
                          where score.Attributes.AttributesWithName("content").FirstOrDefault() != null
                          select score.Attributes.AttributesWithName("content").FirstOrDefault().Value;
            }

            //TODO: GET THIS INTO THE RESTAURANT CLASS
            overallScore = ratings.ElementAt(0);

            //Extracts publish dates of reviews
            IEnumerable<string> publishDates = Enumerable.Empty<string>();
            if ((published = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'date smaller')]")) == null)
                published = null;
            else
            {
                publishDates = from publish in published
                               select publish.InnerHtml;
            }


            //Extracts comments of review authors
            IEnumerable<string> comments = Enumerable.Empty<string>();
            if ((notes = htmlDoc.DocumentNode.SelectNodes("//p[@class='review_comment ieSucks']")) == null)
                notes = null;
            else
            {
                comments = from note in notes
                           select note.InnerHtml;
            }

            for (int i = 0; i < authors.Count(); i++)
            {
                Review rev = new Review(authors.ElementAt(i), locations.ElementAt(i), ratings.ElementAt(i + 1),
                    publishDates.ElementAt(i), comments.ElementAt(i));

                reviewCollection.Add(rev);
            }

            if (restaurantCollection.ContainsKey(restaurant))
            {
                restaurantCollection[restaurant].AddRange(reviewCollection);
            }
            else
                restaurantCollection[restaurant] = reviewCollection;
        }
    }
}