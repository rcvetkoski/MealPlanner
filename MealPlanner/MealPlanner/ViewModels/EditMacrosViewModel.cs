using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.ViewModels
{
    public class EditMacrosViewModel : BaseViewModel
    {
        public EditMacrosViewModel()
        {
            Title = $"Edit {RefData.User.SelectedTypeOfRegime.Name} plan macros";
            InitProperties();
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

        public void InitProperties()
        {
            ProtsPercentage = RefData.User.SelectedTypeOfRegime.ProteinPercentage * 100;
            CarbsPercentage = RefData.User.SelectedTypeOfRegime.CarbsPercentage * 100;
            FatsPercentage = RefData.User.SelectedTypeOfRegime.FatsPercentage * 100;

            OnPropertyChanged(nameof(ProtsPercentage));
            OnPropertyChanged(nameof(CarbsPercentage));
            OnPropertyChanged(nameof(FatsPercentage));

            ProtsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.ProteinPercentage;
            CarbsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.CarbsPercentage;
            FatsCalories = RefData.User.TDEE * RefData.User.SelectedTypeOfRegime.FatsPercentage;

            OnPropertyChanged(nameof(ProtsCalories));
            OnPropertyChanged(nameof(CarbsCalories));
            OnPropertyChanged(nameof(FatsCalories));
        }
    }
}
