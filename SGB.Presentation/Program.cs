using SGB.Presentation.Handlers;
using SGB.Presentation.Service;
using SGB.Presentation.Services.Base;

namespace SGB.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("SGBDatabase");

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IHttpService, HttpService>();

            builder.Services.AddScoped<IConfiguracionAppHandler, ConfiguracionAppHandler>();

            builder.Services.AddHttpClient<IConfiguracionHttpService, ConfiguracionHttpService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiConfig:Url"]);
            });


            var app = builder.Build();

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
            pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
