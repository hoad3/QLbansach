using Microsoft.EntityFrameworkCore;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using QLbansach_tutorial.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(option => 
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddScoped<INhaxuatbanService, NhaxuatbanService>();
builder.Services.AddScoped<IChudeService, ChudeService>();
builder.Services.AddScoped<ISachService, SachService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();


// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Auth";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(7);
    });


builder.Services.AddScoped<CartCountFilter>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CartCountFilter>();
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("User"));
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IVnPayService, VnPayService>();
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

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
