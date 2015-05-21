using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Operators;
using Labworks.Framework;
using System.Diagnostics;
using System.IO;
using System.Collections.Concurrent;

namespace Labworks.ConsoleApp
{
    // y(x) = a + bx + cx2 + dx3
    abstract class TargetFunction : TargetFunctionRange, ITargetFunction
    {
        int accuracy;
        double ranging;

        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }

        public TargetFunction() : base(-10, 53)
        {
            // task #3 (23 - 20)
            A = 10;
            B = -20;
            C = -40;
            D = 1;
        }

        public double CalculateX(double N) { return N * (XMax - XMin) + XMin; }
        public double CalculateY(double X) { return A + B * X + C * X * X + D * X * X * X; }
        public abstract double CalculateFitness(double Y);

        public void InitializeRanging(Population initialpopulation)
        {
            accuracy = initialpopulation.ChromosomeLength;
            ranging = 2.0 / (System.Math.Pow(2.0, accuracy) - 1.0);
        }

        public double CalculateN(Chromosome genes)
        {
            var rawX = Convert.ToInt32(genes.ToBinaryString(0, genes.Count), 2);
            var adjustedX = (rawX * ranging) - 1;
            return adjustedX * 0.5 + 0.5;
        }
    }

    class TargetFunctionMax : TargetFunction
    {
        public override double CalculateFitness(double Y) { return ((10000) + Y) / (10000); }
    }

    class TargetFunctionMin : TargetFunction
    {
        public override double CalculateFitness(double Y) { return 1 - ((10000) + Y) / (10000); }
    }

    class SolverHooks
    {
        ConcurrentQueue<string> fileContent;

        TargetFunction targetFunction;
        public void LaunchAndTrace(TargetFunction function, TargetFunctionSolver solver)
        {
            targetFunction = function;

            fileContent = new ConcurrentQueue<string>();

            StreamWriter fileStream = new StreamWriter("func.txt");
            for (double x = function.XMin; x <= function.XMax; x += 0.01)
                fileStream.WriteLine(x + "\t" + function.CalculateY(x));
            fileStream.Close();

            solver.ParentSelection = ParentSelectionMethod.TournamentSelection;
            solver.MutateOperator = new AutoMutateOperator(0.08, false);
            solver.GenerationCompleteCallback = OnGenerationCompleteCallback;
            solver.TerminateCallback = TerminateCallback;
            solver.FitnessEvaluated += OnSolverFitnessEvaluated;

            var fileStreamSlns = new StreamWriter("solutions.txt");
            foreach (var solution in solver.Solve(function, function))
            {
                fileStreamSlns.WriteLine(solution.X + "\t" + solution.Y);
                Console.WriteLine(solution.X + "\t" + solution.Y);
            }
            fileStreamSlns.Close();

            int gi = 0;
            bool openNewFile = true;

            fileStream = null;
            foreach (var entry in fileContent)
            {
                switch (entry[0])
                {
                    case 'c':
                        if (openNewFile)
                        {
                            if (fileStream != null)
                                fileStream.Close();
                            fileStream = new StreamWriter("generation" + gi++ + ".txt");
                            openNewFile = false;
                        }

                        fileStream.WriteLine(entry);
                        break;
                    case 'g':
                        openNewFile = true;
                        fileStream.WriteLine(entry);
                        break;
                }
            }

            fileStream.Close();
        }

        private readonly object sync = new object();
        private void OnSolverFitnessEvaluated(Chromosome chromosome, TargetFunctionSolution solution)
        {
            fileContent.Enqueue("c\t\""
                + chromosome.ToString() + "\"\t"
                + solution.X + "\t"
                + solution.Y + "\t"
                + solution.Fitness
                );
        }

        internal void OnGenerationCompleteCallback(object sender, GaEventArgs e)
        {
            int i = 0;
            foreach (var item in e.Population.GetTop(1))
            {
                var solution = new TargetFunctionSolution(
                    targetFunction,
                    item
                    );

                fileContent.Enqueue(string.Format("g{0}\t{1}\t{2}\t{3}\t{4}",
                    e.Generation,
                    i,
                    solution.X,
                    solution.Y,
                    e.Population.MaximumFitness
                    ));
                ++i;
            }
        }

        internal bool TerminateCallback(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 10;
        }
    }

    class Program
    {
        static void LaunchLabwork4()
        {
            var hooks = new SolverHooks();
            hooks.LaunchAndTrace(new TargetFunctionMin(), new TargetFunctionSolver());
        }
        

        static void Main(string[] args)
        {
            LaunchLabwork4();
        }
    }
}
