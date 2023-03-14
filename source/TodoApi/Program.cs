using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using TodoApi.Endpoints;
using TodoApi.Infrastructure;
using TodoApi.Infrastructure.Common;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
            StaticLogger.EnsureInitialized();
            Log.Information("Starting the server.");
            try
            {

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(setup =>
                {
                    setup.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Description = "TodoApi for testing QA candidates",
                        Title = "TodoApi",
                        Version = "v1",
                        Contact = new OpenApiContact() { Name = "IKT" }
                    });
                    setup.EnableAnnotations();
                    setup.CustomSchemaIds(type => type.FullName);
                });

                builder.Services.AddInfrastructure(builder.Configuration);

                var app = builder.Build();

                app.UseInfrastructure(builder.Configuration);

                app.MapTodoEndpoints();
                app.MapTodoItemEndpoints();
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1");
                    c.DefaultModelRendering(ModelRendering.Example);
                    c.DefaultModelsExpandDepth(-1);
                    c.DocExpansion(DocExpansion.None);
                });

                app.UseHttpsRedirection();

                using (var scope = app.Services.CreateScope())
                {
                    scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>().InitializeDatabasesAsync().Wait();
                }

                app.Run();
            }
            catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
            {
                StaticLogger.EnsureInitialized();
                Log.Fatal(ex, "Unhandled exception");
            }
            finally
            {
                StaticLogger.EnsureInitialized();
                Log.Information("Shutting down the server.");
                Log.CloseAndFlush();
            }
        }

    }
}