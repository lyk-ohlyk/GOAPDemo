using System.Collections.Generic;

namespace BehaviorTree
{
    public class Attack : AIBehaviorTree
    {
        protected override void SetTreeName()
        {
            TreeName = "Attack";
        }

        protected override Node SetupTree()
        {

            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new Fire(),
                }),
            });

            return root;
        }
    }
}
