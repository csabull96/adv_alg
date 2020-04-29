using Algorithms.Genetic;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.HillClimbing
{
    public class Route
    {
        public List<City> Cities { get; set; }

        public Route(List<City> cities)
        {
            Cities = cities;
        }

        public double TotalDistance()
        {
            double totalDistance = 0;

            for (int i = 0; i < Cities.Count(); i++)
            {
                totalDistance += Cities.ElementAt(i).DistanceFrom(Cities.ElementAt((i + 1) % Cities.Count()));
            }

            return totalDistance;
        }

        public double TotalDistance(Chromosome chromosome)
        {
            double totalDistance = 0;
            for (int i = 0; i < chromosome.Genes.Length; i++)
            {
                int a = chromosome.Genes[i];
                int b = chromosome.Genes[(i + 1) % chromosome.Genes.Count()];

                totalDistance += Cities.ElementAt(a).DistanceFrom(Cities.ElementAt(b));
            }
            return totalDistance;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (City city in Cities)
            {
                sb.Append(city.Name + " - ");
            }
            sb.Append("Total Distance: " + TotalDistance());
            return sb.ToString();
        }
    }
}
