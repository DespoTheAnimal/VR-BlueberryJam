using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerVibrate : MonoBehaviour
{
    [Range(0,1)]
    public float intensity;
    public float duration;
    [SerializeField] private XRBaseController xr;


    private void ActivateHaptic()
    {
        xr.SendHapticImpulse(intensity, duration);
    }

}
