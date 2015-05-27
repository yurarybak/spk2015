using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using Rand = GeneticSharp.Domain.Randomizations.RandomizationProvider;

namespace Labworks.Framework
{
    public enum TargetFunctionType
    {
        Min, 
        Max
    }

    public class TargetFunctionChromosome : ChromosomeBase
    {
        internal static readonly int geneCount;
        internal static readonly double geneRanging;
        internal static readonly double fitnessEvaluationYMax;

        static TargetFunctionChromosome()
        {
            geneCount = 12;
            fitnessEvaluationYMax = 1000000;
            geneRanging = 2.0 / (System.Math.Pow(2.0, geneCount) - 1.0);
        }

        public interface IFitnessEvaluation
        {
            TargetFunctionType FunctionType { get; }
            double CalculateFitness(double Y);
        }

        public class FitnessEvaluationMax : IFitnessEvaluation
        {
            public virtual TargetFunctionType FunctionType { get { return TargetFunctionType.Max; } }
            public virtual double CalculateFitness(double Y) { return (Y + 11000) / 55000; }
        }

        internal class FitnessEvaluationMin : FitnessEvaluationMax
        {
            public override TargetFunctionType FunctionType { get { return TargetFunctionType.Min; } }
            public override double CalculateFitness(double Y) { return 1 - base.CalculateFitness(Y); }
        }

        IFitnessEvaluation fitnessEvaluation;
        public ITargetFunction Function { get; internal set; }
        public TargetFunctionRange FunctionRange { get; internal set; }

        public TargetFunctionChromosome(ITargetFunction function, TargetFunctionRange functionRange, TargetFunctionType type)
            : base(geneCount)
        {
            Function = function;
            FunctionRange = functionRange;

            switch (type)
            {
                case TargetFunctionType.Min: fitnessEvaluation = new FitnessEvaluationMin(); break;
                default: fitnessEvaluation = new FitnessEvaluationMax(); break;
            }

            var geneValues = Rand.Current.GetInts(geneCount, 0, 2);
            foreach (var geneIndex in Enumerable.Range(0, geneCount))
                ReplaceGene(geneIndex, new Gene(geneValues[geneIndex]));
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(Rand.Current.GetInt(0, 2));
        }

        public override IChromosome CreateNew()
        {
            return new TargetFunctionChromosome(
                Function, 
                FunctionRange, 
                fitnessEvaluation.FunctionType
                );
        }

        public override IChromosome Clone()
        {
            return CreateNew();
        }

        public string ToBinaryString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var gene in GetGenes())
                stringBuilder.Append((int)gene.Value);
            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var gene in GetGenes())
                stringBuilder.AppendFormat("{0} ", (int)gene.Value);
            return stringBuilder.ToString();
        }

        public double CalculateN()
        {
            var x = Convert.ToInt32(ToBinaryString(), 2);
            var xx = (x * geneRanging) - 1.0;
            return xx * 0.5 + 0.5;
        }

        public double CalculateX(double N) 
        { 
            return N * (FunctionRange.XMax - FunctionRange.XMin) + FunctionRange.XMin; 
        }

        public double CalculateY(double X) 
        { 
            return Function.A + Function.B * X + Function.C * X * X + Function.D * X * X * X; 
        }

        public double CalculateFitness(double Y)
        {
            return fitnessEvaluation.CalculateFitness(Y);
        }

    }
}
