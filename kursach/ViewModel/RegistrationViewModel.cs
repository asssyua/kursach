using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Model;
using kursach.Database;
using kursach.View;

namespace kursach.ViewModel
{
    public class RegistrationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly AppDbContext _appDbContext;

        public ICommand OpenAuthorizationWindowCommand { get; set; }
        public ICommand RegisterCommand { get; }


        public interface IClosable
        {
            void Close();
        }

        private string _loginError;
        public string LoginError
        {
            get => _loginError;
            set
            {
                _loginError = value;
                OnPropertyChanged();
            }
        }

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string passwordHash
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged();
            }
        }

        public RegistrationViewModel(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            OpenAuthorizationWindowCommand = new RelayCommand<IClosable>(OpenAuthorizationWindow);
            RegisterCommand = new RelayCommand<IClosable>(RegisterUser);

        }

        private void OpenAuthorizationWindow(IClosable registrationWindow)
        {
            Autorization authorizationWindow = new Autorization();
            authorizationWindow.ShowDialog();

            registrationWindow?.Close();
        }

        private void RegisterUser(IClosable closableWindow)
        {
            LoginError = string.Empty;
            PasswordError = string.Empty;

            if (string.IsNullOrEmpty(Login))
            {
                LoginError = "Логин не может быть пустым";
            }
            else if (Login.Length > 20)
            {
                LoginError = "Логин не может содержать более 20 символов";
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                PasswordError = "Пароль не может быть пустым";
            }
            else if (passwordHash.Length > 20)
            {
                PasswordError = "Пароль не может содержать более 20 символов";
            }
            else if (passwordHash.Contains(" "))
            {
                PasswordError = "Пароль не может содержать пробелы";
            }
            else if (passwordHash.Length < 8)
            {
                PasswordError = "Пароль должен содержать не менее 8 символов";
            }
            if (!string.IsNullOrEmpty(LoginError) || !string.IsNullOrEmpty(PasswordError))
            {
                return;
            }

            var existingUser = _appDbContext.Users.FirstOrDefault(u => u.login == Login);
            if (existingUser != null)
            {
                LoginError = "Пользователь с таким логином уже существует";
                return;
            }

            string salt;
            string hashedPassword = PasswordHelper.HashPassword(passwordHash, out salt);

            var newUser = new User
            {
                login = Login,
                passwordHash = hashedPassword, 
                passwordSalt = salt            
            };

            _appDbContext.Users.Add(newUser);
            _appDbContext.SaveChanges();

            Login = string.Empty;
            passwordHash = string.Empty;

            MessageBox.Show("Регистрация успешна!");
            closableWindow?.Close();
        }

       

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
