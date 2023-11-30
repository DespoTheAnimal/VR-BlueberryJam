using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletBehaviour : NetworkBehaviour
{
    public float bulletSpeed = 10f;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.velocity = transform.forward * bulletSpeed;

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
