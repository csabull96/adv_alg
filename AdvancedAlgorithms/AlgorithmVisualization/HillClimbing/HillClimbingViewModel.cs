using Algorithms;
using Algorithms.HillClimbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgorithmVisualization.HillClimbing
{
    class HillClimbingViewModel
    {
        public event EventHandler UpdateUI;
        public event EventHandler Loaded;

        public List<Point> PointsToEnclose { get; set; }
        public Polygon BoundaryPolygon { get; set; }


        public HillClimbingViewModel()
        {
            Random random = new Random();

            PointsToEnclose = new List<Point>();
            for (int i = 0; i < 10; i++)
            {
                PointsToEnclose.Add(new Point(random.Next(20, 980), random.Next(20, 680)));
            }
            //PointsToEnclose.Add(new Point(140, 30));
            //PointsToEnclose.Add(new Point(30, 30));

            // the enclosing points has to be provided in clockwise order
            List<Point> enclosingPoints = new List<Point>();
            //enclosingPoints.Add(new Point(10, 160));
            //enclosingPoints.Add(new Point(10, 10));
            //enclosingPoints.Add(new Point(90, 10));
            //enclosingPoints.Add(new Point(160, 10));
            //enclosingPoints.Add(new Point(160, 90));
            //enclosingPoints.Add(new Point(160, 160));
            //enclosingPoints.Add(new Point(90, 160));

            //enclosingPoints.Add(new Point(10, 160));
            //enclosingPoints.Add(new Point(10, 10));
            //enclosingPoints.Add(new Point(90, 15));
            //enclosingPoints.Add(new Point(160, 10));
            //enclosingPoints.Add(new Point(150, 90));
            //enclosingPoints.Add(new Point(160, 160));
            //enclosingPoints.Add(new Point(90, 150));

            enclosingPoints.Add(new Point(10, 10));
            enclosingPoints.Add(new Point(490, 10));

            enclosingPoints.Add(new Point(990, 10));
            enclosingPoints.Add(new Point(990, 340));

            enclosingPoints.Add(new Point(990, 690));
            enclosingPoints.Add(new Point(490, 690));

            enclosingPoints.Add(new Point(10, 690));
            enclosingPoints.Add(new Point(10, 340));

            BoundaryPolygon = new Polygon(enclosingPoints);
            

            Loaded?.Invoke(this, EventArgs.Empty);
        }

        public void Solve()
        {
            HillClimbingAlgorithm hca = new HillClimbingAlgorithm(BoundaryPolygon);

            hca.UpdateUI += Hca_UpdateUI;

            
            
            hca.FindSmallestBoundaryPolygon(PointsToEnclose);

        }

        private void Hca_UpdateUI(object sender, EventArgs e)
        {
            UpdateUI?.Invoke(this, e);
        }
    }
}
