using Computer.Graphics.UniversalApp.DataModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Computer.Graphics.UniversalApp.Edit
{
    /// <summary>
    /// Interaction logic for SceneEdit.xaml
    /// </summary>
    public partial class SceneEdit : Window
    {
        public Scene Scene { get; private set; }

        public SceneEdit(ref Scene scene)
        {
            InitializeComponent();
            Scene = scene;
            DataContext = Scene;
        }

        private void PlanesGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.Items.Count >= 6)
            {
                grid.CanUserAddRows = false;
            }
        }
    }
}
