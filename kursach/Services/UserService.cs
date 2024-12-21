using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Services
{
    
        public class UserService : INotifyPropertyChanged
        {
            private int _currentUserId = -1;
            private string _currentUserLogin = "Гость";

            public int CurrentUserId
            {
                get => _currentUserId;
                private set
                {
                    if (_currentUserId != value)
                    {
                        _currentUserId = value;
                        OnPropertyChanged(nameof(CurrentUserId));
                    }
                }
            }

            public string CurrentUserLogin
            {
                get => _currentUserLogin;
                private set
                {
                    if (_currentUserLogin != value)
                    {
                        _currentUserLogin = value;
                        OnPropertyChanged(nameof(CurrentUserLogin));
                    }
                }
            }

            public void LoginUser(int userId)
            {
                CurrentUserId = userId;
            }

            public void LoginUserName(string userLogin)
            {
                CurrentUserLogin = userLogin;
            }

            public void LogoutUser()
            {
                CurrentUserId = -1;
                CurrentUserLogin = "Гость";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    

}
