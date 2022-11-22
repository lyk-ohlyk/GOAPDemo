
using UnityEngine;

using BehaviorTree;
using Unity.FPS.AI;

public class Fire : Node
{
    EnemyController m_EnemyController;
    public override NodeState Evaluate()
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
        m_EnemyController?.TryAtack(target.transform.position);

        return NodeState.SUCCESS;
    }
}
