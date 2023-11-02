using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Extension : MonoBehaviour
{
    [SerializeField] Transform[] vertebrae = new Transform[17];
    [Range(-5, 5)]
    [SerializeField] public float extensionAmountDeg;
    float extensionAmountAcc;
    [SerializeField] float extensionModifier;
    [SerializeField] GameObject test;
    [SerializeField] float sign;
    [SerializeField] GameObject slider;
    [SerializeField] float sigmoidDampening;
    [Range(0, 1)]
    [SerializeField] float percent;
    public void UpdateDegrees()
    {
        extensionAmountDeg = slider.GetComponent<UnityEngine.UI.Slider>().value;
    }

    float Sigmoid(float x)
    {
        return sigmoidDampening / (1 + Mathf.Exp(-x));
    }
    void Update()
    {
        /*
        foreach(Transform vert in vertebrae)
        {
            vert.localRotation = Quaternion.Euler(0, 0, extensionAmountDeg);
        }*/
        extensionAmountAcc = extensionAmountDeg;
        
        /*for (int x = 1; x <= vertebrae.Length; x++)
        {
            sign = extensionAmountDeg >= 0 ? 1 : -1;
            extensionAmountAcc = (extensionModifier * extensionAmountAcc*(vertebrae.Length-x));            
            vertebrae[^x].localRotation = Quaternion.Euler(0, 0, extensionAmountAcc);
        }*/
        /* TEST 1
        for (int x = 0; x < vertebrae.Length; x++)
        {
            sign = extensionAmountDeg >= 0 ? 1 : -1;
            //extensionAmountAcc = (extensionModifier * extensionAmountAcc*(vertebrae.Length-x));
            extensionAmountAcc = Mathf.Pow((percent * extensionAmountDeg), 2);
            vertebrae[x].localRotation = Quaternion.Euler(0, 0, extensionAmountAcc);            

        } TEST 2*/
        
        for (int x = 1; x <= vertebrae.Length; x++)
        {
            sign = extensionAmountDeg >= 0 ? 1 : -1;
            //extensionAmountAcc = (extensionModifier * extensionAmountAcc*(vertebrae.Length-x));
            extensionAmountAcc = Mathf.Pow((percent * extensionAmountDeg), 2) * sign;
            vertebrae[^x].localRotation = Quaternion.Euler(0, 0, extensionAmountAcc);
        }
        
    }
}
