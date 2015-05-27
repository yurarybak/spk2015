using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labworks.Framework
{
    public class OnePointMutation : MutationBase
    {
        public OnePointMutation()
        {
            IsOrdered = true;
        }
        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                var indexPrimary = RandomizationProvider.Current.GetInt(0, chromosome.Length - 1);
                var indexSecondary = indexPrimary + 1;
                var genePrimary = chromosome.GetGene(indexPrimary);
                var geneSecondary = chromosome.GetGene(indexSecondary);
                chromosome.ReplaceGene(indexPrimary, genePrimary);
                chromosome.ReplaceGene(indexSecondary, geneSecondary);
            }
        }
    }
}
