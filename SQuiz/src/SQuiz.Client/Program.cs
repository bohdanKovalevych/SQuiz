using Majorsoft.Blazor.Extensions.BrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SQuiz.Client;
using SQuiz.Client.Interfaces;
using SQuiz.Client.Services;
using SQuiz.Client.Services.JoinGameStrategies;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Game;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddTransient<IJoinGameStrategy<RegularGameOptionDto>, JoinRegularGameStrategy>();
builder.Services.AddTransient<IJoinGameStrategy<RealtimeGameOptionDto>, JoinRealtimeGameStrategy>();
builder.Services.AddTransient<IJoinGameProcessor, JoinGameProcessor>();
builder.Services.AddScoped<IInitGameService, InitGameService>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<ICurrentRealtimePlayerService, CurrentRealtimePlayerService>();
builder.Services.AddScoped<IRealtimeQuizHubPushReceiver, RealtimeQuizHubPushReceiver>();
builder.Services.AddScoped<IRealtimeQuizHubClient, RealtimeQuizHubClient>();
builder.Services.AddScoped<IManageRealtimeQuizHubPushReceiver, ManageRealtimeQuizHubPushReceiver>();
builder.Services.AddScoped<IManageRealtimeQuizHubClient, ManageRealtimeQuizHubClient>();
builder.Services.AddScoped<CrudDialogService>();
builder.Services.AddScoped<IClipboardService, ClipboardService>();
builder.Services.AddHttpClient("SQuiz.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddHttpClient<PublicClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddBrowserStorage();
// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SQuiz.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetSection("ServerApi")["Scopes"]);
});
builder.Services.AddMudServices();
builder.Services.AddShared();

await builder.Build().RunAsync();
