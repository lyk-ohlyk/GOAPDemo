using System;
using UnityEngine;

namespace GOAP
{
    public enum CurveType
    {
        Linear,
        Square,
        Cubic,
    }

    public abstract class BaseCurve
    {
        protected float m_MinValue;
        protected float m_MaxValue;
        protected float m_MinX;
        protected float m_MaxX;

        protected BaseCurve()
        {
            SetCurveEnds();
        }

        public void SetCurveEnds(float minValue=0.0f, float maxValue=1.0f, float minX=0.0f, float maxX=1.0f)
        {
            Debug.Assert(minValue < maxValue);
            Debug.Assert(minX < maxX);

            m_MinValue = minValue;
            m_MaxValue = maxValue;
            m_MinX = minX;
            m_MaxX = maxX;
        }

        public abstract float GetPointValue(float point);

        // lyk dev TODO: editable point insearting.
    }

    class LinearCurve : BaseCurve
    {

        public LinearCurve(): base()
        {

        }

        public override float GetPointValue(float x)
        {
            if (x < m_MinX)
                return m_MinValue;
            else if (x > m_MaxX)
                return m_MaxValue;
            else
            {
                return m_MinValue + (x - m_MinX) / (m_MaxX - m_MinX) * (m_MaxValue - m_MinValue);
            }
        }
    }

    class SquareCurve : BaseCurve
    {
        public SquareCurve() : base()
        {

        }

        public override float GetPointValue(float x)
        {
            if (x < m_MinX)
                return m_MinValue;
            else if (x > m_MaxX)
                return m_MaxValue;
            else
            {
                return m_MinValue + (float)Math.Pow((x - m_MinX) / (m_MaxX - m_MinX), 2) * (m_MaxValue - m_MinValue);
            }
        }
    }

    class CubicCurve : BaseCurve
    {
        public CubicCurve() : base()
        {

        }

        public override float GetPointValue(float x)
        {
            if (x < m_MinX)
                return m_MinValue;
            else if (x > m_MaxX)
                return m_MaxValue;
            else
            {
                return m_MinValue + (float)Math.Pow((x - m_MinX) / (m_MaxX - m_MinX), 3) * (m_MaxValue - m_MinValue);
            }
        }
    }

    public class CurveCreator
    {
        public static BaseCurve CreateCurve(CurveType curveType)
        {
            switch (curveType)
            {
                case CurveType.Linear: 
                    return new LinearCurve();
                case CurveType.Square:
                    return new SquareCurve();
                case CurveType.Cubic:
                    return new CubicCurve();
                default:
                    return new LinearCurve();
            }
        }
    }
}
