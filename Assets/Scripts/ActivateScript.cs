using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScript : MonoBehaviour
{
    [SerializeField] ThoracicHandler script;
    void Start()
    {
        script.enabled = true;
    }
}
