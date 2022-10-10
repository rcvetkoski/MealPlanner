using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class TestModel
    {
        public class Product
        {
            public double EnergyKcal100g { get; set; }

            public double carbohydrates_100g { get; set; }
            public double fat_100g { get; set; }
            public string image_front_url { get; set; }
            public string product_name { get; set; }
            public double proteins_100g { get; set; }
        }
    }
}
