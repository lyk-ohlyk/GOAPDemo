using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.AI
{
    public class JumpPoints : MonoBehaviour
    {
        [Tooltip("Enemies that will be assigned to this path on Start")]
        public List<EnemyController> EnemiesToAssign = new List<EnemyController>();

        [Tooltip("The Nodes making up the path")]
        public List<Transform> JumpNodes = new List<Transform>();

        [Tooltip("Points that is occupied.")]
        public HashSet<int> OccupyiedNodes = new HashSet<int>();

        [Tooltip("Points that is pre-occupied by action.")]
        public HashSet<int> PreOccupyiedNodes = new HashSet<int>();

        void Start()
        {
            foreach (var enemy in EnemiesToAssign)
            {
                enemy.JumpPoints = this;
            }
        }

        public bool TryOccupyJumpNode(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex > JumpNodes.Count)
                return false;
            if (PreOccupyiedNodes.Contains(nodeIndex))
                return false;

            PreOccupyiedNodes.Add(nodeIndex);
            OccupyiedNodes.Add(nodeIndex);
            return true;
        }

        public void ReleaseJumpNode(Vector3 jumpPos)
        {

            for (int i = 0; i < JumpNodes.Count; ++i)
            {
                Transform node = JumpNodes[i];
                if (node.position == jumpPos)
                {
                    PreOccupyiedNodes.Remove(i);
                }
            }
        }

        protected void Update()
        {
            foreach (var enemy in EnemiesToAssign)
            {
                if (enemy == null)
                {
                    continue;
                }

                foreach (var point in JumpNodes)
                {
                    int index = JumpNodes.IndexOf(point);
                    if (Vector3.Distance(enemy.transform.position, point.position) < 1.0f)
                    {
                        OccupyiedNodes.Add(index);
                    } else if (!PreOccupyiedNodes.Contains(index))
                    {
                        OccupyiedNodes.Remove(index);
                    }
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < JumpNodes.Count; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= JumpNodes.Count)
                {
                    nextIndex -= JumpNodes.Count;
                }

                Gizmos.DrawIcon(JumpNodes[i].position, "ID:" + i);
                Gizmos.DrawLine(JumpNodes[i].position, JumpNodes[nextIndex].position);
                Gizmos.DrawSphere(JumpNodes[i].position, 0.1f);
            }
        }
    }
}