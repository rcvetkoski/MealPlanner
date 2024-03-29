﻿using MealPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExercicePage : ContentPage
    {
        public AddExercicePage()
        {
            InitializeComponent();

            SearchEntry = this.searchEntry;
        }

        public Entry SearchEntry;

        private void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as AddExerciceViewModel;
            vm.SearchExercices();
        }
    }
}