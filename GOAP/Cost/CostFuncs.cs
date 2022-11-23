using System;
using UnityEngine;

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
            if (target == null) return COST_MAX;

            float distance = (gameObject.transform.position - target.transform.position).sqrMagnitude;

            // TODO: Jump point query.

            return distance;
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
