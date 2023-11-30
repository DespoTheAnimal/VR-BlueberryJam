using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkGrabable : NetworkBehaviour
{

     private NetworkObject netObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        netObject = GetComponent<NetworkObject>();
    }

    public void requestOwnerip(){
         requestOwnership_ServerRpc(NetworkManager.Singleton.LocalClient.ClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void requestOwnership_ServerRpc(ulong clientID){
        netObject.ChangeOwnership(clientID);
        Debug.Log("Changing ownership");
    }
}
