using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_database.Managers;
using Lab2_database.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lab2_database.ViewModels.PlaylistViewModel
{
    public class ModifyViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateAddCommand { get; }
        public IRelayCommand NavigateRemoveCommand { get; }

        private string _playlistName;
        public string PlaylistName
        {
            get { return _playlistName; }
            set { _playlistName = value; }
        }

        private ObservableCollection<Track> _tracksInPlaylist;
        public ObservableCollection<Track> TracksInPlaylist
        {
            get { return _tracksInPlaylist; }
            set
            {
                _tracksInPlaylist = value;
                OnPropertyChanged(nameof(TracksInPlaylist));
            }
        }

        private ObservableCollection<Track> _tracks;
        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
            set { _tracks = value; }
        }
        private Track _selectedTrackInPlaylist;

        public Track SelectedTrackInPlaylist
        {
            get { return _selectedTrackInPlaylist; }
            set
            {
                SetProperty(ref _selectedTrackInPlaylist, value);
                NavigateRemoveCommand.NotifyCanExecuteChanged();
            }
        }

        private Track _selectedTrack;
        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                SetProperty(ref _selectedTrack, value);
                NavigateAddCommand.NotifyCanExecuteChanged();
            }
        }

        public ModifyViewModel(NavigationManager navigationManager, Playlist playlist)
        {
            _navigationManager = navigationManager;
            _playlistName = playlist.Name;
            var context = new MusicLabb2Context();
            TracksInPlaylist = new ObservableCollection<Track>(playlist.Tracks.ToList());
            Tracks = new ObservableCollection<Track>(context.Tracks);

            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditViewModel(_navigationManager));
            NavigateAddCommand = new RelayCommand(() =>
            {
                if (SelectedTrack is null)
                {
                    return;
                }

                var tempPlaylist = context.Playlists.Include(p => p.Tracks).Single(p => p.PlaylistId == playlist.PlaylistId);

                if (tempPlaylist.Tracks.Any(t => t.TrackId == SelectedTrack.TrackId))
                {
                    return;
                }
                tempPlaylist.Tracks.Add(SelectedTrack);
                context.SaveChanges();
                TracksInPlaylist = new ObservableCollection<Track>(context.Playlists.Single(p => p.PlaylistId == playlist.PlaylistId).Tracks.ToList());
            });

            NavigateRemoveCommand = new RelayCommand(() =>
            {
                if (SelectedTrackInPlaylist is null)
                {
                    return;
                }
                var tempPlaylist = context.Playlists.Include(p => p.Tracks).Single(p => p.PlaylistId == playlist.PlaylistId);
                tempPlaylist.Tracks.Remove(SelectedTrackInPlaylist);
                context.SaveChanges();
                TracksInPlaylist = new ObservableCollection<Track>(context.Playlists.Single(p => p.PlaylistId == playlist.PlaylistId).Tracks.ToList());
            });
        }
    }
}
