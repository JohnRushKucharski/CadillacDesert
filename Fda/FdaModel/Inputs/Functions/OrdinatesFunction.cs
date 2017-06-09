using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    internal sealed class OrdinatesFunction: IFunction //FunctionBase //Database.IEntity
    {
        #region Fields (without Auto Implemented Properties)
        private bool _IsValid;
        private Statistics.CurveIncreasing _Function;
        #endregion

        #region Properties     
        public bool IsValid
        {
            get
            {
                return Validate();
            }
            internal set
            {
                _IsValid = value;
            }
        }
        public Statistics.CurveIncreasing Function
        {
            get
            {
                return _Function;
            }
            private set
            {
                _Function = value;
                _IsValid = Validate();
            }
        }
        public FunctionType Type { get; }
        #endregion

        #region Constructors
        internal OrdinatesFunction(Statistics.CurveIncreasing function)
        {
            Function = function;
            IsValid = Validate();
            ReportValidationErrors();
        }
        #endregion

        #region Methods
        public bool Validate()
        {
            if (Function.Count < 2) return false;
            for (int i = 0; i < Function.Count - 1; i++)
            {
                if (Function.get_X(i + 1) == Function.get_X(i))
                {
                    Function.RemoveAt(i);
                    if (Function.Count < 2) return false;
                    else i--;
                }
            }
            if (Function.IsValid == false) return false;
            else return true;
        }

        public IEnumerable<string> ReportValidationErrors()
        {
            List<string> errors = new List<string>();
            if (Function.Count < 2)
            {
                IsValid = false;
                errors.Add("The domain and range (e.g. X and Y ordinates) must contain two or more equal length data pairs. The provided data does not; it will be stored in an unusable state and must be edited before it can be used in a compute.");
            }
            else
            {
                for (int i = 0; i < Function.Count - 1; i++)
                {
                    if (Function.get_X(i + 1) == Function.get_X(i))
                    {
                        errors.Add("A duplicate ordinate (" + Function.get_X(i) + "," + Function.get_Y(i) + ") was found. This ordinate has been removed.");
                        Function.RemoveAt(i);
                        if (Function.Count < 2)
                        {
                            IsValid = false;
                            errors.Add("One or more duplicate ordinates was removed and the provided data no longer contains two or more equal length pairs of data (as is required by the program); the function will be stored in an unusable state and must be edited before it can be used in a compute.");
                        }
                        else i--;
                    }
                }
            }
            if (Function.IsValid == false)
            {
                IsValid = false;
                errors.AddRange(Function.GetErrors());
            }
            if (errors.Count == 0)
            {
                errors.Add("No errors were found.");
            }
            return errors;
        }

        public double GetXfromY(double y)
        {
            if (Function.get_Y(0) >= y) return Function.get_X(0);
            if (Function.get_Y(Function.Count - 1) <= y) return Function.get_X(Function.Count - 1);

            int i = 0;
            while (Function.get_Y(i + 1) < y)
            {
                i++;
            }
            return Function.get_X(i) + (y - Function.get_Y(i)) / (Function.get_Y(i + 1) - Function.get_Y(i)) * (Function.get_X(i + 1) - Function.get_X(i));
        }
        #endregion
    }
}
