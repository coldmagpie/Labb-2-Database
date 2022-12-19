using System;
using System.Collections.Generic;
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

        private List<Track> _tracks;
        public List<Track> Tracks
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
        public EditTrackViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            var context = new MusicLabb2Context();
            Tracks =  context.Tracks.ToList();

            for (int i = 1; i < 61; i++)
            {
                NewMinutes.Add(i);
            }

            for (int i = 0; i < 61; i++)
            {
                NewSeconds.Add(i);
            }

            NavigateConfirmCommand = new RelayCommand(() =>
            {
                var track = context.Tracks.ToList()
                    .Single(t => t.TrackId == SelectedTrack.TrackId);
                track.Name = NewName;
                track.Milliseconds = SelectedMinute * 60000 + SelectedSecond * 1000;
                context.SaveChanges();
                Tracks = context.Tracks.ToList();
                NewName = string.Empty;
            }, CanExecute);
            NavigateDeleteCommand = new RelayCommand(() =>
            {
                var toDelete = context.Tracks.ToList()
                    .Single(t => t.TrackId == SelectedTrack.TrackId);
                context.Tracks.Remove(toDelete);
                context.SaveChanges();
                Tracks = context.Tracks.ToList();
                NewName = string.Empty;
            }, () => _selectedTrack != null);
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager));
        }
        private bool CanExecute()
        {

            return !string.IsNullOrEmpty(NewName) && _selectedMinute != default;
        }
    }
}
