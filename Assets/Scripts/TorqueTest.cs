using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTest : MonoBehaviour
{
    [SerializeField] DefinitionHandler values;
    public enum IVDType {
        Flexion,
        Extension,
        LatFlexion,
        AxialRotation
    }

    public double Torque(float theta, float dTheta, IVDType type)
    {
        float p1Val;
        Debug.Log(values.p1.TryGetValue(type, out p1Val));
        Debug.Log(values.p2.TryGetValue(type, out float p2Val));
        Debug.Log(values.p3.TryGetValue(type, out float p3Val));
        return (p1Val * System.Math.Tanh(Mathf.Pow(theta,3)/p2Val) + p3Val * theta) + 100*dTheta;
    }
}
