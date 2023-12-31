using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;
using TMPro;
using System;
using Meta.WitAi;



public class FPIShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform spawnPosition;
    public AudioClip rifleReloadSound; // Reload sound clip
    public AudioClip rifleShootingSound, slowMo, boost, returnToMonke; // Shooting sound clip
    public AudioSource audioSource; // Audio source component

    [SerializeField] private GameObject uiCanvas; // Reference to the UI Canvas
    [SerializeField] private TextMeshProUGUI ammoDisplay; // Reference to the TextMeshPro GUI element
    [SerializeField] private Wit wit;

    private int max_ammo = 30, current_ammo = 30;
    private float reloadTime = 3, reloadTimer = 3;
    private float shotCooldown = 0.15f; // Time between shots
    private float timeSinceLastShot = 0.0f; // Timer to track time since last shot

    private bool isTriggerHeldDown = false; // Flag to check if trigger is held down

    private XRBaseController currentController;

    public AudioClip emptyGunSound; // Sound to play when the gun is empty

    public event Action OnMagazineEmpty;


    //LOUDNESS
    /*public AudioLoudnessDetect detect;
    public float loudnessSens = 100;
    public float threshold = 2.5f;
    float loudness;*/


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
        //loudness = detect.GetLoudnessFromMicrophone() * loudnessSens;
        timeSinceLastShot += Time.deltaTime;

        if (isTriggerHeldDown && timeSinceLastShot >= shotCooldown)
        {
            BangBang();
            timeSinceLastShot = 0.0f; // Reset the timer
        }
        else if (current_ammo <= 0 && reloadTimer == reloadTime)
        {
            //Reload(); // Implement voice recognition reload function
        }

        if (current_ammo <= 0)
        {
            //OnMagazineEmpty?.Invoke();
            if (wit.Active == false)
                wit.Activate();
        }
    }



    public void BangBang()
    {
        if (current_ammo > 0)
        {
            SpawnBullet();
            current_ammo--;
            UpdateAmmoDisplay();
        }
        else
        {
            // Play empty gun sound effect
            audioSource.PlayOneShot(emptyGunSound);
        }
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



    public IEnumerator Reload(string[] strings)
    {
        if (strings[0] == "Reload")
        {
            Debug.Log("Reloading...");
            audioSource.PlayOneShot(rifleReloadSound);

            // Wait for 3 seconds
            yield return new WaitForSeconds(3);

            current_ammo = max_ammo; // Reset ammo to max
            UpdateAmmoDisplay();
            wit.Deactivate();
        }
    }



    public void ReloadwithVoice(string[] strings)
    {
        if (strings[0] == "Reload")
        {
            StartCoroutine(ReloadCoroutine());
        }
        else if (strings[0] == "Stop")
        {
            Time.timeScale = 0.5f;
            Debug.Log("Testslow");
            audioSource.PlayOneShot(slowMo);
            wit.Deactivate();
        }
        else if (strings[0] == "Fast")
        {
            Time.timeScale = 1.5f;
            Debug.Log("Testspeed");
            audioSource.PlayOneShot(boost);
            wit.Deactivate();
        }
        else if (strings[0] == "Normal")
        {
            Time.timeScale = 1f;
            Debug.Log("Testnormal");
            audioSource.PlayOneShot(returnToMonke);
            wit.Deactivate();
        }
    }
    public IEnumerator ReloadCoroutine()
    {
        Debug.Log("Reloading...");
        audioSource.PlayOneShot(rifleReloadSound);

        // Wait for 3 seconds
        yield return new WaitForSeconds(3);

        current_ammo = max_ammo; // Reset ammo to max
        UpdateAmmoDisplay();
        wit.Deactivate();
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

        if (arg.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            currentController = controllerInteractor.xrController;
        }
    }


    private void OnReleased(SelectExitEventArgs arg)
    {
        if (uiCanvas != null)
            uiCanvas.SetActive(false); // Hide the UI when the rifle is released

        currentController = null;
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
