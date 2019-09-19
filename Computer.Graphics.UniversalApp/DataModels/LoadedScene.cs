using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp.DataModels
{
    public class LoadedScene
    {
        public DateTime LoadDateTime { get; private set; }
        public string StringDateTime { get { return LoadDateTime.ToString("dd.MM.yyyy HH':'mm':'ss"); } }
        public int PointCount { get { return Scene.Coordinates.Count; } }
        public string Path { get; private set; }
        public Scene Scene { get; private set; }

        public LoadedScene(string path)
        {
            Path = path;
            LoadDateTime = DateTime.Now;
            string json = File.ReadAllText(path);
            Scene = JsonConvert.DeserializeObject<Scene>(json);
        }
    }
}
