using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SaviaOro.WEB;
using SaviaOro.WEB.Auth;
using SaviaOro.WEB.Repositories;
using Blazored.Modal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//TODO:
//PROD
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://api.lousoftware.eu/") });

//TEST
//builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7223/") });

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddBlazoredModal();

await builder.Build().RunAsync();
