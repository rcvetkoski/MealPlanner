using System;
using CoreGraphics;
using MealPlanner.Controls;
using MealPlanner.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(CustomScrollViewRenderer))]
namespace MealPlanner.iOS.CustomRenderers
{
	public class CustomScrollViewRenderer : ScrollViewRenderer, Meheinjection
    {
		public CustomScrollViewRenderer()
		{
			this.Bounces = false;
		}

        public void DoScroll(double x, double y)
        {
            this.SetContentOffset(new CoreGraphics.CGPoint(x, y), false);
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                (Element as CustomScrollView).SetMeheInjection(this);
            }
        }
    }
}

