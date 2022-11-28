using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.AI;

namespace BehaviorTree
{
    public class Jump : AIBehaviorTree
    {
        JumpToPos jumpNode;
        EnemyController m_EnemyController;

        public Jump()
        {
            EnterTime = 0.5f;
        }

        protected override void SetTreeName()
        {
            TreeName = "Jump";
        }

        protected override Node SetupTree()
        {
            jumpNode = new JumpToPos(1.5f);
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    jumpNode,
                }),
            });

            return root;
        }

        public override void InitTreeStates()
        {
            jumpNode.lastExecutionTime = 0;
            m_EnemyController = Owner.GetComponent<EnemyController>();
            m_EnemyController.StopNavigation();

            jumpNode.SetJumpPos(m_EnemyController.GetJumpPoint());
        }

    }
}
