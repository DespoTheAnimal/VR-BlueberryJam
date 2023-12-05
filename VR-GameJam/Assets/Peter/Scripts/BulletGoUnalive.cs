using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit; // Make sure you have this namespace if you're using XR Interaction Toolkit
using Unity.XR.CoreUtils;

public class BulletGoUnalive : NetworkBehaviour
{
    private Vector3 SpawnPosP1 = new Vector3(140.52f, 1.37f, -150.52f);
    private Vector3 SpawnPosP2 = new Vector3(-14.56f, 0.37f, 15.15f);

    public void OnTriggerEnter(Collider other)
    {
        // Rest of your code...

        if (other.gameObject.CompareTag("Player1"))
        {
            // Get the XR Origin component from the player
            XROrigin playerXROrigin = other.gameObject.GetComponentInParent<XROrigin>();
            ulong playerNetworkObjectId = playerXROrigin.GetComponent<NetworkObject>().NetworkObjectId;
            PlayerHitServerRpc(playerNetworkObjectId, SpawnPosP1);
            // Rest of your code...
        }
        // Similar handling for Player2
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerHitServerRpc(ulong playerNetworkObjectId, Vector3 spawnPosition)
    {
        if (IsServer)
        {
            foreach (var networkObject in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)
            {
                if (networkObject.NetworkObjectId == playerNetworkObjectId)
                {
                    XROrigin xrOrigin = networkObject.GetComponent<XROrigin>();
                    if (xrOrigin != null)
                    {
                        TeleportPlayer(xrOrigin, spawnPosition);
                    }
                    break;
                }
            }
        }
    }

    private void TeleportPlayer(XROrigin xrOrigin, Vector3 newPosition)
    {
        // Adjust the position of the XR Origin
        xrOrigin.transform.position = newPosition;
        Debug.Log("XR Origin teleported to: " + newPosition);
    }
}
