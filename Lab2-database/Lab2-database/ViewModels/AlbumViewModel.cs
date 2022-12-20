using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_database.DataModels;
using Lab2_database.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lab2_database.ViewModels
{
    public class AlbumViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private readonly DataManager _dataManager;
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

        private ObservableCollection<Artist> _artists;

        public ObservableCollection<Artist> Artists
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

        private ObservableCollection<Album> _albums;

        public ObservableCollection<Album> Albums
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

                if (_selectedAlbum != null)
                {
                    NewTitle = _selectedAlbum.Title;
                }
            }
        }

        public AlbumViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            _albums = new ObservableCollection<Album>(_dataManager.MusicLabb2Context.Albums.ToList());
            _artists = new ObservableCollection<Artist>(_dataManager.MusicLabb2Context.Artists.ToList());

            NavigateGoBackCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager));

            NavigateConfirmCommand = new RelayCommand(CreateAlbum,
                () => !string.IsNullOrEmpty(NewAlbum) && _selectedArtist != null);

            NavigateChangeCommand = new RelayCommand(EditAlbum, () => _selectedAlbum != null);

            NavigateDeleteCommand = new RelayCommand(DeleteAlbum, () => _selectedAlbum != null);
        }

        private void DeleteAlbum()
        {
            var toDelete = _dataManager.MusicLabb2Context.Albums.ToList()
                .Single(p => p.AlbumId == SelectedAlbum.AlbumId);
            _dataManager.MusicLabb2Context.Albums.Remove(toDelete);
            _dataManager.MusicLabb2Context.SaveChanges(); 
            UpdateAlbums();
            NewTitle = string.Empty;
        }

        private void EditAlbum()
        {
            var album = _dataManager.MusicLabb2Context.Albums.ToList()
                .Single(p => p.AlbumId == SelectedAlbum.AlbumId);
            album.Title = NewTitle;
            _dataManager.MusicLabb2Context.SaveChanges();
            UpdateAlbums();
            NewTitle = string.Empty;
        }

        private void CreateAlbum()
        {
            var newAlbum = new Album()
            {
                Title = NewAlbum,
                AlbumId = _dataManager.MusicLabb2Context.Albums.ToList().Count > 0 ? _dataManager.MusicLabb2Context.Albums.ToList().MaxBy(album => album.AlbumId).AlbumId + 1 : 1,
                ArtistId = _dataManager.MusicLabb2Context.Artists.ToList()
                    .Single(a => a.ArtistId == SelectedArtist.ArtistId).ArtistId
            };
            _dataManager.MusicLabb2Context.Albums.Add(newAlbum);
            _dataManager.MusicLabb2Context.SaveChanges();
            UpdateAlbums();
            NewAlbum = string.Empty;
        }

        private void UpdateAlbums()
        {
            Albums.Clear();
            foreach (var album in _dataManager.MusicLabb2Context.Albums.ToList())
            {
                Albums.Add(album);
            }
        }
    }
}

