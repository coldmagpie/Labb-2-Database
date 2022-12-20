using Lab2_database.Managers;
using Lab2_database.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Lab2_database.DataModels;

namespace Lab2_database
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly DataManager _dataManager;

        public App()
        {
            _navigationManager = new NavigationManager();
            _dataManager = new DataManager(new MusicLabb2Context());
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager);
            var mainWindow = new MainWindow { DataContext = new MainViewModel(_navigationManager) };
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
