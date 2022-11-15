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
        private List<ToolbarItem> TempToolbarItems;
        public AddAlimentPage()
        {
            InitializeComponent();
            TempToolbarItems = new List<ToolbarItem>();
            foreach (ToolbarItem item in ToolbarItems)
                TempToolbarItems.Add(item);
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                (BindingContext as AddAlimentViewModel).FilteredAlimentsRefresh();

                if(!this.ToolbarItems.Any())
                {
                    foreach (ToolbarItem item in TempToolbarItems)
                        ToolbarItems.Add(item);

                    title.IsVisible = true;
                    entry.IsVisible = false;
                }
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            this.ToolbarItems.Clear();
            title.IsVisible = false;
            entry.IsVisible = true;
        }
    }
}