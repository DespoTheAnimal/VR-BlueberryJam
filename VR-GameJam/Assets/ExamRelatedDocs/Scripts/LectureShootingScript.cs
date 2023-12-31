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
    public AudioClip rifleShootingSound; // Shooting sound clip
    public AudioSource audioSource; // Audio source component

    [SerializeField] private GameObject uiCanvas; // Reference to the UI Canvas
    [SerializeField] private TextMeshProUGUI ammoDisplay; // Reference to the TextMeshPro GUI element

    private int max_ammo = 30, current_ammo = 30;
    private float reloadTime = 3, reloadTimer = 3;
    private float shotCooldown = 0.15f; // Time between shots
    private float timeSinceLastShot = 0.0f; // Timer to track time since last shot

    private bool isTriggerHeldDown = false; // Flag to check if trigger is held down


    private void Start()
    {
        if (NetworkManager.Singleton == null) Debug.Log("No network manager yet!");
        NetworkManager.Singleton.AddNetworkPrefab(bullet);

        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(TriggerHaptic);

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

        if (current_ammo <= 0)
        {
            StopHapticFeedback(); // Stop haptic feedback when ammo is depleted
            Reload();
        }
    }

    public void BangBang()
    {
        SpawnBulletServerRPC();
        current_ammo--;
        UpdateAmmoDisplay();

        // Call TriggerHaptic here
        if (GetComponent<XRBaseInteractor>() is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    private Coroutine hapticCoroutine;

    private void StartContinuousHaptic(XRBaseController controller)
    {
        if (hapticCoroutine != null)
        {
            StopCoroutine(hapticCoroutine);
        }
        hapticCoroutine = StartCoroutine(ContinuousHapticRoutine(controller));
    }

    private IEnumerator ContinuousHapticRoutine(XRBaseController controller)
    {
        while (isTriggerHeldDown && current_ammo > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
            yield return new WaitForSeconds(shotCooldown); // Wait for the next shot
        }
    }



    public void TriggerPulled(ActivateEventArgs arg)
    {
        isTriggerHeldDown = true; // Trigger is being held down

        // Start continuous haptic feedback
        if (arg.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            StartContinuousHaptic(controllerInteractor.xrController);
        }
    }

    private void StopHapticFeedback()
    {
        if (hapticCoroutine != null)
        {
            StopCoroutine(hapticCoroutine);
            hapticCoroutine = null;
        }
    }

    public void TriggerReleased(DeactivateEventArgs arg)
    {
        isTriggerHeldDown = false; // Trigger is released
        StopHapticFeedback(); // Stop haptic feedback
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRPC()
    {
        GameObject newBullet = Instantiate(bullet, spawnPosition.position, spawnPosition.rotation);
        NetworkObject netBullet = newBullet.GetComponent<NetworkObject>();
        netBullet.Spawn();

        // Play shooting sound effect
        PlaySoundServerRPC();
        //audioSource.PlayOneShot(rifleShootingSound);
    }

    [ServerRpc]
    private void PlaySoundServerRPC()
    {
        PlaySoundClientRPC();
    }

    [ClientRpc]
    private void PlaySoundClientRPC()
    {
        audioSource.PlayOneShot(rifleShootingSound);
    }

    private void Reload()
    {
        // Check if reloading needs to start
        if (current_ammo <= 0 && reloadTimer == reloadTime)
        {
            audioSource.PlayOneShot(rifleReloadSound); // Start reload sound
            reloadTimer -= Time.deltaTime; // Start counting down the timer
        }
        else if (reloadTimer < reloadTime && reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime; // Continue counting down the timer
        }
        else if (reloadTimer <= 0)
        {
            current_ammo = max_ammo; // Reset ammo to max
            reloadTimer = reloadTime; // Reset the reload timer for the next reload
            UpdateAmmoDisplay(); // Update the display after reloading
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

    public float intensity = 0.7f;
    public float duration = 0.15f;


    public void TriggerHaptic(XRBaseController controller)
    {
        controller.SendHapticImpulse(intensity, duration);
    }

    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }
}

