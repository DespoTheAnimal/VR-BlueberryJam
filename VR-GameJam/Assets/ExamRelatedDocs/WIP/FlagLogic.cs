using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlagLogic : MonoBehaviour
{
    public Vector3 FlagSpawn = new Vector3(0f, 1.45f, 0f);
    public XRGrabInteractable flag; // Ensure this is the flag GameObject

    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioSource CaptureSound;
    public int Player1Score = 0;
    public int Player2Score = 0;
    private bool isCaptured = false;

    public void YouHaveTheFlag(AudioClip clip)
    {
        AS.PlayOneShot(clip);
    }

    public void FlagDropped(AudioClip clip)
    {
        AS.PlayOneShot(clip);
    }

    private void OnTriggerEnter(Collider Other)
    {
        if ((Other.gameObject.tag == "Endzone1" || Other.gameObject.tag == "Endzone2") && !isCaptured)
        {
            isCaptured = true;
            Player1Score += Other.gameObject.tag == "Endzone1" ? 1 : 0;
            Player2Score += Other.gameObject.tag == "Endzone2" ? 1 : 0;
            CaptureFlag();
        }
    }

    private void CaptureFlag()
    {
        if (flag.isSelected)
        {
            flag.interactionManager.CancelInteractableSelection(flag);
        }

        StartCoroutine(RespawnFlag());
    }

    private IEnumerator RespawnFlag()
    {
        // Wait for the next frame to ensure all physics and interaction updates are completed
        yield return null;

        // Reset flag position and Rigidbody state
        flag.transform.position = FlagSpawn;
        Rigidbody flagRigidbody = flag.GetComponent<Rigidbody>();
        if (flagRigidbody != null)
        {
            flagRigidbody.velocity = Vector3.zero;
            flagRigidbody.angularVelocity = Vector3.zero;
        }

        // Reset interaction layers to make the flag grabbable again
        flag.interactionLayers = -1; // -1 sets all layers to be interactable

        CaptureSound.PlayOneShot(CaptureSound.clip);
        isCaptured = false;
    }
}
