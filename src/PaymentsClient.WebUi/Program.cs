using Microsoft.Extensions.Options;
using PaymentsClient.Domain;
using PaymentsClient.Infrastructure.NagApiHttpClient;
using PaymentsClient.WebUi.Components;

namespace PaymentsClient.WebUi;

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

        builder.Services.AddSingleton<SessionContext>();
        
        builder.Services.AddDomainServices();
        builder.Services.AddNagApiHttpClientServices(nagApiSettings);
        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app
            .MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}