using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class NetworkGrabable : NetworkBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I am Colliders");
        if (collision.gameObject.CompareTag("Player") && IsOwner)
        {
            var player = collision.gameObject.GetComponent<PlayerNetting>();

            player.IncrementScore();
        }
    }

    private NetworkObject netObject;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        netObject = GetComponent<NetworkObject>();
        XRGrabInteractable theGunGrabable = GetComponent<XRGrabInteractable>();
        theGunGrabable.hoverEntered.AddListener(Touched);
    }

    public void requestOwnerip()
    {
         requestOwnership_ServerRpc(NetworkManager.Singleton.LocalClient.ClientId);
    }

    public void Touched(HoverEnterEventArgs arg0)
    {

        var interactorXrRig = arg0.interactorObject.transform.gameObject.GetComponentInParent<XROrigin>();
        if (interactorXrRig.gameObject == VRrigReference.Singleton.gameObject)
        {
            var player = VRrigReference.Singleton.localPlayer;
            player.IncrementScore();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void requestOwnership_ServerRpc(ulong clientID)
    {
        netObject.ChangeOwnership(clientID);
        Debug.Log("Changing ownership");
    }
}
