using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;

namespace Model.Inputs.Functions.Implementations
{
    internal sealed class ExteriorInteriorStage: BaseImplementation, IFunctionTransform
    {
        #region Properties
        public override FunctionTypeEnum Type { get; }
        public IList<Tuple<double, double>> Ordinates { get; }
        #endregion

        #region Constructor
        internal ExteriorInteriorStage(IFunctionBase function, IList<Tuple<double, double>> ordinates): base(function)
        {
            Ordinates = ordinates;
            IsValid = Validate();
            Type = FunctionTypeEnum.ExteriorInteriorStage;
        }
        #endregion

        #region BaseImplementation Methods
        public override double GetXfromY(double y)
        {
            return Function.GetXfromY(y);
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
            StringBuilder exteriorInteriorMessages = new StringBuilder();
            List<string> messages = (List<string>)Function.ReportValidationErrors();
            for (int i = 0; i < Ordinates.Count; i++)
            {
                if (Ordinates[i].Item1 < Ordinates[i].Item2) messages.Add(exteriorInteriorMessages.Append("The interior (e.g. land side) water surface elevation must less than or equal to the exterior (e.g. river side) water surface elevation. At the exterior stage ordinate: ").Append(Ordinates[i].Item1).Append(" the interior stage is listed at: ").Append(Ordinates[i].Item2).Append(", causing an error.").ToString());
            }
            return messages;
        }
        #endregion


    }
}
