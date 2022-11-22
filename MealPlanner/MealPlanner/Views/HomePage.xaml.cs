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
        private HomePageTypeEnum homePageType;
        private DayOfWeek dayOfWeek;

        public HomePage()
        {
            InitializeComponent();
            var vm = (BindingContext as HomeViewModel);
            vm.HomePageType = HomePageTypeEnum.Normal;
            this.homePageType = HomePageTypeEnum.Normal;
            vm.SelectedJournalTemplateDayOfWeek = -1;
            vm.RefData.LastUsedHomePageType = HomePageTypeEnum.Normal;
        }

        public HomePage(HomePageTypeEnum homePageTypeEnum, DayOfWeek dayOfWeek)
        {
            InitializeComponent();
            this.homePageType = homePageTypeEnum;
            this.dayOfWeek = dayOfWeek;

            var vm = (BindingContext as HomeViewModel);

            vm.SelectedJournalTemplateDayOfWeek = (int)dayOfWeek;
            vm.HomePageType = homePageTypeEnum;
            vm.RefData.LastUsedHomePageType = homePageTypeEnum;
            vm.ImportFromSavedDaysVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            datePicker.DateSelected += DateSelected;

            var vm = (BindingContext as HomeViewModel);

            if (homePageType == HomePageTypeEnum.JournalTemplate)
                vm.RefData.CreateJournalTemplates(dayOfWeek);

            if(homePageType == HomePageTypeEnum.Normal && vm.RefData.LastUsedHomePageType == HomePageTypeEnum.JournalTemplate)
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
            (BindingContext as HomeViewModel).RefData.UpdateDailyValues();  
        }
    }
}