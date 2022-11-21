using UnityEngine;

namespace GOAP
{
    public class AIGoal
    {
        public string name;
        public StateCondition TargetStates { get; private set; }
        public StateCondition PreCondition { get; private set; }

        public void SetTargetState(StateCondition state)
        {
            TargetStates = state;
        }

        public void SetPreCondition(StateCondition condition)
        {
            PreCondition = condition;
        }

        public int GetUnmetStatesCount(AIWorldStates worldStates)
        {
            return worldStates.GetUnmetCount(TargetStates);
        }
    }
}
