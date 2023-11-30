using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagLogic : MonoBehaviour
{
    [SerializeField] AudioSource AS;
    public void YouHaveTheFlag(AudioClip clip)
    {
        AS.PlayOneShot(clip);
    }
    public void FlagDropped(AudioClip clip)
    {
        AS.PlayOneShot(clip);
    }
}
