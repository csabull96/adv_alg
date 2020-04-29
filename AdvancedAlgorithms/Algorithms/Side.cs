using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Algorithms
{
    public class Side
    {
        public int Id { get; private set; } 
        public Point Start { get; private set; }
        public Point End { get; set; }
        public bool Checked { get; set; }

        public bool IsVertical { get { return Start.X == End.X; } }
        public bool IsHorizontal { get { return Start.Y == End.Y; } }

        public Side(int id, Point start, Point end)
        {
            Id = id;
            Start = start;
            End = end;
            Checked = false;
        }

        public Side()
        {
           
        }

        public bool IsPartOfMe(Point point)
        {
            //if (point.X == 131 && point.Y == 130)
            //{

            //}
            List<Point> points = new List<Point>() { Start, End };
            // mi van ha egyenlőek
            Point left = points.OrderBy(p => p.X).First();
            Point right = points.OrderByDescending(p => p.X).First();

            Point bottom = points.OrderBy(p => p.Y).First();
            Point top = points.OrderByDescending(p => p.Y).First();

            if (point.Equals(Start) || point.Equals(End))
            {
                return true;
            }
            else if (IsVertical)
            {
                return (Start.X == point.X && bottom.Y <= point.Y && point.Y <= top.Y);
            }
            else if (IsHorizontal)
            {
                return (Start.Y == point.Y && left.X <= point.X && point.X <= right.X);
            }

            
            bool res1 = (left.X <= point.X && point.X <= right.X) && (bottom.Y <= point.Y && point.Y <= top.Y);
            double res2 = ((point.Y - right.Y) / (point.X - right.X) - (right.Y - left.Y) / (right.X - left.X));

            // fordított y koordináták?
            //return  ((left.X <= point.X && point.X <= right.X) && (bottom.Y <= point.Y && point.Y <= top.Y)) && ((point.Y - right.Y) / (point.X - right.X) - (right.Y - left.Y) / (right.X - left.X) == 0);
            return ((left.X <= point.X && point.X <= right.X) && (bottom.Y <= point.Y && point.Y <= top.Y)) && Math.Abs((point.Y - right.Y) / (point.X - right.X) - (right.Y - left.Y) / (right.X - left.X)) < 0.001;

        }

        public bool Equals(Side side)
        {
            return (Start.Equals(side.Start) && End.Equals(side.End)) || (Start.Equals(side.End) && End.Equals(side.Start));
        }
    }
}
