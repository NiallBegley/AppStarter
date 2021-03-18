using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AppStarter
{
    public partial class DetailsWindow : Window
    {
        public ApplicationDetails ApplicationDetails {  get;   set; }

        public DetailsWindow(string path)
        {
            InitializeComponent();

            string filename = System.IO.Path.GetFileName(path);
            displayNameTextBox.Text = filename;
            pathTextBox.Text = path;

            ApplicationDetails = null;

        }

        public DetailsWindow(ApplicationDetails details)
        {
            InitializeComponent();

            displayNameTextBox.Text = details.Name;
            pathTextBox.Text = details.Path;
            argumentsTextBox.Text = details.Arguments;

            ApplicationDetails = details;
        }

        private void onButtonClickedSave(object sender, RoutedEventArgs e)
        {
            Save();

            Close();
        }
        
        private void Save()
        {
            if (ApplicationDetails == null)
            {
                ApplicationDetails = new ApplicationDetails { Name = displayNameTextBox.Text, Path = pathTextBox.Text, Arguments = argumentsTextBox.Text };
            }
            else
            {
                ApplicationDetails.Name = displayNameTextBox.Text;
                ApplicationDetails.Path = pathTextBox.Text;
                ApplicationDetails.Arguments = argumentsTextBox.Text;
            }

            DialogResult = true;
              
        }
        
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Save();
                Close();
            }
        }
        private void OnButtonClickedCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
