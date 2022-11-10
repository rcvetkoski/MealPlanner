using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAlimentPage : ContentPage
    {
        public AddAlimentPage()
        {
            InitializeComponent();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                (BindingContext as AddAlimentViewModel).FilteredAlimentsRefresh();
            }
        }
    }
}