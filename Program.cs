using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using projectMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// SQLServer connection configuration
var connectionString = builder.Configuration.GetConnectionString("ConnectionSQL") ??
throw new InvalidOperationException("Error de conexión con la base de datos");
builder.Services.AddDbContext<Context>(options=>options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();

// authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/security/login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Run();
