using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class MealAliment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public int MealId { get; set; }
        public int AlimentId { get; set; }
        public AlimentTypeEnum AlimentType { get; set; }
        public double ServingSize { get; set; }
    }
}
