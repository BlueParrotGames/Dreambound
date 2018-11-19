using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class Slope
    {
        public readonly string TransformName;
        private List<Node> _slopeNodes;

        public Slope(string transformName)
        {
            TransformName = transformName;

            _slopeNodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            _slopeNodes.Add(node);
        }

        public int NodeCount
        {
            get { return _slopeNodes.Count; }
        }
    }
}
