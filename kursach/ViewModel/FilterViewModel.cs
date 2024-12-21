using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Model;
using kursach.Database;

namespace kursach.ViewModel
{
    public class FilterViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private readonly AppDbContext _appDbContext;

        private readonly ObservableCollection<CardInformation> _allCards;

        public ObservableCollection<CardInformation> FilteredCards
        {
            get => _filteredCards;
            set
            {
                _filteredCards = value;
                RaisePropertyChanged(nameof(FilteredCards));
            }
        }

        public string SelectedTitle { get; set; }
        private string _selectedRentalPeriod;
        public string SelectedRentalPeriod
        {
            get => _selectedRentalPeriod;
            set
            {
                _selectedRentalPeriod = value;
                RaisePropertyChanged(nameof(SelectedRentalPeriod));
                MessageBox.Show($"SelectedRentalPeriod: {_selectedRentalPeriod}"); // Для отладки
            }
        }


        private double _minPrice;
        public double MinPrice
        {
            get => _minPrice;
            set
            {
                _minPrice = value;
                RaisePropertyChanged(nameof(MinPrice));
            }
        }

        private double _maxPrice;
        public double MaxPrice
        {
            get => _maxPrice;
            set
            {
                _maxPrice = value;
                RaisePropertyChanged(nameof(MaxPrice));
            }
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand ApplyFilterCommand { get; }

        public FilterViewModel(AppDbContext appDbContext, ObservableCollection<CardInformation> allCards)
        {
            _appDbContext = appDbContext;
            _allCards = allCards;
            FilteredCards = new ObservableCollection<CardInformation>(allCards);

            ApplyFilterCommand = new RelayCommand(ApplyFilter);
        }

        private ObservableCollection<CardInformation> _filteredCards;



        public void ApplyFilter()
        {
            // Начинаем с полной коллекции
            var filtered = _allCards.AsEnumerable();

            // Фильтрация по количеству комнат
            if (!string.IsNullOrWhiteSpace(SelectedTitle))
            {
                filtered = filtered.Where(card => card.Title != null
                    && card.Title.Equals(SelectedTitle, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по сроку аренды
            if (!string.IsNullOrWhiteSpace(SelectedRentalPeriod))
            {
                filtered = filtered.Where(card => card.Rental_Period != null
                && card.Rental_Period.Equals(SelectedRentalPeriod, StringComparison.OrdinalIgnoreCase));
            }

            // Фильтрация по минимальной цене
            if (MinPrice > 0)
            {
                filtered = filtered.Where(card => card.Price >= (decimal)MinPrice);
            }

            // Фильтрация по максимальной цене
            if (MaxPrice > 0)
            {
                filtered = filtered.Where(card => card.Price <= (decimal)MaxPrice);
            }

            // Обновляем отфильтрованный список
            FilteredCards = new ObservableCollection<CardInformation>(filtered);

            // Оповещаем об изменении
            RaisePropertyChanged(nameof(FilteredCards));

            // Отладка
           
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
