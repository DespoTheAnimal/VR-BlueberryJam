using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigReference : MonoBehaviour
{
    public static VRRigReference Singleton;

    public Transform root;
    public Transform Head;
    public Transform LeftHand;
    public Transform RightHand;

    private void Awake(){
        Singleton = this;
    }
}
