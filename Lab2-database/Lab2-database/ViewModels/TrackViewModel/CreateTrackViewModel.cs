using Lab2_database.Managers;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Lab2_database.DataModels;

namespace Lab2_database.ViewModels.TrackViewModel
{
    public class CreateTrackViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private readonly DataManager _dataManager;
        public IRelayCommand NavigateConfirmCommand { get; }
        public IRelayCommand NavigateGoBackCommand { get; }

        private string _newTrack;
        public string NewTrack
        {
            get { return _newTrack; }
            set
            {
                SetProperty(ref _newTrack, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private List<Album> _albums;
        public List<Album> Albums
        {
            get { return _albums; }
            set { _albums = value; }
        }

        private Album _selectedAlbum;

        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private List<MediaType> _mediaTypes;
        public List<MediaType> MediaTypes
        {
            get { return _mediaTypes; }
            set { _mediaTypes = value; }
        }

        private MediaType _selectedMediaType;

        public MediaType SelectedMediaType
        {
            get { return _selectedMediaType; }
            set
            {
                _selectedMediaType = value;
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private List<Genre> _genres;
        public List<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }

        private Genre _selectedGenre;

        public Genre SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }
        private string _trackComposer;
        public string TrackComposer
        {
            get { return _trackComposer; }
            set
            {
                SetProperty(ref _trackComposer, value);
            }
        }

        private List<int> _minutes = new List<int>();
        public List<int> Minutes
        {
            get { return _minutes; }
            set { _minutes = value; }
        }

        private int _selectedMinute;

        public int SelectedMinute
        {
            get { return _selectedMinute; }
            set
            {
                _selectedMinute = value;
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }
        private List<int> _seconds = new List<int>();
        public List<int> Seconds
        {
            get { return _seconds; }
            set { _seconds = value; }
        }

        private int _selectedSecond;

        public int SelectedSecond
        {
            get { return _selectedSecond; }
            set
            {
                _selectedSecond = value;
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private double price = 0.99;

        public CreateTrackViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            _albums = _dataManager.MusicLabb2Context.Albums.ToList();
            _mediaTypes = _dataManager.MusicLabb2Context.MediaTypes.ToList();
            _genres = _dataManager.MusicLabb2Context.Genres.ToList();

            for (int i = 1; i < 61; i++)
            {
                Minutes.Add(i);
            }

            for (int i = 0; i < 61; i++)
            {
                Seconds.Add(i);
            }

            NavigateConfirmCommand = new RelayCommand(CreateTrack, CanExecute);
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager));
        }

        private void CreateTrack()
        {
            var newTrack = new Track()
            {
                TrackId = _dataManager.MusicLabb2Context.Tracks.ToList().Count > 0 ? _dataManager.MusicLabb2Context.Tracks.ToList().MaxBy(track => track.TrackId).TrackId + 1 : 1,
                Name = NewTrack,
                AlbumId = _dataManager.MusicLabb2Context.Albums.ToList().Single(a => a.AlbumId == SelectedAlbum.AlbumId).AlbumId,
                MediaTypeId = _dataManager.MusicLabb2Context.MediaTypes.ToList().Single(m => m.MediaTypeId == SelectedMediaType.MediaTypeId).MediaTypeId,
                GenreId = _dataManager.MusicLabb2Context.Genres.ToList().Single(g => g.GenreId == SelectedGenre.GenreId).GenreId,
                Composer = _trackComposer is null ? "-" : TrackComposer,
                Milliseconds = _selectedMinute * 60000 + _selectedSecond * 1000,
                UnitPrice = price
            };
            _dataManager.MusicLabb2Context.Tracks.Add(newTrack);
            _dataManager.MusicLabb2Context.SaveChanges();
            NewTrack = string.Empty;
            TrackComposer = string.Empty;
        }

        private bool CanExecute()
        {

            return !string.IsNullOrEmpty(NewTrack) && _selectedAlbum != null && _selectedMediaType != null &&
                   _selectedMinute != default && _selectedGenre != null;
        }

    }
}
