using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Fitnesses;

namespace Labworks.Framework
{
    public class TargetFunctionFitness : IFitness
    {
        public TargetFunctionFitness()
        {
        }

        public double Evaluate(GeneticSharp.Domain.Chromosomes.IChromosome chromosome)
        {
            var targetFunctionChromosome = chromosome as TargetFunctionChromosome;
            if (targetFunctionChromosome != null)
            {
                var n = targetFunctionChromosome.CalculateN();
                var x = targetFunctionChromosome.CalculateX(n);
                var y = targetFunctionChromosome.CalculateY(x);
                return  targetFunctionChromosome.CalculateFitness(y);
            }
            else
                throw new ArgumentException("chromosome is the instance of an unexpected type");
        }
    }
}
