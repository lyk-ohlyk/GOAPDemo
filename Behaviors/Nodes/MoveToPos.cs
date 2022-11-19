using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using GOAP;

public class NodeMoveTo : Node
{
    Blackboard blackboard;

    public NodeMoveTo()
    {
        GameObject owner = GetOwner() as GameObject;
        blackboard = owner.GetComponent("Blackboard") as Blackboard;
    }

    public override NodeState Evaluate()
    {
        GameObject owner = GetOwner() as GameObject;
        if (owner == null)
        {
            return NodeState.FAILURE;
        }

        double distance = blackboard.GetBlackboardValue<double>(BlackboardKeys.BBTargetDist.Str);
        GameObject target = blackboard.GetTarget();

        if (distance > 0.01f)
        {
            owner.transform.position = Vector3.MoveTowards(
                owner.transform.position, 
                target.transform.position,
                MoveToTarget.speed * Time.deltaTime);

            owner.transform.LookAt(target.transform.position);
        }

        state = NodeState.RUNNING;
        return state;
    }

}