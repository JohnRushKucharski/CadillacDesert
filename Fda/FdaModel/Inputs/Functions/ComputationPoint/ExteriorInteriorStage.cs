using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;

namespace Model.Inputs.Functions.ComputationPoint
{
    public sealed class ExteriorInteriorStage: ComputationPointFunctionBase, IFunctionTransform
    {
        #region Properties
        public override ComputationPointFunctionEnum Type { get; }
        public IList<Tuple<double, double>> Ordinates { get; }
        #endregion

        #region Constructor
        internal ExteriorInteriorStage(IFunctionBase function, IList<Tuple<double, double>> ordinates): base(function)
        {
            Ordinates = ordinates;
            Type = ComputationPointFunctionEnum.ExteriorInteriorStage;
            IsValid = Validate();
        }
        #endregion

        #region IFunctionTransform Methods
        public IFunctionTransform Sample(double probability)
        {
            return ComputationPointFunctionFactory.CreateNew(Function.Sample(probability), Ordinates, Type);
        }
        #endregion

        #region IValidateData Methods
        public override bool Validate()
        {
            if (Function.IsValid == false) { ReportValidationErrors(); return false; }
            for (int i = 0; i < Ordinates.Count; i++)
            {
                if (Ordinates[i].Item1 < Ordinates[i].Item2) { ReportValidationErrors(); return false; }
            }
            return true;
        }
        public override IEnumerable<string> ReportValidationErrors()
        {
            List<string> messages = new List<string>();
            StringBuilder exteriorInteriorMessages = new StringBuilder();
            for (int i = 0; i < Ordinates.Count; i++)
            {
                if (Ordinates[i].Item1 < Ordinates[i].Item2)
                {
                    IsValid = false;
                    messages.Add(exteriorInteriorMessages
                                            .AppendLine("The interior(e.g.land side) water surface elevation must less than or equal to the exterior(e.g.river side) water surface elevation.At the exterior stage ordinate: ")
                                            .Append(Ordinates[i].Item1)
                                            .Append(" the interior stage is listed at: ")
                                            .Append(Ordinates[i].Item2)
                                            .Append(", causing an error.").ToString()
                                            );
                }
            }
            if (Function.IsValid == false)
            {
                IsValid = false;
                messages.AddRange(Function.ReportValidationErrors());
            }
            return messages;
        }
        #endregion
    }
}
