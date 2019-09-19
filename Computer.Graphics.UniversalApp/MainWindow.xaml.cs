using Computer.Graphics.UniversalApp.DataModels;
using Computer.Graphics.UniversalApp.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Computer.Graphics.UniversalApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            scenesList.ItemsSource = new List<LoadedScene>();
        }

        private void AddExampleBtn_Click(object sender, RoutedEventArgs e)
        {
            var exampleScene = new Scene()
            {
                WindowWidth = 800,
                WindowHeight = 600,
                WorldLeft = -100,
                WorldRight = 100,
                WorldBottom = -100,
                WorldTop = 100,
                Coordinates = new List<Coordinates>() {
                    new Coordinates(-80, -80),
                    new Coordinates(-80, 80),
                    new Coordinates(80, 80),
                    new Coordinates(80, -80)
                },
                IndexesSequence = new List<int>() { -1, 2, 3, 4, 1 }
            };

            var fileName = "exmapleScene.json";
            using (StreamWriter writer = File.CreateText(fileName) )
            {
                var json = JsonConvert.SerializeObject(exampleScene);
                writer.Write(json);
            }

             AddScene(new LoadedScene(fileName));
        }

        private void AddScene(LoadedScene loadedScene)
        {
            scenesList.ItemsSource = new List<LoadedScene>(
                scenesList.ItemsSource as List<LoadedScene>
                ) { loadedScene };
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var loadedScene = (sender as Button)?.DataContext as LoadedScene;
            if (loadedScene != null)
            {
                new VisualWindow(loadedScene.Scene);
            }

        }
    }
}
