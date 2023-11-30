using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetting : NetworkBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner){
            root.position = VRrigReference.Singleton.root.position;
            root.rotation = VRrigReference.Singleton.root.rotation;

            head.position = VRrigReference.Singleton.head.position;
            head.rotation = VRrigReference.Singleton.head.rotation;

            leftHand.position = VRrigReference.Singleton.leftHand.position;
            leftHand.rotation = VRrigReference.Singleton.leftHand.rotation;

            rightHand.position = VRrigReference.Singleton.rightHand.position;
            rightHand.rotation = VRrigReference.Singleton.rightHand.rotation;
        }
    }
}