using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions.ComputationPoint
{
    public static class ComputationPointFunctionFactory
    {
        #region CreateNew() Methods
        public static IFunctionCompose CreateNew(Statistics.LogPearsonIII logPearsonFrequencyFunction)
        {
            return new InflowFrequency(new FrequencyFunction(logPearsonFrequencyFunction));
        }
        public static IFunctionCompose CreateNew(FrequencyFunction logPearsonFrequencyFunction)
        {
            return new InflowFrequency(logPearsonFrequencyFunction);
        }
        /// <summary> Creates a new IFunctionCompose from a set of composed ordinates. Only intended for use by the IFunctionBase.Compose function. An exception is thrown if the inputs cannot be expressed as an IFunctionCompose computation point function. </summary>
        /// <param name="composedOrdinates"> The set of ordinates created by the IFunctionBase.Compose function. </param>
        /// <param name="type"> The type of computation point function that is being targeted for creation. An exception will be thrown if the target is not a frequency function type. </param>
        /// <returns> A new IFunctionCompose computation point function, if one can be created. Otherwise an exception is thrown. </returns>
        internal static IFunctionCompose CreateNew(IList<Tuple<double, double>> composedOrdinates, ComputationPointFunctionEnum type)
        {
            if ((int)type % 2 == 1) return ConstructImplementation(composedOrdinates, type);
            else throw new NotImplementedException();
        }
        /// <summary> Creates a new IFunctionTransform, provided that the input IFunctionBase and target computation point function type can be expressed as an IFunctionTransform. An exception is thrown if the inputs cannot be expressed as an IFunctionTransform computation point function. </summary>
        /// <param name="function"> An IFunctionBase that can be expressed as an IFunctionTransform. </param>
        /// <param name="ordinates"> The IFunctionBase ordinates property. </param>
        /// <param name="type"> The computation point functions target type. An exception will be thrown if the target is not a tranform function type. </param>
        /// <returns> A IFunctionTransform compuation point function, if one can be created. Otherwise an exception is thrown. </returns>
        public static IFunctionTransform CreateNew(Statistics.CurveIncreasing function, ComputationPointFunctionEnum type)
        {
            OrdinatesFunction ordinatesFunction = new OrdinatesFunction(function);
            if ((int)type % 2 == 0) return ConstructImplementation(ordinatesFunction, ordinatesFunction.Ordinates, type);
            else throw new NotImplementedException();
        }
        public static IFunctionTransform CreateNew(Statistics.UncertainCurveIncreasing function, ComputationPointFunctionEnum type)
        {
            UncertainOrdinatesFunction uncertainOrdinatesFunction = new UncertainOrdinatesFunction(function);
            if ((int)type % 2 == 0) return ConstructImplementation(uncertainOrdinatesFunction, uncertainOrdinatesFunction.Ordinates, type);
            else throw new NotImplementedException();
        }
        public static IFunctionTransform CreateNew(IFunctionBase function, IList<Tuple<double, double>> ordinates, ComputationPointFunctionEnum type)
        {
            if ((int)type % 2 == 0) return ConstructImplementation(function, function.GetOrdinates(), type);
            else throw new NotImplementedException();
        }
        internal static ComputationPointFunctionBase CreateNew(IFunctionBase function, ComputationPointFunctionEnum type)
        {
            return ConstructImplementation(function, type, true);
        }
        #endregion
           
        private static ComputationPointFunctionBase ConstructImplementation(IFunctionBase function, ComputationPointFunctionEnum type, bool constructCPFunctionBase = true)
        {
            switch (type)
            {
                case ComputationPointFunctionEnum.InflowOutflow:
                    return new InflowOutflow(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.OutflowFrequency:
                    return new OutflowFrequency(function);
                case ComputationPointFunctionEnum.Rating:
                    return new Rating(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.ExteriorStageFrequency:
                    return new ExteriorStageFrequency(function);
                case ComputationPointFunctionEnum.ExteriorInteriorStage:
                    return new ExteriorInteriorStage(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.InteriorStageFrequency:
                    return new InteriorStageFrequency(function);
                case ComputationPointFunctionEnum.InteriorStageDamage:
                    return new InteriorStageDamage(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.DamageFrequency:
                    return new DamageFrequency(function);
                default:
                    return new UnUsed(function, type);
            }
        }
        private static IFunctionCompose ConstructImplementation(IFunctionBase function, ComputationPointFunctionEnum type)
        {
            if ((int)type % 2 == 0) throw new NotImplementedException();
            switch (type)
            {
                case ComputationPointFunctionEnum.OutflowFrequency:
                    return new OutflowFrequency(function);
                case ComputationPointFunctionEnum.ExteriorStageFrequency:
                    return new ExteriorStageFrequency(function);
                case ComputationPointFunctionEnum.InteriorStageFrequency:
                    return new InteriorStageFrequency(function);
                case ComputationPointFunctionEnum.DamageFrequency:
                    return new DamageFrequency(function);
                default:
                    throw new NotImplementedException();
            }
        }
        private static IFunctionCompose ConstructImplementation(IList<Tuple<double, double>> composedOrdinates, ComputationPointFunctionEnum type)
        {
            IFunctionBase baseFunction = new OrdinatesFunction(new Statistics.CurveIncreasing(composedOrdinates.Select(x => x.Item1).ToList(), composedOrdinates.Select(y => y.Item2).ToList(), true, false));
            switch (type)
            {
                case ComputationPointFunctionEnum.OutflowFrequency:
                    return new OutflowFrequency(baseFunction);
                case ComputationPointFunctionEnum.ExteriorStageFrequency:
                    return new ExteriorStageFrequency(baseFunction);
                case ComputationPointFunctionEnum.InteriorStageFrequency:
                    return new InteriorStageFrequency(baseFunction);
                case ComputationPointFunctionEnum.DamageFrequency:
                    return new DamageFrequency(baseFunction);
                default:
                    throw new NotImplementedException();
            }
        }
        private static IFunctionTransform ConstructImplementation(IFunctionBase function, IList<Tuple<double, double>> ordinates, ComputationPointFunctionEnum type)
        {
            switch (type)
            {
                case ComputationPointFunctionEnum.InflowOutflow:
                    return new InflowOutflow(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.Rating:
                    return new Rating(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.ExteriorInteriorStage:
                    return new ExteriorInteriorStage(function, function.GetOrdinates());
                case ComputationPointFunctionEnum.InteriorStageDamage:
                    return new InteriorStageDamage(function, function.GetOrdinates());
                default:
                    throw new NotImplementedException();
            }
        }   
    }
}
