using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MOTM.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MOTMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MOTMContext")));

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MOTMContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Profile/Login";
    options.AccessDeniedPath = "/Profile/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MOTMContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);

    var RoleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var UserManager = services.GetRequiredService<UserManager<AppUser>>();
    string[] roleNames = { "Customer", "Admin" };
    foreach (var roleName in roleNames)
    {
        var roleExists = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    if (await UserManager.FindByNameAsync("Admin@MOTM.co.uk") == null)
    {
        var Admin = new AppUser { UserName = "Admin@MOTM.co.uk", Email = "Admin@MOTM.co.uk" };
        await UserManager.CreateAsync(Admin);
        await UserManager.AddPasswordAsync(Admin, "P@$$w0rd");
        await UserManager.AddToRoleAsync(Admin, "Admin");
        Customer Customer = new Customer
        {
            Id = Admin.Id,
            EMail = Admin.Email,
            FirstName = "Masseur",
            LastName = "OnTheMove",
            PhoneNumber = "+440123456789",
            FirstAddressLine = "MOTM",
            SecondAddressLine = "Heath Business Park",
            PostTown = "RUNCORN",
            PostCode = "WA6 4QZ"
        };
        context.Customers.Add(Customer);
        await context.SaveChangesAsync();
    };
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();