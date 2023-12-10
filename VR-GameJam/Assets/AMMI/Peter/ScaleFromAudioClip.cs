using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetect detect;

    public float loudnessSens = 100;
    public float threshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detect.GetLoudnessFromClip(source.timeSamples, source.clip) * loudnessSens;

        if (loudness > threshold)
        {
            Time.timeScale = 0.5f;
            Debug.Log("Testslow");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("normalspeed");

        }
        //transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
