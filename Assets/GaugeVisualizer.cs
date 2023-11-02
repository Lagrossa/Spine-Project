using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GaugeVisualizer : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float angle;
    [SerializeField] GameObject point;
    [Range(0,1)][SerializeField] public float t;
    [SerializeField] GameObject hand;
    [SerializeField] ThoracicHandler tHandler;
    private void Update()
    {
        t = Mathf.InverseLerp(-20,20,tHandler.currOffsetDeg);
        hand.transform.LookAt(UpdatePosition(Mathf.Lerp(0, Mathf.Deg2Rad * angle, t)), hand.transform.up);
    }

    Vector3 UpdatePosition(float theta)
    {
        return point.transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(theta) * radius, transform.position.z + Mathf.Cos(theta) * radius);
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Handles.color = Color.cyan;
        Handles.DrawWireArc(transform.position, Vector3.left, transform.right, angle, radius);
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
    }
#endif
}
