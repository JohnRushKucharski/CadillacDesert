using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using Model.Inputs.Functions.ComputationPoint;

namespace Model.Inputs.Conditions
{
    public sealed class Condition : ICondition
    {
        #region Fields
        private bool _IsValid;
        #endregion

        #region Properties
        public string Id
        {
            get
            {
                return new StringBuilder(Name).Append(Year).ToString();
            }
        }
        public int Year { get; }
        public string Name { get; }
        public IFunctionCompose EntryPoint { get; }
        public IList<IFunctionCompose> FrequencyFunctions { get; private set; }
        public IList<IFunctionTransform> TransformFunctions { get; }
        public IList<ComputePoint> ComputePoints { get; }
        public IList<Tuple<ComputePoint, double>> Metrics { get; private set; }
        public bool IsValid
        {
            get
            {
                return Validate();
            }
            private set
            {
                _IsValid = value;
            }
        }        
        #endregion

        #region Constructor
        public Condition(string name, int year, IFunctionCompose frequencyFunction, IList<IFunctionTransform> transformFunctions, IList<ComputePoint> computePoints)
        {
            Name = name;
            Year = year;
            EntryPoint = frequencyFunction;
            FrequencyFunctions = new List<IFunctionCompose>() { frequencyFunction };
            TransformFunctions = transformFunctions.OrderBy(i => i.Type).ToList();
            ComputePoints = (IList<ComputePoint>)computePoints.OrderBy(i => i.ComputePointFunction).ToList();
            Metrics = new List<Tuple<ComputePoint, double>>();
            ReportValidationErrors();
        }
        #endregion

        #region Methods
        //private IReadOnlyCollection<ComputationPointFunctionEnum> GetComputableList()
        //{
        //    bool hasErrors = false;
        //    IList<ComputationPointFunctionEnum> computableList = new List<ComputationPointFunctionEnum>() { EntryPoint.Type };
        //    foreach (IFunctionTransform function in TransformFunctions)
        //    {
        //        if (function.Type < EntryPoint.Type ||
        //            function.Type > Thresholds[Thresholds.Count - 1].ThresholdFunction) hasErrors = true;
        //        else computableList.Add(function.Type); 
        //    }
        //    if (hasErrors == true) ReportValidationErrors();
        //    return new ReadOnlyCollection<ComputationPointFunctionEnum>(computableList);
        //}
        
        #endregion

        public void TestCompute()
        {
            double[] testProbability = new double[] {0.0001, 0.9999 };
            throw new NotImplementedException();
        }
        public void Compute(int seed)
        {
            int j = 0, J = ComputePoints.Count;
            Random numberGenerator = new Random(seed);
            for (int i = 0; i < TransformFunctions.Count; i++)
            {
                FrequencyFunctions.Add(FrequencyFunctions[i].Compose(TransformFunctions[i], numberGenerator.NextDouble(), numberGenerator.NextDouble()));
                while (FrequencyFunctions[FrequencyFunctions.Count - 1].Type == ComputePoints[j].ComputePointFunction)
                {
                    if (ComputePoints[j].Unit < ComputePointUnitTypeEnum.ExpectedAnnualDamage)
                        Metrics.Add(new Tuple<ComputePoint, double>(ComputePoints[j], FrequencyFunctions[FrequencyFunctions.Count - 1].GetXFromY(ComputePoints[j].Value)));
                    else
                        Metrics.Add(new Tuple<ComputePoint, double>(ComputePoints[j], FrequencyFunctions[FrequencyFunctions.Count - 1].Integrate()));
                    if (j + 1 == J) return; else j++;
                }
            }
        }
        #region IValidate Members
        public bool Validate()
        {
            bool isValid = true;
            if (Year < 1849 || //Swamp Lands Act of 1849 was the first major US flood control act.
                Year > DateTime.Today.Year + 200)
            {
                IsValid = false;
                ReportAnalysisYearErrors();
            }
            if (ComputePoints.Count == 0)
            {
                isValid = false;
                ReportComputePointErrors();
            }
            foreach (IFunctionTransform function in TransformFunctions)
            {
                if (function.Type < EntryPoint.Type &&
                    function.Type > ComputePoints[ComputePoints.Count - 1].ComputePointFunction)
                {
                    ReportTransformFunctionErrors(); break;
                }
            }
            foreach (ComputePoint exitPoint in ComputePoints)
            {
                if ((int)exitPoint.ComputePointFunction < (int)EntryPoint.Type ||
                    (int)exitPoint.ComputePointFunction > (int)TransformFunctions[TransformFunctions.Count - 1].Type + 1)
                {
                    isValid = false;
                    ReportComputePointErrors(); break;
                }
            }
            return isValid;
        }

        public IEnumerable<string> ReportValidationErrors()
        {
            IsValid = true;
            StringBuilder messages = ReportAnalysisYearErrors()
                                    .Append(ReportTransformFunctionErrors())
                                    .Append(ReportComputePointErrors());

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
        private StringBuilder ReportAnalysisYearErrors()
        {
            StringBuilder analysisYearMessage = new StringBuilder();            
            if (Year < 1849 || //Swamp Lands Act of 1849 was the first major US flood control act.
                Year > DateTime.Today.Year + 200)
            {
                IsValid = false;
                return analysisYearMessage.Append("The condition year must be between 1849 and ")
                                          .Append(DateTime.Today.Year + 200)
                                          .Append(".");
            }
            else return analysisYearMessage;
        }
        private StringBuilder ReportTransformFunctionErrors()
        {
            StringBuilder transformFunctionMessages = new StringBuilder();
            if (ComputePoints.Count == 0)
            {
                IsValid = false;
                transformFunctionMessages.AppendLine("The condition can not be computed without a targeted performance metric or expected annual damages.");
            }
            foreach (IFunctionTransform function in TransformFunctions)
            {
                if (function.Type < EntryPoint.Type &&
                    function.Type > ComputePoints[ComputePoints.Count - 1].ComputePointFunction)
                {
                    TransformFunctions.Remove(function);
                    transformFunctionMessages.AppendLine("A ")
                                             .Append(function.Type)
                                             .Append(" transform function was removed from the compute sequence because it appears before the condition's computational entry point or after the condition's computational exit point.");
                }
            }
            return transformFunctionMessages;
        }
        private StringBuilder ReportComputePointErrors()
        {
            StringBuilder thresholdMessages = new StringBuilder();
            foreach (ComputePoint exitPoint in ComputePoints)
            {
                if ((int)exitPoint.ComputePointFunction < (int)EntryPoint.Type ||
                    (int)exitPoint.ComputePointFunction > (int)TransformFunctions[TransformFunctions.Count - 1].Type + 1)
                {
                    IsValid = false;
                    thresholdMessages.AppendLine("The ")
                                     .Append(exitPoint.ComputePointFunction)
                                     .Append(" metric can not be calculated because it preceeds the computational entry point (a ")
                                     .Append(EntryPoint.Type).Append(" function). ")
                                     .Append("or follows the last function that can be calculated in the compute sequence (a ")
                                     .Append(TransformFunctions[TransformFunctions.Count - 1].Type + 1).Append(" function).");
                }
            }
            return thresholdMessages;    
        }
        #endregion
    }
}
