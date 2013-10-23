using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Scraper
{
	class Restaurant
	{
		private String restName, averageScore, cuisine, address, city, state, zipCode, phone, website;

		public Restaurant(String restName, String averageScore, String cuisine, String address, String city, 
			String state, String zipCode, String phone, String website)
		{
			this.restName = restName;
			this.averageScore = averageScore;
			this.cuisine = cuisine;
			this.address = address;
			this.city = city;
			this.state = state;
			this.zipCode = zipCode;
			this.phone = phone;
			this.website = website;
		}
	}
}