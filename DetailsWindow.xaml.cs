using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AppStarter
{
    public partial class DetailsWindow : Window
    {
        public ApplicationDetails applicationDetails {  get;   set; }

        public DetailsWindow(string path)
        {
            InitializeComponent();

            string filename = System.IO.Path.GetFileName(path);
            displayNameTextBox.Text = filename;
            pathTextBox.Text = path;

            applicationDetails = null;

        }

        private void onButtonClickedSave(object sender, RoutedEventArgs e)
        {
            applicationDetails = new ApplicationDetails { name = displayNameTextBox.Text, path = pathTextBox.Text, arguments = argumentsTextBox.Text };
            Close();
        }
        
        private void onButtonClickedCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
