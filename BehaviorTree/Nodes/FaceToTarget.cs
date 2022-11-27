
using UnityEngine;

using BehaviorTree;
using Unity.FPS.AI;

public class FaceToTarget : Node
{
    EnemyController m_EnemyController;

    protected override NodeState Evaluate()
    {
        GameObject owner = GetOwner() as GameObject;
        if (m_EnemyController == null)
        {
            m_EnemyController = owner.GetComponent<EnemyController>();
        }
        CheckBlackboard();
        if (blackboard == null || m_EnemyController == null)
        {
            return NodeState.FAILURE;
        }
        GameObject target = blackboard.GetTarget();
        if (target == null)
        {
            return NodeState.FAILURE;
        }

        owner.transform.LookAt(target.transform);
        return NodeState.SUCCESS;
    }
}
