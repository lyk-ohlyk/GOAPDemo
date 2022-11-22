
using UnityEngine;
using BehaviorTree;
using Unity.FPS.AI;
using GOAP;

public class PlayFrightenedAction : Node
{
    public override NodeState Evaluate()
    {
        GameObject owner = GetOwner() as GameObject;
        if (owner == null)
        {
            return NodeState.FAILURE;
        }
        CheckBlackboard();

        owner.GetComponent<EnemyMobile>().Animator.SetTrigger(EnemyMobile.k_AnimIsFrightened);
        GameObject target = blackboard.GetTarget();
        if (target == null)
        {
            return NodeState.FAILURE;
        }

        owner.transform.position = Vector3.MoveTowards(
            owner.transform.position,
            owner.transform.position + (owner.transform.position - target.transform.position),
            0.3f * MoveToTarget.speed * Time.deltaTime);

        state = NodeState.SUCCESS;
        return state;
    }
}
