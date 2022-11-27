using System;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class AIBehaviorTree
    {
        public string TreeName;
        protected GameObject Owner;

        private Node m_Root = null;
        private bool m_NeedExit = false;

        protected float TreeStartTimestamp = 0f;  // Used in bahavior tree timer.
        protected float EnterTime = 0f;  // Run behaviors after EnterTime.

        public void TreeInit(GameObject owner)
        {
            Owner = owner;
            m_Root = SetupTree();
            m_Root.SetOwner(owner);
            SetTreeName();
        }

        /// <summary>
        /// Initialize tree parameters every excution.
        /// </summary>
        public virtual void InitTreeStates()
        {

        }

        public void TryStopTree()
        {
            m_NeedExit = true;
        }

        public void SetStartTime(float startTimestamp)
        {
            TreeStartTimestamp = startTimestamp;
        }

        protected virtual void SetTreeName()
        {
            TreeName = "";
        }

        protected abstract Node SetupTree();

        public NodeState BehaviorTreeTick()
        {
            if (m_NeedExit)
            {
                return NodeState.FAILURE;
            }

            if (m_Root != null)
                return m_Root.TryEvaluate();

            return NodeState.FAILURE;
        }

        public float GetTreeEnterTime()
        {
            return EnterTime;
        }
    }

}