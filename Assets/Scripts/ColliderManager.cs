using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ColliderManager : MonoBehaviour
{
    enum Direction {Right, Left}
    [SerializeField] ThoracicHandler t2;
    [SerializeField] Direction direction;
    [SerializeField] float dotProduct;
    [SerializeField] float currentIntensity;
    [SerializeField] float fingerDist;
    GameObject collis;
    Vector3 contactPoint;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Finger") { return; }
        collis = collision.gameObject;
        float intensity = Mathf.Lerp(.1f, 2, Mathf.Pow(1-(transform.position - collis.transform.position).magnitude / .04f,2));
        fingerDist = (transform.position - collis.transform.position).magnitude;
        currentIntensity = intensity;
        t2.touching = true;
        dotProduct = Vector3.Dot(transform.right.normalized, (transform.position - collis.transform.position).normalized);
        Debug.Log($"Dot Product of: {dotProduct}");
        /* THIS WORKS IN THEORY.. HOWEVER FOR OUR CURRENT PURPOSES WE WILL NOT BE MULTIPLYING BY THE DOT PRODUCT AND INSTEAD USE IT ONLY TO DETERMINE DIRECTION
        //t2.maxOffsetDeg = direction == Direction.Left ? Mathf.Lerp(t2.maxOffsetDeg, 10*-dotProduct, Time.deltaTime) : Mathf.Lerp(t2.maxOffsetDeg, -10 * -dotProduct, Time.deltaTime);
        if(dotProduct < 0) { t2.maxOffsetDeg = direction == Direction.Left ? Mathf.Lerp(t2.maxOffsetDeg, 10, Time.deltaTime) : Mathf.Lerp(t2.maxOffsetDeg, -10, Time.deltaTime); }
        else if (dotProduct > 0) { t2.maxOffsetDeg = direction == Direction.Left ? Mathf.Lerp(t2.maxOffsetDeg, -10, Time.deltaTime) : Mathf.Lerp(t2.maxOffsetDeg, 10, Time.deltaTime); }
        else if (dotProduct == 0) { t2.maxOffsetDeg = t2.maxOffsetDeg; } */
        t2.currOffsetDeg = direction == Direction.Left ? Mathf.Lerp(t2.currOffsetDeg, t2.max, Time.deltaTime * intensity) : Mathf.Lerp(t2.currOffsetDeg, t2.min, Time.deltaTime * intensity);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Finger") { return; }
        
        t2.touching = false;
    }

    private void OnDrawGizmos()
    {
        if(collis == null || !t2.touching) { return; }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(collis.transform.position, transform.position + transform.position - collis.transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 10);
    }
}
