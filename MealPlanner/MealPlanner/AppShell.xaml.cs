using MealPlanner.ViewModels;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MealPlanner
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
            Routing.RegisterRoute(nameof(MealPage), typeof(MealPage));
            Routing.RegisterRoute(nameof(AddAlimentPage), typeof(AddAlimentPage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
        }
    }
}
