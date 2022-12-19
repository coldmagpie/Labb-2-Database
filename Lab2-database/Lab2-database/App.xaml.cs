using Lab2_database.Managers;
using Lab2_database.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Lab2_database
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;

        public App()
        {
            _navigationManager = new NavigationManager();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager);
            var mainWindow = new MainWindow { DataContext = new MainViewModel(_navigationManager) };
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
