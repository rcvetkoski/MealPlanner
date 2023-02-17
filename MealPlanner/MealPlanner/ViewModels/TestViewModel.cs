﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class TestViewModel : BaseViewModel
    {
        public TestViewModel()
        {
            //Title = "Test";

            TestPropertie = "Mehe";

        }

        private Rectangle rect;
        public Rectangle Rect
        { 
            get
            {
                return rect;
            }
            set
            {
                if(rect != value)
                {
                    rect = value;
                    OnPropertyChanged(nameof(Rect));
                }
            }
        }

        private string testPropertie;
        public string TestPropertie { get { return testPropertie; } 
            set
            { 
                if(value != testPropertie)
                {
                    testPropertie = value;
                    OnPropertyChanged("TestPropertie");
                }
            } 
        }
    }
}
