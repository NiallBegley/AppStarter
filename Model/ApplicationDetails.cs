using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppStarter
{
    [Serializable()]
    public class ApplicationDetails : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private string _arguments;
        private bool _isSelected;
        public string Name 
        { 
            get { return _name; } 
            set {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                if (_path == value)
                    return;
                _path = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Path"));
            }
        }
        public string Arguments
        {
            get { return _arguments; }
            set
            {
                if (_arguments == value)
                    return;
                _arguments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Arguments"));
            }
        }
        public bool IsSelected {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);

        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
