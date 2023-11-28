using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolGoPew : MonoBehaviour
{
    [SerializeField] Transform shoot_point;
    [SerializeField] GameObject bullet;
    private int factor = 100;
    private void Start() 
    {
        XRGrabInteractable gun = GetComponent<XRGrabInteractable>();
        gun.activated.AddListener(Shoot);

    }

    public void Shoot(ActivateEventArgs arg)
    {
        GameObject new_bullet = Instantiate(bullet);
        new_bullet.transform.position = shoot_point.position;
        new_bullet.transform.forward = shoot_point.forward;
        new_bullet.transform.Rotate(90f, 0f, 0f);
        new_bullet.GetComponent<Rigidbody>().AddForce(shoot_point.forward * factor);
        Destroy(new_bullet,factor);
    }
}
