using System.Collections;
using System.Collections.Generic;

using GOAP;

public class AIActions
{
    StateCondition m_Precondition;
    StateCondition m_Effect;

    public string BehaviorName;

    public bool IsPreconditionMet(AIWorldStates worldStates)
    {
        return worldStates.IsConditionMet(m_Precondition);
    }

    public AIWorldStates TryPlayAction(AIWorldStates worldStates)
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
}
