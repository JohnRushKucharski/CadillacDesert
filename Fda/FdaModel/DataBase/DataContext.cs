namespace Model.DataBase
{
    using System;
    using System.Data.Entity;
    using Inputs.Functions;
    using System.Linq;

    public class DataContext : DbContext
    {
        //public DbSet<Project> Projects { get; set; }
        //public DbSet<FunctionBase> Functions { get; set; }
        public DbSet<OutflowFrequencyDecorator> f { get; set; }

        public DataContext(string nameOrConnectionString): base(nameOrConnectionString)
        {
            //Configure();
        } 

        public DataContext(System.Data.Common.DbConnection connection, bool contextOwnsConnection): base(connection, contextOwnsConnection)
        {
            //Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Project>();
            //modelBuilder.Entity<Decorator>().HasRequired(t=>t.Function);
            //modelBuilder.Entity<FunctionBase>();
            modelBuilder.Entity<OutflowFrequencyDecorator>().HasRequired(t => t.Function);
            //

            
            var sqLiteConnectionInitializer = new SQLite.CodeFirst.SqliteCreateDatabaseIfNotExists<DataContext>(modelBuilder);
            Database.SetInitializer(sqLiteConnectionInitializer);
        }
    }
}