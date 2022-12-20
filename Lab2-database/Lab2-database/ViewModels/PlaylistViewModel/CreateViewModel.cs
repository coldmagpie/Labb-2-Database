using Lab2_database.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Lab2_database.DataModels;

namespace Lab2_database.ViewModels.PlaylistViewModel
{
    public class CreateViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private readonly DataManager _dataManager;

        public IRelayCommand NavigateGoBackCommand { get; }

        public IRelayCommand NavigateConfirmCommand { get; }

        private string _newName;

        public string NewName
        {
            get { return _newName; }
            set
            {
                SetProperty(ref _newName, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        public CreateViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager));
            NavigateConfirmCommand = new RelayCommand(CreatePlaylist, () => !string.IsNullOrEmpty(NewName));
        }

        private void CreatePlaylist()
        {
            var newPlaylist = new Playlist()
            {
                Name = NewName,
                PlaylistId = _dataManager.MusicLabb2Context.Playlists.ToList().Count > 0 ? _dataManager.MusicLabb2Context.Playlists.ToList().MaxBy(playlist => playlist.PlaylistId).PlaylistId + 1 : 1
            };
            _dataManager.MusicLabb2Context.Playlists.Add(newPlaylist);
            _dataManager.MusicLabb2Context.SaveChanges();
            NewName = string.Empty;
        }
    }

}
