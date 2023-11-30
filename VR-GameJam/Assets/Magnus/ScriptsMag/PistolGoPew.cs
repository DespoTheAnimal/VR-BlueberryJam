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
    public AudioClip HRANG, pistol_reload;
    private int factor_for_bullet = 250;
    private int time_to_delete = 5; 
    
    private int max_ammo = 12, current_ammo = 12;

    private float time_to_reload = 3, max_time = 3;

    private void Start() 
    {
        XRGrabInteractable gun = GetComponent<XRGrabInteractable>();
        gun.activated.AddListener(Shoot);
        AS = GameObject.Find("Audio").GetComponent<AudioSource>();
    }
    private void Update() 
    {
        if (current_ammo <= 0)
            Reload();
    }

    public void Shoot(ActivateEventArgs arg)
    {
        if (HasAmmo())
        {
            GameObject new_bullet = Instantiate(bullet); // Instantiate the bullet 
            new_bullet.transform.position = shoot_point.position; 
            new_bullet.transform.forward = shoot_point.forward;
            new_bullet.transform.Rotate(90f, 0f, 0f);
            new_bullet.GetComponent<Rigidbody>().AddForce(shoot_point.forward * factor_for_bullet);
            AS.PlayOneShot(HRANG, 0.5f);
            current_ammo--;
            Destroy(new_bullet,time_to_delete);
        }
    }

    private bool HasAmmo()
    {
        if (current_ammo <= 0)
        {
            return false;
        }
        else
            return true;

    }

    private void Reload()
    {
        if (!AS.isPlaying)
            AS.PlayOneShot(pistol_reload);
        time_to_reload -= Time.deltaTime;
        if (current_ammo == 0 && time_to_reload <= 0)
        {
           current_ammo = max_ammo; 
           time_to_reload = max_time;
        }
    }
}
