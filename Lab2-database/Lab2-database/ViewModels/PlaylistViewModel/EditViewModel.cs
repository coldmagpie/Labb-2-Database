using Lab2_database.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_database.DataModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using Microsoft.EntityFrameworkCore;

namespace Lab2_database.ViewModels.PlaylistViewModel
{
    public class EditViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        public IRelayCommand NavigateDeleteCommand { get; }
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateChangeCommand { get; }

        public IRelayCommand NavigateModifyCommand { get; }


        private string _newName;

        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }

        private List<Playlist> _playlists;
        public List<Playlist> Playlists
        {
            get { return _playlists; }
            set
            {
                _playlists = value;
                OnPropertyChanged(nameof(Playlists));
            }
        }

        private Playlist _selectedPlaylist;

        public Playlist SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                SetProperty(ref _selectedPlaylist, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateChangeCommand.NotifyCanExecuteChanged();
                NavigateModifyCommand.NotifyCanExecuteChanged();
                if (SelectedPlaylist != null)
                {
                    NewName = SelectedPlaylist.Name;
                }
            }
        }
        public EditViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            var context = new MusicLabb2Context();
            Playlists = context.Playlists.Include(playlist => playlist.Tracks).ToList();

            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager));
            NavigateChangeCommand = new RelayCommand(() =>
            {
                var pl = context.Playlists.Include(p => p.Tracks)
                    .Single(p => p.PlaylistId == SelectedPlaylist.PlaylistId);
                pl.Name = NewName;
                context.SaveChanges();
                Playlists = context.Playlists.ToList();
            }, (() => _selectedPlaylist != null));

            NavigateDeleteCommand = new RelayCommand(() =>
            {
                var toDelete = context.Playlists.Include(p => p.Tracks)
                    .Single(p => p.PlaylistId == SelectedPlaylist.PlaylistId);
                context.Playlists.Remove(toDelete);
                context.SaveChanges();
                Playlists = context.Playlists.ToList();
            }, () => _selectedPlaylist != null);

            NavigateModifyCommand = new RelayCommand(() =>
            {
                _navigationManager.CurrentViewModel = new ModifyViewModel(_navigationManager, SelectedPlaylist);
            }, () => _selectedPlaylist != null);
        }
    }
}
