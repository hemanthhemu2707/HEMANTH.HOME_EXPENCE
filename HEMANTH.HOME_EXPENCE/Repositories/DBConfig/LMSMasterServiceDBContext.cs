using HEMANTH.HOME_EXPENCE.Repositories.AdminMaster;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.ExpenseMasterTableConfiguration;
using IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.EntityFrameworkCore;
using IIITS.LMS.Repositories.GeneralTables.ExpenseDetailsTableDBTypes;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.ExpenseDetailsTableConfiguration;

namespace IIITS.EFCore.Repositories
{
    public class LMSMasterServiceDBContext : DbContext
	{
		private IConfiguration _configuration;
		private string _connectionString;
		private string _connectionStringNpg;
		private string _connectionStringWorkFlow;

		public LMSMasterServiceDBContext(IConfiguration config)
		{
		
				_configuration=config ;
            _connectionString = _configuration["ConnectionStrings:SqlServerConnString"];


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            int commandTimeout = int.Parse(_configuration.GetSection("APIDBContext:CommandTimeout").Value);
            optionsBuilder.UseSqlServer(_connectionString, options => options.CommandTimeout(commandTimeout))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string schemaName = _configuration["APIDBContext:SchemaName"];

            modelBuilder.ApplyConfiguration(new LoginConfiguration(schemaName));
            modelBuilder.ApplyConfiguration(new CategoryTablConfiguration(schemaName));
            modelBuilder.ApplyConfiguration(new FamilyConfiguration(schemaName));
            modelBuilder.ApplyConfiguration(new ExpenseMasterTableConfiguration(schemaName));
            modelBuilder.ApplyConfiguration(new ExpenseDetailsTableConfiguration(schemaName));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LoginDBTypes> LoginDetails { get; set; }
        public DbSet<CategoryTableDBTypes> CategoryTable { get; set; }
        public DbSet<ExpenseMasterTableDBTypes> ExpMaster { get; set; }
        public DbSet<ExpenseDetailsTableDBTypes> ExpDetails { get; set; }
        public DbSet<FamilyTableDBTypes> FamilyTbl { get; set; }
    }
}