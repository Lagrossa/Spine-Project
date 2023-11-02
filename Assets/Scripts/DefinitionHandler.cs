using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DefinitionHandler : MonoBehaviour
{
    public Dictionary<TorqueTest.IVDType, float> p1;
    public Dictionary<TorqueTest.IVDType, float> p2;
    public Dictionary<TorqueTest.IVDType, float> p3;
    void Start()
    {
        //P1 Definitions
        p1.Add(TorqueTest.IVDType.Flexion, 10.67f);
        p1.Add(TorqueTest.IVDType.Extension, 5.196f);
        p1.Add(TorqueTest.IVDType.LatFlexion, 6.929f);
        p1.Add(TorqueTest.IVDType.AxialRotation, 4.399f);
        Debug.Log("Added P1");
        //P2 Definitions
        p2.Add(TorqueTest.IVDType.Flexion, 0.005913f);
        p2.Add(TorqueTest.IVDType.Extension, 0.008417f);
        p2.Add(TorqueTest.IVDType.LatFlexion, 0.001482f);
        p2.Add(TorqueTest.IVDType.AxialRotation, 0.001141f);
        Debug.Log("Added P2");
        //P3 Definitions
        p3.Add(TorqueTest.IVDType.Flexion, -1.685f);
        p3.Add(TorqueTest.IVDType.Extension, 24.50f);
        p3.Add(TorqueTest.IVDType.LatFlexion, 19.76f);
        p3.Add(TorqueTest.IVDType.AxialRotation, 42.38f);
        Debug.Log("Added P3");
    }
}
