using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertebrae : MonoBehaviour
{
    [SerializeField] TorqueTest.IVDType type;
    [SerializeField] float xTheta;
    [SerializeField] float yTheta;
    [SerializeField] float zTheta;
    [SerializeField] DefinitionHandler dictionary;
    [SerializeField] TorqueTest torque;
    private void Update()
    {
        double xRot = torque.Torque(0,0, type);
        double yRot = torque.Torque(10, 0, type);
        double zRot = torque.Torque(0, 0, type);
        transform.Rotate(new Vector3((float)xRot, (float)yRot, (float)zRot));
    }
}
