namespace Algorithms.FunctionApproximation
{
    public class Individual
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public Vector LocalOpt { get; set; }

        public Individual(Vector position, Vector velocity)
        {
            Position = position;
            Velocity = velocity;
            LocalOpt = new Vector();
        }

        public void Move()
        {
            Position.Add(Velocity);
        }
    }
}
