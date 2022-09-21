using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }    
        public double Height { get; set; }  
        public double Weight { get; set; }  
        public int Age { get; set; }    
        public double TargetCalories { get; set; }  
        public double TargetProteins { get; set; }
        public double TargetCarbs { get; set; }
        public double TargetFats { get; set; }
    }
}
