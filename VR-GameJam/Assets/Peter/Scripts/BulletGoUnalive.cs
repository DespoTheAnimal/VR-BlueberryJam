using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;
using Unity.Netcode;

public class BulletGoUnalive : NetworkBehaviour
{
    private Vector3 SpawnPosP1 = new Vector3(140.52f,1.37f,-150.52f);
    private Vector3 SpawnPosP2 = new Vector3(-14.56f,0.37f,15.15f);

    GameObject happened; 
    public void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "SafeZone"){
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player1")
        {
            //PlayerHitServerRPC(other.gameObject.transform.parent.gameObject, SpawnPosP1);
            //other.gameObject.transform.position = PlayerHitServerRPC(SpawnPosP1);
            //happened = other.gameObject;
            ulong playerNetworkObjectId = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            PlayerHitServerRPC(playerNetworkObjectId, SpawnPosP1);
            
            
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player1Kills++;
        } else if (other.gameObject.tag == "Player2")
        {
            //PlayerHitServerRPC(SpawnPosP2);
            //happened = other.gameObject;
            ulong playerNetworkObjectId = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            PlayerHitServerRPC(playerNetworkObjectId, SpawnPosP2);
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player2Kills++;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHitServerRPC(ulong playerNetworkObjectId, Vector3 spawnPosition) {
        //other.GetComponent<clientNetworkTransform>().Teleport(spawn, transform.rotation, transform.localScale);
        foreach (var networkObjects in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)
        {
            if (networkObjects.NetworkObjectId == playerNetworkObjectId)
            {
                networkObjects.GetComponent<NetworkTransform>().Teleport(spawnPosition, Quaternion.identity, Vector3.one);
                Debug.Log("virker det her?");
                break;
            }
        }
        /*NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetNetworkObject(playerNetworkObjectId);
        if (playerNetworkObject != null)
        {
            playerNetworkObject.GetComponent<NetworkTransform>().Teleport(spawnPosition, Quaternion.identity, Vector3.one);
        }*/
    }
}
