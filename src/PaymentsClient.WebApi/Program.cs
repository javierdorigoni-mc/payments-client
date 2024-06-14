using PaymentsClient.Domain;
using PaymentsClient.Domain.Models;
using PaymentsClient.Infrastructure.NagApiHttpClient;

namespace PaymentsClient.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.*.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>(reloadOnChange: true, optional: false);

        builder.Services.AddDomainServices();
        builder.Services.AddNagApiHttpClientServices(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}