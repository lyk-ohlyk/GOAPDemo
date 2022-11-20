
using UnityEngine;

using BehaviorTree;
using Unity.FPS.AI;

public class GetPatrolPoint : Node
{
    EnemyController m_EnemyController;
    public override NodeState Evaluate()
    {
        GameObject owner = GetOwner() as GameObject;
        if (m_EnemyController == null)
        {
            m_EnemyController = owner.GetComponent<EnemyController>();
        }

        m_EnemyController?.UpdatePathDestination();
        m_EnemyController?.SetNavDestination(m_EnemyController.GetDestinationOnPath());

        return NodeState.SUCCESS;
    }
}
