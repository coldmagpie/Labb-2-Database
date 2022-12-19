using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_database.DataModels;
using Lab2_database.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lab2_database.ViewModels
{
    public class AlbumViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        public IRelayCommand NavigateConfirmCommand { get; }
        public IRelayCommand NavigateChangeCommand { get; }
        public IRelayCommand NavigateDeleteCommand { get; }
        public IRelayCommand NavigateGoBackCommand { get; }

        private string _newAlbum;
        public string NewAlbum
        {
            get { return _newAlbum; }
            set
            {
                SetProperty(ref _newAlbum, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private List<Artist> _artists;

        public List<Artist> Artists
        {
            get { return _artists; }
            set { _artists = value; }
        }

        private Artist _selectedArtist;

        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                _selectedArtist = value;
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private string _newTitle;
        public string NewTitle
        {
            get { return _newTitle; }
            set
            {
                SetProperty(ref _newTitle, value);
                NavigateChangeCommand.NotifyCanExecuteChanged();
            }
        }

        private List<Album> _albums;

        public List<Album> Albums
        {
            get { return _albums; }
            set
            {
                _albums = value;
                OnPropertyChanged(nameof(Albums));
            }
        }

        private Album _selectedAlbum;

        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                SetProperty(ref _selectedAlbum, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateChangeCommand.NotifyCanExecuteChanged();

                if (SelectedAlbum != null)
                {
                    NewTitle = SelectedAlbum.Title;
                }
            }
        }
        public AlbumViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            var context = new MusicLabb2Context();
            Albums = context.Albums.ToList();
            Artists= context.Artists.ToList();
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager));
            NavigateConfirmCommand = new RelayCommand(() =>
            {
                var newAlbum = new Album()
                {
                    Title = NewAlbum,
                    AlbumId = context.Albums.ToList().Count > 0 ? context.Albums.ToList().MaxBy(album => album.AlbumId).AlbumId + 1 : 1,
                    ArtistId = context.Artists.ToList().Single(a=>a.ArtistId == SelectedArtist.ArtistId).ArtistId
                };
                context.Albums.Add(newAlbum);
                context.SaveChanges();
                NewAlbum = string.Empty;
            }, () => !string.IsNullOrEmpty(NewAlbum) && _selectedArtist != null);

            NavigateChangeCommand = new RelayCommand(() =>
            {
                var album = context.Albums.ToList()
                    .Single(p => p.AlbumId == SelectedAlbum.AlbumId);
                album.Title = NewTitle;
                context.SaveChanges();
                Albums = context.Albums.ToList();
                NewTitle = string.Empty;
            }, () => _selectedAlbum != null);

            NavigateDeleteCommand = new RelayCommand(() =>
            {
                var toDelete = context.Albums.ToList()
                    .Single(p => p.AlbumId == SelectedAlbum.AlbumId);
                context.Albums.Remove(toDelete);
                context.SaveChanges();
                Albums = context.Albums.ToList();
                NewTitle = string.Empty;
            }, () => _selectedAlbum != null);
        }
    }
}

