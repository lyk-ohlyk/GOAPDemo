using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class StateCondition
    {
        public Dictionary<StateDef, bool> StateDict { get; private set; }

        public StateCondition()
        {
            StateDict = new Dictionary<StateDef, bool>();
        }

        public void SetState(string stateName, bool value)
        {
            StateDef state;
            if (Enum.TryParse<StateDef>(stateName, out state))
            {
                StateDict[state] = value;
            }
        }

        public void DebugPrint()
        {
            foreach (var state in StateDict)
            {
                Debug.Log("StateCondition:" + state.Key + ", " + state.Value);
            }
        }
    }
}
