using kursach.Helpers;
using kursach.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kursach.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public Catalog()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();


        }

        private void OnProductCardClick(object sender, MouseButtonEventArgs e)
        {
            var clickedProduct = (sender as FrameworkElement)?.DataContext as CardInformation; // Замените на тип ваших данных
            if (clickedProduct != null)
            {
                var fullCardInformationWindow = new FullCardInformation(clickedProduct);
                fullCardInformationWindow.ShowDialog();
            }
        }

        private void ToggleFilterPanel(object sender, RoutedEventArgs e)
        {
            FilterPanel.Visibility = FilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public void RequestWindowOpenedHandler(object sender, OnRequestWindowOpenedEventArgs e)
        {
            (DataContext as MainViewModel).OpenRequestWindow(sender, e);
        }

    }
}
