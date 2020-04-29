using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.FunctionApproximation
{
    public class Swarm
    {
        public List<Individual> Population { get; set; }
        public Vector GlobalOpt { get; set; } = new Vector(1);

        private Random random = new Random();

        private const double POS_RANGE = 100;
        private const double VEL_RANGE = 50;

        public Swarm()
        {
            Population = new List<Individual>();
        }

        public Swarm(int size)
        {

            Population = new List<Individual>();

            for (int i = 0; i < size; i++)
            {
                Vector position = GeneratePosition();
                Vector velocity = GenerateVelocity();
                Population.Add(new Individual(position, velocity));
            }
        }

        private Vector GeneratePosition()
        {
            double x = random.NextDouble() * POS_RANGE;// - POS_RANGE / 2;
            double y = random.NextDouble() * POS_RANGE;// - POS_RANGE / 2;
            double z = random.NextDouble() * POS_RANGE;// - POS_RANGE / 2;
            return new Vector(x, y, z);
        }

        private Vector GenerateVelocity()
        {
            double x = random.NextDouble() * VEL_RANGE - VEL_RANGE / 2;
            double y = random.NextDouble() * VEL_RANGE - VEL_RANGE / 2;
            double z = random.NextDouble() * VEL_RANGE - VEL_RANGE / 2;
            return new Vector(x, y, z);
        }
    }
    
}
