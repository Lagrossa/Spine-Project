using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ligamentManager : MonoBehaviour
{
    [SerializeField] GameObject lig1;
    [SerializeField] GameObject lig2;
    [SerializeField] Vector3 vectl1l2;
    [SerializeField] float dist;
    private void OnDrawGizmos()
    {
        vectl1l2 = lig2.transform.position - lig1.transform.position;
        dist = vectl1l2.magnitude;
        Gizmos.DrawLine(lig1.transform.position, lig2.transform.position);
    }
}
