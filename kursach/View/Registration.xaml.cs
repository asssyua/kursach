using kursach.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static kursach.ViewModel.RegistrationViewModel;

namespace kursach.View
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window, IClosable
    {
        public Registration()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<RegistrationViewModel>();
         
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

            // Передаем значение пароля из PasswordBox в ViewModel
            var viewModel = DataContext as RegistrationViewModel;
            if (viewModel != null)
            {
                viewModel.passwordHash = PasswordBox.Password;
            }
        }
    }
}
