using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Algorithms
{
    public enum Orientation { Clockwise, CounterClockwise, Colinear }

    public class Polygon
    {
        private const int DRAWING_SCREEN_WIDTH = int.MaxValue;

        public List<Point> Vertices { get; private set; }
        public List<Side> Sides { get; private set; } = new List<Side>();

 

        public Polygon(List<Point> vertices)
        {
            Vertices = vertices;
            GetSidesFromVertices();
        }

        public Polygon(Polygon source)
        {
            Vertices = new List<Point>();
            foreach (Point vertex in source.Vertices)
            {
                Point copy = new Point(vertex.X, vertex.Y);
                Vertices.Add(copy);
            }
            GetSidesFromVertices();
        }

        public void ExchangeVertexAt(int index, Point vertex)
        {
            Vertices[index] = vertex;
            GetSidesFromVertices();
        }


        public bool IsVertex(Point point)
        {
            foreach (Point vertex in Vertices)
            {
                if (vertex.Equals(point)) return true;
            }
            return false;
        }


        public List<Side> GetVisualSides()
        {
            // if there are many consecutive horizontal sides they have to be united
            //       (0) (1)  (2)  (3)
            //        o---o----o----o             o-------------o  (1) o     o---------o
            //       /              |            /              |      |\   /          |
            // (10) ■               |           o               |      | \ /           |
            //      |          (5)  |   ===>    |               |      |  o            o
            //  (9) o           o---o (4)       o           o---o      |               |
            //      |          /                |          /           |               |
            //      o----o----o                 o---------o        (0) o-------o-------o
            //     (8)  (7)  (6)
            // the vertical ones could be united, too bot not necessarily
            List<Side> result = new List<Side>();

            List<Point> points = new List<Point>();

            Point start = Vertices.OrderBy(v => v.X).ThenBy(v => v.Y).First();
            points.Add(start);
            int id = Vertices.IndexOf(start);
           
            for (int i = id + 1; i < Vertices.Count + id; i++)
            {
                Point next = Vertices.ElementAt(i % Vertices.Count);
                if (start.Y != next.Y)
                {
                    points.Add(next);
                    start = next;
                }
                else
                {
                    if (start.Y != Vertices.ElementAt((i + 1) % Vertices.Count).Y)
                    {
                        points.Add(next);
                        start = next;
                    }
                    else
                    {
                        start = next;
                    }
                    // if they're equal
                }
            }
            //result.Add(new Side(count++, side.Start, side.End));

            // this is going to be the ■ vertex (the y axis increases downwards)
            //Point start = Vertices.OrderBy(v => v.X).ThenBy(v => v.Y).First();
            //int id = Vertices.IndexOf(start);
            //int vId = 0;
            //Point end = Vertices.ElementAt((id + 1) % Vertices.Count);
            //Point prev = end;
            //for (int i = 0; i <= Vertices.Count; i++)
            //{

            //    if (start.Y != end.Y)
            //    {
            //        result.Add(new Side(vId++, start, prev));
            //        start = prev;
            //        end = Vertices.ElementAt((id + 2 + i) % Vertices.Count);
            //        prev = end;
            //    }
            //    else
            //    {
            //        prev = end;
            //        end = Vertices.ElementAt((id + 2 + i) % Vertices.Count);
            //    }
            //}
            for (int i = 0; i < points.Count; i++)
            {
                result.Add(new Side(i, points.ElementAt(i), points.ElementAt((i + 1) % points.Count)));
            }

            return result;
        }

        private void GetSidesFromVertices()
        {
            Sides.Clear();
            for (int i = 0; i < Vertices.Count; i++)
            {
                Sides.Add(new Side(i, Vertices.ElementAt(i), Vertices.ElementAt((i + 1) % Vertices.Count)));
            }
        }

        public double Area()
        {
            double area = 0;

            double minX = Vertices.Min(v => v.X);
            Point leftMostTop = Vertices.Where(v => v.X.Equals(minX)).OrderBy(v => v.Y).First();
            int leftMostTopIndex = Vertices.IndexOf(leftMostTop);

            int count = 0;

            while (count < Vertices.Count)
            {

                Point one = Vertices.ElementAt((leftMostTopIndex + count) % Vertices.Count);
                Point two = Vertices.ElementAt((leftMostTopIndex + count + 1) % Vertices.Count);

                // it's -= because the values on the Y axis of the FrameWork element are raising from the top to the bottom
                area -= (one.Y + two.Y) * (two.X - one.X) / 2;
               
                count++;
            }

            return area;
        }

        public bool IsPolygon()
        {
            for (int i = 0; i < Sides.Count - 1; i++)
            {
                for (int j = i + 1; j < Sides.Count; j++)
                {
                    Orientation first = GetOrientation(Sides.ElementAt(i).Start, Sides.ElementAt(i).End, Sides.ElementAt(j).Start);
                    Orientation second = GetOrientation(Sides.ElementAt(i).Start, Sides.ElementAt(i).End, Sides.ElementAt(j).End);
                    Orientation third = GetOrientation(Sides.ElementAt(j).Start, Sides.ElementAt(j).End, Sides.ElementAt(i).Start);
                    Orientation fourth = GetOrientation(Sides.ElementAt(j).Start, Sides.ElementAt(j).End, Sides.ElementAt(i).End);

                    List<Orientation> orientations = new List<Orientation>() { first, second, third, fourth };

                    if (orientations.Count(o => o == Orientation.Colinear) == 0 &&
                        first != second && third != fourth)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Encloses(List<Point> points)
        {
            List<Line> helpers = new List<Line>();
            foreach (Point point in points)
            {
                helpers.Add(new Line(point, new Point(DRAWING_SCREEN_WIDTH, point.Y)));
            }

            foreach (Line helper in helpers)
            {
                //foreach (Side side1 in Sides)
                //{
                //    side1.Checked = false;
                //}

                List<Side> VisualSides = GetVisualSides();

                int intersectCount = 0;

                bool isPointPartOfTheEdge = VisualSides.Any(s => s.IsPartOfMe(helper.Start));

                if (!isPointPartOfTheEdge)
                {
                    foreach (Side side in VisualSides)
                    {
                        if (side.Checked == false)
                        {
                            Orientation first = GetOrientation(helper.Start, helper.End, side.Start);
                            Orientation second = GetOrientation(helper.Start, helper.End, side.End);
                            Orientation third = GetOrientation(side.Start, side.End, helper.Start);
                            Orientation fourth = GetOrientation(side.Start, side.End, helper.End);

                            List<Orientation> orientations = new List<Orientation>() { first, second, third, fourth };

                            if (orientations.Count(o => o == Orientation.Colinear) == 0 &&
                                first != second && third != fourth)
                            {
                                // ténylegesen metszik egymást
                                intersectCount++;
                            }
                            else if (orientations.Count(o => o == Orientation.Colinear) == 1)
                            {
                                if (first == Orientation.Colinear || second == Orientation.Colinear)
                                {
                                    //   o     o
                                    //    \    |
                                    //     \   |
                                    // o----o--o-o--o---------o helper
                                    //           |   \       
                                    //           |    \
                                    //           o     o

                                    if (third != fourth)
                                    {
                                        //this condition should be always true
                                        //az éppen vizsgált él azon csúcsa, ami rajta van a helperen
                                        Point vertex = Vertices.Single(v => v.Y == helper.Start.Y && side.IsPartOfMe(v));
                                        // a vizsgált élhez az előbb megkapott csúcsban csatlakozó másik él
                                        //Side sideTwo = Sides.Single(s => (s.Start.Equals(vertex) || s.End.Equals(vertex)) && s.Id != side.Id);



                                        
                                        Side sideTwo = VisualSides.Single(s => (s.Start.Equals(vertex) || s.End.Equals(vertex)) && s.Id != side.Id);




                                        List<Point> pts = new List<Point>() { side.Start, side.End, sideTwo.Start, sideTwo.End };
                                        List<Point> pts2 = pts.Where(p => !p.Equals(vertex)).ToList();

                                                 
                                        if (pts2.ElementAt(0).Y > helper.Start.Y && pts2.ElementAt(1).Y > helper.Start.Y)
                                        {
                                            //      o     o
                                            //       \   /
                                            //        \ /
                                            // o-------o-------------- helper
                                            //                  
                                            //           
                                            //  
                                            intersectCount += 2;
                                            side.Checked = true;
                                            foreach (Side s in VisualSides)
                                            {
                                                if (s.Equals(sideTwo))
                                                {
                                                    s.Checked = true;
                                                }
                                            }
                                        }
                                        else if (pts2.ElementAt(0).Y < helper.Start.Y && pts2.ElementAt(1).Y < helper.Start.Y)
                                        {
                                            //     
                                            //      
                                            //        
                                            // o--------o------------- helper
                                            //         / \       
                                            //        /   \
                                            //       o     o
                                            intersectCount += 2;
                                            side.Checked = true;
                                            foreach (Side s in VisualSides)
                                            {
                                                if (s.Equals(sideTwo))
                                                {
                                                    s.Checked = true;
                                                }
                                            }
                                        }
                                        else if (pts2.Any(v => v.Y == helper.Start.Y))
                                        {
                                            // itt a v-e1 él a vizsgált él, ha a e0-v lenne, akkor mind a négy colinear lenne      
                                            //       
                                            //    (e0)   v    
                                            // o---o-----o------------- helper
                                            //            \       
                                            //             \
                                            //              o (e1)

                                            //akkor hagyjuk az egészet és majd ez az oldal ott kerül feldolgozásra
                                            // ahol a vizzszintes oldalt vizsgáljuk és a két hozzá kapcsolódó élt
                                        }
                                        else
                                        {
                                            //ez az általános eset
                                            //      o   
                                            //       \  
                                            //        \ 
                                            // o-------o-------------- helper
                                            //        /          
                                            //       /    
                                            //      o
                                            intersectCount++;
                                            side.Checked = true;
                                            foreach (Side s in VisualSides)
                                            {
                                                if (s.Equals(sideTwo))
                                                {
                                                    s.Checked = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (orientations.Count(o => o == Orientation.Colinear) == 4)
                            {
                                if (helper.Start.X < side.Start.X)
                                {
                                    //      o (connectedOne)
                                    //       \  
                                    //        \ (side)
                                    // o-------o------o-------- helper
                                    //                |
                                    //                |
                                    //                o (connectedTwo)

                                    //Side connectedSideOne = Sides.Single(s => s.End.Equals(side.Start));
                                    //Side connectedSideTwo = Sides.Single(s => s.Start.Equals(side.End));



                                    Side connectedSideOne = VisualSides.Single(s => s.End.Equals(side.Start));
                                    Side connectedSideTwo = VisualSides.Single(s => s.Start.Equals(side.End));





                                    List<Point> pts = new List<Point>() { connectedSideOne.Start, connectedSideOne.End, connectedSideTwo.Start, connectedSideTwo.End };
                                    List<Point> pts2 = pts.Where(p => !p.Equals(side.Start) && !p.Equals(side.End)).ToList();

                                    if ((side.Start.Y < pts2.ElementAt(0).Y && side.Start.Y < pts2.ElementAt(1).Y) ||
                                        (pts2.ElementAt(0).Y < side.Start.Y && pts2.ElementAt(1).Y < side.Start.Y))
                                    {
                                        //      o            o
                                        //       \          /
                                        //        \        /
                                        // o-------o------o---- helper
                                        //      
                                        //      
                                        //  

                                        //               
                                        //      
                                        // o--------o------o-------- helper
                                        //         /        \
                                        //        /          \
                                        //       o            o
                                        intersectCount += 2;
                                    }
                                    else if (false)
                                    {
                                        //     ezek még nincsenek lekezelve          
                                        //      
                                        // o---o~~~~o~~~~~~o~~~~o---- helper
                                        //      
                                        //      
                                        //     

                                        //      o
                                        //       \        
                                        //        \
                                        // o-------o~~~~~~o~~~~o---- helper
                                        //      
                                        //      
                                        //  

                                        //               
                                        //      
                                        // o---o~~~~o~~~~~~o-------- helper
                                        //                  \
                                        //                   \
                                        //                    o
                                    }
                                    else
                                    {
                                        //      o
                                        //       \        
                                        //        \
                                        // o-------o-------o---- helper
                                        //                  \
                                        //                   \
                                        //                    o

                                        //                    o
                                        //                   /
                                        //                  /
                                        // o-------o-------o---- helper
                                        //        /         
                                        //       /            
                                        //      o

                                        intersectCount += 3;
                                    }

                                    side.Checked = true;
                                    foreach (Side s in VisualSides)
                                    {
                                        if (s.Equals(connectedSideOne) || s.Equals(connectedSideTwo))
                                        {
                                            s.Checked = true;
                                        }
                                    }
                                }
                                
                            }
                        }
                        
                    }
                }
                else
                {
                    intersectCount = 1;
                }


                if (intersectCount % 2 == 0)
                {
                    return false;
                }
            }

            return true;
        }

        
       

        private bool ProjectionsIntersect(Side a, Side b)
        {
            double absoluteStartX = Math.Min(Math.Min(a.Start.X, a.End.X), Math.Min(b.Start.X, b.End.X));
            double absoluteEndX = Math.Max(Math.Max(a.Start.X, a.End.X), Math.Max(b.Start.X, b.End.X));
            double width = Math.Abs(absoluteEndX - absoluteStartX);

            double xProjectionA = Math.Abs(a.End.X - a.Start.X);
            double xProjectionB = Math.Abs(b.End.X - b.Start.X);
            double shouldBeWidth = xProjectionA + xProjectionB;


            double absoluteStartY = Math.Min(Math.Min(a.Start.Y, a.End.Y), Math.Min(b.Start.Y, b.End.Y));
            double absoluteEndY = Math.Max(Math.Max(a.Start.Y, a.End.Y), Math.Max(b.Start.Y, b.End.Y));
            double height = Math.Abs(absoluteEndY - absoluteStartY);

            double yProjectionA = Math.Abs(a.End.Y - a.Start.Y);
            double yProjectionB = Math.Abs(b.End.Y - b.Start.Y);
            double shouldBeHeight = yProjectionA + yProjectionB;


            if (shouldBeWidth <= width && shouldBeHeight <= height)
            {
                return false;
            }

            return true;
        }

        public Orientation GetOrientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return Orientation.Colinear; 

            return (val < 0) ? Orientation.Clockwise : Orientation.CounterClockwise;

        }
    }
}
