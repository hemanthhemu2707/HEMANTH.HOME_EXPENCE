using IIITS.LMS.Repositories.GeneralTables.ExpenseDetailsTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.ExpenseDetailsTableConfiguration
{
    public class ExpenseDetailsTableConfiguration : IEntityTypeConfiguration<ExpenseDetailsTableDBTypes>
    {
        private const string _tableName = "tblexpence_details";
        private string _schemaName;

        public ExpenseDetailsTableConfiguration(string schemaName) => _schemaName = schemaName;

        public void Configure(EntityTypeBuilder<ExpenseDetailsTableDBTypes> builder)
        {
            builder.ToTable(_tableName, _schemaName);
            builder.HasKey(b => b.ExpenceDetailsID);
        }
    }
}
