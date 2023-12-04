using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlzVirkeMinVen : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefabA;
    [SerializeField] private GameObject playerPrefabB;


    [ServerRpc(RequireOwnership = false)]
    public void SpawnCorrectplayerServerRpc(ulong clientId, int prefabId)
    {
        GameObject newPlayer;
        if (prefabId == 0)
            newPlayer = (GameObject)Instantiate(playerPrefabA);
        else
            newPlayer = (GameObject)Instantiate(playerPrefabB);
        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId, true);
    }
}
