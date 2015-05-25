using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labworks.Framework
{
    public class TargetFunctionSolution
    {
        public double X { get;set; }
        public double Y { get; set; }
        public double Fitness { get; set; }

        public TargetFunctionSolution()
        {
        }

        public TargetFunctionSolution(ITargetFunction function, GAF.Chromosome chromosome)
        {
            Decode(function, chromosome);
        }

        public void Decode(ITargetFunction function, GAF.Chromosome chromosome)
        {
            var n = function.CalculateN(chromosome);
            X = function.CalculateX(n);
            Y = function.CalculateY(X);
            Fitness = chromosome.Fitness == 0.0f 
                ? function.CalculateFitness(Y)
                : chromosome.Fitness;
        }

    }
}
