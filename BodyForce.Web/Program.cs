using BodyForce;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BodyForceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Optional: Configure identity options, including password complexity and unique email requirement
    options.User.RequireUniqueEmail = true;  // Ensure that the email is unique in the system
})
.AddEntityFrameworkStores<BodyForceDbContext>()
.AddRoles<ApplicationRole>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = builder.Configuration["Application:LoginPath"];
    config.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Optional: Set a timeout for the session cookie
    config.SlidingExpiration = true;  // Optional: Refresh the expiration time if the user is active
    config.Cookie.IsEssential = true; // Ensures the cookie is necessary for the application to work
    config.Cookie.HttpOnly = true;  // Makes the cookie more secure by restricting access to JavaScript
    config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Onl
});

builder.Services.AddAutoMapper(typeof(MappingProfiles));

#region Services & Repositaries
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISmsService, TwilioSmsService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMemberShipService, MemberShipService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<DataSeeder>();
#endregion

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BodyForceDbContext>();
    dbContext.Database.Migrate();

    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedRolesAndAdminAsync();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LogIn}/{id?}"); // Set Account/SignUp as the default route



app.Run();
