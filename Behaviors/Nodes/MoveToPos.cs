using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using GOAP;

public class NodeMoveTo : Node
{
    public float MinDist;

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

        double distance = blackboard.GetBlackboardValue<double>(BlackboardKeys.BBTargetDist.Str);
        GameObject target = blackboard.GetTarget();
        if (target == null)
        {
            return NodeState.FAILURE;
        }

        if (distance > MinDist)
        {
            float speedCoefficient = Mathf.Min((float)(distance - MinDist), 1f);
            owner.transform.position = Vector3.MoveTowards(
                owner.transform.position, 
                target.transform.position,
                speedCoefficient * MoveToTarget.speed * Time.deltaTime);

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