using Microsoft.EntityFrameworkCore;
using Practic;
using Practic.Data;
using Microsoft.AspNetCore.Identity;
using Practic.Middleware;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity.UI.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
        builder.Services.AddDbContext<PracticdataContext>(options => options.UseSqlite(connectionString));

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        

        // Configure the HTTP request pipeline.
        builder.Services.AddSession();
        builder.Services.AddRazorPages();
        //services.AddControllersWithViews();
        var app = builder.Build();
        
        app.UseStaticFiles();

        
        app.UseSession();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseHttpsRedirection();
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseDbInitializer();
        app.UseRouting();
        app.MapRazorPages();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();

        });

        app.Run();
    }
}