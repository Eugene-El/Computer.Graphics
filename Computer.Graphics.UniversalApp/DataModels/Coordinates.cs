﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp.DataModels
{
    public class Coordinates
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Coordinates(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Coordinates() : this(0, 0) { }

    }
}
