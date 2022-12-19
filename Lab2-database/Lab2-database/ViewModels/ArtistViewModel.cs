using System;
using System.Collections.Generic;
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

        private List<Artist> _artists;

        public List<Artist> Artists
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
                
                if (SelectedArtist != null)
                {
                    NewName = SelectedArtist.Name;
                }
            }
        }
        public ArtistViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            var context = new MusicLabb2Context();
            Artists = context.Artists.ToList();
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager));
            NavigateConfirmCommand = new RelayCommand(() =>
            {
                var newArtist = new Artist()
                {
                    Name = NewArtist,
                    ArtistId = context.Artists.ToList().Count > 0 ? context.Artists.ToList().MaxBy(artist => artist.ArtistId).ArtistId + 1 : 1
                };
                context.Artists.Add(newArtist);
                context.SaveChanges();
                NewArtist = string.Empty;
            }, () => !string.IsNullOrEmpty(NewArtist));

            NavigateChangeCommand = new RelayCommand(() =>
            {
                var artist = context.Artists.ToList()
                    .Single(p => p.ArtistId == SelectedArtist.ArtistId);
                artist.Name = NewName;
                context.SaveChanges();
                Artists = context.Artists.ToList();
            }, () => SelectedArtist != null);

            NavigateDeleteCommand = new RelayCommand(() =>
            {
                var toDelete = context.Artists.ToList()
                    .Single(p => p.ArtistId == SelectedArtist.ArtistId);
                context.Artists.Remove(toDelete);
                context.SaveChanges();
                Artists = context.Artists.ToList();
            }, () => SelectedArtist != null);
        }
    }
}
