using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labworks.Framework
{
    public class TargetFunctionSolver
    {
        public delegate void FitnessEvaluatedHandler(GAF.Chromosome chromosome, TargetFunctionSolution solution);

        private ITargetFunction targetFunction;
        private TargetFunctionRange targetFunctionRange;

        public int ElitismPercentage { get; set; }
        public double CrossoverProbability { get; set; }
        public double MutationProbability { get; set; }

        public GAF.ParentSelectionMethod ParentSelection { get; set; }
        public GAF.Operators.CrossoverType CrossoverType { get; set; }

        public GAF.IGeneticOperator ElitismOperator { get; set; }
        public GAF.IGeneticOperator CrossoverOperator { get; set; }
        public GAF.IGeneticOperator MutateOperator { get; set; }

        public event FitnessEvaluatedHandler FitnessEvaluated;

        public GAF.FitnessFunction EvaluateFitnessCallback { get; set; }
        public GAF.TerminateFunction TerminateCallback { get; set; }
        public GAF.GeneticAlgorithm.GenerationCompleteHandler GenerationCompleteCallback { get; set; }

        public TargetFunctionSolver()
        {
            ElitismPercentage = 5;
            CrossoverProbability = 0.65;
            MutationProbability = 0.08;
            CrossoverType = GAF.Operators.CrossoverType.SinglePoint;
            ParentSelection = GAF.ParentSelectionMethod.FitnessProportionateSelection;
        }

        public IEnumerable<TargetFunctionSolution> Solve(ITargetFunction function, TargetFunctionRange functionRange)
        {
            this.targetFunction = function;
            this.targetFunctionRange = functionRange;

            var population = new GAF.Population(100, 22, false, false, ParentSelection);
            targetFunction.InitializeRanging(population);

            var ga = new GAF.GeneticAlgorithm(population, InternalEvaluateFitness);
            if (ElitismOperator == null) ElitismOperator = new GAF.Operators.Elite(ElitismPercentage);
            if (CrossoverOperator == null) CrossoverOperator = new GAF.Operators.Crossover(CrossoverProbability, false, CrossoverType);
            if (MutateOperator == null) MutateOperator = new GAF.Operators.BinaryMutate(MutationProbability, false);

            ga.Operators.Add(ElitismOperator);
            ga.Operators.Add(CrossoverOperator);
            ga.Operators.Add(MutateOperator);
            ga.OnGenerationComplete += GenerationCompleteCallback ?? InternalOnGenerationComplete;

            try
            {
                ga.Run(TerminateCallback ?? InternalTerminateCallback);
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            foreach (var item in ga.Population.Solutions)
                if (item != null && item.IsElite) 
                    yield return new TargetFunctionSolution(targetFunction, item);
        }

        private bool InternalTerminateCallback(GAF.Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 200;
        }

        private double InternalEvaluateFitness(GAF.Chromosome chromosome)
        {
            double fitnessValue = -1;
            if (chromosome != null)
            {
                var solution = new TargetFunctionSolution(targetFunction, chromosome);
                FitnessEvaluated(chromosome, new TargetFunctionSolution(targetFunction, chromosome));

                fitnessValue = solution.Fitness;
            }
            else
            {
                throw new ArgumentNullException("chromosome");
            }

            return fitnessValue;
        }

        private void InternalOnGenerationComplete(object sender, GAF.GaEventArgs e)
        {
            //get the best solution 
            var chromosome = e.Population.GetTop(1)[0];
            var solution = new TargetFunctionSolution(targetFunction, chromosome);

            //display the X, Y and fitness of the best chromosome in this generation 
            System.Console.WriteLine("{0}> x:{1} y:{2} Fitness:{3}", 
                e.Generation, 
                solution.X, 
                solution.Y, 
                e.Population.MaximumFitness
                );
        }
    }
}
