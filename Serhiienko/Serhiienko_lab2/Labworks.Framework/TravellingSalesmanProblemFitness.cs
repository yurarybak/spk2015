using GeneticSharp.Extensions.Tsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labworks.Framework
{
    public class TspSphericalFitness : TspFitness
    {
        public TspSphericalFitness(int numberOfCities, int minX, int maxX, int minY, int maxY)
            : base(numberOfCities, minX, maxX, minX, maxY)
		{
		}

        private static double DegreesToRadians(double deg)
        {
            return deg * (System.Math.PI / 180);
        }

        //protected override double CalcDistanceTwoCities(TspCity one, TspCity two)
        //{
        //    var R = 6371.0; 
        //    var dLat = DegreesToRadians(two.X - one.X);
        //    var dLon = DegreesToRadians(two.Y - one.Y);
        //    var a =
        //        Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
        //        Math.Cos(DegreesToRadians(one.X)) * Math.Cos(DegreesToRadians(two.X)) *
        //        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        //    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        //    var d = R * c; 
        //    return d;
        //}
    }
}
