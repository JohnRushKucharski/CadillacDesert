using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using System.Data.Entity.ModelConfiguration;

namespace Model.DataBase.Mapping
{
    public class DecoratorMap: EntityTypeConfiguration<IEntityFunctionDecorator>
    {
        public DecoratorMap()
        {
            HasKey(t => t.Id);
            //HasRequired(t => t.Function).WithRequiredDependent(u => u.Id);

            Property(t => t.Type);
            Property(t => t.IsValid);

            ToTable("ComputationPointFunctions");
        }
    }
}
