using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Extensions;

namespace Labworks.Framework
{
    public class TravellingSalesmanProblemSolver
    {
        public delegate void EvaluateFitnessHandler(Chromosome chromosome, TravellingSalesmanProblemSolution solution);
        public delegate void GenerationCompleteHandler(GaEventArgs args);

        public int ElitismPercentage { get; set; }
        public double CrossoverProbability { get; set; }
        public double MutationProbability { get; set; }

        public GAF.ParentSelectionMethod ParentSelection { get; set; }
        public GAF.Operators.CrossoverType CrossoverType { get; set; }

        public GAF.IGeneticOperator ElitismOperator { get; set; }
        public GAF.IGeneticOperator CrossoverOperator { get; set; }
        public GAF.IGeneticOperator MutateOperator { get; set; }

        public GAF.FitnessFunction EvaluateFitnessCallback { get; set; }
        public GAF.TerminateFunction TerminateCallback { get; set; }
        public GAF.GeneticAlgorithm.GenerationCompleteHandler GenerationCompleteCallback { get; set; }

        public event EvaluateFitnessHandler EvaluateFitness;
        public event GenerationCompleteHandler GenerationComplete;

        private List<City> cities;

        public TravellingSalesmanProblemSolver()
        {
            ElitismPercentage = 5;
            CrossoverProbability = 0.65;
            MutationProbability = 0.02;
            CrossoverType = GAF.Operators.CrossoverType.DoublePointOrdered;
            ParentSelection = GAF.ParentSelectionMethod.StochasticUniversalSampling;
        }

        public IEnumerable<TravellingSalesmanProblemSolution> Solve(IEnumerable<City> cities )
        {
            this.cities = new List<City>(cities ?? City.Cities);
            var population = new Population(100);//, this.cities.Count, false, false, ParentSelection);

            foreach (var p in Enumerable.Range(0, 100))
            {
                var chromosome = new Chromosome();
                for (var g = 0; g < this.cities.Count; g++)
                    chromosome.Genes.Add(new Gene(g));
                chromosome.Genes.Shuffle();
                population.Solutions.Add(chromosome);
            }

            var ga = new GeneticAlgorithm(population, InternalEvaluateFitness);
            if (ElitismOperator == null) ElitismOperator = new GAF.Operators.Elite(ElitismPercentage);
            if (CrossoverOperator == null) CrossoverOperator = new GAF.Operators.Crossover(CrossoverProbability, false, CrossoverType);
            if (MutateOperator == null) MutateOperator = new GAF.Operators.SwapMutate(MutationProbability);

            ga.Operators.Add(ElitismOperator);
            ga.Operators.Add(CrossoverOperator);
            ga.Operators.Add(MutateOperator);
            ga.OnGenerationComplete += GenerationCompleteCallback ?? InternalOnGenerationComplete;
            ga.OnRunComplete += OnRunComplete;

            try
            {
                ga.Run(TerminateCallback ?? InternalTerminateCallback);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            foreach (var item in ga.Population.Solutions)
                if (item != null && item.IsElite)
                    yield return new TravellingSalesmanProblemSolution(item, this.cities);
        }

        private void OnRunComplete(object sender, GaEventArgs e)
        {
        }

        private bool InternalTerminateCallback(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 200;
        }

        private void InternalOnGenerationComplete(object sender, GaEventArgs e)
        {
            GenerationComplete(e);
        }

        private double InternalEvaluateFitness(Chromosome chromosome)
        {
            var solution = new TravellingSalesmanProblemSolution(chromosome, cities);
            var fitness = solution.Fitness;
            EvaluateFitness(chromosome, solution);
            return fitness;
        }

        private double CalculateDistance(Chromosome chromosome)
        {
            var solution = new TravellingSalesmanProblemSolution(chromosome, cities);
            return solution.Distance;
        }
    }
}
