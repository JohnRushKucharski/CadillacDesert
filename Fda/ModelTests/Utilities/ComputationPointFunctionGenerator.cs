using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using Model.Inputs.Functions.ComputationPoint;

namespace ModelTests.Utilities
{
    public class ComputationPointFunctionGenerator
    {
        #region TransformGenerator
        public static IFunctionTransform GenerateValidRealisticRandomTransformFunction(int seed = 0)
        {
            Random numberGenerator = new Random(seed);
            int type = numberGenerator.Next(1, 8); if (type % 2 == 1) type++; ComputationPointFunctionEnum typeEnum = (ComputationPointFunctionEnum)type;

            switch (typeEnum)
            {
                case ComputationPointFunctionEnum.InflowOutflow:
                    GenerateInflowOutflowFunction(numberGenerator.Next());
                    break;
                case ComputationPointFunctionEnum.Rating:
                    //xMin = numberGenerator.Next(0, 100000) + numberGenerator.NextDouble();           //same flow range as inflow-outflow, inflow range.
                    //xEpsilonMax = numberGenerator.Next(10, 10000); 

                    //yMin = numberGenerator.Next(-100, 10000) + numberGenerator.NextDouble();         //base elevation must be less than 10,000, above -100
                    //yEpsilonMax = numberGenerator.Next(0, 10);                                       //max epsilon range is 0 - 10;
                    break;
                case ComputationPointFunctionEnum.ExteriorInteriorStage:
                    break;
            }
            throw new NotImplementedException();
        }
        #endregion

        #region InflowFrequencyGenerator
        public static InflowFrequency GenerateValidRandomLogPearsonIIIFunction(int seed = 0)
        {
            Random numberGenerator = new Random(seed);

            int periodOfRecord;
            double mean, sd, skew;
            InflowFrequency generatedFunction;
            do
            {
                periodOfRecord = numberGenerator.Next(200);
                mean = numberGenerator.Next(3) + numberGenerator.NextDouble();
                sd = numberGenerator.Next(2) + numberGenerator.NextDouble();
                skew = numberGenerator.Next(-2, 2) + numberGenerator.NextDouble();
                generatedFunction = new InflowFrequency(new FrequencyFunction(new Statistics.LogPearsonIII(mean, sd, skew, periodOfRecord)));
            } while (generatedFunction.IsValid == false);
            return generatedFunction;
        }
        #endregion

        #region InflowOutflowGenerator
        public static IFunctionTransform GenerateInflowOutflowFunction(int seed, int n = 10)
        {
            Random numberGenerator = new Random(seed);

            double[] xs = new double[n];
            double[] ys = new double[n];
            double xEpsilon, yEpsilon;
            int xEpsilonMax = numberGenerator.Next(10, 10000);
            InflowOutflow generatedFunction;
            do
            {
                for (int i = 0; i < n; i++)
                {
                    if (i == 0)
                    {
                        xs[i] = numberGenerator.Next(0, 100000) + numberGenerator.NextDouble();
                        ys[i] = numberGenerator.Next(0, (int)xs[i]) + numberGenerator.NextDouble();
                    }
                    else
                    {
                        xEpsilon = numberGenerator.Next(0, xEpsilonMax) + numberGenerator.NextDouble();
                        if (numberGenerator.NextDouble() > xEpsilon / xEpsilonMax) yEpsilon = 0;
                        else
                        {
                            if (i > n / 2) yEpsilon = numberGenerator.Next(0, (int)xEpsilon * 2) + numberGenerator.NextDouble();
                            else yEpsilon = numberGenerator.Next(0, (int)xEpsilon) + numberGenerator.NextDouble();
                        }
                        xs[i] = xs[i - 1] + xEpsilon;
                        ys[i] = ys[i - 1] + yEpsilon;
                    }
                }
                generatedFunction = (InflowOutflow)ComputationPointFunctionFactory.CreateNew(new OrdinatesFunction(new Statistics.CurveIncreasing(xs, ys, true, false)), ComputationPointFunctionEnum.InflowOutflow);
            } while (generatedFunction.IsValid == false);
            return generatedFunction;
        }
        #endregion

        #region RatingGenerator
        public static IFunctionTransform GenerateRatingFunction(int seed, int n = 10)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
