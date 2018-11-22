using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar.Editor
{
    public class GenerationSettings : ScriptableObject
    {
        //General Settings
        public readonly Vector3 GridWorldSize;
        public readonly float NodeRadius;

        //Weight Settings
        public readonly int ObstacleProximityPenalty;

        //Agent Settins
        public readonly float AgentHeight;
        public readonly float AgentRadius;
        public readonly float AgentJumpHeight;

        //Baking Settings
        public readonly int BlurSize;
        public readonly Vector3 ColliderSizeMultiplier;
        public readonly float ColliderRadiusMultiplier;

        public GenerationSettings(Vector3 gridWorldSize, float nodeRadius, int obstacleProximityPenalty, float agentHeight, float agentRadius, float agentJumpHeight, int blurSize, Vector3 colliderSizeMultiplier, float colliderRadiusMultiplier)
        {
            GridWorldSize = gridWorldSize;
            NodeRadius = nodeRadius;
            ObstacleProximityPenalty = obstacleProximityPenalty;
            AgentHeight = agentHeight;
            AgentRadius = agentRadius;
            AgentJumpHeight = agentJumpHeight;
            BlurSize = blurSize;
            ColliderSizeMultiplier = colliderSizeMultiplier;
            ColliderRadiusMultiplier = colliderRadiusMultiplier;
        }
    }
}