using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;

namespace Labworks.Framework
{
    class UniformCrossover : IGeneticOperator
    {
        public bool Enabled
        {
            get;
            set;
        }

        int evaluationCount = 0;

        public int GetOperatorInvokedEvaluations()
        {
            return evaluationCount;
        }

        public void Invoke(Population currentPopulation, ref Population newPopulation, FitnessFunction fitnesFunctionDelegate)
        {

        }
    }
}
