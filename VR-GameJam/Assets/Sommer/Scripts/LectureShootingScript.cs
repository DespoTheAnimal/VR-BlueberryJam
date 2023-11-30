using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;

public class LectureShootingScript : NetworkBehaviour
{
    public GameObject bullet;
    public Transform spawnPosition;

    private float bulletSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable theGunGrabable = GetComponent<XRGrabInteractable>();
        theGunGrabable.activated.AddListener(BangBang);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BangBang(ActivateEventArgs arg){
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = spawnPosition.position;
        newBullet.GetComponent<Rigidbody>().velocity = spawnPosition.forward * bulletSpeed;

        Destroy(newBullet, 10);
    }

    [ServerRpc]
    public void SpawnBulletServerRPC(){
        GameObject newBullet = Instantiate(bullet, spawnPosition.position, spawnPosition.rotation);
        NetworkObject netBullet = newBullet.GetComponent<NetworkObject>();
        netBullet.Spawn();
        newBullet.GetComponent<Rigidbody>().velocity = spawnPosition.forward * bulletSpeed;

        Destroy(newBullet, 10);
    }
}
