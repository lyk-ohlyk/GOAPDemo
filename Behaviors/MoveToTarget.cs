using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class MoveToTarget : Tree
    {
        public UnityEngine.Transform[] waypoints;

        public static float speed = 2f;
        public static float fovRange = 6f;
        public static float attackRange = 1f;

        public GameObject Owner;

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new NodeMoveTo(),
                }),
            });

            return root;
        }
    }
}