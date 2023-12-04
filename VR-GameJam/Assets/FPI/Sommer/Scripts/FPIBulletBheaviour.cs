using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPIBulletBheaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private float bulletSpeed = 20f;

    public void Start()
    {
        var rigidBody = GetComponent<Rigidbody>();
        //rigidBody.isKinematic = false;
        rigidBody.velocity = transform.forward * bulletSpeed;
        transform.Rotate(90f, 0f, 0f);
        Destroy(gameObject, 10); 
    }
}
