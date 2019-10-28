using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp._3D.Models
{
    public class Coordinates
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Coordinates(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
