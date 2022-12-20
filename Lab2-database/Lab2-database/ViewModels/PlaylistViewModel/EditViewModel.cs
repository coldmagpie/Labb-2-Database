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
        private readonly DataManager _dataManager;
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

        private ObservableCollection<Playlist> _playlists;
        public ObservableCollection<Playlist> Playlists
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
        public EditViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            _playlists = new ObservableCollection<Playlist>(_dataManager.MusicLabb2Context.Playlists.Include(playlist => playlist.Tracks).ToList());

            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager));
            NavigateChangeCommand = new RelayCommand(EditPlaylist, () => _selectedPlaylist != null);

            NavigateDeleteCommand = new RelayCommand(DeletePlaylist, () => _selectedPlaylist != null);

            NavigateModifyCommand = new RelayCommand(() =>
            {
                _navigationManager.CurrentViewModel = new ModifyViewModel(_navigationManager, SelectedPlaylist, _dataManager);
            }, () => _selectedPlaylist != null);
        }

        private void EditPlaylist()
        {
            var pl = _dataManager.MusicLabb2Context.Playlists.Include(p => p.Tracks)
                .Single(p => p.PlaylistId == SelectedPlaylist.PlaylistId);
            pl.Name = NewName;
            _dataManager.MusicLabb2Context.SaveChanges();
            NewName = string.Empty;
            UpdatePlaylists();
        }

        private void DeletePlaylist()
        {
            var toDelete = _dataManager.MusicLabb2Context.Playlists.Include(p => p.Tracks)
                .Single(p => p.PlaylistId == SelectedPlaylist.PlaylistId);
            _dataManager.MusicLabb2Context.Playlists.Remove(toDelete);
            _dataManager.MusicLabb2Context.SaveChanges();
            NewName = string.Empty;
            UpdatePlaylists();
        }
        private void UpdatePlaylists()
        {
            Playlists.Clear();
            foreach (var playlist in _dataManager.MusicLabb2Context.Playlists.ToList())
            {
                Playlists.Add(playlist);
            }
        }
    }
}
