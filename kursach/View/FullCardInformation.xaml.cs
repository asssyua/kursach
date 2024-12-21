using kursach.Helpers;
using kursach.Model;
using kursach.Database;
using kursach.Services;
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

namespace kursach.View
{
    /// <summary>
    /// Логика взаимодействия для FullCardInformation.xaml
    /// </summary>
    public partial class FullCardInformation : Window
    {
        private readonly MainViewModel _mainViewModel;
        public FullCardInformation(CardInformation card)
        {
            InitializeComponent();

            // Создаем ViewModel с передачей CardInformation
            _mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
            var vm = App.ServiceProvider.GetRequiredService<FullCardViewModel>();
            vm.LoadCardInformation(card);
            DataContext = vm;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.OpenRequestWindow(null, new OnRequestWindowOpenedEventArgs((DataContext as FullCardViewModel).Card.Id));
        }
    }

}
