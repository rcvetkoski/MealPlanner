using MealPlanner.Helpers;
using MealPlanner.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;
using ZXing.QrCode.Internal;
using static MealPlanner.Models.TestModel;

namespace MealPlanner.Services
{
    public class OpenFoodFactsRestService : IRestService
    {
        public async Task<Aliment> ScanBarCodeAsync(string code)
        {
            Aliment aliment = null;
            Uri uri = new Uri(string.Format("https://world.openfoodfacts.org/api/v2/product/code=" + code + "&fields=product_name,image_front_url,proteins_100g,proteins_unit,proteins_value,carbohydrates_100g,energy-kcal_100g,carbohydrates_100g,fat_100g,fiber_100g", string.Empty));

            HttpResponseMessage response = await HttpClientHelper.Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                OpenFoodFactsObject openFoodFactsObject = JsonConvert.DeserializeObject<OpenFoodFactsObject>(json, new JsonSerializerSettings() { Culture = System.Globalization.CultureInfo.InvariantCulture });
                Product product = openFoodFactsObject.product as Product;

                aliment = new Food();

                aliment.Name = product.product_name;
                aliment.ImageSourcePath = product.image_front_url;
                aliment.Proteins = product.proteins_100g;
                aliment.Carbs = product.carbohydrates_100g;
                aliment.Fats = product.fat_100g;
                aliment.Calories = product.EnergyKcal100g;

                aliment.OriginalServingSize = 100;
                aliment.ServingSize = 100;
            }
            else
            {
                Console.WriteLine(response.StatusCode); 
            }

            return aliment; 
        }


        public async Task<List<Aliment>> SearchAlimentAsync(string searchText)
        { 
            List<Aliment> alimentList = new List<Aliment>();

            Uri uri = new Uri(string.Format($"https://world.openfoodfacts.org/cgi/search.pl?search_terms={searchText}&search_simple=1&action=process&fields=" +
                $"product_name," +
                $"image_front_url," +
                $"proteins_100g," +
                $"carbohydrates_100g," +
                $"energy-kcal_100g," +
                $"carbohydrates_100g," +
                $"fat_100g," +
                $"fiber_100g," +
                $"salt_100g," +
                $"sodium_100g," +
                $"sugars_100g," +
                $"saturated-fat_100g," +
                $"vitamins_prev_tags," +
                $"vitamins_tags," +
                $"serving_quantity," +
                $"serving_size," +
                $"energy-kcal_serving," +
                $"carbohydrates_serving," +
                $"fat_serving," +
                $"saturated-fat_serving," +
                $"fiber_serving," +
                $"sodium_serving," +
                $"proteins_serving," +
                $"salt_serving," +
                $"sugars_serving" +
                $"&json=1", string.Empty));

            HttpResponseMessage response = await HttpClientHelper.Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var openFoodFactsObjectSearch = JsonConvert.DeserializeObject<OpenFoodFactsObjectSearch>(json, new JsonSerializerSettings() { Culture = System.Globalization.CultureInfo.InvariantCulture });

                foreach (var openFoodFact in openFoodFactsObjectSearch.products)
                {
                    if (string.IsNullOrEmpty(openFoodFact.image_front_url) || string.IsNullOrEmpty(openFoodFact.product_name))
                        continue;

                    Product product = openFoodFact as Product;

                    var aliment = new Food();

                    aliment.Name = product.product_name;
                    aliment.ImageSourcePath = product.image_front_url;

                    aliment.Calories = product.EnergyKcal100g;
                    aliment.Proteins = product.proteins_100g;
                    aliment.Carbs = product.carbohydrates_100g;
                    aliment.Fibers = product.fiber_100g;
                    aliment.Sugars = product.sugars_100g;
                    aliment.Fats = product.fat_100g;
                    aliment.SaturatedFat = product.SaturatedFat100g;
                    aliment.Salt = product.salt_100g;
                    aliment.Sodium = product.sodium_100g;


                    aliment.ServingQuantity = product.serving_quantity;
                    aliment.ServingQuantityUnit = product.serving_size;

                    aliment.CaloriesServing = product.EnergyKcalServing;
                    aliment.ProteinsServing = product.proteins_serving;
                    aliment.CarbsServing = product.carbohydrates_serving;
                    aliment.FibersServing = product.fiber_serving;
                    aliment.SugarsServing = product.sugars_serving;
                    aliment.SaturatedFatServing = product.SaturatedFatServing;
                    aliment.FatsServing = product.fat_serving;
                    aliment.SaltServing = product.salt_serving;
                    aliment.SodiumServing = product.sodium_serving;


                    aliment.OriginalServingSize = 100;
                    aliment.ServingSize = 100;

                    alimentList.Add(aliment);
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }

            return alimentList;
        }
    }
}
