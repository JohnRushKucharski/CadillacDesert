using System;
using System.Linq;
using System.Collections.Generic;
using Model.Inputs.Functions.Implementations;

namespace Model.Inputs.Functions
{
    public static class FunctionFactory
    {
        public static BaseImplementation CreateNew(Statistics.CurveIncreasing ordinates, FunctionTypeEnum type, bool addToRegistry = true)
        {
            if (addToRegistry == true)
            {
                BaseImplementation implementation = ConstructImplementation(new OrdinatesFunction(ordinates), type);
                FunctionRegistry.AddToRegistry(implementation); return implementation;
            }
            else return ConstructImplementation(new OrdinatesFunction(ordinates), type);
        }
        public static BaseImplementation CreateNew(string name, Statistics.CurveIncreasing ordinates, FunctionTypeEnum type)
        {
            BaseImplementation implementation = ConstructImplementation(new OrdinatesFunction(ordinates), type);
            FunctionRegistry.AddToRegistry(name, implementation); return implementation;
        }
        public static BaseImplementation CreateNew(Statistics.LogPearsonIII function, FunctionTypeEnum type, bool addToRegistry = true)
        {
            IFunctionBase baseFunction = new FrequencyFunction(function);
            if (addToRegistry == true)
            {
                BaseImplementation implementation = ConstructImplementation(new FrequencyFunction(function), type);
                FunctionRegistry.AddToRegistry(implementation); return implementation;
            }
            else return ConstructImplementation(new FrequencyFunction(function), type);
            
        }
        public static BaseImplementation CreateNew(string name, Statistics.LogPearsonIII function, FunctionTypeEnum type)
        {
            BaseImplementation implementation = ConstructImplementation(new FrequencyFunction(function), type);
            FunctionRegistry.AddToRegistry(name, implementation); return implementation;
        }
        internal static BaseImplementation CreateNew(IFunctionBase function, FunctionTypeEnum type, bool addToRegistry = false)
        {
            return ConstructImplementation(function, type);
        }
        internal static IFunctionCompose CreateNew(IList<Tuple<double,double>> composedOrdinates, FunctionTypeEnum type, bool addToRegistry = false)
        {
            double[] Xs = composedOrdinates.Select(x => x.Item1).ToArray(), Ys = composedOrdinates.Select(y => y.Item2).ToArray();
            if ((int)type % 2 == 1) return (IFunctionCompose)CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), type, addToRegistry);
            else throw new NotImplementedException();
        }       
        private static BaseImplementation ConstructImplementation(IFunctionOrdinates function, FunctionTypeEnum type)
        {
            switch (type)
            {
                case FunctionTypeEnum.InflowOutflow:
                    return new InflowOutflow(function, function.Ordinates);
                case FunctionTypeEnum.OutflowFrequency:
                    return new OutflowFrequency(function);
                case FunctionTypeEnum.Rating:
                    return new Rating(function, function.Ordinates);
                case FunctionTypeEnum.ExteriorStageFrequency:
                    return new ExteriorStageFrequency(function);
                default:
                    return new UnUsed(function, type);
            }
        }
        private static BaseImplementation ConstructImplementation(IFunctionBase function, FunctionTypeEnum type)
        {
            switch (type)
            {
                case FunctionTypeEnum.InflowFrequency:
                    return new InflowFrequency(function);
                default:
                    return new UnUsed(function, type);
            }
        }
    }
}
