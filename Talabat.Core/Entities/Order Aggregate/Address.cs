﻿

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Address 
	{
		public Address(string fName, string lName, string street, string city, string country)
		{
			FName=fName;
			LName=lName;
			Street=street;
			City=city;
			Country=country;
		}

        public Address() 
        {
            
        }
        public string FName { get; set; }
		public string LName { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
	}
}
