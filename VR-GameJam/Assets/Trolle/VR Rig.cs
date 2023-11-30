using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;


    public Transform headConstraint;
    public Vector3 headBodyOffset;
    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;
    transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * 90f);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}


[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.forward = vrTarget.TransformDirection(trackingRotationOffset); // forward is the direction the object is facing in world space
    }
}
