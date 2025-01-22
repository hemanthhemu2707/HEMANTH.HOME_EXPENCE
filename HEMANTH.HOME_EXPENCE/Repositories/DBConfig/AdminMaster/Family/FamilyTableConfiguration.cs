using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family
{
    public class FamilyConfiguration : IEntityTypeConfiguration<FamilyTableDBTypes>
    {
        private const string _tableName = "tblfamily";
        private string _schemaName;

        public FamilyConfiguration(string schemaName) => _schemaName = schemaName;

        public void Configure(EntityTypeBuilder<FamilyTableDBTypes> builder)
        {
            builder.ToTable(_tableName, _schemaName);
            builder.HasKey(b => b.FamilyID);
        }
    }
}
