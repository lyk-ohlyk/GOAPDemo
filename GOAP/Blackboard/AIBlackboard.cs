using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GOAP;

public class AIBlackboard : MonoBehaviour
{
    Dictionary<string, object> m_BlackboardDict;


    [Tooltip("BlackboardValue")]
    [TextArea(5, 10)]
    public string BlackboardString;

    WeakReference m_TargetRef;

    // Start is called before the first frame update
    void Start()
    {
        m_BlackboardDict = new Dictionary<string, object>();

        // lyk dev TODO: load from config file.
        SetBlackboardValue(BlackboardKeys.BBTargetDist.Str, 100.0);
        SetBlackboardValue(BlackboardKeys.BBTargetName.Str, "");
    }

    public void SetBlackboardValue<T> (string name, T value)
    {
        BlackboardParam<T> param = new BlackboardParam<T>();
        param.SetValue(value);
        m_BlackboardDict[name] = param;

        GetBlackboardString();
    }

    public string GetBlackboardString()
    {
        BlackboardString = "";
        foreach (var valuePair in m_BlackboardDict)
        {
            BlackboardString += "" + valuePair.Key + ":" + valuePair.Value.ToString() + ";\n";
        }

        return BlackboardString;
    }

    public GameObject GetTarget()
    {
        if (m_TargetRef != null && m_TargetRef.IsAlive)
        {
            return m_TargetRef.Target as GameObject;
        }

        string targetName = GetBlackboardValue<string>(BlackboardKeys.BBTargetName.Str);
        GameObject target = null;
        if (targetName != "")
        {
            target = GameObject.Find(targetName);
            if (target != null)
            {
                m_TargetRef = new WeakReference(target);
            }
        }

        return target;
    }

    public T GetBlackboardValue<T> (string name)
    {
        BlackboardParam<T> param = m_BlackboardDict[name] as BlackboardParam<T>;
        return param.GetValue();
    }

    void Update()
    {
        GameObject target = GetTarget();
        if (target == null)
            return;

        // Update the blackboard values with the target.
        double dist = Vector3.Distance(transform.position, target.transform.position);
        SetBlackboardValue(BlackboardKeys.BBTargetDist.Str, dist);
    }
}
