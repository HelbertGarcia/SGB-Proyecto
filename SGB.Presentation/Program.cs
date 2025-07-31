using SGB.Presentation.Services.Base;
using SGB.Presentation.Services;

namespace SGB.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient("ApiSGB", client =>
            {
                var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped<IPrestamoHttpService, PrestamoHttpService>();
            builder.Services.AddScoped<IPenalizacionHttpService, PenalizacionHttpService>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddLogging();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
