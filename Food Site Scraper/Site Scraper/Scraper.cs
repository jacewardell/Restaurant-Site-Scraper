using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Site_Scraper
{

	abstract class Scraper
	{
		public String url;
		public HtmlDocument htmlDoc;
		public String source;
		public HtmlNodeCollection names;
		public HtmlNodeCollection locales;
		public HtmlNodeCollection scores;
		public HtmlNodeCollection published;
		public HtmlNodeCollection notes;
		public Restaurant restaurant;
        public String overallScore;
		public static List<Review> reviewCollection;
		public static Dictionary<Restaurant, List<Review>> restaurantCollection;

		/**
		 * Constructs a site scraper
		 */
		public Scraper(String url, HtmlDocument htmlDoc, String source)
		{
			this.url = url;
			this.htmlDoc = htmlDoc;
			this.source = source;
			reviewCollection = new List<Review>();
			restaurantCollection = new Dictionary<Restaurant, List<Review>>();
		}

		/*
		 * Parses a webpage
		 */
		public abstract void parseSite();

		/*
		 * Extracts the restaurant information
		 */
		public abstract void extractRestaurant();

		/*
		 * Extracts the review information
		 */
		public abstract void extractReviews();

        /*
         * Parses a string by taking /n, /t, and /r off
         */
        public String removeChars(String s)
        {
            s = s.Replace("\t", "");
            s = s.Replace("\n", "");
            s = s.Replace("\r", "");
			s = s.Trim();

            return s;
        }
	}
}
