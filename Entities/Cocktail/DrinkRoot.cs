namespace BlazorPlaygroundWasm.Entities.Cocktail;


   public class DrinkRoot
    {
        public static string _urlBase = "https://www.thecocktaildb.com/api/json/v1/1/";

        public List<Drink> drinks { get; set; }
    }
