using System;
using System.Collections.Generic;

namespace GOAP
{
    class BlackboardParam<T>
    {
        protected T m_Value;

        public void SetValue(T value)
        {
            m_Value = value;
        }

        public T GetValue()
        {
            return m_Value;
        }

        override public string ToString()
        {
            if (typeof(T) == typeof(double))
            {
                double v = (double)(object)m_Value;
                return "" + Math.Round(v, 2);
            }
            else if (typeof(T) == typeof(float))
            {
                float v = (float)(object)m_Value;
                return "" + Math.Round(v, 2);
            }

            return m_Value.ToString();
        }
    }
}
