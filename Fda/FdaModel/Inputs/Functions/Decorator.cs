using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    public abstract class Decorator: IFunction //FunctionBase //Database.IEntity?
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
        public IFunction Function { get; protected set; }
        public abstract FunctionType Type { get; }
        public bool IsValid { get; protected set; }
        #endregion

        #region Constructor
        protected Decorator(IFunction function) { Function = function; }
        #endregion

        #region Methods
        public virtual bool Validate() { return Function.IsValid; }

        public virtual IEnumerable<string> ReportValidationErrors() { return Function.ReportValidationErrors(); }

        public virtual double GetXfromY(double y) { return Function.GetXfromY(y); }

        //public abstract IComputableFunction Compute(IFunction transformFunction);

        //public void Compose(ICondition condition)
        #endregion
    }
}