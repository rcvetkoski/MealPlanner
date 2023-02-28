using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MealPlanner.Controls;
using MealPlanner.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

// Mehe usings
//using System;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using Android.Animation;
//using Android.Content;
//using Android.Graphics;
//using Android.Views;
//using Android.Widget;
//using AndroidX.Core.Widget;
//using Xamarin.Forms.Internals;
//using AView = Android.Views.View;


[assembly: ExportRenderer(typeof(Shell), typeof(CustomShellRenderer))]
[assembly: ExportRenderer(typeof(CustomScrollView), typeof(MeheScrollRenderer))]
namespace MealPlanner.Droid.CustomRenderers
{
    public  class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context)
        {
        }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new MyShellToolbarAppearanceTracker(this);
        }
    }

    public class MyShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
    {
        public MyShellToolbarAppearanceTracker(IShellContext context) : base(context)
        {
        }

        public override void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            base.SetAppearance(toolbar, toolbarTracker, appearance);
            //toolbar.SetPadding(0, 0, 0, 0);
            toolbar.SetContentInsetsAbsolute(0, 0);
            //toolbar.SetContentInsetsRelative(0, 0);
            //toolbar.NavigationIcon = null;
        }
    }

    // mehe test

    public class MeheScrollRenderer : ScrollViewRenderer, Meheinjection
    {
        public MeheScrollRenderer(Context context) : base(context)
        {
            for (int index = 0; index < ((Android.Views.ViewGroup)ViewGroup).ChildCount; index++)
            {
                Android.Views.View nextChild = ((Android.Views.ViewGroup)ViewGroup).GetChildAt(index);
                nextChild.ToString();
            }
        }

        public void DoScroll(double x, double y)
        {
            for (int index = 0; index < ((Android.Views.ViewGroup)ViewGroup).ChildCount; index++)
            {
                Android.Views.View nextChild = ((Android.Views.ViewGroup)ViewGroup).GetChildAt(index);
                if (nextChild is AHorizontalScrollView)
                {
                    int nx = (int)Context.ToPixels(x);
                    int ny = (int)Context.ToPixels(y);
                    (nextChild as AHorizontalScrollView).ScrollTo(nx, ny);
                }
                    
            }
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if(Element != null)
            {
                //Element.PropertyChanged += Element_PropertyChanged;

                (Element as CustomScrollView).SetMeheInjection(this);
            }


        }

        private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if(e.PropertyName == "SCROLLX")
            //{
            //    for (int index = 0; index < ((Android.Views.ViewGroup)ViewGroup).ChildCount; index++)
            //    {
            //        Android.Views.View nextChild = ((Android.Views.ViewGroup)ViewGroup).GetChildAt(index);
            //        nextChild.ToString();
            //        (nextChild as AHorizontalScrollView).ScrollTo((int)(Element as CustomScrollView).SCROLLX, 0);
            //    }
            //}
        }
    }


}