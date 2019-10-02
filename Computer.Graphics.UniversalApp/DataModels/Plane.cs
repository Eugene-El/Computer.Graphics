using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp.DataModels
{
    public class Plane
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }

        public double[] Array { get { return new double[] { A, B, C, D }; } }
    }
}
