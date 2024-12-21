using kursach.ViewModel;
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


namespace kursach.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
           homePage = new Pages.HomePage();
           MyFrame.Content = homePage;
           aboutUsPage = new Pages.AboutUs();

        }
        private Pages.HomePage homePage;
        private Pages.AboutUs aboutUsPage;

        private void HomeClick(object sender, RoutedEventArgs e)
        {   
            
            MyFrame.Content = homePage;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = aboutUsPage;
        }
    }
}
