using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    enum ButtonTypes { FLEX, RESET, EXTEND }
    [SerializeField] ThoracicHandler handler;
    [SerializeField] ButtonTypes buttonType;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Finger")) { return; }
        switch (buttonType)
        {
            case ButtonTypes.FLEX:
                handler.Flexion(Data.getInstance());
                break;
            case ButtonTypes.RESET:
                handler.Base(Data.getInstance());
                break;
            case ButtonTypes.EXTEND:
                handler.Extension(Data.getInstance());
                break;
        }
    }
}
