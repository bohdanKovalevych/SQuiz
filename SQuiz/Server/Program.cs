using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using SQuiz.Application;
using SQuiz.Application.Interfaces;
using SQuiz.Application.Services;
using SQuiz.Identity;
using SQuiz.Infrastructure;
using SQuiz.Server.Application.Security.Authorization.Policies;
using SQuiz.Server.Application.Security.Authorization.Requirements;
using SQuiz.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(conf => PolicySettings.SetPolicySettings(conf));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityByConfiguration(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddShared();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, QuizAuthorRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, QuizModeratorRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PlayerInGameRequirementHandler>();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
