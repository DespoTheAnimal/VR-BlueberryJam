using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public GameObject[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            root.position = VRRigReference.Singleton.root.position;
            root.rotation = VRRigReference.Singleton.root.rotation;

            head.position = VRRigReference.Singleton.root.position;
            head.rotation = VRRigReference.Singleton.root.rotation;

            leftHand.position = VRRigReference.Singleton.root.position;
            leftHand.rotation = VRRigReference.Singleton.root.rotation;

            rightHand.position = VRRigReference.Singleton.root.position;
            rightHand.rotation = VRRigReference.Singleton.root.rotation;
        }
    }
}
