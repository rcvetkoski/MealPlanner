using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Models
{
    public class Food: Aliment
    {
        public int MealFoodId { get; set; } = 0;
        public override AlimentTypeEnum AlimentType 
        { 
            get
            {
                return AlimentTypeEnum.Food;
            } 
        }
    }
}
