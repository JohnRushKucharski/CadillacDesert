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
        public IDictionary<ComputePointUnitTypeEnum, double[]> MetricsRange { get; private set; }
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
            ComputePoints = computePoints.OrderBy(i => i.ComputePointFunction).ToList();
            Validate();
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
        private bool RunTestCompute(int nTests = 100)
        {
            InitializeMetricsRange();
            try
            {
                SetMetricRange(InnerCompute(GenerateTestProbabilities(TransformFunctions.Count * 2)), true);

                for (int i = 1; i < nTests; i++)
                {
                    SetMetricRange(InnerCompute(GenerateTestProbabilities(TransformFunctions.Count * 2)), false);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void InitializeMetricsRange()
        {
            MetricsRange = new Dictionary<ComputePointUnitTypeEnum, double[]>();
            for (int i = 0; i < ComputePoints.Count; i++) MetricsRange.Add(ComputePoints[i].Unit, new double[2]);
        }
        private double[] GenerateTestProbabilities(int nPs)
        {
            Random random = new Random();
            int N = TransformFunctions.Count * 2;
            double[] samplePs = new double[N], testPs = new double[2] { 0.0001, 0.9999 };
            for (int n = 0; n < N; n++)
            {
                if (random.NextDouble() < 0.5) samplePs[n] = testPs[0];
                else samplePs[n] = testPs[1];
            }
            return samplePs;
        }
        private void SetMetricRange(IDictionary<ComputePointUnitTypeEnum, double> result, bool firstPass)
        {
            if (firstPass == false)
            {
                foreach (var metric in result)
                {
                  
                    Math.Min(MetricsRange[metric.Key][0], metric.Value);
                    Math.Max(MetricsRange[metric.Key][0], metric.Value);
                }
            }
            else
            {
                foreach (var metric in result)
                {
                    MetricsRange[metric.Key][0] = metric.Value;
                    MetricsRange[metric.Key][1] = metric.Value;
                }    
            }
        }

        public IDictionary<ComputePointUnitTypeEnum, double> Compute(int seed)
        { 
            Random numberGenerator = new Random();
            int N = TransformFunctions.Count * 2; double[] randoms = new double[N];
            for (int i = 0; i < N; i++) randoms[i] = numberGenerator.NextDouble();
            return InnerCompute(randoms);
        }
        private IDictionary<ComputePointUnitTypeEnum, double> InnerCompute(double[] samplePs)
        {
            int j = 0, J = ComputePoints.Count, n;
            IDictionary<ComputePointUnitTypeEnum, double> metrics = new Dictionary<ComputePointUnitTypeEnum, double>();
            for (int i = 0; i < TransformFunctions.Count; i++)
            {
                n = i * 2;
                FrequencyFunctions.Add(FrequencyFunctions[i].Compose(TransformFunctions[i], samplePs[n], samplePs[n + 1]));
                while (FrequencyFunctions[FrequencyFunctions.Count - 1].Type == ComputePoints[j].ComputePointFunction)
                {
                    if (ComputePoints[j].Unit < ComputePointUnitTypeEnum.ExpectedAnnualDamage)
                        metrics.Add(ComputePoints[j].Unit, FrequencyFunctions[FrequencyFunctions.Count - 1].GetXFromY(ComputePoints[j].Value));
                    else
                        metrics.Add(ComputePoints[j].Unit, FrequencyFunctions[FrequencyFunctions.Count - 1].Integrate());
                    if (j + 1 == J) break;
                    else j++;
                }
            }
            return metrics;
        }
        #endregion

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
            if (isValid == true) if (!(RunTestCompute() == true)) isValid = false;
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
