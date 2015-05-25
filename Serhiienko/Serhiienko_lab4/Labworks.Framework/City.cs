using System;
using System.Collections.Generic;

namespace Labworks.Framework
{
    public class City
    {
        private static readonly List<City> cities;
        public static IEnumerable<City> Cities { get { return cities; } }
        static City()
        {
            cities = new List<City>();
            cities.Add(new City("Birmingham",   52.486125, -1.890507));
            cities.Add(new City("Bristol",      51.460852, -2.588139));
            cities.Add(new City("London",       51.512161, -0.116215));
            cities.Add(new City("Leeds",        53.803895, -1.549931));
            cities.Add(new City("Manchester",   53.478239, -2.258549));
            cities.Add(new City("Liverpool",    53.409532, -3.000126));
            cities.Add(new City("Hull",         53.751959, -0.335941));
            cities.Add(new City("Newcastle",    54.980766, -1.615849));
            cities.Add(new City("Carlisle",     54.892406, -2.923222));
            cities.Add(new City("Edinburgh",    55.958426, -3.186893));
            cities.Add(new City("Glasgow",      55.862982, -4.263554));
            cities.Add(new City("Cardiff",      51.488224, -3.186893));
            cities.Add(new City("Swansea",      51.624837, -3.94495));
            cities.Add(new City("Exeter",       50.726024, -3.543949));
            cities.Add(new City("Falmouth",     50.152266, -5.065556));
            cities.Add(new City("Canterbury",   51.289406, 1.075802));
        }


        public City(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Name { set; get; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public double GetDistanceFromPosition(double latitude, double longitude)
        {
            var R = 6371.0; // radius of the earth in km
            var dLat = DegreesToRadians(latitude - Latitude);
            var dLon = DegreesToRadians(longitude - Longitude);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(Latitude)) * Math.Cos(DegreesToRadians(latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // distance in km
            return d;
        }

        private static double DegreesToRadians(double deg)
        {
            return deg * (System.Math.PI / 180);
        }

        public byte[] ToBinaryString()
        {
            var result = new byte[6];
            return result;
        }
    }
}