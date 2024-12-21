using kursach.Model;
using kursach.Database;
using kursach.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace kursach.ViewModel
{
    public class FullCardViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserService _userService;

        public CardInformation Card { get; private set; }

        public string Title => Card?.Title;
        public string Description => Card?.Description;
        public string ImageUrl => Card?.ImageUrl;
        public string RentalPeriod => Card?.Rental_Period;
        public decimal Price => Card?.Price ?? 0;
        public string Location => Card?.Location;

        public List<SliderImage> SliderImages { get; private set; }

        private int _currentImageIndex;
        public int CurrentImageIndex
        {
            get => _currentImageIndex;
            set
            {
                _currentImageIndex = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public string CurrentImage =>
            SliderImages != null && SliderImages.Count > 0 && _currentImageIndex >= 0
                ? SliderImages[_currentImageIndex].ImageUrl
                : null;

        public ICommand NextImageCommand { get; }
        public ICommand PrevImageCommand { get; }

        public FullCardViewModel(AppDbContext appDbContext, UserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            SliderImages = new List<SliderImage>();

            NextImageCommand = new RelayCommand(NextImage);
            PrevImageCommand = new RelayCommand(PrevImage);
        }

        public void LoadCardInformation(CardInformation card)
        {
            Card = card;

            if (Card != null)
            {
                SliderImages = _appDbContext.SliderImages
                    .Where(s => s.CardInformationId == Card.Id)
                    .ToList();

                CurrentImageIndex = SliderImages.Count > 0 ? 0 : -1; 
                OnPropertyChanged(nameof(SliderImages)); 
                OnPropertyChanged(nameof(CurrentImage)); 
            }
        }

        private void NextImage()
        {
            if (SliderImages?.Count > 0)
            {
                CurrentImageIndex = (CurrentImageIndex + 1) % SliderImages.Count;
            }
        }

        private void PrevImage()
        {
            if (SliderImages?.Count > 0)
            {
                CurrentImageIndex = (CurrentImageIndex - 1 + SliderImages.Count) % SliderImages.Count; 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
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
}
