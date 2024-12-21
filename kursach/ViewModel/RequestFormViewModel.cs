using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Model;
using kursach.Database;
using kursach.Services;

namespace kursach.ViewModel
{
    public class RequestFormViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly UserService _userService;
        private readonly AppDbContext _appDbContext;

        public int CardId { get; set; }
        public ICommand SubmitCommand { get; }

        #region Поля для ввода данных

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private ComboBoxItem _rentDuration;
        public ComboBoxItem RentDuration
        {
            get => _rentDuration;
            set
            {
                _rentDuration = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Поля для ошибок

        private string _fullNameError;
        public string FullNameError
        {
            get => _fullNameError;
            set
            {
                _fullNameError = value;
                OnPropertyChanged();
            }
        }

        private string _birthDateError;
        public string BirthDateError
        {
            get => _birthDateError;
            set
            {
                _birthDateError = value;
                OnPropertyChanged();
            }
        }

        private string _emailError;
        public string EmailError
        {
            get => _emailError;
            set
            {
                _emailError = value;
                OnPropertyChanged();
            }
        }

        private string _rentDurationError;
        public string RentDurationError
        {
            get => _rentDurationError;
            set
            {
                _rentDurationError = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public RequestFormViewModel(AppDbContext appDbContext, UserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            SubmitCommand = new RelayCommand(SubmitForm);
        }

        private void SubmitForm()
        {
            FullNameError = string.Empty;
            BirthDateError = string.Empty;
            EmailError = string.Empty;
            RentDurationError = string.Empty;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(FullName))
            {
                FullNameError = "ФИО не может быть пустым.";
                hasError = true;
            }
            else
            {
                var nameParts = FullName.Trim().Split(' ');

                if (nameParts.Length != 3)
                {
                    FullNameError = "Введите полное ФИО (Фамилия Имя Отчество).";
                    hasError = true;
                }
            }

            if (BirthDate == null)
            {
                BirthDateError = "Дата рождения не может быть пустой.";
                hasError = true;
            }
            else if (BirthDate > DateTime.Today)
            {
                BirthDateError = "Дата рождения не может быть в будущем.";
                hasError = true;
            }
            else if (BirthDate < new DateTime(1920, 1, 1))
            {
                BirthDateError = "Дата рождения не может быть ранее 1 января 1920 года.";
                hasError = true;
            }
            else if (BirthDate > new DateTime(2008, 12, 31))
            {
                BirthDateError = "Вы не достигли 16 лет";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Электронная почта не может быть пустой.";
                hasError = true;
            }
            else
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

                if (!emailRegex.IsMatch(Email))
                {
                    EmailError = "Электронная почта должна соответствовать формату (example@mail.com).";
                    hasError = true;
                }
            }

            if (string.IsNullOrWhiteSpace((string)RentDuration.Content))
            {
                RentDurationError = "Выберите срок аренды.";
                hasError = true;
            }

            if (!hasError)
            {
                var userId = _userService.CurrentUserId;

                if (_appDbContext.UserRequests.Any(ur => ur.UserId == userId && ur.CardId == CardId))
                {
                    MessageBox.Show("Вы уже отправили заявку на эту квартиру");
                }
                else
                {
                    UserRequest userRequest = new UserRequest()
                    {
                        CardId = CardId,
                        UserId = userId,
                        UserEmail = Email,
                        UserFullName = FullName,
                        UserBirthDate = (DateTime)BirthDate,
                        RentalTime = (string)RentDuration.Content,
                        Status = false,
                    };

                    _appDbContext.UserRequests.Add(userRequest);
                    _appDbContext.SaveChanges();

                    MessageBox.Show("Заявка успешно отправлена!", "Успех", MessageBoxButton.OK);

                    Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)?.Close();
                }

                ClearForm();
            }
        }


        private void ClearForm()
        {
            FullName = string.Empty;
            BirthDate = null;
            Email = string.Empty;
            RentDuration = null;
        }

        #region Реализация INotifyPropertyChanged

        public event  PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
