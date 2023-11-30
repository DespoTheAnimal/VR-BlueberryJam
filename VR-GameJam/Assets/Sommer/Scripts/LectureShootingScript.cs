using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;

public class LectureShootingScript : NetworkBehaviour
{
    public GameObject bullet;
    public Transform spawnPosition;

    private void Start()
    {
        if (NetworkManager.Singleton == null) Debug.Log("No network manager yet!");
        NetworkManager.Singleton.AddNetworkPrefab(bullet);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        XRGrabInteractable theGunGrabable = GetComponent<XRGrabInteractable>();
        theGunGrabable.activated.AddListener(BangBang);
    }

    public void BangBang(ActivateEventArgs arg)
    {
        SpawnBulletServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRPC(){
        GameObject newBullet = Instantiate(bullet, spawnPosition.position, spawnPosition.rotation);
        NetworkObject netBullet = newBullet.GetComponent<NetworkObject>();
        netBullet.Spawn();
    }
}
