using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using System.Data.Entity.ModelConfiguration;


namespace Model.DataBase.Mapping
{
    public class OrdinatesFunctionMap: EntityTypeConfiguration<OrdinatesFunction> 
    {
        public OrdinatesFunctionMap()
        {
            HasKey(t => t.Id);

            Property(t => t.Type);
            Property(t => t.IsValid);
            
        }
    }
}
