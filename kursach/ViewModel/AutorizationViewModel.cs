using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Model;
using kursach.Database;
using kursach.Services;

namespace kursach.ViewModel
{
    public class AutorizationViewModel : ViewModelBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserService _userService;


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
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
     
        private string _loginError;
        public string LoginError
        {
            get => _loginError;
            set => Set(ref _loginError, value);
        }

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set => Set(ref _passwordError, value);
        }
        
        public interface IClosable
        {
            void Close();
        }
      
        public ICommand LoginCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AutorizationViewModel(AppDbContext appDbContext, UserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            LoginCommand = new RelayCommand<IClosable>(AuthorizeUser);
           

        }

        private void AuthorizeUser(IClosable closableWindow)
        {
            LoginError = string.Empty;
            PasswordError = string.Empty;

            if (string.IsNullOrEmpty(Login))
            {
                LoginError = "Логин не может быть пустым";
            }

            if (string.IsNullOrEmpty(Password))
            {
                PasswordError = "Пароль не может быть пустым";
            }

            if (!string.IsNullOrEmpty(LoginError) || !string.IsNullOrEmpty(PasswordError))
            {
                return;
            }

            var user = _appDbContext.Users.FirstOrDefault(u => u.login == Login);

            if (user == null || !PasswordHelper.VerifyPassword(Password, user.passwordHash, user.passwordSalt))
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }


            if (user != null)
            {
               
                 MessageBox.Show("Авторизация успешна!");
                _userService.LoginUser(user.Id);
                _userService.LoginUserName(user.login);
                
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
               
            }

            closableWindow?.Close();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
