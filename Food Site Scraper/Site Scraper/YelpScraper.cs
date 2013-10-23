using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Site_Scraper
{
	class YelpScraper : Scraper
	{
		public YelpScraper(String url, HtmlDocument htmlDoc, String source)
			: base(url, htmlDoc, source) { parseSite(); }


		/**
		 * Parses a Yelp website using web scraping
		 */
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
			String restName;
			if ((restName = htmlDoc.DocumentNode.SelectNodes("//h1[@itemprop='name']")[0].InnerHtml) == null)
				restName = "N/A";

            removeChars(restName);


			//Extract cuisine type
			String cuisine;
			if ((cuisine = htmlDoc.DocumentNode.SelectNodes("//span[@itemprop='title']")[1].InnerHtml) == null)
				cuisine = "N/A";

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
			String phone;
			if ((phone = htmlDoc.DocumentNode.SelectNodes("//span[@id='bizPhone']")[0].InnerHtml) == null)
				phone = "N/A";

            phone = removeChars(phone);

			//Extract restaurant website
			String website;
			if ((website = htmlDoc.DocumentNode.SelectNodes("//a[@target='_blank']")[0].InnerHtml) == null)
				website = "N/A";

            website = removeChars(website);

			restaurant = new Restaurant(restName, overallScore, cuisine, address, city, state, zipCode, phone, website);
		}

        /**
		* Extracts restaurant info from saved html code
		*/
        public override void extractReviews()
        {
            //Extracts names of review authors
            IEnumerable<String> authors = Enumerable.Empty<string>();
            if ((names = htmlDoc.DocumentNode.SelectNodes("//span[@itemprop='author']")) == null)
                names = null;
            else
            {
                authors = from name in names
                          select name.InnerHtml;
            }

            //Extracts locations of review authors
            IEnumerable<String> locations = Enumerable.Empty<string>();
            if ((locales = htmlDoc.DocumentNode.SelectNodes("//p[@class='reviewer_info']")) == null)
                locales = null;
            else
            {
                locations = from locale in locales
                            select locale.InnerHtml;
            }

            //Extracts ratings from review authors
            IEnumerable<String> ratings = Enumerable.Empty<string>();
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
            IEnumerable<String> publishDates = Enumerable.Empty<string>();
            if ((published = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'date smaller')]")) == null)
                published = null;
            else
            {
                publishDates = from publish in published
                               select publish.InnerHtml;
            }


            //Extracts comments of review authors
            IEnumerable<String> comments = Enumerable.Empty<string>();
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
