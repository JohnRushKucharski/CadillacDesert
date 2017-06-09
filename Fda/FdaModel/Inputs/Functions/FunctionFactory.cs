using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    public static class FunctionFactory
    {
        public static IFunction CreateNew(Statistics.CurveIncreasing function, FunctionType type)
        {
            IFunction baseFunction = new OrdinatesFunction(function);

            switch (type)
            {
                case FunctionType.InflowOutflow:
                    return new InflowOutflowDecorator(baseFunction);
                case FunctionType.OutflowFrequency:
                    return new OutflowFrequencyDecorator(baseFunction);
                case FunctionType.Rating:
                    return new RatingDecorator(baseFunction);
                default:
                    return baseFunction;
            }
        }

        public static IFunction CreateNew(Statistics.LogPearsonIII function, FunctionType type)
        {
            IFunction baseFunction = new FrequencyFunction(function);

            switch (type)
            {
                case FunctionType.InflowFrequency:
                    return new InflowFrequencyDecorator(baseFunction);
                case FunctionType.OutflowFrequency:
                    return new OutflowFrequencyDecorator(baseFunction);
                default:
                    return baseFunction;    
            }
        }

        public static IFunction CreateNew(IFunction function, FunctionType type)
        {
            switch (type)
            {
                case FunctionType.OutflowFrequency:
                    return new OutflowFrequencyDecorator(function);
                default:
                    return function;
            }

        }
    }
}
