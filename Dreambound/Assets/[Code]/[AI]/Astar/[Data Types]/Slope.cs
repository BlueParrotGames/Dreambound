using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class Slope
    {
        public readonly Node SlopeStart;
        public readonly Node SlopeEnd;
        public readonly int SlopeMovementPenalty;

        public Slope(Node slopeStart, Node slopeEnd, int slopeMovemntPenalty)
        {
            SlopeStart = slopeStart;
            SlopeEnd = slopeEnd;
            SlopeMovementPenalty = slopeMovemntPenalty;
        }
    }
}
