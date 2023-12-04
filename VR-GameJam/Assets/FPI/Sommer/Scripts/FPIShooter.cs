using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;
using TMPro;

public class FPIShooter : MonoBehaviour
{
    // Start is called before the first frame update
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
        XRGrabInteractable theGunGrabable = GetComponent<XRGrabInteractable>();
        theGunGrabable.activated.AddListener(TriggerPulled);
        theGunGrabable.deactivated.AddListener(TriggerReleased);
        theGunGrabable.selectEntered.AddListener(OnGrabbed);
        theGunGrabable.selectExited.AddListener(OnReleased);

        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(TriggerHaptic);

        UpdateAmmoDisplay(); // Initial ammo display update
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
            Reload(); //Implement voice recognition reload function
        }
    }

    public void BangBang()
    {
        SpawnBullet();
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

    public void SpawnBullet()
    {
        GameObject newBullet = Instantiate(bullet, spawnPosition.position, spawnPosition.rotation);

        // Play shooting sound effect
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

    public void TriggerHaptic(BaseInteractionEventArgs eventArgs){
        if(eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor){
            TriggerHaptic(controllerInteractor.xrController);
        }
    }
}
