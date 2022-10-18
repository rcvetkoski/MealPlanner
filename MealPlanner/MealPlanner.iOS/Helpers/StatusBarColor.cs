using Foundation;
using MealPlanner.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(MealPlanner.iOS.Helpers.StatusBarColor))]
namespace MealPlanner.iOS.Helpers
{
    public class StatusBarColor : IStatusBarColor
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
            //if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            //{
            //    var statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
            //    statusBar.BackgroundColor = color.ToPlatformColor();
            //    UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
            //}
            //else
            //{
            //    var statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            //    if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            //    {
            //        statusBar.BackgroundColor = color.ToPlatformColor();
            //    }
            //}
            var style = darkStatusBarTint ? UIStatusBarStyle.DarkContent : UIStatusBarStyle.LightContent;
            UIApplication.SharedApplication.SetStatusBarStyle(style, false);
            Xamarin.Essentials.Platform.GetCurrentUIViewController()?.SetNeedsStatusBarAppearanceUpdate();
        }
    }
}