using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Algorithms.HillClimbing
{
    public class HillClimbingAlgorithm
    {
        public Polygon BoundaryPolygon { get; set; }

        public double CurrentArea { get; set; }

        public event EventHandler UpdateUI;

        public List<Point> PointsToEnclose { get; set; }

        public HillClimbingAlgorithm(Polygon boundaryPolygon)
        {
            BoundaryPolygon = boundaryPolygon;
            var sth = BoundaryPolygon.GetVisualSides();
            CurrentArea = boundaryPolygon.Area();
        }

        public void FindSmallestBoundaryPolygon(List<Point> points)
        {
            int step = 0;
            PointsToEnclose = points;

            int vertices = BoundaryPolygon.Vertices.Count;
            bool developed = true;

            while (developed)
            {
                developed = false;
                for (int v = 0; v < vertices; v++)
                {
                    step++;
                    Point vertex = BoundaryPolygon.Vertices.ElementAt(v);
                    Point bestNeighbour = FindBestNeighbour(vertex, v);

                    if (!vertex.Equals(bestNeighbour))
                    {
                        BoundaryPolygon.ExchangeVertexAt(v, bestNeighbour);
                        //v--;
                        developed = true;

                        Thread.Sleep(5);
                        UpdateUI?.Invoke(this,  new HillClimbingEventArgs(new Polygon(BoundaryPolygon)));
                    }
                }
            }
        }

        private Point FindBestNeighbour(Point point, int index)
        {
            Point bestNeighbour = point;
            double area = BoundaryPolygon.Area();
            
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Point neighbour = new Point(point.X + x, point.Y + y);
                    Polygon copy = new Polygon(BoundaryPolygon);
                    if (!copy.Vertices.Any(v => v.Equals(neighbour)))
                    {
                        copy.ExchangeVertexAt(index, neighbour);
                        double ar = copy.Area();
                        if (0 < ar && ar < area && copy.IsPolygon() && copy.Encloses(PointsToEnclose))
                        {
                            if (copy.Encloses(PointsToEnclose))
                            {
                                bestNeighbour = neighbour;
                                area = copy.Area();
                            }
                        }
                    }
                    
                }
            }

            return bestNeighbour;
        } 
    }
}
