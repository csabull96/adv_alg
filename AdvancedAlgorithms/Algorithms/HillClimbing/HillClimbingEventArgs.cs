using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.HillClimbing
{
    public class HillClimbingEventArgs : EventArgs
    {
        public Polygon BoundaryPolygon { get; private set; }

        public HillClimbingEventArgs(Polygon boundaryPolygon)
        {
            BoundaryPolygon = boundaryPolygon;
        }

    }
}
