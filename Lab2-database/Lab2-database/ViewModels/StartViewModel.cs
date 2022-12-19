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
        public IRelayCommand NavigateCreateCommand { get; }
        public IRelayCommand NavigateEditCommand { get; }
        public IRelayCommand NavigateArtistCommand { get; }
        public IRelayCommand NavigateAlbumCommand { get; }
        public IRelayCommand NavigateCreateTrackCommand { get; }
        public IRelayCommand NavigateEditTrackCommand { get; }


        public StartViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            NavigateCreateCommand =new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateViewModel(_navigationManager));
            NavigateEditCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditViewModel(_navigationManager));
            NavigateArtistCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new ArtistViewModel(_navigationManager));
            NavigateAlbumCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new AlbumViewModel(_navigationManager));
            NavigateCreateTrackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateTrackViewModel(_navigationManager));
            NavigateEditTrackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditTrackViewModel(_navigationManager));

        }
    }
}
