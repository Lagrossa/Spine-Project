using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisTest : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.rotation * Vector3.up);
        Gizmos.DrawLine(transform.position, transform.position + transform.rotation * -Vector3.up);
    }
}
