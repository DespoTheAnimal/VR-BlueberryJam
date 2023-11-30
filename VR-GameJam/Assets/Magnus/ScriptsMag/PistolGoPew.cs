using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolGoPew : MonoBehaviour
{
    [SerializeField] Transform shoot_point;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioSource AS;
    public AudioClip HRANG;
    private int factor = 100;
    private int time_to_delete = 5; 
    private void Start() 
    {
        XRGrabInteractable gun = GetComponent<XRGrabInteractable>();
        gun.activated.AddListener(Shoot);

    }

    public void Shoot(ActivateEventArgs arg)
    {
        GameObject new_bullet = Instantiate(bullet); // Instantiate the bullet 
        new_bullet.transform.position = shoot_point.position; 
        new_bullet.transform.forward = shoot_point.forward;
        new_bullet.transform.Rotate(90f, 0f, 0f);
        new_bullet.GetComponent<Rigidbody>().AddForce(shoot_point.forward * factor);
        AS.PlayOneShot(HRANG);
        Destroy(new_bullet,time_to_delete);
    }
}
