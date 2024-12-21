using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kursach.Model;
using kursach.View;



namespace kursach.ViewModel
{
    public class RegistrationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand OpenAuthorizationWindowCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegistrationViewModel() 
        {
            OpenAuthorizationWindowCommand = new RelayCommand(openAuthorizationWindow);
        }
        public void openAuthorizationWindow()
        {
            Autorization authorizationWindow = new Autorization();
            authorizationWindow.Show();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
