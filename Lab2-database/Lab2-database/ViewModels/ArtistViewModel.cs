using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_database.DataModels;
using Lab2_database.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lab2_database.ViewModels
{
    public class ArtistViewModel: ObservableObject
    {
        private NavigationManager _navigationManager;
        private readonly DataManager _dataManager;
        public IRelayCommand NavigateConfirmCommand { get; }
        public IRelayCommand NavigateChangeCommand { get; }
        public IRelayCommand NavigateDeleteCommand { get; }
        public IRelayCommand NavigateGoBackCommand { get; }

        private string _newArtist;
        public string NewArtist
        {
            get { return _newArtist; }
            set
            {
                SetProperty(ref _newArtist, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }
        private string _newName;
        public string NewName
        {
            get { return _newName; }
            set
            {
                SetProperty(ref _newName, value);
                NavigateChangeCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableCollection<Artist> _artists;

        public ObservableCollection<Artist> Artists
        {
            get { return _artists; }
            set
            {
                _artists = value;
                OnPropertyChanged(nameof(Artists));
            }
        }

        private Artist _selectedArtist;

        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                SetProperty(ref _selectedArtist, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateChangeCommand.NotifyCanExecuteChanged();
                
                if (_selectedArtist != null)
                {
                    NewName = _selectedArtist.Name;
                }
            }
        }
        public ArtistViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            _artists = new ObservableCollection<Artist>(_dataManager.MusicLabb2Context.Artists.ToList());
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager));
            NavigateConfirmCommand = new RelayCommand(CreateArtist, () => !string.IsNullOrEmpty(NewArtist));

            NavigateChangeCommand = new RelayCommand(EditArtist, () => _selectedArtist != null);

            NavigateDeleteCommand = new RelayCommand(DeleteArtist, () => _selectedArtist != null);
        }

        private void CreateArtist()
        {
            var newArtist = new Artist()
            {
                Name = NewArtist,
                ArtistId = _dataManager.MusicLabb2Context.Artists.ToList().Count > 0 ? _dataManager.MusicLabb2Context.Artists.ToList().MaxBy(artist => artist.ArtistId).ArtistId + 1 : 1
            };
            _dataManager.MusicLabb2Context.Artists.Add(newArtist);
            _dataManager.MusicLabb2Context.SaveChanges();
            UpdateArtists();
            NewArtist = string.Empty;
        }

        private void EditArtist()
        {
            var artist = _dataManager.MusicLabb2Context.Artists.ToList()
                .Single(p => p.ArtistId == SelectedArtist.ArtistId);
            artist.Name = NewName;
            _dataManager.MusicLabb2Context.SaveChanges();
            NewName = string.Empty;
            UpdateArtists();
        }

        private void DeleteArtist()
        {
            var toDelete = _dataManager.MusicLabb2Context.Artists.Include(a=> a.Albums).ToList()
                .Single(p => p.ArtistId == SelectedArtist.ArtistId);
            _dataManager.MusicLabb2Context.Artists.Remove(toDelete);
            _dataManager.MusicLabb2Context.SaveChanges();
            NewName = string.Empty;
            UpdateArtists();
        }

        private void UpdateArtists()
        {
            Artists.Clear();
            foreach (var artist in _dataManager.MusicLabb2Context.Artists.ToList())
            {
                Artists.Add(artist);
            }
        }
    }
}
