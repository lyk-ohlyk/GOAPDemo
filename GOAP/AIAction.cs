using System;


namespace GOAP
{ 
    public class AIAction
    {
        public string name;
        public string BehaviorName;
        public float ShowCost;

        StateCondition m_Precondition;
        public StateCondition ActionEffect { get; private set; }

        AICost m_Cost;

        public AIAction()
        {
            m_Cost = new AICost();
        }

        public AIAction(CostType paramType, string costParamName, CurveType curveType, float coefficient)
        {
            m_Cost = new AICost(paramType, costParamName, curveType, coefficient);
        }

        public float CalcCost(AIBlackboard blackboard)
        {
            ShowCost = m_Cost.Evaluate(GetParamValue(blackboard));
            return ShowCost;
        }

        public void SetCostFunc(Func<float> costFuncInfo)
        {
            m_Cost.SetCostFunc(costFuncInfo);
        }

        private float GetParamValue(AIBlackboard blackboard)
        {
            // lyk dev TODO: Read m_CostParamName from AI blackboard and world states.
            if (m_Cost.CostType == CostType.Blackboard)
            {
                return blackboard.GetBlackboardValue<float>(m_Cost.CostParamName);
            }
            else if (m_Cost.CostType == CostType.WorldState)
            {
                return 1.0f;  // lyk dev TODO
            }

            return 0.0f;
        }

        public bool IsPreconditionMet(AIWorldStates worldStates)
        {
            return worldStates.IsConditionMet(m_Precondition);
        }

        public bool IsEffectMet(AIWorldStates worldStates)
        {
            return worldStates.IsConditionMet(ActionEffect);
        }

        public AIWorldStates GetEffectedStates(AIWorldStates worldStates)
        {
            if (!IsPreconditionMet(worldStates))
            {
                return worldStates;
            }

            AIWorldStates statesAfterEffect = new AIWorldStates();
            statesAfterEffect.CopyFromWorldStates(worldStates);

            foreach (var state in ActionEffect.StateDict)
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
            ActionEffect = stateCondition;
        }
    }

}
