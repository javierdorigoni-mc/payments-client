using Microsoft.Extensions.Options;
using PaymentsClient.Domain;
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

        var nagApiSettings = new NagApiSettings();
        builder.Configuration.GetSection("NagApi").Bind(nagApiSettings);
        builder.Services.AddSingleton(Options.Create(nagApiSettings));
        
        builder.Services.AddDomainServices();
        builder.Services.AddNagApiHttpClientServices(nagApiSettings);
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