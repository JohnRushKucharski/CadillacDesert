using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions.PercentDamageFunctions
{
    public sealed class DepthPercentDamage: IPercentDamageFunction, IValidateData
    {
        #region Fields
        private IFunctionBase _Function;
        #endregion

        #region Properties
        public bool IsValid { get; private set; }
        private IFunctionBase Function
        {
            get
            {
                return _Function;
            }
            set
            {
                _Function = value;
                IsValid = Validate();
            }
        }
        #endregion

        #region Constructor
        //internal DepthPercentDamage(string name, IFunctionBase depthPercentDamageFunction, bool addToRegistry = true)
        //{
        //    Function = depthPercentDamageFunction;
        //    if (addToRegistry == true) DepthPercentDamageRegistry.AddToRegistry(this);
        //    ReportValidationErrors();
        //}
        public DepthPercentDamage(IFunctionBase depthPercentDamageFunction)
        {
            Function = depthPercentDamageFunction;
            ReportValidationErrors();
        }
        #endregion

        //#region Methods
        //private IList<Tuple<double, double>> InvertOrdinates(IList<Tuple<double, double>> ordinates)
        //{
        //    return new List<Tuple<double,double>>(ordinates.Select(f => new Tuple<double, double>(f.Item2, f.Item1)).ToList());
        //}
        //#endregion

        #region Methods
        public IPercentDamageFunction Sample(double probability)
        {
            return new DepthPercentDamage(Function.Sample(probability));
        }
        public double ComputePercentDamage(double depth)
        {
            return _Function.GetYfromX(depth);
        }
        #endregion

        #region IValidateData Methods
        public bool Validate()
        {
            if (Function.ValidateFrequencyValues() == false) { ReportValidationErrors(); return false; }
            else return Function.IsValid;
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            List<string> messages = Function.ReportValidationErrors().ToList();
            if (Function.ValidateFrequencyValues() == false) { IsValid = false; messages.Add("The percent damage function is invalid because it contains ordinates outside of the valid domain of [0, 1]."); }
            return messages;
        }
        #endregion
    }
}
