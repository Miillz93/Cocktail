using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorPlaygroundWasm;
using MudBlazor.Services;
using BlazorPlaygroundWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/") });
builder.Services.AddHttpClient("cocktail", httpClient => {
    httpClient.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");
});
builder.Services.AddSingleton<ICocktailService, CocktailService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
