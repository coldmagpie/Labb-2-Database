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

        public CreateViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            var context = new MusicLabb2Context();
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager));
            NavigateConfirmCommand = new RelayCommand(() =>
            {
                var newPlaylist = new Playlist()
                {
                    Name = NewName,
                    PlaylistId = context.Playlists.ToList().Count > 0 ? context.Playlists.ToList().MaxBy(playlist => playlist.PlaylistId).PlaylistId + 1 : 1
                };
                context.Playlists.Add(newPlaylist);
                context.SaveChanges();
                NewName = string.Empty;
            }, () => !string.IsNullOrEmpty(NewName));
        }
    }

}
