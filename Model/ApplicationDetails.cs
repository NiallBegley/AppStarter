﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppStarter
{
    [Serializable()]
    public class ApplicationDetails : INotifyPropertyChanged
    {
        public string name { get; set; }
        public string path { get; set; }
        public string arguments { get; set; }


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
