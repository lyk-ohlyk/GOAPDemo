using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using GOAP;
using Unity.FPS.AI;

public class JumpToPos : Node
{
    public float MinJumpDist;

    Vector3 m_StartPos;
    Vector3 m_JumpPos;
    EnemyController m_EnemyController;

    public JumpToPos(float deltaT) : base(deltaT)
    {
    }

    public void SetJumpPos(Vector3 pos)
    {
        m_JumpPos = pos;
    }

    public override NodeState TryEvaluate()
    {

        GameObject owner = GetOwner() as GameObject;
        if (owner == null || m_JumpPos == null)
        {
            return NodeState.FAILURE;
        }

        if (m_EnemyController == null)
            m_EnemyController = owner.GetComponent<EnemyController>();
        if (m_EnemyController == null)
            return NodeState.FAILURE;

        if (lastExecutionTime == 0 && !m_EnemyController.IsJumping())
        {
            Debug.Log(string.Format("Start:{0}, jump:{1}, dist:{2}", m_StartPos, m_JumpPos, Vector3.Distance(m_JumpPos, owner.transform.position)));
            if (Vector3.Distance(m_JumpPos, owner.transform.position) < 1.0f)
            {
                m_EnemyController.ReleaseJumpPoint(m_JumpPos);
                return NodeState.SUCCESS;
            }

            m_StartPos = owner.transform.position;
            lastExecutionTime = Time.time;
        }

        NodeState preState = Evaluate(); // check blackboard.
        if (preState == NodeState.FAILURE)
        {
            return preState;
        }

        m_EnemyController.SetIsJumping(false);
        if (Time.time - lastExecutionTime > timerDelta)
        {
            return NodeState.SUCCESS;
        }
        
        m_EnemyController.SetIsJumping(true);

        Vector3 curPos;

        GameObject target = blackboard.GetTarget();
        owner.transform.LookAt(target.transform);

        float lerpRate = (Time.time - lastExecutionTime) / timerDelta;
        curPos = Vector3.Lerp(m_StartPos, m_JumpPos, lerpRate);
        owner.transform.position = new Vector3(curPos.x, curPos.y + 2f * Mathf.Sin(lerpRate * Mathf.PI), curPos.z);
        Debug.Log("owner.transform.position :" + owner.transform.position);
        return NodeState.RUNNING;
    }

    protected override NodeState Evaluate()
    {
        if (!CheckBlackboard())
        {
            return NodeState.FAILURE;
        }

        state = NodeState.RUNNING;
        return state;
    }

}