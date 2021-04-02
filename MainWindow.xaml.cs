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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace AppStarter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ApplicationDetails> Programs = new ObservableCollection<ApplicationDetails>();
        private string PreferencesFile = null;
        private string LocalApplicationDataDirectory = null;
        public MainWindow()
        {
            InitializeComponent();

            LocalApplicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AppStarter");
            PreferencesFile = Path.Combine(LocalApplicationDataDirectory,"Applications.bin");
            Stream openFileStream = null;

            if(!Directory.Exists(LocalApplicationDataDirectory))
            {
                Directory.CreateDirectory(LocalApplicationDataDirectory);
            }

            if (File.Exists(PreferencesFile))
            {
                openFileStream = File.OpenRead(PreferencesFile);
                if (openFileStream.Length != 0)
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    Programs = (ObservableCollection<ApplicationDetails>)deserializer.Deserialize(openFileStream);
                }

            }
            else
            {
                openFileStream = File.Create(PreferencesFile);
            }

            openFileStream.Close();
            AppList.ItemsSource = Programs;
        }

        private void OnButtonClickedStart(object sender, RoutedEventArgs e)
        {
            foreach (ApplicationDetails details in Programs)
            {
                if(details.IsSelected)
                {
                    System.Diagnostics.Process.Start(details.Path, details.Arguments);
                }
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
                detail.DeleteIcon();

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
                    DetailsWindow detailsDialog = new DetailsWindow(file);
                    detailsDialog.ShowDialog();

                    if (detailsDialog.ApplicationDetails != null)
                    {
                        //Extract the icon from the executable
                        using (System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(file))
                        {
                            //Create an image from the icon 
                            BitmapSource iconImage = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                            string iconPath = Path.Combine(LocalApplicationDataDirectory, detailsDialog.ApplicationDetails.Name + ".png");

                            //Save the icon to disk
                            using (var fileStream = new FileStream(iconPath, FileMode.Create))
                            {
                                PngBitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(iconImage));
                                encoder.Save(fileStream);
                                fileStream.Close();

                                //Set the icon path, which will cause the icon to load from disk automatically
                                detailsDialog.ApplicationDetails.IconPath = iconPath;
                            }
                        }

                        Programs.Add(detailsDialog.ApplicationDetails);
                        WriteToDisk();
                    }
                }
            }
        }

        private void WriteToDisk()
        {

            Stream openFileStream = null;

            File.WriteAllText(PreferencesFile, string.Empty);
            openFileStream = File.OpenWrite(PreferencesFile);

            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(openFileStream, Programs);

            openFileStream.Close();
        }
    }
}
