using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Algorithms.FunctionApproximation
{
    public class FunctionApproximationAlgorithm
    {
        private const int DIMENSIONS = 3;

        public Swarm MySwarm { get; set; }
        public List<Point> Solutions { get; set; }
        private Random random = new Random();


        private const double w = 0.8;
        private const double op = 0.35;
        private const double og = 0.35;

        public event EventHandler Update;

        public FunctionApproximationAlgorithm(int sizeOfPopulation)
        {
            GenerateSwarm(sizeOfPopulation);
        }

        public void Solve(List<Point> solutions)
        {
            Solutions = solutions;

            Evaluation();
            int i = 0;
            while (i < 1000)
            {
                CalculateVelocity();
                DoMovement();
                Evaluation();
                i++;
                Thread.Sleep(50);
                Update?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GenerateSwarm(int size)
        {
            MySwarm = new Swarm(size);
        }

        private double Evaluation()
        {
            foreach (Individual individual in MySwarm.Population)
            {
                double fitness = CalculateFitness(individual);
                if (fitness <= individual.LocalOpt.Fitness)
                {
                    individual.LocalOpt = new Vector(individual.Position);
                    individual.LocalOpt.Fitness = fitness;

                    if (fitness < MySwarm.GlobalOpt.Fitness)
                    {
                        MySwarm.GlobalOpt = new Vector(individual.Position);
                        MySwarm.GlobalOpt.Fitness = fitness;
                    }
                }
                
            }

            return double.MinValue;
        }

        private double CalculateFitness(Individual individual)
        {
            double result = 0;

            foreach (Point solution in Solutions)
            {
                double shouldBe = solution.Y;
                double actual = SecondDegreeFunction(individual.Position.X, individual.Position.Y, individual.Position.Z, solution.X);
                result += Math.Abs(shouldBe - actual);
            }

            return result;
        }


      

        private double SecondDegreeFunction(double a, double b, double c, double x)
        {
            return a * Math.Pow(x, 2) + b * x + c;
        }



        private void CalculateVelocity()
        {
            foreach (Individual individual in MySwarm.Population)
            {
                for (int i = 0; i < DIMENSIONS; i++)
                {
                    double rp = random.NextDouble();
                    double rg = random.NextDouble();

                    individual.Velocity.VectorAsList[i] = w * individual.Velocity.VectorAsList[i] +
                        op * rp * (individual.LocalOpt.VectorAsList[i] - individual.Position.VectorAsList[i]) +
                        og * rg * (MySwarm.GlobalOpt.VectorAsList[i] - individual.Position.VectorAsList[i]);
                }
            }

        }

        private void DoMovement()
        {
            foreach (Individual individual in MySwarm.Population)
            {
                individual.Move();
            }
        }
    }
}
