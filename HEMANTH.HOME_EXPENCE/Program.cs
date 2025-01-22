using HEMANTH.HOME_EXPENCE;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.AdminMaster;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.Login;
using HEMANTH.HOME_EXPENCE.RepoInterfaces.UserMaster;
using HEMANTH.HOME_EXPENCE.Repositories.AdminMaster;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Category;
using HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family;
using HEMANTH.HOME_EXPENCE.Repositories.Login;
using HEMANTH.HOME_EXPENCE.Repositories.UserMaster;
using HEMANTH.HOME_EXPENCE.ServiceInterface.AdminMaster;
using HEMANTH.HOME_EXPENCE.ServiceInterface.Login;
using HEMANTH.HOME_EXPENCE.ServiceInterface.UserMaster;
using HEMANTH.HOME_EXPENCE.Services;
using HEMANTH.HOME_EXPENCE.Services.AdminMaster;
using HEMANTH.HOME_EXPENCE.Services.UserMaster;
using HEMANTH.HOME_EXPENSE.ServiceInterface;
using IIITS.EFCore.Repositories;
using IIITS.LMS.Repositories.GeneralTables.ExpenseDetailsTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes;
using IIITS.LMS.Repositories.GeneralTables.Login;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // Set session timeout
    options.Cookie.HttpOnly = true; // Protect the session cookie
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});
// Register DbContext properly with connection string
builder.Services.AddDbContext<LMSMasterServiceDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnString")));


// Register your services and repositories
builder.Services.AddScoped<ILoingService, LoginService>();
builder.Services.AddScoped<ILoginRepo, LoginRepo>();

builder.Services.AddScoped<IAdminMasterService, AdminMasterService>();
builder.Services.AddScoped<IAdminMasterRepo, AdminMasterRepo>();

builder.Services.AddScoped<IUserMasterService, UserMasterService>();
builder.Services.AddTransient<IUserMasterRepo, UserMasterRepo>();
builder.Services.AddTransient<LoginDBTypes>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<CategoryTableDBTypes>();
builder.Services.AddTransient<FamilyTableDBTypes>();
builder.Services.AddTransient<ExpenseMasterTableDBTypes>();
builder.Services.AddTransient<ExpenseDetailsTableDBTypes>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");
app.UseSession();

//app.UseMiddleware<SessionCheckMiddleware>();
app.Run();
