﻿using MealPlanner.ViewModels;
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
            Routing.RegisterRoute(nameof(EditFoodPage), typeof(EditFoodPage));
            Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
            //Routing.RegisterRoute(nameof(AddAlimentPage), typeof(AddAlimentPage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute(nameof(UnitsPage), typeof(UnitsPage));
            Routing.RegisterRoute(nameof(CustomizeMealsPage), typeof(CustomizeMealsPage));
            Routing.RegisterRoute(nameof(ObjectifsPage), typeof(ObjectifsPage));
            Routing.RegisterRoute(nameof(MacrosPage), typeof(MacrosPage));
            Routing.RegisterRoute(nameof(EditJournalTemplatePage), typeof(EditJournalTemplatePage));
            Routing.RegisterRoute(nameof(JournalTemplatePage), typeof(JournalTemplatePage));
            Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
        }
    }
}
