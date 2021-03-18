using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppStarter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ApplicationDetails> Programs = new ObservableCollection<ApplicationDetails>();
        private string FileName = null;

        public MainWindow()
        {
            InitializeComponent();

            FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Applications.bin";
            Stream openFileStream = null;

            if (File.Exists(FileName))
            {
                openFileStream = File.OpenRead(FileName);
                if (openFileStream.Length != 0)
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    Programs = (ObservableCollection<ApplicationDetails>)deserializer.Deserialize(openFileStream);
                }

            }
            else
            {
                openFileStream = File.Create(FileName);
            }

            openFileStream.Close();
            AppList.ItemsSource = Programs;
        }

        private void OnButtonClickedStart(object sender, RoutedEventArgs e)
        {
            foreach (ApplicationDetails details in AppList.SelectedItems)
            {
                System.Diagnostics.Process.Start(details.Path, details.Arguments);
            }
        }

        private void OnButtonClickedDelete(object sender, RoutedEventArgs e)
        {
            MenuItem sen = (MenuItem)sender;
            ApplicationDetails detail = (ApplicationDetails) sen.DataContext;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to delete the selected item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Programs.Remove(detail);
                WriteToDisk();
            }
        }

        private void OnButtonClickedEdit(object sender, RoutedEventArgs e)
        {
            MenuItem sen = (MenuItem)sender;
            ApplicationDetails detail = (ApplicationDetails)sen.DataContext;

            DetailsWindow detailsDialog = new DetailsWindow(detail);
            bool? success = detailsDialog.ShowDialog();

            if (success == true && detailsDialog.ApplicationDetails != null)
            {
                WriteToDisk();
            }
        }

        private void OnButtonClickedAdd(object sender, RoutedEventArgs e)
        {
            var picker = new Microsoft.Win32.OpenFileDialog();
            picker.Filter = "Executables(.exe) | *.exe";
            picker.DefaultExt = ".exe";

            bool? result = picker.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string file = picker.FileName;
                if (file != null)
                {
                    if (File.Exists(FileName))
                    {
                        DetailsWindow detailsDialog = new DetailsWindow(file);
                        detailsDialog.ShowDialog();

                        if (detailsDialog.ApplicationDetails != null)
                        {
                            Programs.Add(detailsDialog.ApplicationDetails);
                            WriteToDisk();
                        }
                    }
                }
            }
        }

        private void WriteToDisk()
        {

            Stream openFileStream = null;

            File.WriteAllText(FileName, string.Empty);
            openFileStream = File.OpenWrite(FileName);

            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(openFileStream, Programs);

            openFileStream.Close();
        }
    }
}
