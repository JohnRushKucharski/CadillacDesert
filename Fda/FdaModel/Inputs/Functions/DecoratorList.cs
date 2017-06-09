using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    public sealed class DecoratorList
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

        #region Properties
        public static DecoratorList Instance { get; private set; }
        internal List<Decorator> Functions { get; private set; }
        #endregion

        #region Constructors
        private DecoratorList() { Functions = new List<Decorator>(); }
        #endregion

        #region Methods
        public static DecoratorList CreateNew()
        {
            if (Instance == null) Instance = new DecoratorList();
            return Instance;
        }

        internal static void Add(Decorator item)
        {
            if (Instance == null) Instance = CreateNew();
            Instance.Functions.Add(item);
        }
        #endregion
    }
}
