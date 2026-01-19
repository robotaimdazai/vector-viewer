using Microsoft.Win32;
using System.IO;
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
using WSCAD.IO;
using WSCAD.Rendering;

namespace WSCAD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISceneReader _sceneReader = new JsonSceneReader();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void  OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialogue = new OpenFileDialog 
            {
                Title = "Select JSON file",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                CheckFileExists = true
            };

            if (dialogue.ShowDialog(this) != true) return;

            try
            {
                await using var fs = File.OpenRead(dialogue.FileName);
                var scene = await _sceneReader.ReadAsync(fs,CancellationToken.None);
                SceneView.Scene = scene;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this, ex.Message, 
                    "Failed to load scene",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }
    }
}