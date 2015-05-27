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
    public class GaussianMutation : MutationBase
    {
        public GaussianMutation()
        {
            IsOrdered = true;
        }

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                foreach (var geneIndex in Enumerable.Range(0, chromosome.Length))
                {
                    var gaussian = GaussianRandom.Instance.NextGaussian();
                    var gene = chromosome.GetGene(geneIndex);
                    var geneBit = (int)gene.Value;
                    if (geneBit > 0 && gaussian >= 0.0)
                        chromosome.ReplaceGene(geneIndex, new Gene(0));
                    else if (geneBit == 0 && gaussian < 0.0)
                        chromosome.ReplaceGene(geneIndex, new Gene(1));
                }
            }
        }
    }
}
