using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class TestModel
    {
        public class Product
        {
            [JsonProperty("energy-kcal_100g")]
            public double EnergyKcal100g { get; set; }

            public double carbohydrates_100g { get; set; }
            public double fat_100g { get; set; }
            public string image_front_url { get; set; }
            public string product_name { get; set; }
            public double proteins_100g { get; set; }
        }

        public class OpenFoodFactsObject
        {
            public string code { get; set; }

            public Product product { get; set; }
            public string status { get; set; }

            public string status_erbose { get; set; }
        }

        public class OpenFoodFactsObjectSearch
        {
            public string code { get; set; }

            public List<Product> products { get; set; }
            public string status { get; set; }

            public string status_erbose { get; set; }
        }
    }
}
