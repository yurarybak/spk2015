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
    class SolverHooks
    {
        ConcurrentQueue<string> fileContent;

        List<City> cityList = City.Cities.ToList();
        internal void LaunchAndTrace(TravellingSalesmanProblemSolver solver)
        {
            fileContent = new ConcurrentQueue<string>();
            solver.EvaluateFitness += TSPSolverOnEvaluateFitness;
            solver.GenerationComplete += TSPSolverOnGenerationComplete;

            var fileStreamSlns = new StreamWriter("cities-solutions.txt");

            int slnInd = 0;
            foreach (var solution in solver.Solve(cityList))
            {
                fileStreamSlns.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}"
                     , "s"
                     , ""
                     , solution.Distance
                     , solution.Fitness
                     , slnInd
                     , solution.CityIndexString
                     , solution.CityCoordsString
                     ));
                slnInd++;
            }
            fileStreamSlns.Close();


            int gi = 0;
            bool openNewFile = true;

            StreamWriter fileStreamGnrs = null;
            foreach (var entry in fileContent)
            {
                switch (entry[0])
                {
                    case 'c':
                        if (openNewFile)
                        {
                            if (fileStreamGnrs != null)
                                fileStreamGnrs.Close();
                            fileStreamGnrs = new StreamWriter("cities-generation" + gi++ + ".txt");
                            openNewFile = false;
                        }

                        fileStreamGnrs.WriteLine(entry);
                        break;
                    case 'g':
                        openNewFile = true;
                        fileStreamGnrs.WriteLine(entry);
                        break;
                }
            }

            fileStreamGnrs.Close();
        }

        private void TSPSolverOnGenerationComplete(GaEventArgs args)
        {
            Console.Write('*');

            int slnInd = 0;
            foreach (var item in args.Population.Solutions)
            {
                if (!item.IsElite) continue;
                var solution = new TravellingSalesmanProblemSolution(item, cityList);
                fileContent.Enqueue(string.Format("g{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}"
                    , args.Generation
                    , ""
                    , solution.Distance
                    , solution.Fitness
                    , slnInd
                    , solution.CityIndexString
                    , solution.CityCoordsString
                    ));
                ++slnInd;
            }
        }

        private void TSPSolverOnEvaluateFitness(Chromosome chromosome, TravellingSalesmanProblemSolution solution)
        {
            Console.Write('.');
            fileContent.Enqueue(string.Format("c\t{0}\t{1}\t{2}\t{3}\t{4}"
                , chromosome.ToString()
                , solution.Distance
                , solution.Fitness
                , solution.CityIndexString
                , solution.CityCoordsString
                ));
        }
    }

    class Program
    {
        static void LaunchLabwork5()
        {
            var hooks = new SolverHooks();
            hooks.LaunchAndTrace(new TravellingSalesmanProblemSolver());
        }

        static void Main(string[] args)
        {
            LaunchLabwork5();
        }
    }
}
