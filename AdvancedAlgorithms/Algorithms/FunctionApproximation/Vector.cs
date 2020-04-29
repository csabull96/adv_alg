using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.FunctionApproximation
{
    public class Vector
    {
        public double X { get { return VectorAsList.ElementAt(0); } }
        public double Y { get { return VectorAsList.ElementAt(1); } }
        public double Z { get { return VectorAsList.ElementAt(2); } }

        public double Length { get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(X, 2) + Math.Pow(X, 2)); } }

        public double Fitness { get; set; }

        public List<double> VectorAsList { get; set; }


        public Vector(Vector source)
        {

            VectorAsList = new List<double>();

            Fitness = double.MaxValue;

            foreach (double coordinate in source.VectorAsList)
            {
                VectorAsList.Add(coordinate);
            }
        }

        public Vector(double x = 0, double y = 0, double z = 0)
        {
            VectorAsList = new List<double>() { x, y, z };

            Fitness = double.MaxValue;
        }
        
        public Vector()
        {
            VectorAsList = new List<double>() { 0, 0, 0 };
            Fitness = double.MaxValue;
        }

        public void Add(Vector vector)
        {

            VectorAsList[0] += vector.X;
            VectorAsList[1] += vector.Y;
            VectorAsList[2] += vector.Z;
        }

    }
}
