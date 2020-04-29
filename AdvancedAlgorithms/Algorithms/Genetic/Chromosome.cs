using System.Text;

namespace Algorithms.Genetic
{
    public class Chromosome
    {
        public int[] Genes { get; set; }
        public double Fitness { get; set; }


        public Chromosome(int lengthOfGene)
        {
            Genes = new int[lengthOfGene];
            for (int i = 0; i < lengthOfGene; i++)
            {
                Genes[i] = -1;
            }
        }

        public Chromosome(int[] genes)
        {
            Genes = genes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int gene in Genes)
            {
                sb.Append(gene + " ");
            }
            return sb.ToString();
        }
    }
}
