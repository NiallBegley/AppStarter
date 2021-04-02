using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AppStarter
{
    [Serializable()]
    public class ApplicationDetails : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private string _arguments;
        private bool _isSelected;
        private string _iconPath;

        [field: NonSerialized]
        private BitmapImage _icon;

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

        public BitmapImage Icon
        {
            get { return _icon; }
            set
            {
                if (_icon == value)
                    return;
                _icon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Icon"));
            }
        }

        public string IconPath
        {
            get { return _iconPath; }
            set
            {
                if (_iconPath == value)
                    return;
                _iconPath = value;

                LoadIcon();
                OnPropertyChanged(new PropertyChangedEventArgs("IconPath"));
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

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            LoadIcon();
        }

        private void LoadIcon()
        {
            _icon = new BitmapImage();
            using (FileStream stream = File.OpenRead(_iconPath))
            {
                _icon.BeginInit();
                _icon.CacheOption = BitmapCacheOption.OnLoad;
                _icon.StreamSource = stream;
                _icon.EndInit();
            }
        }

        public void DeleteIcon()
        {
            File.Delete(_iconPath);
        }
    }
}
