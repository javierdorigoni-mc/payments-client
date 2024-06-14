using PaymentsClient.Domain;
using PaymentsClient.Infrastructure.NagApiHttpClient;
using PaymentsClient.WebUi.Components;

namespace PaymentsClient.WebUi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();
        
        builder.Services.AddDomainServices();
        builder.Services.AddNagApiHttpClientServices(builder.Configuration);
        builder.Services.AddRazorComponents()
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