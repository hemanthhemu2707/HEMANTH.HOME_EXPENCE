using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Category
{
    public class CategoryTablConfiguration : IEntityTypeConfiguration<CategoryTableDBTypes>
    {
        private const string _tableName = "tblexpencecategory";
        private string _schemaName;

        public CategoryTablConfiguration(string schemaName) => _schemaName = schemaName;
        public void Configure(EntityTypeBuilder<CategoryTableDBTypes> builder)
        {
            builder.ToTable(_tableName, _schemaName);
            builder.HasKey(b => b.CategoryId);
        }
    }
}
