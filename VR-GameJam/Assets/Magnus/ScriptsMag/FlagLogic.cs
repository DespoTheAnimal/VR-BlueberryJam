using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlagLogic : MonoBehaviour
{
    public Vector3 FlagSpawn = new Vector3(0f, 1.45f, 0f);
    public XRGrabInteractable flag;
    
    [SerializeField] AudioSource AS;
    public int Player1Score = 0;
    public int Player2Score = 0;

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
        if (Other.gameObject.tag == "Endzone1")
        {
            CaptureFlag();
            Player1Score++;
            flag.interactionLayers = 1;

        }
        else if (Other.gameObject.tag == "Endzone2")
        {
            CaptureFlag();
            Player2Score++;
            flag.interactionLayers = 1;
        }
        Debug.Log(flag.interactionLayers);
    }

    private void CaptureFlag()
    {
        //flag.GetComponent<XRGrabInteractable>().ForceRelease();
        flag.interactionLayers = 0;
        transform.position = FlagSpawn;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
