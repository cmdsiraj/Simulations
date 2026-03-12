using System;
using System.Collections.Generic;
using System.Text;

namespace Simulations.Common.Structs
{
    internal struct World
    {
        public double leftMargin, rightMargin, topMargin, bottomMargin;

        public World(double left, double right, double top, double bottom)
        {
            this.leftMargin = left;
            this.rightMargin = right;
            this.topMargin = top;
            this.bottomMargin = bottom;
        }
    }
}
