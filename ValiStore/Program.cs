using Microsoft.EntityFrameworkCore;
using ValiStore.Models;
using ValiStore.Repository;

var builder = WebApplication.CreateBuilder(args);
//Console.WriteLine($"Chuỗi Kết Nối: {builder}");
// Add services to the container.
builder.Services.AddControllersWithViews();

//var connectionString = builder.Configuration.GetConnectionString("QLVaLi");

builder.Services.AddDbContext<QLBanVaLiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("QLVaLi")));

builder.Services.AddScoped<ILoaiSpRepository, LoaiSpRepository>();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
app.UseSession();
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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
