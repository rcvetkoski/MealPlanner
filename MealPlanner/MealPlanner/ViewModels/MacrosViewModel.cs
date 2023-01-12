using System;
using System.Collections.Generic;
using System.Text;
using static MealPlanner.Models.User;
using System.Windows.Input;
using Xamarin.Forms;
using MealPlanner.Views;

namespace MealPlanner.ViewModels
{
    public class MacrosViewModel : BaseViewModel
    {
        public MacrosViewModel()
        {
            Title = "Macros";
            SelectTypeOfRegimeCommand = new Command<object[]>(TypeOfRegime);
            IsTypeOfRegimeItemCustom = RefData.User.SelectedTypeOfRegime.Name == "Custom" ? true : false;
            OpenEditMacrosPageCommand = new Command(OpenEditMacrosPage);
        }

        public ICommand SelectTypeOfRegimeCommand { get; set; }
        private async void TypeOfRegime(object[] objects)
        {
            RadioButton radioButton = objects[0] as RadioButton;
            MacrosPage macrosPage = objects[1] as MacrosPage;

            if (radioButton.IsChecked)
                return;

            var typeOfRegimeItem = radioButton.BindingContext as TypeOfRegimeItem;
            radioButton.IsChecked = true;
            RefData.User.SelectedTypeOfRegime = typeOfRegimeItem;
            IsTypeOfRegimeItemCustom = RefData.User.SelectedTypeOfRegime.Name == "Custom" ? true : false;
            InitProperties();
            macrosPage.InitChart();
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
        }

        public ICommand OpenEditMacrosPageCommand { get; set; }
        private async void OpenEditMacrosPage()
        {
            //await Shell.Current.Navigation.PushModalAsync(new EditMacrosPage());
            await Shell.Current.GoToAsync(nameof(EditMacrosPage));
        }

        private bool isTypeOfRegimeItemCustom;
        public bool IsTypeOfRegimeItemCustom 
        {
            get
            {
                return isTypeOfRegimeItemCustom;
            }
            set
            {
                if(isTypeOfRegimeItemCustom != value)
                {
                    isTypeOfRegimeItemCustom = value;
                    OnPropertyChanged(nameof(IsTypeOfRegimeItemCustom));
                }
            }
        }

        public double ProtsCalories { get; set; }
        public double CarbsCalories { get; set; }
        public double FatsCalories { get; set; }

        public void InitProperties()
        {
            ProtsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.ProteinPercentage;
            CarbsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.CarbsPercentage;
            FatsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.FatsPercentage;

            OnPropertyChanged(nameof(ProtsCalories));
            OnPropertyChanged(nameof(CarbsCalories));
            OnPropertyChanged(nameof(FatsCalories));
        }
    }
}
