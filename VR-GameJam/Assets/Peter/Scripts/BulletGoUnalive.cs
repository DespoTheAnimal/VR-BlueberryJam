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
    GameObject happened;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player1"))
            {
                // Assuming this script is on the server
                GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().IncreasePlayer1KillsServerRpc();
                happened = other.gameObject;
                ulong playerNetworkObjectId1 = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                PlayerHitServerRpc(playerNetworkObjectId1, SpawnPosP1);
            }
            else if (other.gameObject.CompareTag("Player2"))
            {
                // Assuming this script is on the server
                GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().IncreasePlayer2KillsServerRpc();
                happened = other.gameObject;
                ulong playerNetworkObjectId2 = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                PlayerHitServerRpc(playerNetworkObjectId2, SpawnPosP2);
            }
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
                    /*XROrigin xrOrigin = networkObject.GetComponent<XROrigin>();
                    if (xrOrigin != null)
                    {
                        TeleportPlayer(xrOrigin, spawnPosition);
                    }*/
                    happened.transform.position = spawnPosition;
                    break;
                }
            }
        }
    }

    [ClientRpc]
    private void PlayerHitClientRPC(ulong playerNetworkObjectId, Vector3 spawnPosition)
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

    private void TeleportPlayer(XROrigin xrOrigin, Vector3 newPosition)
    {
        // Adjust the position of the XR Origin
        xrOrigin.transform.position = newPosition;
        Debug.Log("XR Origin teleported to: " + newPosition);
    }
}
