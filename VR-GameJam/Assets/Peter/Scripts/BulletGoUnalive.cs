using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit; // Make sure you have this namespace if you're using XR Interaction Toolkit
using Unity.XR.CoreUtils;

public class BulletGoUnalive : NetworkBehaviour
{
using UnityEngine;
using Unity.Netcode;

public class BulletGoUnalive : NetworkBehaviour
{
    // Define an array of spawn positions
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(14.56f, 0.37f, -15.15f),
        new Vector3(-14.56f, 0.37f, 15.15f),
        new Vector3(-0.03f, -1.77f, -27.70f),
        new Vector3(32f, -1.57f, 1.63f)
    };

    GameObject happened;
    [SerializeField] private GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            // Select a random spawn position
            Vector3 randomPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];

            happened = other.gameObject;
            player.transform.position = randomPosition;

            ulong playerNetworkObjectId = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            // PlayerHitServerRpc logic here
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Rest of your existing methods...
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
                    TeleportPlayer(happened,spawnPosition);
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
                    TeleportPlayer(happened, spawnPosition);
                }
                break;
            }
        }
    }

    private void TeleportPlayer(GameObject xrOrigin, Vector3 newPosition)
    {
        // Adjust the position of the XR Origin
        xrOrigin.transform.position = newPosition;
        Debug.Log("XR Origin teleported to: " + newPosition);
    }
}
