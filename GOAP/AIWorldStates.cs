using System;
using System.Collections.Generic;
using UnityEngine;
 
namespace GOAP
{
    public enum StateDef
    {
        HAS_TARGET,
        IS_TARGET_NEAR,
        IS_TARGET_IN_RANGE,
        TARGET_IN_SIGHT,
        IS_IN_DANGER, 
    };

    public class WorldState
    {
        public StateDef WSState { get; private set; }
        public bool WSValue { get; private set; }

        public WorldState(StateDef state, bool value)
        {
            SetState(state, value);
        }

        public void SetState(StateDef state, bool value)
        {
            Debug.Assert(Enum.IsDefined(typeof(StateDef), value));

            WSState = state;
            WSValue = value;
        }
    }

    public class AIWorldStates
    {
        public char[] WStates { get; private set; }  // All world states has the same length of states - length of StateDef.
        public static int StateCount;

        private char[] TempStates;  // Used for copy WStates.

        Dictionary<StateDef, char> m_InitStates;

        public AIWorldStates()
        {
            m_InitStates = new Dictionary<StateDef, char>();
            StateCount = Enum.GetNames(typeof(StateDef)).Length;
            WStates = new char[StateCount];
            TempStates = new char[StateCount];
        }

        public AIWorldStates(string stateString)
        {
            m_InitStates = new Dictionary<StateDef, char>();
            StateCount = Enum.GetNames(typeof(StateDef)).Length;
            WStates = new char[StateCount];
            TempStates = new char[StateCount];

            for (int i = 0; i < StateCount; ++i)
            {
                WStates[i] = stateString[i];
            }
        }

        public void ConstructStates()
        {
            foreach (var state in m_InitStates)
            {
                WStates[((int)state.Key)] = state.Value;
            }
        }

        public void AddState(string stateName, char value)
        {
            StateDef state;
            if (Enum.TryParse<StateDef>(stateName, out state))
            {
                m_InitStates[state] = value;
            }
            ConstructStates();
        }

        public string GetStatesString()
        {  
            // string is good for serialization and hashing.
            return new string(WStates);
        }

        public string GetStateAfterEffect(StateCondition effect)
        {
            // Copy states from current states.
            for (int i = 0; i < StateCount; ++i)
            {
                TempStates[i] = WStates[i];
            }

            foreach (var state in effect.StateDict)
            {
                char stateValue = state.Value ? '1' : '0';
                TempStates[((int)state.Key)] = stateValue;
            }

            return new string(TempStates);
        }

        public List<string> GetDebugList()
        {
            List<string> debugList = new List<string>();

            int len = Enum.GetNames(typeof(StateDef)).Length;
            for (int i = 0; i < len; ++i)
            {
                debugList.Add("" + (StateDef)i + ": " + WStates[i]);
            }

            return debugList;
        }

        public string GetStates()
        {
            return GetStatesString();
        }

        public void SetState(StateDef key, bool value)
        {
            WStates[((int)key)] = value ? '1' : '0';
        }

        public void CopyFromWorldStates(AIWorldStates states)
        {
            Array.Copy(states.WStates, WStates, StateCount);
        }

        public bool IsConditionMet(StateCondition condition)
        {
            foreach (var state in condition.StateDict)
            {
                char stateValue = state.Value ? '1' : '0';
                if (WStates[((int)state.Key)] != stateValue)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetUnmetCount(StateCondition condition)
        {
            int count = 0;

            foreach (var state in condition.StateDict)
            {
                char c = state.Value ? '1' : '0';
                if (WStates[((int)state.Key)] != c)
                {
                    count += 1;
                }
            }

            return count;
        }

    }
}

