using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationControllerMag : MonoBehaviour
{
    public InputActionProperty pinchAnimationMag;
    public InputActionProperty fistAnimationMag;

    public Animator handAnimator;

    private float pinchValue = 0;
    private float fistValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        //pinchAnimationMag.action.performed += ReadValueForPinch;
        // fistAnimationMag.action.performed += ReadValueForFist;
    }

    // Update is called once per frame
    void Update()
    {
        pinchValue = pinchAnimationMag.action.ReadValue<float>();
        fistValue = fistAnimationMag.action.ReadValue<float>();
        handAnimator.SetFloat("Fist", fistValue);
        handAnimator.SetFloat("Pinch", pinchValue);
    }

    /* 
    
    Tried to use CallbackContext for changing the values, but the values would get stuck


    private void ReadValueForPinch(InputAction.CallbackContext ctx)
    {
        pinchValue = ctx.ReadValue<float>();
        //handAnimator.SetFloat("Pinch", pinchValue);
    }

    private void ReadValueForFist(InputAction.CallbackContext ctx)
    {
        fistValue = ctx.ReadValue<float>();
        //handAnimator.SetFloat("Fist", fistValue);
    }
    */
}
