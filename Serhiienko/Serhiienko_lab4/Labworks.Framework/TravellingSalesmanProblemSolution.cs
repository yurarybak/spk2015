using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;

namespace Labworks.Framework
{
    public class TravellingSalesmanProblemSolution
    {
        public TravellingSalesmanProblemSolution(Chromosome item, List<City> cities)
        {
            Cities = cities;
            CityIndices = new List<int>(item.Count);
            foreach (var g in item.Genes)
                CityIndices.Add((int)g.RealValue);
        }

        public readonly List<City> Cities;
        public List<int> CityIndices { get; set; }

        public string CityIndexString
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var item in CityIndices)
                    builder.Append(item + "\t");
                return builder.ToString();
            }
        }
        public string CityCoordsString
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("\n\t\t");
                foreach (var item in CityIndices)
                {
                    var city = Cities[item];
                    builder.Append(city.Latitude + "\t" + city.Longitude + "\n\t\t");
                }
                return builder.ToString();
            }
        }

        public double Fitness
        {
            get
            {
                return 1.0 - Distance / 10000.0;
            }
        }

        double distance = -1.0f;

        public double Distance
        {
            get
            {
                if (distance >= 0.0)
                    return distance;

                distance = 0.0;
                City previousCity = null;
                foreach (var gene in CityIndices)
                {
                    var currentCity = Cities[gene];
                    if (previousCity != null)
                    {
                        distance += previousCity.GetDistanceFromPosition(
                            currentCity.Latitude,
                            currentCity.Longitude
                            ); ;
                    }

                    previousCity = currentCity;
                }

                return distance;
            }
        }


    }
}
