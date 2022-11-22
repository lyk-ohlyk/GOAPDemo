using System.Collections.Generic;

namespace BehaviorTree
{
    public class Frightened : AIBehaviorTree
    {
        protected override void SetTreeName()
        {
            TreeName = "Frightened";
        }

        protected override Node SetupTree()
        {

            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new PlayFrightenedAction(),
                }),
            });

            return root;
        }
    }
}
