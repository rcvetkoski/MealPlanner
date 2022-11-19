using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    public partial class HomePage : ContentPage
    {
        private HomePageTypeEnum homePageTypeEnum;
        private DayOfWeek dayOfWeek;

        public HomePage()
        {
            InitializeComponent();
            this.homePageTypeEnum = HomePageTypeEnum.Normal;
            (BindingContext as HomeViewModel).RefData.HomePageType = HomePageTypeEnum.Normal;
        }

        public HomePage(HomePageTypeEnum homePageTypeEnum, DayOfWeek dayOfWeek)
        {
            InitializeComponent();
            this.homePageTypeEnum = homePageTypeEnum;
            this.dayOfWeek = dayOfWeek;

            var vm = (BindingContext as HomeViewModel);

            vm.RefData.HomePageType = homePageTypeEnum;
            vm.ImportFromSavedDaysVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            datePicker.DateSelected += DateSelected;

            var vm = (BindingContext as HomeViewModel);

            if (homePageTypeEnum == HomePageTypeEnum.JournalTemplate)
                vm.RefData.CreateJournalTemplates(dayOfWeek);

            if(homePageTypeEnum == HomePageTypeEnum.Normal && vm.RefData.HomePageType == HomePageTypeEnum.JournalTemplate)
                vm.RefData.GetMealsAtDate(vm.RefData.CurrentDay, vm.RefData.CurrentDay.DayOfWeek);

            //collectionView.ItemsSource = null;
            //collectionView.ItemsSource = (BindingContext as HomeViewModel).RefData.Meals;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            datePicker.DateSelected -= DateSelected;
        }


        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as HomeViewModel).SetTitle();
            (BindingContext as HomeViewModel).RefData.GetMealsAtDate(e.NewDate, e.NewDate.DayOfWeek);
        }
    }
}