using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp._3D.Models
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
        public float WorldForward { get; set; }
        public float WorldBack { get; set; }

        // Content
        public List<Parallelepiped> Parallelepipeds { get; set; }
    }
}
