using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node m_Root = null;

        protected void Start()
        {
            m_Root = SetupTree();
            m_Root.SetOwner(gameObject);
        }

        private void Update()
        {
            if (m_Root != null)
                m_Root.Evaluate();
        }

        protected abstract Node SetupTree();

    }

}