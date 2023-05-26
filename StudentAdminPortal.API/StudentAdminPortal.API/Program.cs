using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.Repositories;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;

namespace StudentAdminPortal.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<StudentAdminContext>();
                // Perform any database initialization or migrations if needed
                // dbContext.Database.Migrate();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(app =>
                    {
                        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

                        app.UseCors("angularApplication");

                        app.UseHttpsRedirection();

                        app.UseStaticFiles(new StaticFileOptions
                        {
                            FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources")),
                            RequestPath = "/Resources"
                        });

                        app.UseRouting();

                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });

                        // Enable Swagger UI
                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Admin Portal API");
                        });
                    });
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<Program>());
                    services.AddControllers().AddNewtonsoftJson();
                    services.AddDbContext<StudentAdminContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("StudentAdminPortalDb")));
                    services.AddScoped<IStudentRepo, StudentRepo>();
                    services.AddScoped<IImageRepo, LocalStorageImageRepo>();
                    services.AddAutoMapper(typeof(Program).Assembly);
                    services.AddCors(options =>
                    {
                        options.AddPolicy("angularApplication", builder =>
                        {
                            builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .WithMethods("GET", "POST", "PUT", "DELETE")
                                .WithExposedHeaders("*");
                        });
                    });

                    // Configure Swagger
                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Admin Portal API", Version = "v1" });
                    });

                    services.AddEndpointsApiExplorer();
                });
    }
}
