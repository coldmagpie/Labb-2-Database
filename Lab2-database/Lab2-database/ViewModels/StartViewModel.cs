using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Lab2_database.Managers;
using Lab2_database.DataModels;
using Lab2_database.ViewModels.PlaylistViewModel;
using Lab2_database.ViewModels.TrackViewModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lab2_database.ViewModels
{
    public class StartViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private readonly DataManager _dataManager;
        public IRelayCommand NavigateCreateCommand { get; }
        public IRelayCommand NavigateEditCommand { get; }
        public IRelayCommand NavigateArtistCommand { get; }
        public IRelayCommand NavigateAlbumCommand { get; }
        public IRelayCommand NavigateCreateTrackCommand { get; }
        public IRelayCommand NavigateEditTrackCommand { get; }


        public StartViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            NavigateCreateCommand =new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateViewModel(_navigationManager, _dataManager));
            NavigateEditCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditViewModel(_navigationManager, _dataManager));
            NavigateArtistCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new ArtistViewModel(_navigationManager, _dataManager));
            NavigateAlbumCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new AlbumViewModel(_navigationManager, _dataManager));
            NavigateCreateTrackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateTrackViewModel(_navigationManager, _dataManager));
            NavigateEditTrackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditTrackViewModel(_navigationManager, _dataManager));

        }
    }
}
