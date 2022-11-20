using System;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class AIBehaviorTree
    {
        public string TreeName;

        private Node m_Root = null;


        public void TreeInit(GameObject owner)
        {
            m_Root = SetupTree();
            m_Root.SetOwner(owner);
            SetTreeName();
        }

        protected virtual void SetTreeName()
        {
            TreeName = "";
        }

        protected abstract Node SetupTree();

        public NodeState BehaviorTreeTick()
        {
            if (m_Root != null)
                return m_Root.Evaluate();

            return NodeState.FAILURE;
        }

    }

}