using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TreeManager : MonoBehaviour
// The behaviour tree runner which manages all trees' execution.
{
    [SerializeField]
    private string CurrentTreeName;

    public int CurrentTreeIndex;

    [Tooltip("The behaviour trees that this GameObject has.")]
    public List<string> BehaviourTreeNames;

    public List<AIBehaviorTree> m_BehaviorTrees;

    private int m_LastTreeIndex;
    private float m_LastTreeExitTimeStamp;

    private float m_ActionTickTime = 0.1f;
    private float m_TickTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_BehaviorTrees = new List<AIBehaviorTree>();
        BehaviourTreeNames = new List<string>();
        CurrentTreeIndex = 0;
        m_LastTreeIndex = -1;

        LoadBehaviorTrees();

        // lyk dev TODO: read trees from config.
        foreach (var tree in m_BehaviorTrees)
        {
            tree.TreeInit(gameObject);
            BehaviourTreeNames.Add(tree.TreeName);
        }
    }

    void LoadBehaviorTrees()
    // lyk dev TODO
    {
        m_BehaviorTrees.Add(new Patrol());
        m_BehaviorTrees.Add(new MoveToTarget());
        m_BehaviorTrees.Add(new Frightened());
        m_BehaviorTrees.Add(new Attack());
    }

    // Update is called once per frame
    void Update()
    {

        m_TickTimer += Time.deltaTime;
        if (m_TickTimer < m_ActionTickTime)
        {

            while (m_TickTimer >= m_ActionTickTime)
            {
                m_TickTimer -= m_ActionTickTime;
            }
        }

        if (CurrentTreeIndex < 0)
        {
            return;
        }

        CurrentTreeName = m_BehaviorTrees?[CurrentTreeIndex].TreeName;

        AIBehaviorTree tree = m_BehaviorTrees?[CurrentTreeIndex];
        if (tree != null && (Time.time - m_LastTreeExitTimeStamp) > tree.GetTreeExitTime())
        {
            tree.BehaviorTreeTick();
        }
    }

    public void SetCurTreeName(string treeName)
    {
        int index = 0;
        foreach (var tree in m_BehaviorTrees)
        {
            if (tree.TreeName == treeName)
            {
                CurrentTreeName = treeName;
                CurrentTreeIndex = index;
                break;
            }

            index += 1;
        }

        if (index == m_BehaviorTrees.Count)
        {
            CurrentTreeName = "";
            CurrentTreeIndex = -1;
        }

        if (CurrentTreeIndex != m_LastTreeIndex)
        {
            m_LastTreeExitTimeStamp = Time.time;
        }
        m_LastTreeIndex = CurrentTreeIndex;
    }

    private void OnDestroy()
    {
        m_BehaviorTrees.Clear();
    }

}
