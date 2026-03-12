using System;
using System.Collections.Generic;
using System.Text;

namespace Simulations.Common
{
    internal struct Vector
    {
        public double X, Y;
        public Vector(double x, double y) { this.X = x; this.Y = y; }
        public static Vector operator +(Vector a, Vector b) => new (a.X + b.X, a.Y + b.Y);
        public static Vector operator -(Vector a, Vector b) => new (a.X - b.X, a.Y - b.Y);
        public static Vector operator *(Vector a, double b) => new (a.X * b, a.Y * b);
        public static Vector operator /(Vector a, double b) => new (a.X / b, a.Y / b);
        public double Magnitude => Math.Sqrt((X * X) + (Y * Y));
        public Vector Normalize
        {
            get
            {
                var mag = Magnitude;
                return mag == 0 ? Zero : new Vector(X / mag, Y / mag);
            }
        }

        public static double Distance(Vector a, Vector b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static Vector Zero => new Vector(0, 0);
    }
}
