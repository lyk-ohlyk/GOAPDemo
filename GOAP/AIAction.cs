using System.Collections.Generic;


namespace GOAP
{ 
    public class AIAction
    {
        public string name;
        public string BehaviorName;
        public double ShowCost;

        StateCondition m_Precondition;
        StateCondition m_Effect;

        AICost m_Cost;

        public AIAction()
        {
            m_Cost = new AICost();
        }

        public AIAction(CostParamType paramType, string costParamName, CurveType curveType, double coefficient)
        {
            m_Cost = new AICost(paramType, costParamName, curveType, coefficient);
        }

        public double CalcCost(AIBlackboard blackboard)
        {
            ShowCost = m_Cost.Evaluate(GetParamValue(blackboard));
            return ShowCost;
        }

        private double GetParamValue(AIBlackboard blackboard)
        {
            // lyk dev TODO: Read m_CostParamName from AI blackboard and world states.
            if (m_Cost.CostType == CostParamType.Blackboard)
            {
                return blackboard.GetBlackboardValue<double>(m_Cost.CostParamName);
            }
            else if (m_Cost.CostType == CostParamType.WorldState)
            {
                return 1.0;
            }

            return 0.0;
        }

        public bool IsPreconditionMet(AIWorldStates worldStates)
        {
            return worldStates.IsConditionMet(m_Precondition);
        }

        public bool TryPlayAction(AIWorldStates worldStates)
        {
            // lyk dev TODO: Run behavior tree.
            return false;
        }

        public AIWorldStates GetEffectedStates(AIWorldStates worldStates)
        {
            if (!IsPreconditionMet(worldStates))
            {
                return worldStates;
            }

            AIWorldStates statesAfterEffect = new AIWorldStates();
            statesAfterEffect.CopyFromWorldStates(worldStates);

            foreach (var state in m_Effect.StateDict)
            {
                statesAfterEffect.SetState(state.Key, state.Value);
            }

            return statesAfterEffect;
        }

        public void SetPrecondition(StateCondition stateCondition)
        {
            m_Precondition = stateCondition;
        }

        public void SetEffect(StateCondition stateCondition)
        {
            m_Effect = stateCondition;
        }
    }

}
