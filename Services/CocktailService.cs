using System.Text.Json;
using System.Net.Http;
using BlazorPlaygroundWasm.Entities.Cocktail;

namespace BlazorPlaygroundWasm.Services;

internal interface ICocktailService{
    Task<List<Drink>> GetCocktails(string inputDrinkItem);
    Task<Drink> GetCocktail(string _id);
    Task<Ingredient> GetIngredientInfos(string _ingredients);
}



public class CocktailService :  ICocktailService {

    public string ? InputDrinkItem {get; set;}
    private string ? Ingredient {get;set;}
    private Ingredient? DrinkIngredient {get;set;}
    private List<Drink>? Drinks {get; set;}
    ILogger<CocktailService> Ilogger {get;set;}

    private readonly IHttpClientFactory _httpClientFactory;
    
    // private static readonly HttpClient Http = new(new SocketsHttpHandler
    // {
    //     PooledConnectionLifetime = TimeSpan.FromMinutes(1),
    //     PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
    //     MaxConnectionsPerServer = 10
    // }) 
    private static readonly HttpClient Http = new() 
    {
        BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/"),
        
    };

    // public CocktailService(IHttpClientFactory httpClientFactory){
    public CocktailService(IHttpClientFactory httpClientFactory){
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }
    
    /*Fetch Cocktail List*/
     public async Task<List<Drink>> GetCocktails(string inputDrinkItem){
        string CheckSearchApi="", SearchDrinkApi="";

        //var http = _httpClientFactory.CreateClient("cocktail");

        if(String.IsNullOrWhiteSpace(inputDrinkItem)) CheckSearchApi = "search.php?f=m";

        CheckSearchApi = (inputDrinkItem.Length == 1 ) ? SearchDrinkApi = $"search.php?f={inputDrinkItem}": SearchDrinkApi= $"search.php?s={inputDrinkItem}" ;


        try{
            //v1
            // Stream stream = await Http.GetStreamAsync(CheckSearchApi);
            // StreamReader reader = new StreamReader(stream);
            // string jsonString = reader.ReadToEnd();
            // var cocktailsValues = JsonSerializer.Deserialize<DrinkRoot>(jsonString);

            //v2
            // var response = await Http.GetAsync(CheckSearchApi);
            // var content = response.Content.ReadAsStringAsync();
            // var cocktailsValues = JsonSerializer.Deserialize<DrinkRoot>(content.Result);
            // Ilogger.LogInformation(content.Result);
            // System.Console.WriteLine(content.Result);
            // Drinks = cocktailsValues?.drinks;

            //v3
            var result = await Http.GetStringAsync(CheckSearchApi).ConfigureAwait(false);
            var Drinks = JsonSerializer.Deserialize<List<Drink>>(result);

            return Drinks; 
        }
        catch (Exception ex)
        {
            throw;
        }

    }
    
    /*Get Cocktail by id*/
    public async Task<Drink> GetCocktail(string _id){
        Drink drink = new();
        if(String.IsNullOrEmpty(_id)) throw new ArgumentNullException("_id is null");
        Stream stream = await Http.GetStreamAsync($"lookup.php?i={_id}");
        string reader = new StreamReader(stream).ReadToEnd();
        var cocktails = System.Text.Json.JsonSerializer.Deserialize<DrinkRoot>(reader);
        drink = cocktails.drinks.First(); 
        return drink;
    }
    
    /*Get Cocktail Ingredients*/
    public async Task<Ingredient> GetIngredientInfos(string _ingredients){   
        if(string.IsNullOrEmpty(_ingredients)) throw new ArgumentNullException();
        
        Ingredient = $"search.php?i={_ingredients}";

        Stream stream = await Http.GetStreamAsync(Ingredient);
        string reader = new StreamReader(stream).ReadToEnd();
        var _ingredientValues = System.Text.Json.JsonSerializer.Deserialize<DrinkRoot>(reader);

        if(_ingredientValues != null) DrinkIngredient = _ingredientValues.ingredients.First();
        return DrinkIngredient;
    }
    
}




