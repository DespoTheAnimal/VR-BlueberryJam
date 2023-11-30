using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletBehaviour : NetworkBehaviour
{
    private float bulletSpeed = 250f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.AddForce(transform.forward * bulletSpeed);
        StartCoroutine(Despawn());
        
    }


    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRPC()
    {
        var netObject = GetComponent<NetworkObject>();
        netObject.Despawn();
    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10);

        DespawnServerRPC();
    }
}
