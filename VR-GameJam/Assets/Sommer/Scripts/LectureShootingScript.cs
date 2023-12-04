using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;
using TMPro; // Import the TextMeshPro namespace

public class LectureShootingScript : NetworkBehaviour
{
    public GameObject bullet;
    public Transform spawnPosition;
    public AudioClip rifleReloadSound; // Reload sound clip
    public AudioSource audioSource; // Audio source component

    [SerializeField] private GameObject uiCanvas; // Reference to the UI Canvas
    [SerializeField] private TextMeshProUGUI ammoDisplay; // Reference to the TextMeshPro GUI element

    private int max_ammo = 30, current_ammo = 30;
    private float reloadTime = 3, reloadTimer = 3;
    private float shotCooldown = 0.25f; // Time between shots
    private float timeSinceLastShot = 0.0f; // Timer to track time since last shot

    private bool isTriggerHeldDown = false; // Flag to check if trigger is held down

    private void Start()
    {
        if (NetworkManager.Singleton == null) Debug.Log("No network manager yet!");
        NetworkManager.Singleton.AddNetworkPrefab(bullet);

        UpdateAmmoDisplay(); // Initial ammo display update
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        XRGrabInteractable theGunGrabable = GetComponent<XRGrabInteractable>();
        theGunGrabable.activated.AddListener(TriggerPulled);
        theGunGrabable.deactivated.AddListener(TriggerReleased);
        theGunGrabable.selectEntered.AddListener(OnGrabbed);
        theGunGrabable.selectExited.AddListener(OnReleased);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (isTriggerHeldDown && timeSinceLastShot >= shotCooldown && current_ammo > 0)
        {
            BangBang();
            timeSinceLastShot = 0.0f; // Reset the timer
        }
        else if (current_ammo <= 0)
        {
            Reload();
        }
    }

    public void BangBang()
    {
        SpawnBulletServerRPC();
        current_ammo--;
        UpdateAmmoDisplay();
    }

    public void TriggerPulled(ActivateEventArgs arg)
    {
        isTriggerHeldDown = true; // Trigger is being held down
    }

    public void TriggerReleased(DeactivateEventArgs arg)
    {
        isTriggerHeldDown = false; // Trigger is released
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRPC()
    {
        GameObject newBullet = Instantiate(bullet, spawnPosition.position, spawnPosition.rotation);
        NetworkObject netBullet = newBullet.GetComponent<NetworkObject>();
        netBullet.Spawn();
    }

    private void Reload()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(rifleReloadSound);
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
            {
                current_ammo = max_ammo;
                reloadTimer = reloadTime;
                UpdateAmmoDisplay();
            }
        }
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"{current_ammo} / {max_ammo}";
        }
    }

    private void OnGrabbed(SelectEnterEventArgs arg)
    {
        if (uiCanvas != null)
            uiCanvas.SetActive(true); // Show the UI when the rifle is grabbed
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        if (uiCanvas != null)
            uiCanvas.SetActive(false); // Hide the UI when the rifle is released
    }
}

