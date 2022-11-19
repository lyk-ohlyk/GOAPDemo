using System;
using System.Collections.Generic;

namespace GOAP
{
    class BlackboardParam<T>
    {
        T m_Value;

        public void SetValue(T value)
        {
            m_Value = value;
        }

        public T GetValue()
        {
            return m_Value;
        }
    }
}
