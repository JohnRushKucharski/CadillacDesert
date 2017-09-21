using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;

namespace Model.Inputs.Functions.PercentDamageFunctions
{
    class DepthPercentDamageRegistry: IFunctionRegistry
    {
        #region Fields and Properties
        private int NameCounter;
        public static DepthPercentDamageRegistry Instance { get; private set; }
        public IReadOnlyCollection<string> NamedFunctions
        {
            get
            {
                return GetNames();
            }
        }
        public IList<Tuple<string, DepthPercentDamage>> CompleteList { get; }
        #endregion

        #region Constructors
        private DepthPercentDamageRegistry()
        {
            NameCounter = 0;
            CompleteList = new List<Tuple<string, DepthPercentDamage>>();
        }
        #endregion

        #region Methods
        internal static DepthPercentDamageRegistry CreateNew()
        {
            if (Instance == null) Instance = new DepthPercentDamageRegistry();
            return Instance;
        }
        internal static void AddToRegistry(string name, DepthPercentDamage function)
        {
            if (Instance == null) Instance = CreateNew();
            Instance.CompleteList.Add(new Tuple<string, DepthPercentDamage>(Instance.GetValidName(name), function));
        }
        internal static void AddToRegistry(DepthPercentDamage function)
        {
            if (Instance == null) Instance = CreateNew();
            Instance.CompleteList.Add(new Tuple<string, DepthPercentDamage>(Instance.CreateName(function), function));
        }
        private string CreateName(DepthPercentDamage function)
        {
            Instance.NameCounter++;
            return new StringBuilder(function.GetType().ToString()).Append(Instance.NameCounter).ToString();
        }
        private string GetValidName(string name)
        {
            foreach (var item in CompleteList)
            {
                if (name == item.Item1)
                {
                    Instance.NameCounter++;
                    return new StringBuilder(name).Append(Instance.NameCounter).ToString();
                }
            }
            return name;
        }
        private IReadOnlyCollection<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (var implementation in CompleteList)
            {
                names.Add(implementation.Item1);
            }
            return names;
        }
        #endregion
    }
}
