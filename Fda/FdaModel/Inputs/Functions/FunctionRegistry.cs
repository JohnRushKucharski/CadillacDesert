using System;
using System.Text;
using System.Collections.Generic;
using Model.Inputs.Functions.Implementations;

namespace Model.Inputs.Functions
{
    internal sealed class FunctionRegistry
    {
        #region Notes
        //Needs: Reading, Writing, Unit Tests, Add Multiple items (implment IColleciton or IList?)
        //1. This may help most if I don't use Entity.
        /* Entity Notes
        1. public int Id { get; private set; }
        2. public sealed class DecoratorList: Database.IEntityData
        3. A badly written Write method for testing...
        
        public void Write()
        {
            string dbPath = String.Format("Data Source={0};Version=3;", Project.Instance.GetFilePathWithoutExtension() + ".sqlite");
            var sqLiteConnection = new System.Data.SQLite.SQLiteConnection(dbPath);
            using (var context = new DataBase.DataContext(sqLiteConnection, false))
            {
                context.Set<DecoratorList>().Add(this);
                context.SaveChanges();
            }
        }
        */
        #endregion

        #region Fields and Properties
        private int NameCounter;
        public static FunctionRegistry Instance { get; private set; }
        public IList<string> NamedFunctions
        {
            get
            {
                return GetNames();
            }
        }
        public IList<Tuple<string, BaseImplementation>> CompleteList { get; }
        public IList<Tuple<string, IFunctionTransform>> TransformFunctions
        {
            get
            {
                return GetTransformFunctions();
            }
        }
        public IList<Tuple<string, IFunctionCompose>> FrequencyFunctions
        {
            get
            {
                return GetFrequencyFunctions();
            }
        }
        public IList<Tuple<string, BaseImplementation>> UnUsedFunctions
        {
            get
            {
                return GetUnUsableFunctions();
            }
        }
        #endregion

        #region Constructors
        private FunctionRegistry()
        {
            NameCounter = 0;
            CompleteList = new List<Tuple<string, BaseImplementation>>();
        }
        #endregion

        #region Methods
        internal static FunctionRegistry CreateNew()
        {
            if (Instance == null) Instance = new FunctionRegistry();
            return Instance;
        }
        internal static void AddToRegistry(string name, BaseImplementation function)
        {
            if (Instance == null) Instance = CreateNew();
            Instance.CompleteList.Add(new Tuple<string, BaseImplementation>(Instance.GetValidName(name), function));
        }
        internal static void AddToRegistry(BaseImplementation function)
        {
            if (Instance == null) Instance = CreateNew();
            Instance.CompleteList.Add(new Tuple<string, BaseImplementation>(Instance.CreateName(function), function));
        }        
        private string CreateName(BaseImplementation function)
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
        private List<Tuple<string, IFunctionTransform>> GetTransformFunctions()
        {
            List<Tuple<string, IFunctionTransform>> transformList = new List<Tuple<string, IFunctionTransform>>();
            foreach (var implementation in CompleteList)
            {
                if (implementation.Item2.Type != FunctionTypeEnum.NotSet &&
                   (int)implementation.Item2.Type % 2 == 0)
                {
                    transformList.Add(new Tuple<string, IFunctionTransform>(implementation.Item1, (IFunctionTransform)implementation.Item2));
                }
            }
            return transformList;
        }
        private List<Tuple<string, IFunctionCompose>> GetFrequencyFunctions()
        {
            List<Tuple<string, IFunctionCompose>> frequencyList = new List<Tuple<string, IFunctionCompose>>();
            foreach (var implementation in CompleteList)
            {
                if (!((int)implementation.Item2.Type % 2 == 0)) frequencyList.Add(new Tuple<string, IFunctionCompose>(implementation.Item1, (IFunctionCompose)implementation.Item2));
            }
            return frequencyList;
        }
        private List<Tuple<string, BaseImplementation>> GetUnUsableFunctions()
        {
            List<Tuple<string, BaseImplementation>> unUsableList = new List<Tuple<string, BaseImplementation>>();
            foreach (var implementation in CompleteList)
            {
                if (implementation.Item2.Type == FunctionTypeEnum.NotSet) unUsableList.Add(implementation);
            }
            return unUsableList;
        }
        private List<string> GetNames()
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
