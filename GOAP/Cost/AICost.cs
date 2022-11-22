using System;
using UnityEngine;

namespace GOAP
{
    public enum CostType
    {
        Const,
        WorldState,
        Blackboard,
    }

    class AICost
    {
        public CostType CostType { get; private set; }
        public BaseCurve CostCurve { get; private set; }
        public string CostParamName { get; private set; }

        float m_Coefficient;

        Func<float> m_CostFunc = ()=> { return 1f; };

        public AICost(float constValue =1.0f)
        {
            CostType = CostType.Const;
            m_Coefficient = constValue;
        }

        public AICost(CostType paramType, string costParamName, CurveType curveType, float coefficient)
        {
            CostType = paramType;
            CostParamName = costParamName;
            CostCurve = CurveCreator.CreateCurve(curveType);
            m_Coefficient = coefficient;
        }

        public void SetCostCurve(float minValue = 0.0f, float maxValue = 1.0f, float minX = 0.0f, float maxX = 1.0f)
        {
            CostCurve.SetCurveEnds(minValue, maxValue, minX, maxX);
        }

        public float Evaluate(float value)
        {

            float cost;
            if (!(CostType == CostType.Const))
                cost = m_Coefficient * CostCurve.GetPointValue(value) * m_CostFunc.Invoke();
            else
                cost = m_Coefficient * m_CostFunc.Invoke();

            cost += 1.0f;  // 为什么要加1：为了保证A*算法能找到最优解(启发式函数要满足 h(x) ≤ d(x, y) + h(y))
            return cost;
        }

        public void SetCostFunc(Func<float> costFunc)
        {
            m_CostFunc = costFunc;
        }
    }
}
