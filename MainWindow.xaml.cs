using System;
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
        private ObservableCollection<ApplicationDetails> programs = new ObservableCollection<ApplicationDetails>();
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
                    programs = (ObservableCollection<ApplicationDetails>)deserializer.Deserialize(openFileStream);
                }

            }
            else
            {
                openFileStream = File.Create(FileName);
            }

            openFileStream.Close();
            AppList.ItemsSource = programs;
        }

        private async void onButtonClickedStart(object sender, RoutedEventArgs e)
        {
            foreach (ApplicationDetails details in programs)
            {
                System.Diagnostics.Process.Start(details.path, details.arguments);
            }
        }

        private async void onButtonClickedAdd(object sender, RoutedEventArgs e)
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
                    Stream openFileStream = null;

                    if (File.Exists(FileName))
                    {
                        DetailsWindow details = new DetailsWindow(file);
                        details.ShowDialog();

                        if (details.applicationDetails != null)
                        {
                            File.WriteAllText(FileName, string.Empty);
                            openFileStream = File.OpenWrite(FileName);

                            programs.Add(details.applicationDetails);

                            BinaryFormatter serializer = new BinaryFormatter();
                            serializer.Serialize(openFileStream, programs);

                            openFileStream.Close();
                        }
                    }
                }
            }

            
        }
    }
}
