using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class Address
    {
        public string StreetAdd1 { get; set; }
        public string StreetAdd2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public Address()
        {
            StreetAdd1 = "";
            StreetAdd2 = "";
            City = "";
            State = "";
            Zip = "";
        }

        public Address(string add1, string add2, string city, string state, string zip)
        {
            this.StreetAdd1 = add1;
            this.StreetAdd2 = add2;
            this.City = city;
            this.State = state;
            this.Zip = zip;
        }
    }
}