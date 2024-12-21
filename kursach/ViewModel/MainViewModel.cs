using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Helpers;
using kursach.Model;
using kursach.Database;
using kursach.Services;
using kursach.View;
using kursach.View.Pages;

namespace kursach.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserService _userService;

        public int CurrentUserId => _userService.CurrentUserId;
        public string CurrentUserLogin => _userService.CurrentUserLogin;


        public string UserAction => CurrentUserId == -1 ? "Вход/Регистрация" : "Выйти";
        
       

        public bool IsGuest => CurrentUserId == -1;
        public bool IsInterfaceEnabled => !IsGuest;
        public bool IsAdmin => _userService.CurrentUserLogin == "admin";


        public ObservableCollection<CardInformation> FullCardsInformation { get; set; }

        private ObservableCollection<CardInformation> _cardsInformation;
        public ObservableCollection<CardInformation> CardsInformation
        {
            get => _cardsInformation;
            set
            {
                _cardsInformation = value;
                RaisePropertyChanged(nameof(CardsInformation));
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                RaisePropertyChanged(nameof(SearchQuery));
            }
        }

        private ObservableCollection<CardInformation> _randomFullCatalog;
        public ObservableCollection<CardInformation> RandomFullCatalog
        {
            get => _randomFullCatalog;
            set
            {
                _randomFullCatalog = value;
                RaisePropertyChanged(nameof(RandomFullCatalog));
            }
        }

        public ICommand OpenRegistrationWindowCommand { get; set; }
        public ICommand RefreshRandomCardsCommand { get; set; }
        public ICommand LoadFullRandomCatalogCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand OpenAdminWindowCommand {  get; set; }

        public ICommand ApplyFilterCommand { get; set; }


        public ICommand AddCardCommand { get; }
        public ICommand DeleteCardCommand { get; }



        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(AppDbContext appDbContext, UserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;

            OpenRegistrationWindowCommand = new RelayCommand(OpenRegistrationWindow);
            RefreshRandomCardsCommand = new RelayCommand(LoadRandomCards);
            LoadFullRandomCatalogCommand = new RelayCommand(LoadFullRandomCatalog);
            SearchCommand = new RelayCommand(FilterCards);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            OpenAdminWindowCommand = new RelayCommand(OpenAdminWindow);



            AddCardCommand = new RelayCommand(AddCard, CanExecuteAdminCommand);
            DeleteCardCommand = new RelayCommand(DeleteCard, CanExecuteAdminCommand);


            FullCardsInformation = new ObservableCollection<CardInformation>(_appDbContext.CardInformations.ToList());
            CardsInformation = new ObservableCollection<CardInformation>(FullCardsInformation);


            FilterViewModel = new FilterViewModel(appDbContext, FullCardsInformation);

            _userService.PropertyChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(CurrentUserLogin));
                OnPropertyChanged(nameof(CurrentUserId));
                OnPropertyChanged(nameof(UserAction));
                OnPropertyChanged(nameof(IsGuest));
                OnPropertyChanged(nameof(IsInterfaceEnabled));
                OnPropertyChanged(nameof(IsAdmin));
            };

            LoadRandomCards();
            LoadFullRandomCatalog();
        }
        private bool CanExecuteAdminCommand()
        {
            return IsAdmin;
        }
        private void AddCard()
        {
            if (IsAdmin)
            {
                // Логика добавления карточки
                var newCard = new CardInformation
                {
                    Title = "Новая карточка",
                    Description = "Описание новой карточки",
                    // Заполнение других полей
                };
                _appDbContext.CardInformations.Add(newCard);
                _appDbContext.SaveChanges();
                FullCardsInformation.Add(newCard); // Обновляем коллекцию
                CardsInformation = new ObservableCollection<CardInformation>(FullCardsInformation);
            }
            else
            {
                MessageBox.Show("Функция добавления карточки доступна только администратору.");
            }
        }

        private void DeleteCard()
        {
            if (IsAdmin)
            {
                var selectedCard = CardsInformation.FirstOrDefault(); // Выбор карточки для удаления
                if (selectedCard != null)
                {
                    _appDbContext.CardInformations.Remove(selectedCard);
                    _appDbContext.SaveChanges();
                    FullCardsInformation.Remove(selectedCard); // Обновляем коллекцию
                    CardsInformation = new ObservableCollection<CardInformation>(FullCardsInformation);
                }
            }
            else
            {
                MessageBox.Show("Функция удаления карточки доступна только администратору.");
            }
        }

        public void LoadRandomCards()
        {
            var random = new Random();

            CardsInformation = new ObservableCollection<CardInformation>(
                FullCardsInformation
                    .OrderBy(x => random.Next())
                    .Distinct()
                    .Take(6)
            );
        }

        public void LoadFullRandomCatalog()
        {
            var random = new Random();

            RandomFullCatalog = new ObservableCollection<CardInformation>(
                FullCardsInformation
                    .OrderBy(x => random.Next())
                    .ToList()
                    .Distinct()
            );
        }

        private void FilterCards()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                CardsInformation = new ObservableCollection<CardInformation>(FullCardsInformation);
            }
            else
            {
                var filteredItems = FullCardsInformation
                    .Where(item =>
                        (item.Title != null && item.Title.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (item.Description != null && item.Description.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                CardsInformation.Clear();
                foreach (var item in filteredItems)
                {
                    CardsInformation.Add(item);
                }
            }
        }


        public FilterViewModel FilterViewModel { get; }

        public void ApplyFilter()
        {
            // Применяем фильтрацию
            FilterViewModel.ApplyFilter();

            // Обновляем RandomFullCatalog из отфильтрованной коллекции
            RandomFullCatalog.Clear();
            foreach (var card in FilterViewModel.FilteredCards)
            {
                RandomFullCatalog.Add(card);
            }

            // Сообщаем UI об изменении
            RaisePropertyChanged(nameof(RandomFullCatalog));
        }




        public void OpenRegistrationWindow()
        {
            if (CurrentUserId == -1)
            {
                // Логика для открытия окна регистрации
                Registration registrationWindow = new Registration();
                registrationWindow.ShowDialog();
            }
            else
            {
                // Логика выхода пользователя
                _userService.LogoutUser(); // Очистка данных пользователя
                OpenHomePage?.Invoke();    // Открытие страницы HomePage
            }
        }

        public event Action OpenHomePage;
        public void OpenRequestWindow(object sender, OnRequestWindowOpenedEventArgs e)
        {
            if (CurrentUserId == -1)
            {
                MessageBox.Show("Вы не можете арендовать квартиру, не авторизовавшись");
            }
            else
            {

                RequestForm requestForm = new RequestForm(e.CardId);
                requestForm.ShowDialog();
            }
        }

        public void OpenAdminWindow()
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.ShowDialog();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
