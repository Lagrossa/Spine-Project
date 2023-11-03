using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThoracicHandler : MonoBehaviour
{
    enum State { BASE, FLEXED, EXTENDED};
    [SerializeField] State state = State.BASE;
    public float min = -20;
    public float max = 20;
    [SerializeField][Range(-20,20)] public float currOffsetDeg = 0;
    [Header("References")]
    [SerializeField] GameObject pointRef;
    [SerializeField] GameObject pointPerm;
    [SerializeField] Transform[] thoracicVertebrae;
    [SerializeField] GameObject rotationAxis;
    [Tooltip("Whether or not a finger is touching the transverse process.")]
    [SerializeField] public bool touching;
    [Header("Management")]
    [SerializeField] bool returnToNormal;
    [Header("Tweaking")]
    [Range(.01f, .99f)][SerializeField] float weaknessPolicy = .245f;
    Vector3 startingPos;
    Vector3 parRot;
    Quaternion startingRot;
    Vector3 posOffset;
    Vector3 rotOffset;
    Transform flexionRef;

    private void Start()
    {
        posOffset = transform.position - rotationAxis.transform.position;
        startingPos = transform.position;
        startingRot = transform.rotation;
        parRot = transform.parent.rotation.eulerAngles;
    }

    //Touch Rotation
    private void Update()
    {
        //transform.localPosition = startingPos;
        Quaternion multiplier = Quaternion.identity;
        Vector3 properVector = rotationAxis.transform.up;
        switch (state)
        {
            case State.BASE:
                int lastIndex = Data.getInstance().thoracicVertebraeBase.Count-1;
                multiplier = Data.getInstance().thoracicVertebraeBase[1].transform.rotation;
                break;
            case State.FLEXED:
                int lastFlexIndex = Data.getInstance().thoracicVertebraeFlex.Count - 1;
                multiplier = Data.getInstance().thoracicVertebraeFlex[1].transform.rotation;
                break;
            case State.EXTENDED:
                int lastExtendIndex = Data.getInstance().thoracicVertebraeExtend.Count - 1;
                multiplier = Data.getInstance().thoracicVertebraeExtend[1].transform.rotation;
                break;
        }
        transform.parent.rotation = Quaternion.AngleAxis(currOffsetDeg, properVector) * multiplier;
        //Quaternion v3 = (rotationAxis.transform.rotation * Quaternion.AngleAxis(maxOffsetDeg, rotationAxis.transform.up));
        //transform.parent.rotation = v3;
        //Reverts to base rotation.
        if (!returnToNormal) { return; }
        if (!touching) { currOffsetDeg += 5 * (-currOffsetDeg / 2) * Time.deltaTime; }
        else { currOffsetDeg += 5 * (-currOffsetDeg / 2) * Time.deltaTime * weaknessPolicy; }

    }

    private void OnDrawGizmos() 
    {
        //float rotationDeg = Quaternion.Euler(new Vector3(0, maxOffsetDeg, 0)).eulerAngles.y * Mathf.Rad2Deg;
        //print($"{rotationDeg} rotation degrees"); 
        Vector3 rightPos = transform.position + Quaternion.AngleAxis(10, Vector3.up) * pointRef.transform.localPosition;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rightPos, .01f);
        Vector3 leftPos = transform.position + Quaternion.AngleAxis(-10, Vector3.up) * pointRef.transform.localPosition;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftPos, .01f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, rightPos);
        Gizmos.DrawLine(transform.position, leftPos);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(leftPos, pointPerm.transform.position);
        Gizmos.DrawLine(rightPos, pointPerm.transform.position);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(rotationAxis.transform.position, rotationAxis.transform.position + rotationAxis.transform.up);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.parent.up);
    }

    // FLEXED SPINE =-=-=-=-=-=-=-=-=

    public void WriteFlexion(Data data)
    {
        data.ClearFlex();
        flexionRef = transform;
        for (int x = 0; x < 6; x++)
        {
            data.thoracicVertebraeFlex.Add(flexionRef);
            Debug.Log($" Written index {x} as {flexionRef.name}");
            flexionRef = flexionRef.parent;
        }
        data.thoracicVertebraeFlex.Add(rotationAxis.transform);
    }

    public void Flexion(Data data)
    {
        flexionRef = transform;
        for (int x = 0; x < 6; x++)
        {
            StartCoroutine(FlexPositionRoutine(data, x, flexionRef.localPosition, (callBack) =>
            {
                flexionRef.localPosition = callBack;
            }));
            StartCoroutine(FlexRotationRoutine(data, x, flexionRef.localRotation, (callBack) =>
            {
                flexionRef.localRotation = callBack;
            }));

            //Debugging

            /*
            Debug.Log($"Flexed {flexionRef.name} as {x}");
            Debug.Log($"\n{x} position {data.thoracicVertebraeFlex[x].position} \n {x} rotation {data.thoracicVertebraeFlex[x].rotation}");
            */

            flexionRef = flexionRef.parent;
        }
        parRot = transform.parent.rotation.eulerAngles;
        Vector3.Slerp(rotationAxis.transform.position, data.thoracicVertebraeFlex[data.thoracicVertebraeFlex.Count - 1].localPosition, 1);
        Quaternion.Slerp(rotationAxis.transform.rotation, data.thoracicVertebraeFlex[data.thoracicVertebraeFlex.Count - 1].localRotation, 1);
        state = State.FLEXED;
    }

    IEnumerator FlexPositionRoutine(Data data, int index, Vector3 startPos, System.Action<Vector3> callBack)
    {
        for (float posTVal = 0; posTVal < 1; posTVal += .01f)
        {
            callBack(Vector3.Slerp(startPos, data.thoracicVertebraeFlex[index].localPosition, posTVal));
            //yield return 0;
        }
        yield return null;
    }
    IEnumerator FlexRotationRoutine(Data data, int index, Quaternion startRot, System.Action<Quaternion> callBack)
    {
        for (float rotTVal = 0; rotTVal < 1; rotTVal += .01f)
        {
            callBack(Quaternion.Slerp(startRot, data.thoracicVertebraeFlex[index].localRotation, rotTVal));
            //yield return 0;
        }
        yield return null;
    }

    // BASE SPINE =-=-=-=-=-=-=-=-=

    public void WriteBase(Data data)
    {
        data.ClearBase();
        flexionRef = transform;
        for (int x = 0; x < 6; x++)
        {
            data.thoracicVertebraeBase.Add(flexionRef);
            Debug.Log($" Written index {x} as {flexionRef.name}");
            flexionRef = flexionRef.parent;
        }
        data.thoracicVertebraeBase.Add(rotationAxis.transform);
    }

    public void Base(Data data)
    {
        flexionRef = transform;
        for (int x = 0; x < 6; x++)
        {
            StartCoroutine(BasePositionRoutine(data, x, flexionRef.localPosition, (callBack) =>
            {
                flexionRef.localPosition = callBack;
            }));
            StartCoroutine(BaseRotationRoutine(data, x, flexionRef.localRotation, (callBack) =>
            {
                flexionRef.localRotation = callBack;
            }));

            //Debugging

            /*
            Debug.Log($"Flexed {flexionRef.name} as {x}");
            Debug.Log($"\n{x} position {data.thoracicVertebraeFlex[x].position} \n {x} rotation {data.thoracicVertebraeFlex[x].rotation}");
            */

            flexionRef = flexionRef.parent;
        }
        parRot = transform.parent.rotation.eulerAngles;
        Vector3.Slerp(rotationAxis.transform.position, data.thoracicVertebraeBase[data.thoracicVertebraeBase.Count - 1].localPosition, 1);
        Quaternion.Slerp(rotationAxis.transform.rotation, data.thoracicVertebraeBase[data.thoracicVertebraeBase.Count - 1].localRotation, 1);
        state = State.BASE;
    }

    IEnumerator BasePositionRoutine(Data data, int index, Vector3 startPos, System.Action<Vector3> callBack)
    {
        for (float posTVal = 0; posTVal < 1; posTVal += .01f)
        {
            callBack(Vector3.Slerp(startPos, data.thoracicVertebraeBase[index].localPosition, posTVal));
            //yield return 0;
        }
        yield return null;
    }
    IEnumerator BaseRotationRoutine(Data data, int index, Quaternion startRot, System.Action<Quaternion> callBack)
    {
        for (float rotTVal = 0; rotTVal < 1; rotTVal += .01f)
        {
            callBack(Quaternion.Slerp(startRot, data.thoracicVertebraeBase[index].localRotation, rotTVal));
            //yield return 0;
        }
        yield return null;
    }

    // EXTENDED SPINE =-=-=-=-=-=-=-=-=

    public void WriteExtension(Data data)
    {
        data.ClearExtend();
        flexionRef = transform;
        for (int x = 0; x < 6; x++)
        {
            data.thoracicVertebraeExtend.Add(flexionRef);
            Debug.Log($" Written index {x} as {flexionRef.name}");
            flexionRef = flexionRef.parent;
        }
        data.thoracicVertebraeExtend.Add(rotationAxis.transform);
    }

    public void Extension(Data data)
    {
        flexionRef = transform;
        for (int x = 0; x < 6; x++)
        {
            StartCoroutine(ExtendPositionRoutine(data, x, flexionRef.localPosition, (callBack) =>
            {
                flexionRef.localPosition = callBack;
            }));
            StartCoroutine(ExtendRotationRoutine(data, x, flexionRef.localRotation, (callBack) =>
            {
                flexionRef.localRotation = callBack;
            }));

            flexionRef = flexionRef.parent;
        }
        parRot = transform.parent.rotation.eulerAngles;
        Vector3.Slerp(rotationAxis.transform.position, data.thoracicVertebraeExtend[data.thoracicVertebraeExtend.Count - 1].localPosition, 1);
        Quaternion.Slerp(rotationAxis.transform.rotation, data.thoracicVertebraeExtend[data.thoracicVertebraeExtend.Count - 1].localRotation, 1);
        state = State.EXTENDED;
    }

    IEnumerator ExtendPositionRoutine(Data data, int index, Vector3 startPos, System.Action<Vector3> callBack)
    {
        for (float posTVal = 0; posTVal < 1; posTVal += .01f)
        {
            callBack(Vector3.Slerp(startPos, data.thoracicVertebraeExtend[index].localPosition, posTVal));
            //yield return 0;
        }
        yield return null;
    }
    IEnumerator ExtendRotationRoutine(Data data, int index, Quaternion startRot, System.Action<Quaternion> callBack)
    {
        for (float rotTVal = 0; rotTVal < 1; rotTVal += .01f)
        {
            callBack(Quaternion.Slerp(startRot, data.thoracicVertebraeExtend[index].localRotation, rotTVal));
            //yield return 0;
        }
        yield return null;
    }
}
