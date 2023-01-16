using MealPlanner.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.ViewModels
{
    public class UnitsViewModel : BaseViewModel
    {
        private bool isEnergyUnitCal;
        public bool IsEnergyUnitCal
        {
            get 
            {
                return isEnergyUnitCal;
            }
            set
            {
                if(isEnergyUnitCal != value)
                {
                    isEnergyUnitCal = value;
                    
                    if(isEnergyUnitCal)
                    {
                        RefData.User.EnergyUnit = EnergyUnitEnum.kcal;
                        App.DataBaseRepo.UpdateUserAsync(RefData.User);
                        RefData.GetMealsAtDate(RefData.CurrentDay);
                    }
                }
            }
        }

        private bool isEnergyUnitKj;
        public bool IsEnergyUnitKj
        {
            get
            {
                return isEnergyUnitKj;
            }
            set
            {
                if (isEnergyUnitKj != value)
                {
                    isEnergyUnitKj = value;

                    if(IsEnergyUnitKj)
                    {
                        RefData.User.EnergyUnit = EnergyUnitEnum.kj;
                        App.DataBaseRepo.UpdateUserAsync(RefData.User);
                        RefData.GetMealsAtDate(RefData.CurrentDay);
                    }
                }
            }
        }

        private bool isWeightUnitKg;
        public bool IsWeightUnitKg
        {
            get
            {
                return isWeightUnitKg;
            }
            set
            {
                if (isWeightUnitKg != value)
                {
                    isWeightUnitKg = value;

                    if (isWeightUnitKg && canChange)
                    {
                        RefData.User.Weight = RefData.User.Weight / 2.22;
                        RefData.User.WeightUnit = WeightUnitEnum.kg;
                        App.DataBaseRepo.UpdateUserAsync(RefData.User);
                    }
                }
            }
        }

        private bool isWeightUnitLbs;
        public bool IsWeightUnitLbs
        {
            get
            {
                return isWeightUnitLbs;
            }
            set
            {
                if (isWeightUnitLbs != value)
                {
                    isWeightUnitLbs = value;

                    if (isWeightUnitLbs && canChange)
                    {
                        RefData.User.Weight = RefData.User.Weight * 2.22;
                        RefData.User.WeightUnit = WeightUnitEnum.lbs;
                        App.DataBaseRepo.UpdateUserAsync(RefData.User);
                    }
                }
            }
        }

        private bool isHeightUnitCm;
        public bool IsHeightUnitCm
        {
            get
            {
                return isHeightUnitCm;
            }
            set
            {
                if (isHeightUnitCm != value)
                {
                    isHeightUnitCm = value;

                    if (isHeightUnitCm && canChange)
                    {
                        RefData.User.Height = RefData.User.Height * 30.48;
                        RefData.User.HeightUnit = HeightUnitEnum.cm;
                        App.DataBaseRepo.UpdateUserAsync(RefData.User);
                    }
                }
            }
        }

        private bool isHeightUnitft_in;
        public bool IsHeightUnitft_in
        {
            get
            {
                return isHeightUnitft_in;
            }
            set
            {
                if (isHeightUnitft_in != value)
                {
                    isHeightUnitft_in = value;

                    if (isHeightUnitft_in && canChange)
                    {
                        RefData.User.Height = RefData.User.Height / 30.48;
                        RefData.User.HeightUnit = HeightUnitEnum.ft_in;
                        App.DataBaseRepo.UpdateUserAsync(RefData.User);
                    }
                }
            }
        }

        private bool canChange;

        public UnitsViewModel()
        {
            Title = "Units";
            IsEnergyUnitCal = RefData.User.EnergyUnit == EnergyUnitEnum.kcal;
            IsEnergyUnitKj = RefData.User.EnergyUnit == EnergyUnitEnum.kj;

            IsWeightUnitKg = RefData.User.WeightUnit == WeightUnitEnum.kg;
            IsWeightUnitLbs = RefData.User.WeightUnit == WeightUnitEnum.lbs;

            IsHeightUnitCm = RefData.User.HeightUnit == HeightUnitEnum.cm;
            IsHeightUnitft_in = RefData.User.HeightUnit == HeightUnitEnum.ft_in;

            canChange = true;
        }
    }
}
