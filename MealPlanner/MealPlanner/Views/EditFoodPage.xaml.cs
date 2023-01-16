using MealPlanner.Models;
using MealPlanner.ViewModels;
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
    public partial class EditFoodPage : ContentPage
    {
        public EditFoodPage()
        {
            InitializeComponent();
        }

        public bool CheckFields()
        {
            return (servingSizeEntry.CheckIsValid() || nameEntry.CheckIsValid()) == false ? false : true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as EditFoodViewModel;
            vm.InitProperties(vm.CurrentAliment);
        }
    }
}