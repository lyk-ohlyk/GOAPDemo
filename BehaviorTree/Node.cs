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
        protected NodeState state;
        protected List<Node> NodeChildren = new List<Node>();

        public Node NodeParent;

        public WeakReference Owner { get; private set; }

        Dictionary<string, object> m_DataContext = new Dictionary<string, object>();

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

        public virtual NodeState Evaluate() => NodeState.FAILURE;
    }

}