using System.Text;
using System.Xml;
using System.Xml.Linq;
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

        AIWorldStates m_CurStates;
        AIWorldStates m_TargetStates;

        float m_LastUpdateTime = 0f;
        float m_StringUpdateTick = 0.2f;

        public void LoadConfig(string configName="BluffingEnemy")
        {
            string url = "Assets/FPS/Scripts/AI/GOAP/AIConfig/BluffingEnemy.xml";
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
                stateCondition.SetState(stateStr[0], stateStr[0][1] == '1' ? true : false);
            }
            return stateCondition;
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
        private void Start()
        {
            LoadConfig();
        }

        private void Update()
        {
            if (Time.time - m_LastUpdateTime > m_StringUpdateTick)
            {
                m_LastUpdateTime = Time.time;
                UpdateShowStrings();
            }
        }

        void UpdateShowStrings()
        {
            WorldStatesShowList = m_CurStates.GetDebugList();

            GoalShowList = new List<string>();
            foreach (var goal in m_Goals)
            {
                GoalShowList.Add(goal.name);
            }

            ActionsShowList = new List<string>();
            foreach (var action in m_Actions)
            {
                ActionsShowList.Add("Behavior:" + action.BehaviorName + ", Cost:" + action.ShowCost);
            }
            // ActionsShowList = ;
        }

    }
}