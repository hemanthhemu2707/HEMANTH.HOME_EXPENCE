using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIITS.LMS.Repositories.GeneralTables.Login
{
    public class LoginConfiguration : IEntityTypeConfiguration<LoginDBTypes>
    {
        private const string _tableName = "tbluser";
        private string _schemaName;

        public LoginConfiguration(string schemaName) => _schemaName = schemaName;

        public void Configure(EntityTypeBuilder<LoginDBTypes> builder)
        {
            builder.ToTable(_tableName, _schemaName);
            builder.HasKey(b => b.UserId);
        }
    }
}
