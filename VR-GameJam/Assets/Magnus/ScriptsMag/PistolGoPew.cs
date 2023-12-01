using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;
using TMPro; // Import the TextMeshPro namespace to access the various functions.

public class PistolGoPew : NetworkBehaviour
{
    [SerializeField] private GameObject uiCanvas; // Reference to the UI Canvas
    [SerializeField] private TextMeshProUGUI ammoDisplay; // Reference to the TextMeshPro GUI element
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
        if(NetworkManager.Singleton == null) Debug.Log("No network manager yet!");
        NetworkManager.Singleton.AddNetworkPrefab(bullet);
        AS = GameObject.Find("Audio").GetComponent<AudioSource>();

        UpdateAmmoDisplay(); // Update ammo display at start
    }

    private void Awake()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }
    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"{current_ammo} / {max_ammo}"; // Update the text to show current ammo and max ammo
        }
    }

    // Call this method whenever the ammo count changes
    public void ChangeAmmoCount(int amount)
    {
        current_ammo += amount;
        current_ammo = Mathf.Clamp(current_ammo, 0, max_ammo); // Ensure current ammo doesn't exceed max ammo or go below 0
        UpdateAmmoDisplay();
    }

    private void OnGrabbed(SelectEnterEventArgs arg)
    {
        if (uiCanvas != null)
            uiCanvas.SetActive(true); // Show the UI when the gun is grabbed
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        if (uiCanvas != null)
            uiCanvas.SetActive(false); // Hide the UI when the gun is released
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        XRGrabInteractable gun = GetComponent<XRGrabInteractable>();
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gun.activated.AddListener(Shoot);
    }

    private void Update() 
    {
        if (current_ammo <= 0)
            Reload();
    }

    public void Shoot(ActivateEventArgs arg)
    {
        NetShootServerRPC();
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

    [ServerRpc(RequireOwnership = false)]
    public void NetShootServerRPC()
    {
        if (HasAmmo())
        {
            GameObject new_bullet = Instantiate(bullet, shoot_point.position, shoot_point.rotation);
            new_bullet.transform.position = shoot_point.position; 
            new_bullet.transform.forward = shoot_point.forward;
            NetworkObject netBullet = new_bullet.GetComponent<NetworkObject>(); // Instantiate the bullet 
            netBullet.Spawn();
            Debug.Log("Shooting");
            AS.PlayOneShot(HRANG, 0.5f);
            current_ammo--;
            Destroy(new_bullet,time_to_delete);
        }
    }
}
