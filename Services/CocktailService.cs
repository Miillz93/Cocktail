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
    private Ingredient DrinkIngredient {get;set;} = new Ingredient();

    private List<Drink>? Drinks {get; set;} 
    private readonly HttpClient Http = new HttpClient();
     private string ? SearchDrinkApi= default; 
     private string ? CheckSearchApi = default;


    /*Fetch Cocktail List*/
     public async Task<List<Drink>> GetCocktails(string inputDrinkItem){
        
        if(String.IsNullOrWhiteSpace(inputDrinkItem)){
            CheckSearchApi = "search.php?f=a";
        }else{
            CheckSearchApi = (inputDrinkItem.Length == 1 ) ? SearchDrinkApi = $"search.php?f={inputDrinkItem}": SearchDrinkApi= $"search.php?s={inputDrinkItem}" ;
        }
        Stream stream = await Http.GetStreamAsync(DrinkRoot._urlBase+CheckSearchApi);
        StreamReader reader = new StreamReader(stream);
        string jsonString = reader.ReadToEnd();
        var cocktailsValues = System.Text.Json.JsonSerializer.Deserialize<DrinkRoot>(jsonString);
        Drinks = cocktailsValues?.drinks;

        return Drinks;
    }
    
    /*Get Cocktail by id*/
    public async Task<Drink> GetCocktail(string _id){
        Drink drink = new();
        if(String.IsNullOrEmpty(_id)) throw new ArgumentNullException("_id is null");
        Stream stream = await Http.GetStreamAsync(DrinkRoot._urlBase+$"lookup.php?i={_id}");
        string reader = new StreamReader(stream).ReadToEnd();
        var cocktailsValues = System.Text.Json.JsonSerializer.Deserialize<DrinkRoot>(reader);
        drink = cocktailsValues.drinks.First(); 
        return drink;

    }
    
    /*Get Cocktail Ingredients*/
    public async Task<Ingredient> GetIngredientInfos(string _ingredients){   
        if(string.IsNullOrEmpty(_ingredients)) throw new ArgumentNullException();
        
        Ingredient = $"search.php?i={_ingredients}";

        Stream stream = await Http.GetStreamAsync(DrinkRoot._urlBase+Ingredient);

        string reader = new StreamReader(stream).ReadToEnd();
        var _ingredientValues = System.Text.Json.JsonSerializer.Deserialize<DrinkRoot>(reader);

        if(_ingredientValues != null) DrinkIngredient = _ingredientValues.ingredients.First();
        return DrinkIngredient;
    }
    
}




