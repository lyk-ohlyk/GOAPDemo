using UnityEngine;

namespace GOAP
{
    public enum CostParamType
    {
        WorldState,
        Blackboard,
    }

    class AICost
    {
        public CostParamType CostType { get; private set; }
        public BaseCurve CostCurve { get; private set; }
        public string CostParamName { get; private set; }

        double m_Coefficient;
        bool m_IsConstCost;

        public AICost(double constValue=1.0)
        {
            m_IsConstCost = true;
            m_Coefficient = constValue;
        }

        public AICost(CostParamType paramType, string costParamName, CurveType curveType, double coefficient)
        {
            m_IsConstCost = false;

            CostType = paramType;
            CostParamName = costParamName;
            CostCurve = CurveCreator.CreateCurve(curveType);
            m_Coefficient = coefficient;
        }

        public void SetCostCurve(double minValue = 0.0, double maxValue = 1.0, double minX = 0.0, double maxX = 1.0)
        {
            CostCurve.SetCurveEnds(minValue, maxValue, minX, maxX);
        }

        public double Evaluate(double value)
        {
            if (!m_IsConstCost)
                return m_Coefficient * CostCurve.GetPointValue(value);
            else
                return m_Coefficient;
        }
    }
}
