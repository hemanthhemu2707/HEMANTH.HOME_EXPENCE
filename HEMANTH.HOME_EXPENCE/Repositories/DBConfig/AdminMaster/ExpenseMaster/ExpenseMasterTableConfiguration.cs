using IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.ExpenseMasterTableConfiguration
{
    public class ExpenseMasterTableConfiguration : IEntityTypeConfiguration<ExpenseMasterTableDBTypes>
    {
        private const string _tableName = "tblexpencemaster";
        private string _schemaName;

        public ExpenseMasterTableConfiguration(string schemaName) => _schemaName = schemaName;

        public void Configure(EntityTypeBuilder<ExpenseMasterTableDBTypes> builder)
        {
            builder.ToTable(_tableName, _schemaName);
            builder.HasKey(b => b.ExpenceID);
        }
    }
}
