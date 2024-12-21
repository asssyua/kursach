using System.Windows;
using kursach.Database;
using System.Windows.Controls;
using kursach.ViewModel;
using static kursach.ViewModel.AutorizationViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace kursach.View
{
    public partial class Autorization : Window, IClosable
    {
        public Autorization()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<AutorizationViewModel>();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox != null)
            {
                var viewModel = DataContext as AutorizationViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }
        }
    }
}
