
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using kursach.Model;
using kursach.Database;
using kursach.View;
using Microsoft.Win32;

namespace kursach.ViewModel
{
    public class AddCardViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _appDbContext;

        private string title;
        private string description;
        private string imageUrl;
        private string rentalPeriod;
        private decimal price;
        private string location;

       
        private string titleError;
        private string descriptionError;
        private string imageUrlError;
        private string rentalPeriodError;
        private string priceError;
        private string locationError;

        public ICommand ChooseImageCommand { get; }
        public ICommand AddCardCommand { get; }

        
        public CardInformation CardInformation { get; set; }

        public AddCardViewModel(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            CardInformation = new CardInformation();

            ChooseImageCommand = new CommandHandler(() => ChooseImage());
            AddCardCommand = new CommandHandler(() => AddCard(), () => CanAddCard());

            titleError = descriptionError = imageUrlError = rentalPeriodError = priceError = locationError = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                    ValidateTitle();
                    CommandManager.InvalidateRequerySuggested(); 
                }
            }
        }


        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                    ValidateDescription();
                }
            }
        }

        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));
                    ValidateImageUrl();
                }
            }
        }

        public string RentalPeriod
        {
            get => rentalPeriod;
            set
            {
                if (rentalPeriod != value)
                {
                    rentalPeriod = value;
                    OnPropertyChanged(nameof(RentalPeriod));
                    ValidateRentalPeriod();
                }
            }
        }

        public decimal Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                    ValidatePrice();
                }
            }
        }

        public string Location
        {
            get => location;
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                    ValidateLocation();
                }
            }
        }

        private void ValidateTitle()
        {
            var titleRegex = new Regex(@"^\d+-комнатная квартира$"); 

            if (string.IsNullOrWhiteSpace(Title))
            {
                TitleError = "Название не может быть пустым";
            }
            else if (!titleRegex.IsMatch(Title))
            {
                TitleError = "Название должно быть в формате: N-комнатная квартира (например, 1-комнатная квартира)";
            }
            else
            {
                TitleError = string.Empty; 
            }

            CommandManager.InvalidateRequerySuggested();
        }


        private void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                DescriptionError = "Описание не может быть пустым";
            }
            else if (Description.Length > 300)
            {
                DescriptionError = "Описание не может превышать 300 символов";
            }
            else
            {
                DescriptionError = string.Empty; 
            }

            CommandManager.InvalidateRequerySuggested(); 
        }


        private void ValidateImageUrl()
        {
            ImageUrlError = string.IsNullOrWhiteSpace(ImageUrl) ? "Изображение не выбрано" : string.Empty;
            CommandManager.InvalidateRequerySuggested();  
        }

        private void ValidateRentalPeriod()
        {
            RentalPeriodError = string.IsNullOrWhiteSpace(RentalPeriod) ? "Период аренды не может быть пустым" : string.Empty;
            CommandManager.InvalidateRequerySuggested();  
        }

        private void ValidatePrice()
        {
            PriceError = Price <= 0 ? "Цена должна быть больше нуля" : string.Empty;
            CommandManager.InvalidateRequerySuggested();  
        }

        private void ValidateLocation()
        {
           
            if (string.IsNullOrWhiteSpace(Location))
            {
                LocationError = "Расположение не может быть пустым";
            }
            else if (LocationError.Length > 300)
            {
                LocationError = "Описание не может превышать 300 символов";
            }
            else
            {
                LocationError = string.Empty;
            }

            CommandManager.InvalidateRequerySuggested();  
        }

        private void ValidateAll()
        {
            ValidateTitle();
            ValidateDescription();
            ValidateImageUrl();
            ValidateRentalPeriod();
            ValidatePrice();
            ValidateLocation();
        }

        // Command handlers
        private void ChooseImage()
        {
            // Создание диалогового окна
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            // Показываем диалоговое окно
            if (openFileDialog.ShowDialog() == true)
            {
                ImageUrl = openFileDialog.FileName; // Сохраняем путь к выбранному файлу
            }
        }

        private bool CanAddCard()
        {
            return string.IsNullOrEmpty(TitleError) &&
                   string.IsNullOrEmpty(DescriptionError) &&
                   string.IsNullOrEmpty(ImageUrlError) &&
                   string.IsNullOrEmpty(RentalPeriodError) &&
                   string.IsNullOrEmpty(PriceError) &&
                   string.IsNullOrEmpty(LocationError);
        }



        private void AddCard()
        {
            ValidateAll();

            if (!CanAddCard()) {
                MessageBox.Show("Некорректное заполнение формы");
                return;
            }

            var newCard = new CardInformation
            {
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                Rental_Period = RentalPeriod,
                Price = Price,
                Location = Location,
                SliderImages = new List<SliderImage>()
            };

            _appDbContext.CardInformations.Add(newCard);
            _appDbContext.SaveChanges();

            MessageBox.Show("Квартира одобрена!");

            ClearFields();
        }

        private void ClearFields()
        {
            Title = Description = ImageUrl = RentalPeriod = Location = string.Empty;
            Price = 0;
            TitleError = DescriptionError = ImageUrlError = RentalPeriodError = PriceError = LocationError = string.Empty;
        }

    private class CommandHandler : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public CommandHandler(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
            public void Execute(object parameter) => _execute();

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }

        private void SetError(ref string errorField, string errorMessage, string propertyName)
        {
            if (errorField != errorMessage)
            {
                errorField = errorMessage;
                OnPropertyChanged(propertyName);
            }
        }

        public string TitleError
        {
            get => titleError;
            set
            {
                if (titleError != value)
                {
                    titleError = value;
                    OnPropertyChanged(nameof(TitleError));
                    CommandManager.InvalidateRequerySuggested(); 
                }
            }
        }


        public string DescriptionError
        {
            get => descriptionError;
            set => SetError(ref descriptionError, value, nameof(DescriptionError));
        }

        public string ImageUrlError
        {
            get => imageUrlError;
            set => SetError(ref imageUrlError, value, nameof(ImageUrlError));
        }

        public string RentalPeriodError
        {
            get => rentalPeriodError;
            set => SetError(ref rentalPeriodError, value, nameof(RentalPeriodError));
        }

        public string PriceError
        {
            get => priceError;
            set => SetError(ref priceError, value, nameof(PriceError));
        }

        public string LocationError
        {
            get => locationError;
            set => SetError(ref locationError, value, nameof(LocationError));
        }

    }
}