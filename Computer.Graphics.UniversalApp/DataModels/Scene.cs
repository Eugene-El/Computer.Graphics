using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp.DataModels
{
    public class Scene
    {
        // Window configuration
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        // World configuration
        public float WorldLeft { get; set; }
        public float WorldRight { get; set; }
        public float WorldBottom { get; set; }
        public float WorldTop { get; set; }

        // Content
        public List<Coordinates> Coordinates { get; set; }
        public List<int> IndexesSequence { get; set; }
    }
}
