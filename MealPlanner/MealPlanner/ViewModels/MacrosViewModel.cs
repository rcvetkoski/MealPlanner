using System;
using System.Collections.Generic;
using System.Text;
using static MealPlanner.Models.User;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class MacrosViewModel : BaseViewModel
    {
        public MacrosViewModel()
        {
            Title = "Macros";
            SelectTypeOfRegimeCommand = new Command<RadioButton>(TypeOfRegime);
            IsTypeOfRegimeItemCustom = RefData.User.SelectedTypeOfRegime.Name == "Custom" ? true : false;
            InitProperties();
        }

        public ICommand SelectTypeOfRegimeCommand { get; set; }
        private async void TypeOfRegime(RadioButton radioButton)
        {
            var typeOfRegimeItem = radioButton.BindingContext as TypeOfRegimeItem;
            radioButton.IsChecked = true;
            RefData.User.SelectedTypeOfRegime = typeOfRegimeItem;
            IsTypeOfRegimeItemCustom = RefData.User.SelectedTypeOfRegime.Name == "Custom" ? true : false;
            InitProperties();
            OnPropertyChangedPercentageSum100();
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
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

        public double ProtsPercentage { get; set; }
        public double CarbsPercentage { get; set; }
        public double FatsPercentage { get; set; }

        public double ProtsCalories { get; set; }
        public double CarbsCalories { get; set; }
        public double FatsCalories { get; set; }

        public bool IsMacroPercentageSum100
        {
            get
            {
                return (ProtsPercentage + CarbsPercentage + FatsPercentage) == 100;
            }
        }

        public void OnPropertyChangedPercentageSum100()
        {
            OnPropertyChanged(nameof(IsMacroPercentageSum100));
        }

        private void InitProperties()
        {
            ProtsPercentage = RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100;
            CarbsPercentage = RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100;
            FatsPercentage = RefData.User.SelectedTypeOfRegime.FatsPercentage * 100;

            ProtsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.ProteinPercentage;
            CarbsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.CarbsPercentage;
            FatsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.FatsPercentage;


            OnPropertyChanged(nameof(ProtsPercentage));
            OnPropertyChanged(nameof(CarbsPercentage));
            OnPropertyChanged(nameof(FatsPercentage));

            OnPropertyChanged(nameof(ProtsCalories));
            OnPropertyChanged(nameof(CarbsCalories));
            OnPropertyChanged(nameof(FatsCalories));
        }
    }
}
