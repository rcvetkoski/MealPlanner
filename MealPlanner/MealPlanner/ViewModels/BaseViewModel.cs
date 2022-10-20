using MealPlanner.Helpers;
using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            AddImageCommand = new Command<Aliment>(AddImage);
        }


        public ReferentialData RefData { get => App.RefData; }
        public AlimentUnitEnum AlimentUnitEnum { get; set; }

        private Aliment currentAliment;
        public Aliment CurrentAliment { get { return currentAliment; } set { currentAliment = value; OnPropertyChanged("CurrentAliment"); } }

        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        private string imageSourcePath;
        public string ImageSourcePath { get { return imageSourcePath; } set { imageSourcePath = value; OnPropertyChanged("ImageSourcePath"); } }

        private ImageSource imageSource;
        public ImageSource ImageSource { get { return imageSource; } set { imageSource = value; OnPropertyChanged("ImageSource"); } }

        private double servingSize;
        public double ServingSize { get { return servingSize; } set { servingSize = value; OnPropertyChanged("ServingSize"); } }

        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit { get { return unit; } set { unit = value; OnPropertyChanged("Unit"); } }

        private bool isNew;
        public bool IsNew { get { return isNew; } set { isNew = value; OnPropertyChanged("IsNew"); } }

        public ICommand AddImageCommand { get; set; }

        private async void AddImage(Aliment currentAliment)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo, currentAliment);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {ImageSourcePath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo, Aliment currentAliment)
        {
            // canceled
            if (photo == null)
            {
                ImageSourcePath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);


            var resizedFile = Path.Combine(FileSystem.CacheDirectory, $"{currentAliment.Name}{currentAliment.Id}");
            App.ImageService.ResizeImage(newFile, resizedFile, 30);
            ImageSourcePath = resizedFile;

            currentAliment.ImageBlob = File.ReadAllBytes(resizedFile);
            ImageSource = currentAliment.ImageSource;

            if (File.Exists(ImageSourcePath))
                File.Delete(ImageSourcePath);
        }



        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
