using System;
using System.Windows;

namespace Algorithms
{
    public class City
    {
        public string Name { get; set; }
        public Point Location { get; set; }

        public City(string name, double x, double y)
        {
            Name = name;
            Location = new Point(x, y);
        }

        public double DistanceFrom(City city)
        {
            return Math.Sqrt(Math.Pow(Location.X - city.Location.X, 2) + Math.Pow(Location.Y - city.Location.Y, 2));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
