using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        public Node NodeParent;
        public float lastExecutionTime = 0f;

        protected NodeState state;
        protected List<Node> NodeChildren = new List<Node>();
        protected AIBlackboard blackboard;

        protected float timerDelta = 0f;  // Node timer.

        private WeakReference Owner;
        private Dictionary<string, object> m_DataContext = new Dictionary<string, object>();  // lyk dev TODO: 上下文条件

        public Node()
        {
            NodeParent = null;
            Owner = null;
        }
        public Node(List<Node> children)
        {
            foreach (Node child in children)
                _Attach(child);
        }

        public Node(float timerT)
        {
            NodeParent = null;
            Owner = null;
            timerDelta = timerT;
        }

        public object GetOwner()
        {
            if (Owner != null && Owner.IsAlive)
            {
                return Owner.Target;
            }
            if (NodeParent is null)
                return null;

            var parentOwner = NodeParent.GetOwner();
            if (parentOwner != null)
            {
                Owner = new WeakReference(parentOwner);
            }
            return Owner.Target;
        }

        public void SetOwner(GameObject gameObject)
        {
            Owner = new WeakReference(gameObject);
        }

        private void _Attach(Node node)
        {
            node.NodeParent = this;
            NodeChildren.Add(node);
        }

        public bool CheckBlackboard()
        {
            GameObject owner = GetOwner() as GameObject;
            if (owner == null)
            {
                return false;
            }

            if (blackboard == null)
            {
                blackboard = owner.GetComponent<AIBlackboard>();
            }
            return !(blackboard == null);
        }

        protected virtual NodeState Evaluate() => NodeState.FAILURE;

        virtual public NodeState TryEvaluate()
        {
            if (lastExecutionTime == 0)
            {
                lastExecutionTime = Time.time;
                return NodeState.FAILURE;
            }
            if (Time.time - lastExecutionTime < timerDelta)
            {
                return NodeState.FAILURE;
            }
            lastExecutionTime = Time.time;

            return Evaluate();
        }
    }

}