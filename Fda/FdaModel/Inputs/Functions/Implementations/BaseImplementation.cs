using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions.Implementations
{
    public abstract class BaseImplementation 
    {
        #region Notes
        //1. Does the function type really need to be public.
        /*Entity Notes
        1. Entity may be easier if Function is an abstract FunctionBase: public abstract class Decorator: FunctionBase
        2. Database.IEntity exists to force an FDA set of standard Entity conventions. It would be best if it could be implemented at this abstract level: public abstract class Decorator: Database.IEntity
        3. public int FunctionBaseId { get; private set;}
        4. public virtual FunctionBase Function { get; set; } Instead of IFunction property, and : protected Decorator(IFunction function) { Function = function; FunctionBaseId = Function.Id; }
        5. OriginType plus FunctionBaseId could serve as a foreign key.
        */
        #endregion

        #region Properties              
        protected IFunctionBase Function;
        public bool IsValid { get; protected set; }
        public abstract FunctionTypeEnum Type { get; }
        #endregion

        #region Constructor
        protected BaseImplementation(IFunctionBase function) { Function = function; }
        #endregion

        #region Methods
        public abstract double GetXfromY(double y);
        public BaseImplementation Sample(Random randomNumberGenerator)
        {
            return FunctionFactory.CreateNew(Function.Sample(randomNumberGenerator), Type);
        }
        protected IList<Tuple<double, double>> Compose(List<Tuple<double, double>> transformOrdiantes)
        {
            return Function.Compose(transformOrdiantes);
        }
        #endregion

        #region IValidateData Methods
        public virtual bool Validate()
        {
            if (Function.IsValid == false) ReportValidationErrors();
            return Function.IsValid;
        }
        public virtual IEnumerable<string> ReportValidationErrors() { return Function.ReportValidationErrors(); }
        #endregion
    }
}