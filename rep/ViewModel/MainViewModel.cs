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
    public class MainViewModel: ViewModelBase, INotifyPropertyChanged
    {
        public List<CardInformation> CardsInformation {  get; set; }
        public CardInformation SelectedCardInformation {  get; set; }
        public ICommand OpenRegistrationWindowCommand {  get; set; }
       

        public event PropertyChangedEventHandler PropertyChanged;


        public MainViewModel()
        {
            OpenRegistrationWindowCommand = new RelayCommand(openRegistrationWindow);
          
            CardInformation card1 = new CardInformation()
            {
                Title = "123",
                Description = "321",
                ImageUrl = "Images/card1.png"
            };

            CardInformation card2 = new CardInformation()
            {
                Title = "fsf",
                Description = "dgfgf",
                ImageUrl = "Images/card2.png"
            };
            CardsInformation = new List<CardInformation>() { card1, card2 };
        }

     
        public void openRegistrationWindow()
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
        }
     
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
