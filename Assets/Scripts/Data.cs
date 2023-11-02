using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class Data : MonoBehaviour
{
    public static Data instance;

    //Index = T(N). 0 - T1, 1 - T2, 2 - T3, etc.
    //RotAxis is added last.
    public List<Transform> thoracicVertebraeFlex;
    public List<Transform> thoracicVertebraeBase;
    public List<Transform> thoracicVertebraeExtend;
    private void Awake()
    {
        instance = this;
    }

    public static Data getInstance()
    {
        return instance;
    }

    public void ClearFlex()
    {
        thoracicVertebraeFlex.Clear();
    }
    public void ClearExtend()
    {
        thoracicVertebraeExtend.Clear();
    }

    public void ClearBase()
    {
        thoracicVertebraeBase.Clear();
    }
}
