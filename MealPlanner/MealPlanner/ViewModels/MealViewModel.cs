using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.ViewModels
{
    public class MealViewModel : BaseViewModel
    {
        public string Name { get; set; }    
        public double Portion { get; set; } 
        public string Description { get; set; } 
    }
}
