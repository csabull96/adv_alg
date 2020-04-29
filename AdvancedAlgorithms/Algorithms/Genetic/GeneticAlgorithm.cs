using Algorithms.HillClimbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Algorithms.Genetic
{
    public class GeneticAlgorithm
    {
        public int MaxIteration { get; set; }
        public int SizeOfPopulation { get; set; }
        public double ElitRate { get; set; }
        public double MutationRate { get; set; }

        public List<Chromosome> Population { get; set; } = new List<Chromosome>();
        public List<Chromosome> ElitePopulation { get; set; }
        public Chromosome BestChromosome { get; set; } = new Chromosome(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }) { Fitness = Double.MaxValue };

        private Route _baseRoute;
        private int[] _baseChromosome;

        public event EventHandler Update;


        public GeneticAlgorithm(int maxIteration, int sizeOfPopulation, double elitRate, double mutationRate)
        {
            MaxIteration = maxIteration;
            SizeOfPopulation = sizeOfPopulation;
            ElitRate = elitRate;
            MutationRate = mutationRate;
        }

        public void Solve(Route route)
        {
            _baseRoute = route;

            _baseChromosome = new int[route.Cities.Count];
            for (int i = 0; i < _baseChromosome.Length; i++)
            {
                _baseChromosome[i] = i;
            }

            int generations = 1;
            GenerateInitialPopultaion();
            Evaluation();

            while (generations++ < MaxIteration)
            {
                Selection();
                Crossover();
                Mutation();
                Evaluation();

                if (generations % 1 == 0)
                {
                    Thread.Sleep(100);
                    Update?.Invoke(this, EventArgs.Empty);
                }
            }
            string a = "vége";
        }

        private void GenerateInitialPopultaion()
        {
            Random random = new Random();
            for (int i = 0; i < SizeOfPopulation; i++)
            {
                Chromosome ch = new Chromosome(_baseChromosome.OrderBy(g => random.Next(_baseChromosome.Length)).ToArray());
                Population.Add(ch);
            }
        }

        private void Evaluation()
        {
            foreach (Chromosome chromosome in Population)
            {
                chromosome.Fitness = _baseRoute.TotalDistance(chromosome);

            }
        }

        private void Selection()
        {
            ElitePopulation = Population.OrderBy(ch => ch.Fitness).Take((int)(SizeOfPopulation * ElitRate)).ToList();
            if (BestChromosome.Fitness >= ElitePopulation.ElementAt(0).Fitness)
            {
                BestChromosome.Fitness = ElitePopulation.ElementAt(0).Fitness;
                BestChromosome.Genes = ElitePopulation.ElementAt(0).Genes;
            }

                Population[0] = ElitePopulation[0];

        }

        private void Crossover()
        {
            Random random = new Random();
            for (int i = 1; i < SizeOfPopulation; i++)
            {
                Chromosome father = ElitePopulation.ElementAt(random.Next(ElitePopulation.Count));
                Chromosome mother = ElitePopulation.ElementAt(random.Next(ElitePopulation.Count));

                Chromosome kid = Mate(father, mother);
                kid.Fitness = _baseRoute.TotalDistance(kid);
                Population[i] = kid;
            }

            Chromosome Mate(Chromosome father, Chromosome mother)
            {
                Chromosome offspring = new Chromosome(father.Genes.Length);

                for (int i = 0; i < father.Genes.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        offspring.Genes[i] = father.Genes.ElementAt(i);
                    }
                }
                for (int i = 0; i < mother.Genes.Length; i++)
                {
                    if (!offspring.Genes.Contains(mother.Genes[i]))
                    {
                        int index = offspring.Genes.ToList().IndexOf(-1);
                        offspring.Genes[index] = mother.Genes[i];
                    }

                }

                return offspring;
            }
        }

        private void Mutation()
        {
            Random random = new Random();
            for (int i = 1; i < Population.Count; i++)
            {
                Chromosome chromosome = Population[i];
                if (random.NextDouble() < MutationRate)
                {
                    int a = random.Next(chromosome.Genes.Length);
                    int b = random.Next(chromosome.Genes.Length);
                    while (a == b)
                    {
                        b = random.Next(chromosome.Genes.Length);
                    }
                    int temporary = chromosome.Genes.ElementAt(b);
                    chromosome.Genes[b] = chromosome.Genes.ElementAt(a);
                    chromosome.Genes[a] = temporary;
                }
            }
        }

    }
}
