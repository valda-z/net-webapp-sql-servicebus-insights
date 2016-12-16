using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoWebApp.Controllers
{
    public class GeoIPHelper
    {
        public class Names
        {
            public string de { get; set; }
            public string en { get; set; }
            public string es { get; set; }
            public string fr { get; set; }
            public string ja { get; set; }
            public string ru { get; set; }
        }

        public class Subdivision
        {
            public Names names { get; set; }
            public object confidence { get; set; }
            public string iso_code { get; set; }
            public int geoname_id { get; set; }
        }

        public class Names2
        {
            public string en { get; set; }
            public string ru { get; set; }
        }

        public class City
        {
            public Names2 names { get; set; }
            public object confidence { get; set; }
            public int geoname_id { get; set; }
        }

        public class Location
        {
            public int accuracy_radius { get; set; }
            public object average_income { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public object metro_code { get; set; }
            public object population_density { get; set; }
            public string time_zone { get; set; }
        }

        public class Postal
        {
            public string code { get; set; }
            public object confidence { get; set; }
        }

        public class Names3
        {
            public string de { get; set; }
            public string en { get; set; }
            public string es { get; set; }
            public string fr { get; set; }
            public string ja { get; set; }
            public string ru { get; set; }
        }

        public class Continent
        {
            public Names3 names { get; set; }
            public string code { get; set; }
            public int geoname_id { get; set; }
        }

        public class Names4
        {
            public string de { get; set; }
            public string en { get; set; }
            public string es { get; set; }
            public string fr { get; set; }
            public string ja { get; set; }
            public string ru { get; set; }
        }

        public class Country
        {
            public Names4 names { get; set; }
            public object confidence { get; set; }
            public string iso_code { get; set; }
            public int geoname_id { get; set; }
        }

        public class Maxmind
        {
            public object queries_remaining { get; set; }
        }

        public class Names5
        {
            public string de { get; set; }
            public string en { get; set; }
            public string es { get; set; }
            public string fr { get; set; }
            public string ja { get; set; }
            public string ru { get; set; }
        }

        public class RegisteredCountry
        {
            public Names5 names { get; set; }
            public object confidence { get; set; }
            public string iso_code { get; set; }
            public int geoname_id { get; set; }
        }

        public class Names6
        {
        }

        public class RepresentedCountry
        {
            public Names6 names { get; set; }
            public object type { get; set; }
            public object confidence { get; set; }
            public object iso_code { get; set; }
            public object geoname_id { get; set; }
        }

        public class Traits
        {
            public object autonomous_system_number { get; set; }
            public object autonomous_system_organization { get; set; }
            public object connection_type { get; set; }
            public object domain { get; set; }
            public string ip_address { get; set; }
            public bool is_anonymous_proxy { get; set; }
            public bool is_legitimate_proxy { get; set; }
            public bool is_satellite_provider { get; set; }
            public object isp { get; set; }
            public object organization { get; set; }
            public object user_type { get; set; }
        }

        public class RootObject
        {
            public List<Subdivision> subdivisions { get; set; }
            public City city { get; set; }
            public Location location { get; set; }
            public Postal postal { get; set; }
            public Continent continent { get; set; }
            public Country country { get; set; }
            public Maxmind maxmind { get; set; }
            public RegisteredCountry registered_country { get; set; }
            public RepresentedCountry represented_country { get; set; }
            public Traits traits { get; set; }
        }
    }
}