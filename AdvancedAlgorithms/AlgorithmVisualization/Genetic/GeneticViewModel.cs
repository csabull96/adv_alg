using Algorithms;
using Algorithms.Genetic;
using Algorithms.HillClimbing;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AlgorithmVisualization.Genetic
{
    class GeneticViewModel : INotifyPropertyChanged
    {
        public int MaxGenerations { get; set; } = 100;
        public int PopulationSize { get; set; } = 100;
        public double ElitRate { get; set; } = 0.2;
        public double MutationRate { get; set; } = 0.8;

        public int CurrentGeneration { get; set; } = 1;

        public List<City> Cities { get; set; }
        public Chromosome ShortestRouteFound { get; set; }

        public bool Started { get; set; }

        public GeneticViewModel()
        {
            GenerateCities(10);

            Loaded?.Invoke(this, EventArgs.Empty);
        }

        private void GenerateCities(int count)
        {
            Cities = new List<City>();
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                Cities.Add(new City("city #" + i, random.Next(10, 990), random.Next(10, 690)));
            }
        }

        public event EventHandler UpdateUI;
        public event EventHandler Loaded;
       
        public void Solve()
        {
            CurrentGeneration = 1;
            Route route = new Route(Cities);
            GeneticAlgorithm ga = new GeneticAlgorithm(MaxGenerations, PopulationSize, ElitRate, MutationRate);
            ShortestRouteFound = ga.BestChromosome;
            ga.Update += Ga_Update;
            Started = true;
            ga.Solve(route);
            Started = false;
            
        }

        private void Ga_Update(object sender, EventArgs e)
        {
            CurrentGeneration++;
            RaisePropertyChanged(nameof(ShortestRouteFound));
            RaisePropertyChanged(nameof(CurrentGeneration));
            UpdateUI?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
