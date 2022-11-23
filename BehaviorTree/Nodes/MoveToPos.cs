using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using GOAP;
using Unity.FPS.AI;

public class NodeMoveTo : Node
{
    public float MinDist;

    EnemyController m_EnemyController;

    public NodeMoveTo(float dist=0f)
    {
        MinDist = dist;
    }

    public override NodeState Evaluate()
    {
        if (!CheckBlackboard())
        {
            return NodeState.FAILURE;
        }

        GameObject owner = GetOwner() as GameObject;
        if (owner == null)
        {
            return NodeState.FAILURE;
        }
        if (m_EnemyController == null)
        {
            m_EnemyController = owner.GetComponent<EnemyController>();
        }

        double distance = blackboard.GetBlackboardValue<float>(BlackboardKeys.BBTargetDist.Str);
        GameObject target = blackboard.GetTarget();
        if (target == null)
        {
            return NodeState.FAILURE;
        }

        if (distance > MinDist)
        {
            float speedCoefficient = Mathf.Min((float)(distance - MinDist), 1f);
            Debug.Log("Target position:" + target.transform.position);

            m_EnemyController?.SetNavDestination(target.transform.position);
            owner.transform.LookAt(target.transform.position);
        }
        else
        {
            return NodeState.SUCCESS;
        }

        state = NodeState.RUNNING;
        return state;
    }

}