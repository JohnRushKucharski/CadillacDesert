using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    public sealed class Condition : ICondition
    {
        #region Fields
        private IFunction _EntryFunction;
        private IList<IFunction> _FrequencyFunctions;
        private IList<IFunctionTransform> _TransformFunctions;
        #endregion

        #region Properties
        public bool IsBaseline { get; }
        public int Year { get; private set; }
        public string Name { get; private set;}
        public string Id
        {
            get
            {
                return new StringBuilder(Year.ToString()).Append(Name).ToString();
            }
        }

        public IList<Tuple<IThreshold, double>> EAD { get; }
        public IList<Tuple<IThreshold, double>> AEP { get; }

        


        IEnumerable<IFunction> GetConditionFunctions
        {
            get
            {
                return _TransformFunctions.Concat(_FrequencyFunctions.Cast<IFunction>().OrderBy(i => i.Type));
            }
        }
        public IFunction GetCurrentTransformFunction
        {
            get
            {
                return _TransformFunctions[_TransformFunctions.Count];
            }
        }
        public IFunction GetCurrentFrequencyFunction
        {
            get
            {
                return _FrequencyFunctions[_FrequencyFunctions.Count - 1];
            }
        }
        public bool IsValid { get; set; }
        #endregion

        #region Constructor
        public Condition(string name, int year, IThreshold threshold)
        {
            Id = name + year + threshold.ThresholdFunction;
        }

        public Condition(IFunction entryFunction, IEnumerable<IFunctionTransform> transformFunctions, IThreshold threshold)
        {
            _Year = DateTime.Today.Year;
            _EntryFunction = entryFunction;
            _FrequencyFunctions = new List<IFunction>() { _EntryFunction };
            _TransformFunctions = transformFunctions.ToList();
            _Threshold = threshold;
            ReportValidationErrors();
        }
        
        public Condition(IFunction entryFunction, IEnumerable<IFunctionTransform> transformFunctions, IThreshold threshold, int year)
        {
            _Year = year;
            _EntryFunction = entryFunction;
            _FrequencyFunctions = new List<IFunction>() { _EntryFunction };
            _TransformFunctions = transformFunctions.ToList();
            _Threshold = threshold;
            ReportValidationErrors();
        }
        #endregion

        public void ComputeRealization()
        {
            _TransformFunctions = _TransformFunctions.OrderBy(i => i.Type).ToList();

        }

        #region IValidate Members
        public bool Validate()
        {
            int currentType, entry = (int)_EntryFunction.Type, threshold = (int)_Threshold.ThresholdFunction;
      
            _TransformFunctions = _TransformFunctions.OrderBy(i => i.Type).ToList();
            _FrequencyFunctions = _FrequencyFunctions.OrderBy(i => i.Type).ToList();            
            for (int i = 0; i < _TransformFunctions.Count; i++)
            {
                currentType = (int)_TransformFunctions[i].Type;
                if (currentType < entry) { ReportValidationErrors(); return IsValid; }
                if (currentType > threshold) { ReportValidationErrors(); return IsValid; }
                if (currentType % 2 != 0) return false;
            }
            //Swamp Lands Act of 1849 was the first major US flood control act.
            if (_Year < 1849 || _Year > DateTime.Today.Year + 150) return false;
            if (threshold < entry) return false;
            return true;
        }

        public IEnumerable<string> ReportValidationErrors()
        {
            IsValid = true;
            StringBuilder messages = ReportAnalysisYearErrors()
                                    .Append(ReportFrequencyFunctionErrors())
                                    .Append(ReportTransformFunctionErrors())
                                    .Append(ReportThresholdErrors());

            IList<string> report = new List<string>();
            char[] delimiter = Environment.NewLine.ToCharArray();
            string[] lines = messages.ToString()
                                     .Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                report.Add(line);
            }
            return report;
        }

        public StringBuilder ReportAnalysisYearErrors()
        {
            StringBuilder analysisYearMessage = new StringBuilder();
            if (_Year < 1849 || _Year > DateTime.Today.Year + 150)
            {
                IsValid = false;
                //Swamp Lands Act of 1849 was the first major US flood control act.
                return analysisYearMessage.Append("The condition year must be between 1849 and ").Append(DateTime.Today.Year + 100).Append(".");
            }
            else return analysisYearMessage;
        }

        public StringBuilder ReportFrequencyFunctionErrors()
        {
            StringBuilder frequencyMessage = new StringBuilder();
            int frequencyMessageCount = 0, entry = (int)_EntryFunction.Type, currentType;
            
            _FrequencyFunctions = _FrequencyFunctions.OrderBy(i => i.Type).ToList();
            for (int i = 0; i < _FrequencyFunctions.Count; i++)
            {
                currentType = (int)_FrequencyFunctions[i].Type;
                if (currentType < entry)
                {
                    if (frequencyMessageCount == 0) frequencyMessage.Append("The following frequency functions were found before the conditions computational entry point (and were discarded): ").Append(_FrequencyFunctions[i].Type);
                    else frequencyMessage.Append(", ").Append(_FrequencyFunctions[i].Type);
                    _FrequencyFunctions.RemoveAt(i);
                    i--;
                }
            }
            if (frequencyMessageCount == 0) return frequencyMessage;
            else return frequencyMessage.Append(". This may indicate that the original user specified entry point was changed, after a compute occured.");
        }

        public StringBuilder ReportTransformFunctionErrors()
        {
            int preEntryMessageCount = 0, postThresholdMessageCount = 0;
            StringBuilder transformMessage = new StringBuilder(), preEntryMessage = new StringBuilder(), postThresholdMessage = new StringBuilder();
            int currentType, entry = (int)_EntryFunction.Type, threshold = (int)_Threshold.ThresholdFunction;

            _TransformFunctions = _TransformFunctions.OrderBy(i => i.Type).ToList();
            for (int i = 0; i < _TransformFunctions.Count; i++)
            {
                currentType = (int)_TransformFunctions[i].Type;
                if (currentType < entry)
                {
                    if (preEntryMessageCount == 0) preEntryMessage.Append("The following functions occur before the entry point (e.g. frequency function) in the compute sequence (and were discarded): ").Append(_TransformFunctions[i].Type); 
                    else preEntryMessage.Append(", " + _TransformFunctions[i].Type);
                    _TransformFunctions.RemoveAt(i);
                    i--;
                }
                if (currentType > threshold)
                {
                    if (postThresholdMessageCount == 0) postThresholdMessage.Append("The following functions are not required to compute project performance (and will only be used in the computation of EAD): ").Append(_TransformFunctions[i].Type);
                    else postThresholdMessage.Append(", ").Append(_TransformFunctions[i].Type);
                }
                if (currentType % 2 != 0) IsValid = false;
            }
            if (preEntryMessageCount + postThresholdMessageCount == 0) return transformMessage;
            if (preEntryMessageCount == 0) return postThresholdMessage;
            if (postThresholdMessageCount == 0) return preEntryMessage;
            else return transformMessage.AppendLine(preEntryMessage.ToString()).AppendLine(postThresholdMessage.ToString());
        }

        public StringBuilder ReportThresholdErrors()
        {
            if ((int)_Threshold.ThresholdFunction < (int)_EntryFunction.Type)
            {
                IsValid = false;
                return new StringBuilder("The entry point for the compute, in this case a ").Append(_EntryFunction.Type).Append(" function, cannot preceed the threshold for performance calculations, in this case a ").Append(_Threshold.ThresholdFunction).Append(" function.");
            }
            else return new StringBuilder();    
        }
        #endregion
    }
}
