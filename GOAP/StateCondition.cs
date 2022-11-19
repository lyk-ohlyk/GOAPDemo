using System;
using System.Collections.Generic;

namespace GOAP
{
    public class StateCondition
    {
        public Dictionary<StateDef, bool> StateDict { get; private set; }

        public StateCondition()
        {
            StateDict = new Dictionary<StateDef, bool>();
        }

        void SetState(List<WorldState> statesList)
        {
            foreach(var state in statesList)
            {
                StateDict[state.WSState] = state.WSValue;
            }
        }
    }
}
