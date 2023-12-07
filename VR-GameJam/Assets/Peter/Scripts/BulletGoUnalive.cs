using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit; // Make sure you have this namespace if you're using XR Interaction Toolkit
using Unity.XR.CoreUtils;

public class BulletGoUnalive : NetworkBehaviour
{
    // Define an array of spawn positions
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(14.56f, 0.37f, -15.15f),
        new Vector3(-14.56f, 0.37f, 15.15f),
        new Vector3(15.25f,0.37f,14.56f),
        new Vector3(-15.25f,0.37f,-14.56f)
    };

    GameObject happened1;
    GameObject Happened2;
    private GameObject player;
    private GameObject player2;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            // Select a random spawn position
            Vector3 randomPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];

            happened1 = other.gameObject;
            player = other.gameObject.GetComponent<PlayerNetting>().xRorigin;
            player.transform.position = randomPosition;

            ulong playerNetworkObjectId = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            // PlayerHitServerRpc logic here
        }
        if (other.gameObject.CompareTag("Player2")){
            Vector3 randomPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];

            Happened2 = other.gameObject;
            player2 = other.gameObject.GetComponent<PlayerNetting>().xRorigin;
            player2.transform.position = randomPosition;

            ulong playerNetworkObjectId = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;

        }
    }

    void Start()
    {
        //player = GameObject.Find("Player");
    }

    // Rest of your existing methods...

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
                    TeleportPlayer(happened1,spawnPosition);
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
                    TeleportPlayer(happened2, spawnPosition);
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
