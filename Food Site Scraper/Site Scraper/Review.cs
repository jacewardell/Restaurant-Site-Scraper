using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Site_Scraper
{

	class Review
	{
		private string authors, locations, ratings, publishDates, comments;

		public Review(string authors, string locations, string ratings, string publishDates, string comments)
		{
			this.authors = authors;
			this.locations = locations;
			this.ratings = ratings;
			this.publishDates = publishDates;
			this.comments = comments;
		}
	}
}
