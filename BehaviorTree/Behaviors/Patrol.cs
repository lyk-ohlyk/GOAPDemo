using System.Collections.Generic;

namespace BehaviorTree
{
    public class Patrol : AIBehaviorTree
    {
        protected override void SetTreeName()
        {
            TreeName = "Patrol";
        }

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new GetPatrolPoint(),
                }),
            });

            return root;
        }
    }
}
