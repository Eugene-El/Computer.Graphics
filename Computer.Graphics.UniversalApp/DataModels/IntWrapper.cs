using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer.Graphics.UniversalApp.DataModels
{
    public class IntWrapper
    {
        public IntWrapper(int value)
        {
            Value = value;
        }
        public IntWrapper() : this(0) { }

        public int Value { get; set; }
    }
}
