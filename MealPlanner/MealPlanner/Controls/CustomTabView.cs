using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MealPlanner.Controls
{
    public class CustomTabView : Grid, IDisposable
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(CustomTabView), null, propertyChanged: OnPropertyChanged);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //ObservableCollection<object> source = new ObservableCollection<object>();

            //foreach (var item in (IEnumerable)newValue)
            //{
            //    if (item is string)
            //    {
            //        char[] charArray = new char[(item as string).Length];

            //        for (int i = 0; i < (item as string).Length; i++)
            //            charArray[i] = (item as string)[i];

            //        string newObject = new string(charArray);
            //        source.Add(newObject);
            //    }
            //}

            (bindable as CustomTabView).content.ItemsSource = (IEnumerable)newValue;
            BindableLayout.SetItemsSource((bindable as CustomTabView).tabsContent, (IEnumerable)newValue);
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CustomTabView), null, BindingMode.TwoWay);
        public object SelectedItem
        {
            get { return (IEnumerable)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        public static readonly BindableProperty ContentItemTemplateProperty = BindableProperty.Create(nameof(ContentItemTemplate), typeof(DataTemplate), typeof(CustomTabView), propertyChanged: OnContentItemTemplatePropertyChanged);
        public DataTemplate ContentItemTemplate
        {
            get { return (DataTemplate)GetValue(ContentItemTemplateProperty); }
            set { SetValue(ContentItemTemplateProperty, value); }
        }
        private static void OnContentItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomTabView).content.ItemTemplate = (DataTemplate)newValue;
        }


        public static readonly BindableProperty TabsItemTemplateProperty = BindableProperty.Create(nameof(TabsItemTemplate), typeof(DataTemplate), typeof(CustomTabView), null, propertyChanged: OnTabsItemTemplatePropertyChanged);
        public DataTemplate TabsItemTemplate
        {
            get { return (DataTemplate)GetValue(TabsItemTemplateProperty); }
            set { SetValue(TabsItemTemplateProperty, value); }
        }
        private static void OnTabsItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BindableLayout.SetItemTemplate((bindable as CustomTabView).tabsContent, (DataTemplate)newValue);
        }

        public static readonly BindableProperty TabTextColorProperty = BindableProperty.Create(nameof(TabTextColor), typeof(Color), typeof(CustomTabView), Color.White);
        public Color TabTextColor
        {
            get { return (Color)GetValue(TabTextColorProperty); }
            set { SetValue(TabTextColorProperty, value); }
        }

        public static readonly BindableProperty SliderColorProperty = BindableProperty.Create(nameof(SliderColor), typeof(Color), typeof(CustomTabView), Color.White);
        public Color SliderColor
        {
            get { return (Color)GetValue(SliderColorProperty); }
            set { SetValue(SliderColorProperty, value); }
        }


        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(CustomTabView), Color.White);
        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public static readonly BindableProperty TabsItemBindingPathProperty = BindableProperty.Create(nameof(TabsItemBindingPath), typeof(string), typeof(CustomTabView), ".");
        public string TabsItemBindingPath
        {
            get { return (string)GetValue(TabsItemBindingPathProperty); }
            set { SetValue(TabsItemBindingPathProperty, value); }
        }

        public static readonly BindableProperty ContentItemBindingPathProperty = BindableProperty.Create(nameof(ContentItemBindingPath), typeof(string), typeof(CustomTabView), ".");
        public string ContentItemBindingPath
        {
            get { return (string)GetValue(ContentItemBindingPathProperty); }
            set { SetValue(ContentItemBindingPathProperty, value); }
        }


        private object tempVisualElement;
        private CustomScrollView tabs;
        private BoxView slider;
        private StackLayout tabsContent;
        private CarouselView content;
        private BoxView separator;
        private DataTemplate DefaultTabsItemTemplate;
        private DataTemplate DefaultContentItemTemplate;
        private int currentPosition = 0;
        private double scrollRatio = 0;
        private bool IsAutoScroll = false;
        private int FirstIndex = -1;
        private int sign = 0;
        double translateX = 0;
        VisualElement previousSelected = null;
        private ICommand TapCommand;



        public CustomTabView()
        {
            currentPosition = 0;
            TapCommand = new Command<View>(Tap);

            SetDefaultTabsItemTemplate();
            SetDefaultContentItemTemplate();


            // Main Grid
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(15)});
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            SetContent();
            SetTabs();
            SetSlider();
            SetSeparator();


            // Add to main grid
            Children.Add(tabs, 0, 0);
            Children.Add(separator, 0, 1);
            Children.Add(slider, 0, 1);
            Children.Add(content, 0, 2);

            RowSpacing = 0;
        }

        private void SetDefaultTabsItemTemplate()
        {
            DefaultTabsItemTemplate = new DataTemplate(() =>
            {
                var label = new Label()
                {
                    Padding = new Thickness(10, 13, 10, 13),
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                label.SetBinding(Label.TextProperty, TabsItemBindingPath);

                Binding binding = new Binding("TabTextColor", source: this);
                label.SetBinding(Label.TextColorProperty, binding);


                // Visual states
                var visualStateGroup = new VisualStateGroup();
                var visualStateSelected = new VisualState() { Name = "selected" };
                var visualStateNormal = new VisualState() { Name = "normal" };
                visualStateSelected.Setters.Add
                (
                    new Setter()
                    {
                        Property = Label.FontAttributesProperty,
                        Value = FontAttributes.Bold
                    }
                );
                visualStateGroup.States.Add(visualStateSelected);
                visualStateNormal.Setters.Add
                (
                    new Setter()
                    {
                        Property = Label.FontAttributesProperty,
                        Value = FontAttributes.None
                    }
                );
                visualStateGroup.States.Add(visualStateNormal);
                VisualStateManager.GetVisualStateGroups(label).Add(visualStateGroup);

                return label;
            });

        }

        private void SetDefaultContentItemTemplate()
        {
            DefaultContentItemTemplate = new DataTemplate(() =>
            {
                var label = new Label()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    VerticalTextAlignment = TextAlignment.Center
                };

                label.SetBinding(Label.TextProperty, ContentItemBindingPath);

                return label;
            });
        }

        private void SetSeparator()
        {
            separator = new BoxView()
            {
                HeightRequest = 1,
                Margin = new Thickness(0, 2, 0, 12),
                BackgroundColor = SeparatorColor,
                VerticalOptions = LayoutOptions.Start
            };

            Binding binding = new Binding("SeparatorColor", source: this);
            separator.SetBinding(BoxView.BackgroundColorProperty, binding);
        }

        private void SetContent()
        {
            content = new CarouselView()
            {
                Loop = false,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never,
                ItemTemplate = ContentItemTemplate != null ? ContentItemTemplate : null
            };

            ContentItemTemplate = DefaultContentItemTemplate;

            content.Scrolled += Content_Scrolled;
            content.CurrentItemChanged += Content_CurrentItemChanged;
            content.VisibleViews.CollectionChanged += VisibleViews_CollectionChanged;
        }

        private void Content_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            SelectedItem = e.CurrentItem;

            return;

            var item = tabsContent.Children.Single(x => x.BindingContext == e.CurrentItem);
            VisualStateManager.GoToState(item, "selected");

            if(previousSelected != null)
                VisualStateManager.GoToState(previousSelected, "normal");


            previousSelected = item;    
        }

        private void VisibleViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var first = content.VisibleViews.Count > 0 ? content.VisibleViews[0] : null;
            if (tempVisualElement != first)
            {
                if (tempVisualElement == null)
                    FirstIndex = content.Position;
                else if (sign > 0)
                    FirstIndex++;
                else if (sign < 0)
                    FirstIndex--;

                var currentIndex = FirstIndex > 0 ? FirstIndex : 0;
                if (currentPosition != currentIndex)
                {
                    currentPosition = currentIndex;
                }

                tempVisualElement = first;
            }
        }

        private void SetSlider()
        {
            // Slider
            slider = new BoxView()
            {
                HeightRequest = 2,
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Start
            };

            Binding binding = new Binding("SliderColor", source: this);
            slider.SetBinding(BoxView.BackgroundColorProperty, binding);
        }

        private void SetTabs()
        {
            // Tabs scrollView
            tabs = new CustomScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            };

            // tabs content not recycled
            tabsContent = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };
            TabsItemTemplate = DefaultTabsItemTemplate;


            // Add content and slider to grid
            tabs.Content = tabsContent;
            tabsContent.SizeChanged += TabsContent_SizeChanged;
            tabs.Scrolled += Tabs_Scrolled;
            tabsContent.ChildAdded += Tabs_ChildAdded;
        }

        private void TabsContent_SizeChanged(object sender, EventArgs e)
        {
            if (tabsContent.Children.Count > 0 && tabsContent.Children.Count >= currentPosition)
            {
                slider.WidthRequest = tabsContent.Children[currentPosition].Width;
                slider.TranslationX = tabsContent.Children[currentPosition].Bounds.X - tabs.ScrollX;
            }
        }

        private void Tabs_ChildAdded(object sender, ElementEventArgs e)
        {
            var item = (sender as StackLayout).Children.Last();
            item.GestureRecognizers.Add(new TapGestureRecognizer() { Command = TapCommand, CommandParameter = item });

            // Set HorizontalOptions to FillAndExpand otherwise calculations won't work properly
            item.HorizontalOptions = LayoutOptions.FillAndExpand;
            item.VerticalOptions = LayoutOptions.Center; 

            // Used to set slider's width at appearing
            item.SizeChanged += Item_MeasureInvalidated;
        }

        private void Item_MeasureInvalidated(object sender, EventArgs e)
        {
            if (tabsContent.Children.Count > 0 && tabsContent.Children.Count >= currentPosition)
            {
                slider.WidthRequest = tabsContent.Children[currentPosition].Width;
                slider.TranslationX = tabsContent.Children[currentPosition].Bounds.X - tabs.ScrollX;
            }

            (sender as View).SizeChanged -= Item_MeasureInvalidated;
        }

        private void Tabs_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (!IsAutoScroll)
            {
                tabs.SCROLLX = e.ScrollX;
                slider.TranslationX = translateX - tabs.ScrollX;
            }

            IsAutoScroll = false;
        }

        private void Content_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            IsAutoScroll = true;
            var first = content.VisibleViews.Count > 0 ? content.VisibleViews[0] : null;
            //var FirstIndex = first != null ? contentItems.IndexOf(first) : 0;
            var FirstIndex2 = first != null ? tabsContent.Children.IndexOf(tabsContent.Children.FirstOrDefault(x => (x as View).BindingContext == first.BindingContext)) : 0;

            //Console.WriteLine($" FirstIndex2 {FirstIndex2}");

            var currentIndex = FirstIndex > 0 ? FirstIndex : 0;
            sign = Math.Sign(e.HorizontalOffset - content.Width * currentIndex);
            var currentItem = tabsContent.Children.ElementAt(currentIndex);
            var nextItem = tabsContent.Children.Count > (currentIndex + sign) && (currentIndex + sign) >= 0 ? tabsContent.Children.ElementAt(currentIndex + sign) : null;

            // scrollRatio
            scrollRatio = (e.HorizontalOffset - content.Width * currentIndex) / content.Width;

            double currentItemWidth = currentItem.Width;
            double nextItemWidth = nextItem != null ? nextItem.Width : currentItem.Width;


            if (sign > 0)
                translateX = currentItem.Bounds.X + scrollRatio * currentItemWidth;
            else
                translateX = currentItem.Bounds.X + scrollRatio * nextItemWidth;

            //Set slider width
            slider.WidthRequest = currentItemWidth - (currentItemWidth - nextItemWidth) * Math.Abs(scrollRatio);

            double maxScrollX = tabs.ContentSize.Width - tabs.Width;
            var tx = sign > 0 ? scrollRatio * currentItemWidth : scrollRatio * nextItemWidth;
            double toScrollX = 0;
            
            if(FirstIndex == 0)
                toScrollX = currentItem.Bounds.X + tx / 2;
            else if(FirstIndex > 0)
                toScrollX = currentItem.Bounds.X + tx - tabsContent.Children.ElementAt(0).Bounds.Width / 2;
            else
                toScrollX = currentItem.Bounds.X + tx;


            if (toScrollX < 0)
            {
                tabs.GetMeheInjection().DoScroll(0, 0);
            }
            else if (toScrollX > maxScrollX && sign > 0)
            {
                tabs.GetMeheInjection().DoScroll(maxScrollX, 0);
            }
            else 
            {
                tabs.GetMeheInjection().DoScroll(toScrollX, 0);
            }

            //Set slider X position
            slider.TranslationX = translateX - tabs.ScrollX;
        }

        private void Tap(View item) 
        {
            var position = tabsContent.Children.IndexOf(item);
            content.Position = position;
            
            //item.Opacity = 0;   
            //item.BackgroundColor = Color.LightGray;
            //await item.FadeTo(1);
            //await item.FadeTo(0);
            //item.BackgroundColor = Color.Transparent;
            //item.Opacity = 1;
            //await item.ScaleTo(1.1, 120);
            //await item.ScaleTo(1, 120);
        }

        public void Dispose()
        {
            content.Scrolled -= Content_Scrolled;
            content.CurrentItemChanged -= Content_CurrentItemChanged;
            content.VisibleViews.CollectionChanged -= VisibleViews_CollectionChanged;
            tabs.Scrolled -= Tabs_Scrolled;
            tabsContent.ChildAdded -= Tabs_ChildAdded;
            tabsContent.SizeChanged -= TabsContent_SizeChanged;
        }
    }
}
