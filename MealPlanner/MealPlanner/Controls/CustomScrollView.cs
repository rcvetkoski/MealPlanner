using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Controls
{
    public class CustomScrollView : ScrollView
    {
        public double SCROLLX { get; set; }

        private Meheinjection _meheinjection;

        public void SetMeheInjection(Meheinjection inj)
        {
            _meheinjection = inj;
        }

        public Meheinjection GetMeheInjection()
        {
            return _meheinjection;
        }

        public void Update()
        {
            this.InvalidateLayout();
            this.ForceLayout();
        }
    }

    public interface Meheinjection
    {
        void DoScroll(double x, double y);
    }
}
