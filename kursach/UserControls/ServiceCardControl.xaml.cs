using GalaSoft.MvvmLight.Command;
using kursach.Helpers;
using kursach.Model;
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

namespace kursach.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ServiceCardControl.xaml
    /// </summary>
    public partial class ServiceCardControl : UserControl
    {
        public ServiceCardControl()
        {
            InitializeComponent();
        }

        public event EventHandler<OnRequestWindowOpenedEventArgs> RequestWindowOpened;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Вызываем событие, которое будет обработано на родительской странице
            RequestWindowOpened?.Invoke(this, new OnRequestWindowOpenedEventArgs((DataContext as CardInformation).Id));
        }
    }
}
