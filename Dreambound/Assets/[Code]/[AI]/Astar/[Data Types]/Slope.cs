using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class Slope
    {
        public readonly string TransformName;
        private List<Node> _slopeNodes;

        private List<Node> _slopeStartNodes;
        private List<Node> _slopeEndNodes;
        private int _slopeStartHeight;
        private int _slopeEndHeight;


        public Slope(string transformName)
        {
            TransformName = transformName;

            _slopeNodes = new List<Node>();
        }
        public void AddNode(Node node)
        {
            if (node.GridPosition.y < _slopeStartHeight)
            {
                _slopeStartHeight = node.GridPosition.y;
                _slopeStartNodes.Clear();
            }
            if (node.GridPosition.y > _slopeEndHeight)
            {
                _slopeEndHeight = node.GridPosition.y;
                _slopeEndNodes.Clear();
            }

            _slopeNodes.Add(node);

            //If the node is an entry node, add it to the appropiate list
            if (node.GridPosition.y == _slopeStartHeight)
                _slopeStartNodes.Add(node);
            if (node.GridPosition.y == _slopeEndHeight)
                _slopeEndNodes.Add(node);
        }

        public int NodeCount
        {
            get { return _slopeNodes.Count; }
        }

        public Node GetStartNode(int height)
        {
            if (height == _slopeStartHeight)
                return _slopeStartNodes[Mathf.RoundToInt(_slopeStartNodes.Count / 2)];
            else
                return _slopeEndNodes[Mathf.RoundToInt(_slopeEndNodes.Count / 2)];
        }
    }
}
