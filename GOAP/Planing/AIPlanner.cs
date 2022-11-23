using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GOAP
{
    public class AIPlanner : MonoBehaviour
    {
        public List<string> WorldStatesShowList;
        public List<string> GoalShowList;
        public List<string> ActionsShowList;

        List<AIGoal> m_Goals;
        List<AIAction> m_Actions;

        CostFuncs m_CostFuncs;

        AIWorldStates m_CurStates;
        AIBlackboard blackboard;

        TreeManager treeManager;

        float m_LastUpdateTime = 0f;
        float m_StringUpdateTick = 0.2f;

        private void Start()
        {
            blackboard = GetComponent<AIBlackboard>();
            treeManager = GetComponent<TreeManager>();

            m_CostFuncs = new CostFuncs();
            m_CostFuncs.SetCostOwner(gameObject);
            LoadConfig();
        }

        private void Update()
        {
            if (Time.time - m_LastUpdateTime < m_StringUpdateTick)
            {
                return;
            }

            m_LastUpdateTime = Time.time;
            UpdateShowStrings();

            AIAction planAction = null;

            foreach (AIGoal goal in m_Goals)
            {
                if (goal.IsPreconditionMet(m_CurStates))
                {

                    Debug.Log("Goal pre: " + goal.PreCondition.GetConditionString() + ", State:" + m_CurStates.GetStatesString());

                    string targetString = m_CurStates.GetStateAfterEffect(goal.TargetStates);                             
                    AIWorldStates targetStates = new AIWorldStates(targetString);
                    planAction = PlanAction(targetStates);

                    if (planAction != null)
                    {
                        Debug.Log("Goal: " + goal.name + ", plan action: " + planAction.BehaviorName);
                        break;
                    }
                }
            }

            if (planAction != null)
            {
                treeManager = GetComponent<TreeManager>();
                treeManager?.SetCurTreeName(planAction.BehaviorName);

                if (treeManager == null)
                {
                    Debug.Log("Tree Manager is nullllllll.");
                }
            }
        }

        static IEnumerable<XElement> SimpleStreamAxis(string inputUrl)
        {
            using (XmlReader reader = XmlReader.Create(inputUrl))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;
                    XElement el = XNode.ReadFrom(reader) as XElement;
                    if (el == null) continue;
                    yield return el;
                }
            }
        }

        public void LoadConfig(string configName="BluffingEnemy")
        {
            string url = "Assets/FPS/Scripts/AI/GOAP/AIConfig/" + configName + ".xml";
            var xElement = SimpleStreamAxis(url);
            
            foreach (var e in xElement)
            {
                if (e.Name == "WorldStates")
                {
                    ParseWorldStates(e);
                }
                else if (e.Name == "Goals")
                {
                    ParseGoals(e);
                }
                else if (e.Name == "Actions")
                {
                    ParseActions(e);
                }
            }
        }

        void ParseWorldStates(XElement xElement)
        {
            m_CurStates = new AIWorldStates();
            foreach (var elem in xElement.Elements())
            {
                string[] stateStr = elem.Value.Split('|');
                m_CurStates.AddState(stateStr[0], stateStr[1][0]);
            }
        }

        void ParseGoals(XElement xElement)
        {
            m_Goals = new List<AIGoal>();

            foreach (var goal in xElement.Elements()) // Goal
            {
                AIGoal aiGoal = new AIGoal();
                foreach (var attr in goal.Elements())
                {
                    if (attr.Name == "Name")
                    {
                        aiGoal.name = attr.Value;
                    }
                    else if (attr.Name == "PreConditions")
                    {
                        StateCondition stateCondition = ParseStates(attr);
                        aiGoal.SetPreCondition(stateCondition);
                    }
                    else if (attr.Name == "TargetStates")
                    {
                        StateCondition stateCondition = ParseStates(attr);
                        aiGoal.SetTargetState(stateCondition);
                    }
                }

                m_Goals.Add(aiGoal);
            }
        }

        void ParseActions(XElement xElement)
        {
            m_Actions = new List<AIAction>();

            foreach (var goal in xElement.Elements()) // Action
            {
                AIAction action = new AIAction();

                foreach (var attr in goal.Elements())
                {
                    if (attr.Name == "Name")
                    {
                        action.name = attr.Value;
                    }
                    else if (attr.Name == "Behavior")
                    {
                        action.BehaviorName = attr.Value;
                    }
                    else if (attr.Name == "PreConditions")
                    {
                        StateCondition stateCondition = ParseStates(attr);
                        action.SetPrecondition(stateCondition);
                    }
                    else if (attr.Name == "Effects")
                    {
                        StateCondition stateCondition = ParseStates(attr);
                        action.SetEffect(stateCondition);
                    }
                    else if (attr.Name == "CostFunc")
                    {
                        action.SetCostFunc(GetCostFuncFromString(attr.Value));
                    }
                }

                m_Actions.Add(action);
            }
        }

        StateCondition ParseStates(XElement xElement)
        {
            StateCondition stateCondition = new StateCondition();

            foreach (var state in xElement.Elements())
            {
                string[] stateStr = state.Value.Split('|');
                stateCondition.SetState(stateStr[0], stateStr[1][0] == '1' ? true : false);
            }
            return stateCondition;
        }

        public void SetCurrentWorldState(StateDef stateName, bool value)
        {
            m_CurStates.SetState(stateName, value);
        }

        private Func<float> GetCostFuncFromString(string costFuncInfo)
        {
            if (costFuncInfo.IndexOf('|') != -1)  // Constant value
            {
                string[] info = costFuncInfo.Split('|');
                float value = float.Parse(info[1]);
                return m_CostFuncs.ConstCostFunc(value);
            }

            var func = m_CostFuncs.GetType().GetMethod(costFuncInfo);
            return () => (float)func.Invoke(m_CostFuncs, null);
        }

        private void UpdateShowStrings()
        {
            WorldStatesShowList = m_CurStates.GetDebugList();

            GoalShowList = new List<string>();
            foreach (var goal in m_Goals)
            {
                GoalShowList.Add(goal.name + "-" + goal.PreCondition.GetConditionString());
            }

            ActionsShowList = new List<string>();
            foreach (var action in m_Actions)
            {
                ActionsShowList.Add("Behavior:" + action.BehaviorName + ", Cost:" + action.CalcCost(blackboard));
            }

        }

        /// Run A* to get a valid plan.
        ///     Current vertex: m_CurStates; 
        ///     Target vertex:  targetStates
        ///     Edges: AIActions that fit the current WorldStates. Each edge lead to a vertex based on AIAction.m_Effect
        ///     Edge weights: Cost of actions.
        private AIAction PlanAction(AIWorldStates targetStates)
        {
            string startPoint = m_CurStates.GetStatesString();
            string targetPoint = targetStates.GetStatesString();

            Debug.Log("startPoint:" + startPoint + "; targetPoint:" + targetPoint);

            List<string> debugList = new List<string>();

            PriorityQueue<string> priorityQueue = new PriorityQueue<string>();

            // Record state cost, or vertex distance.
            Dictionary<string, float> stateCostsDict = new Dictionary<string, float>();
            stateCostsDict[startPoint] = 0f;

            // Record previous vertex for backtracking, to find the first action.
            Dictionary<string, Tuple<string, AIAction>> preState = new Dictionary<string, Tuple<string, AIAction>>();

            // Init visited set.
            HashSet<string> visited = new HashSet<string>();

            // Init Min Heap
            priorityQueue.Enqueue(startPoint, 0f);

            string curPoint;
            while (priorityQueue.Count > 0)
            {
                curPoint = priorityQueue.Dequeue();
                visited.Add(curPoint);

                // Found target.
                if (curPoint == targetPoint)
                {
                    break;
                }

                AIWorldStates curStates = new AIWorldStates(curPoint);
                List<AIAction> edges = GetAIActions(curStates);

                foreach (var edge in edges)
                {
                    Debug.Log("Found edges:" + edge.name);
                }

                foreach (var edge in edges)
                {
                    float totalCost = stateCostsDict[curPoint];
                    totalCost += edge.CalcCost(blackboard);
                    string nextPoint = curStates.GetStateAfterEffect(edge.ActionEffect);

                    if (visited.Contains(nextPoint))
                        continue;

                    // Heuristic
                    AIWorldStates nextState = new AIWorldStates(nextPoint);
                    totalCost += CalcDistBetweenStates(targetStates, nextState);

                    // Update cost
                    if (!stateCostsDict.ContainsKey(nextPoint))
                    {
                        stateCostsDict[nextPoint] = totalCost;
                        preState[nextPoint] = new Tuple<string, AIAction>(curPoint, edge);

                        priorityQueue.Enqueue(nextPoint, totalCost);
                    }
                    else if (stateCostsDict[nextPoint] > totalCost)
                    {
                        stateCostsDict[nextPoint] = totalCost;
                        preState[nextPoint] = new Tuple<string, AIAction>(curPoint, edge);

                        priorityQueue.UpdatePriority(nextPoint, totalCost);
                    }
                }
            }

            if (!visited.Contains(targetPoint))
            {
                return null; // No Action can be found.
            }

            Debug.Log("Target is reached." + targetPoint);

            // Backtracking to get Action.
            string backString = targetPoint;
            AIAction nextAction = null;
            while (backString != startPoint)
            {
                debugList.Add(backString);

                nextAction = preState[backString].Item2;
                backString = preState[backString].Item1;
            }

            foreach (string path in debugList)
            {
                Debug.Log("Reverse plan path: " + path);
            }

            return nextAction;
        }

        /// <summary>
        /// Check if state string [a] contains state string [b].
        /// Here we only care about the '1's.
        /// lyk dev TODO: States should be defined as "interested" or "not intereseted".
        /// </summary>
        private bool IsContainState(string a, string b)
        {
            bool isContained = true;
            for (int i = 0; i < AIWorldStates.StateCount; ++i)
            {
                if (b[i] == '1' && a[i] == '0')
                {
                    isContained = false;
                    break;
                }
            }

            return isContained;
        }

        /// <summary>
        ///  Find AIActions that fit worldStates.
        /// </summary>
        /// <returns> The list of fitted AIActions. </returns>
        private List<AIAction> GetAIActions(AIWorldStates worldStates)
        {
            List<AIAction> fittedActions = new List<AIAction>();

            foreach (AIAction action in m_Actions)
            {
                if (!(action.IsPreconditionMet(worldStates)))
                    continue;

                // Check if action's effect is already fitted.
                if (action.IsEffectMet(worldStates))
                    continue;

                fittedActions.Add(action);
            }

            return fittedActions;
        }

        private int CalcDistBetweenStates(AIWorldStates a, AIWorldStates b)
        {
            char[] statesA = a.WStates;
            char[] statesB = b.WStates;

            int count = 0;
            int len = AIWorldStates.StateCount;

            for (int i = 0; i < len; ++i)
            {
                if (statesA[i] != statesB[i])
                {
                    ++count;
                }
            }

            return count;
        }

    }
}