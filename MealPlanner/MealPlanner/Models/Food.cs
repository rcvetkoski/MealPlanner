using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class Food: IAliment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }    
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public double ServingSize { get; set; }
        public int NumberOfPortions { get; set; } = 1;
        public AlimentTypeEnum AlimentType { get { return AlimentTypeEnum.Food; } }
        public AlimentUnitEnum Unit { get; set; }
    }
}
