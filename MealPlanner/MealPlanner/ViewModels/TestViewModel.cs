using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class TestViewModel : BaseViewModel
    {
        public TestViewModel()
        {
            //Title = "Test";

            TestPropertie = "Mehe";
            Test = new ObservableCollection<string>();
            Test.Add("Trwol wefl ");
            Test.Add("Trl thrth");
            Test.Add("Trl ");
            Test.Add("Trl ");
            Test.Add("Trwol wefl tz ztjt");
            Test.Add("Trwol  ");
            Test.Add("T");
            Test.Add("Rwwgwwggrw");
            Test.Add("Trwol ergergerg ");

            //Test.Add("1");
            //Test.Add("2");
            //Test.Add("3");
            //Test.Add("4");
            //Test.Add("5");
            //Test.Add("6");
            //Test.Add("7");
            //Test.Add("8");
            //Test.Add("9");

            TestCommand = new Command(TestMethod);
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

        public ObservableCollection<string> Test { get; set; }

        public ICommand TestCommand { get; set; }
        private void TestMethod()
        {
            Test.Add("Trl ");
        }
    }
}
