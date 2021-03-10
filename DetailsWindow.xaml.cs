using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AppStarter
{
    partial class DetailsWindow : Window
    {
        public DetailsWindow(string path)
        {
            InitializeComponent();

            string filename = System.IO.Path.GetFileName(path);
            displayNameTextBox.Text = filename;
            pathTextBox.Text = path;

        }

        private void onButtonClickedSave(object sender, RoutedEventArgs e)
        {

        }
        
        private void onButtonClickedCancel(object sender, RoutedEventArgs e)
        {

        }
    }
}
