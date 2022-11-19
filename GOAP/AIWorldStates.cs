using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace GOAP
{
    public enum StateDef
    {
        HAS_GUN,
        HAS_TARGET,
        IS_TARGET_NEAR,
        IS_LOW_HP,
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

        Dictionary<StateDef, char> m_InitStates;

        public AIWorldStates()
        {
            ConstructStates();
        }

        void ConstructStates()
        {
            LoadFromConfig();

            StateCount = Enum.GetNames(typeof(StateDef)).Length;

            WStates = new char[StateCount];
            foreach (var state in m_InitStates)
            {
                WStates[((int)state.Key)] = state.Value;
            }
        }

        void LoadFromConfig()
        {
            // lyk dev TODO: read config file.
            m_InitStates = new Dictionary<StateDef, char>()
            {
                { StateDef.HAS_GUN, '0'},
                { StateDef.HAS_TARGET, '0'},
                { StateDef.IS_LOW_HP, '0'},
                { StateDef.IS_TARGET_NEAR, '0'},
            };
        }

        public string GetStatesString()
        {  
            // string is good for serialization and hashing.
            return new string(WStates);
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

