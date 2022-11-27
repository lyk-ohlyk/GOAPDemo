using System;
using UnityEngine;
using Unity.FPS.AI;

namespace GOAP
{
    class CostFuncs
    {
        GameObject gameObject;
        public static float COST_MAX = 1e5f;

        AIBlackboard blackboard;

        public void SetCostOwner(GameObject @object)
        {
            gameObject = @object;
        }

        public Func<float> ConstCostFunc(float constValue)
        {
            return () => { return constValue; };
        }


        public float FrightDistCost()
        {
            GameObject target = GetCostTarget();
            if (target == null) return COST_MAX;

            float distance = (gameObject.transform.position - target.transform.position).magnitude;
            return Math.Max(0f, (distance - 2f) / 5f);
        }

        public float JumpCost()
        {

            GameObject target = GetCostTarget();
            if (target == null)
                return COST_MAX;
            EnemyController enemyController = gameObject.GetComponent<EnemyController>();
            if (!enemyController)
                return COST_MAX;

            Vector3 jumpPos = enemyController.GetJumpPoint();
            float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
            float jumpDistance = Vector3.Distance(jumpPos, target.transform.position);
            float nearCost = Vector3.Distance(jumpPos, gameObject.transform.position);

            float cost = Convert.ToInt32(nearCost < 1.0) * 10;  // Avoid too near jump.
            cost += Convert.ToInt32((jumpDistance + 0.2) > distance) * 10f;  // Ensure jump make target near.
            cost += (jumpDistance / distance) * 1.5f;  // Ensure jump make target near.
            return cost;
        }

        bool CheckBlackboard()
        {
            if (blackboard != null) return true;
            blackboard = gameObject.GetComponent<AIBlackboard>();
            return blackboard != null;
        }

        GameObject GetCostTarget()
        {
            if (gameObject == null) return null;
            if (!CheckBlackboard()) return null;

            GameObject target = blackboard.GetTarget();
            if (target == null) return null;

            return target;
        }
    }
}
