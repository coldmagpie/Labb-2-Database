using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab2_database.DataModels;
using Lab2_database.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lab2_database.ViewModels.TrackViewModel
{
    public class EditTrackViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private readonly DataManager _dataManager;
        public IRelayCommand NavigateDeleteCommand { get; }
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateConfirmCommand { get; }


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

        private List<int> _newMinutes = new List<int>();

        public List<int> NewMinutes
        {
            get { return _newMinutes; }
            set { _newMinutes = value; }
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

        private List<int> _newSeconds = new List<int>();

        public List<int> NewSeconds
        {
            get { return _newSeconds; }
            set { _newSeconds = value; }
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

        private ObservableCollection<Track> _tracks;
        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                OnPropertyChanged(nameof(Tracks));
            }
        }

        private Track _selectedTrack;
         
        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                SetProperty(ref _selectedTrack, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateConfirmCommand.NotifyCanExecuteChanged();
                if (SelectedTrack != null)
                {
                    NewName = _selectedTrack.Name;
                }
            }
        }
        public EditTrackViewModel(NavigationManager navigationManager, DataManager dataManager)
        {
            _navigationManager = navigationManager;
            _dataManager = dataManager;
            _tracks =  new ObservableCollection<Track>(_dataManager.MusicLabb2Context.Tracks.ToList());

            for (int i = 1; i < 61; i++)
            {
                _newMinutes.Add(i);
            }

            for (int i = 0; i < 61; i++)
            {
                _newSeconds.Add(i);
            }

            NavigateConfirmCommand = new RelayCommand(EditTrack, CanExecute);
            NavigateDeleteCommand = new RelayCommand(DeleteTrack, () => _selectedTrack != null);
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _dataManager));
        }

        private void EditTrack()
        {
            var track = _dataManager.MusicLabb2Context.Tracks.ToList()
                .Single(t => t.TrackId == SelectedTrack.TrackId);
            track.Name = NewName;
            track.Milliseconds = SelectedMinute * 60000 + SelectedSecond * 1000;
            _dataManager.MusicLabb2Context.SaveChanges();
            UpdateTracks();
            NewName = string.Empty;
        }

        private void DeleteTrack()
        {
            var toDelete = _dataManager.MusicLabb2Context.Tracks.ToList()
                .Single(t => t.TrackId == SelectedTrack.TrackId);
            _dataManager.MusicLabb2Context.Tracks.Remove(toDelete);
            _dataManager.MusicLabb2Context.SaveChanges();
           UpdateTracks();
            NewName = string.Empty;
        }
        private void UpdateTracks()
        {
            Tracks.Clear();
            foreach (var track in _dataManager.MusicLabb2Context.Tracks.ToList())
            {
                Tracks.Add(track);
            }
        }
        private bool CanExecute()
        {

            return !string.IsNullOrEmpty(NewName) && _selectedMinute != default;
        }
    }
}
