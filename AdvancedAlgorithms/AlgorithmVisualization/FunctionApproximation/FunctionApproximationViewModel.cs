using Algorithms.FunctionApproximation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace AlgorithmVisualization.FunctionApproximation
{
    class FunctionApproximationViewModel : INotifyPropertyChanged
    {
        public List<Point> Solutions { get; set; }
        public Swarm ViewModelSwarm { get; set; } = new Swarm();

        public event EventHandler UpdateUI;

        public FunctionApproximationViewModel()
        {
            Solutions = new List<Point>();

            Solutions.Add(new Point(-10, -15.5));
            Solutions.Add(new Point(10, 4.5));
            Solutions.Add(new Point(-20, 12));
        }

        public void Solve()
        {
            FunctionApproximationAlgorithm fa = new FunctionApproximationAlgorithm(1000);
            ViewModelSwarm = fa.MySwarm;
            fa.Update += Fa_Update;
            fa.Solve(Solutions);
        }

        private void Fa_Update(object sender, EventArgs e)
        {
            //raise property canged??
            UpdateUI?.Invoke(this, EventArgs.Empty);
            RaisePropertyChanged(nameof(ViewModelSwarm));
        }

        public double TheFunction(double x)
        {
            // (1/8)(x + 4)^2 - 20
            // 0.125 x^2 + 1x + 2 - 20 
            double a = 0.125;
            double b = 1;
            double c = -18;

            return SecondDegreeFunction(a, b, c, x);
        }

        public double ApproxFunction(double x)
        {
            double a = ViewModelSwarm.GlobalOpt.X;
            double b = ViewModelSwarm.GlobalOpt.Y;
            double c = ViewModelSwarm.GlobalOpt.Z;

            return SecondDegreeFunction(a, b, c, x);
        }

        private double SecondDegreeFunction(double a, double b, double c, double x)
        {
            return a * Math.Pow(x, 2) + b * x + c;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
