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
    /// Логика взаимодействия для RequestForm.xaml
    /// </summary>
    public partial class RequestForm : Window
    {
        public RequestForm(int cardId)
        {
            InitializeComponent();

            var vm = App.ServiceProvider.GetRequiredService<RequestFormViewModel>();
            vm.CardId = cardId;
            DataContext = vm;
        }
    }
}
