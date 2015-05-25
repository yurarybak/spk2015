using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labworks.Framework
{
    public interface ITargetFunction
    {
        double A { get; set; }
        double B { get; set; }
        double C { get; set; }
        double D { get; set; }

        double CalculateX(double N);
        double CalculateY(double X);
        double CalculateFitness(double Y);

        double CalculateN(GAF.Chromosome genes);
        void InitializeRanging(GAF.Population initialpopulation); 

    }

    public class TargetFunctionRange
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public TargetFunctionRange(double min, double max)
        {
            XMin = min;
            XMax = max;
        }
    }


}
