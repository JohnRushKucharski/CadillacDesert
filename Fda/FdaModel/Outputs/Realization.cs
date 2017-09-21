using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs;
using Model.Inputs.Conditions;

namespace Model.Outputs
{
    public class Realization
    {
        #region Properties
        public ICondition Condition { get; }
        public int TimeStampSeed { get; private set; }
        public Random SeedGenerator { get; private set; }
        public int IterationCount { get; private set; } = 0;
        public bool Converged { get; private set; } = false;
        public int MaxIterations { get; set; } = 500000;
        public IList<int> IterationSeedContainer { get; private set; } = new List<int>();
        public Statistics.Histogram Aep { get; private set; } = new Statistics.Histogram(50, 0.5, 0.001, false);
        public Statistics.Histogram Ead { get; private set; } = new Statistics.Histogram(50, 0, 1000000, false);
        #endregion

        #region Constructor
        public Realization(ICondition condition)
        {
            Condition = condition;
        }
        #endregion

        #region Methods
        public void Compute()
        {
            if (Condition.IsValid == false) { Condition.ReportValidationErrors(); return; }

            IterationCount = 0;
            TimeStampSeed = (int)new DateTime().Ticks;
            SeedGenerator = new Random(TimeStampSeed);
            int localIteration, batchCount = 1000;

            while (Converged == false &&
                   IterationCount < MaxIterations)
            {
                localIteration = IterationCount;
                for (int i = 0; i < batchCount; i++) IterationSeedContainer.Add(SeedGenerator.Next());
              
                Parallel.For(localIteration, localIteration + batchCount, i =>
                {
                    Condition.Compute(IterationSeedContainer[i]);
                    IterationCount++;
                });
            }
        }

        #endregion
    }
}
