
using FlowCare.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FlowCare.Application;
using FlowCare.Infrastructure.Data;
namespace FlowCare.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FlowcareDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"),
                    providerOptions => providerOptions.EnableRetryOnFailure()); 
            });
           
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("My API");
                    options.WithTheme(ScalarTheme.BluePlanet);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
