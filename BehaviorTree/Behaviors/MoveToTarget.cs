using System.Collections.Generic;

namespace BehaviorTree
{
    public class MoveToTarget : AIBehaviorTree
    {
        public static float speed = 2f;
        public static float fovRange = 6f;
        public static float attackRange = 1f;

        public float DistanceToStop = 5f;

        public MoveToTarget()
        {
            EnterTime = 0.3f;
        }

        protected override void SetTreeName()
        {
            TreeName = "MoveToTarget";
        }

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new NodeMoveTo(DistanceToStop),
                }),
            });

            return root;
        }
    }
}