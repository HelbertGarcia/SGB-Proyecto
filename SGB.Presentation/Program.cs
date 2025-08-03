using SGB.Presentation.Services.Base;
using SGB.Presentation.Services;
using SGB.Presentation.Endpoints.EndpointsPrestamo;
using SGB.Presentation.Endpoints.EndpointsPenalizacion;



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


            // Servicios base y HTTP
            builder.Services.AddScoped<IHttpService, HttpService>();

        
            builder.Services.AddScoped<IPrestamoHttpService, PrestamoHttpService>();
            builder.Services.AddScoped<IPenalizacionHttpService, PenalizacionHttpService>();

            // Endpoints personalizados
            builder.Services.AddScoped<IPrestamoEndpoints, PrestamoEndpoints>();
            builder.Services.AddScoped<IPenalizacionEndpoints, PenalizacionEndpoints>();



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
