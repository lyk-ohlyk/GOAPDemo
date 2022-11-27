using System.Collections.Generic;

namespace BehaviorTree
{
    public class Attack : AIBehaviorTree
    {
        public Attack()
        {
            EnterTime = .2f;
        }

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
                    new FaceToTarget(),
                    new Fire(1.0f),
                }),
            });;

            return root;
        }
    }
}
