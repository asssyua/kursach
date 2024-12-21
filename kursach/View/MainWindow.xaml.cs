using kursach.Services;
using kursach.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace kursach.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
            DataContext = mainViewModel;

            // Подписка на событие изменения страницы
            mainViewModel.OpenHomePage += OnOpenHomePage;

            // Изначально открываем домашнюю страницу
            MyFrame.Content = new Pages.HomePage();
        }

        // Обработчик события для открытия страницы HomePage
        private void OnOpenHomePage()
        {
            MyFrame.Content = new Pages.HomePage();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Pages.AboutUs();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Pages.HomePage();
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Pages.Catalog();
        }

        private void UserRequests_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Content = new Pages.UserRequests();
        }
    }
}
