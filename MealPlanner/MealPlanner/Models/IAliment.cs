﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Models
{
    public interface IAliment
    {
        int Id { get; set; }    
        string Name { get; set; }   
        double Calories { get; set; }
        double Proteins { get; set; }
        double Carbs { get; set; }
        double Fats { get; set; }
    }
}
