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
        public int Id { get; set; }
        public DateTime LoadDateTime { get; private set; }
        public string StringDateTime { get { return LoadDateTime.ToString("dd.MM.yyyy HH':'mm':'ss"); } }
        public int PointCount { get { return Scene.Coordinates.Count; } }
        public string Path { get; private set; }
        public Scene Scene { get; private set; }

        public LoadedScene(string path)
        {
            Id = ++CurrentId;
            Path = path;
            LoadDateTime = DateTime.Now;
            string json = File.ReadAllText(path);
            Scene = JsonConvert.DeserializeObject<Scene>(json);

            if (Scene.Coordinates == null) Scene.Coordinates = new List<Coordinates>();
            if (Scene.IndexesSequence == null) Scene.IndexesSequence = new List<IntWrapper>();
            if (Scene.Planes == null) Scene.Planes = new List<Plane>();
        }

        public static int CurrentId = 0;
    }
}
