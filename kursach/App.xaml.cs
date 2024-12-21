using kursach.Database;
using kursach.Services;
using kursach.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();

            services.AddScoped<AppDbContext>();  
            services.AddScoped<UserService>();

            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<AutorizationViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<FullCardViewModel>();
            services.AddTransient<RequestFormViewModel>();
            services.AddTransient<UserRequestsViewModel>();
            services.AddTransient<AdminViewModel>();
            services.AddTransient<AddCardViewModel>();
            services.AddTransient<FilterViewModel>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
        }
    }
}
