using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;

public class VoiceActivationSlowMotion : MonoBehaviour
{
    [SerializeField] private Wit wit;
    public AudioLoudnessDetect detect;
    public float loudnessSens = 100;
    public float threshold = 2.5f;

    void Update()
    {
        // Replace with your specific input for the B button
        if (Input.GetButtonDown("Fire2"))
        {
            wit.Activate();
        }
    }

    private void OnEnable()
    {
        // wit.events.onResponse.AddListener(OnWitResponse);
    }

    private void OnDisable()
    {
        //wit.events.onResponse.RemoveListener(OnWitResponse);
    }

    private void OnWitResponse(WitResponseNode response)
    {
        float loudness = detect.GetLoudnessFromMicrophone() * loudnessSens;
        Debug.Log(loudness);
        if (response != null)
        {
            string text = response["text"].Value.ToLower();
            if (text.Contains("speed") && loudness > threshold)
            {
                Time.timeScale = 1.5f;
                Debug.Log("Testspeed");
            }
            else if (text.Contains("slow") && loudness > threshold)
            {
                Time.timeScale = 0.5f;
                Debug.Log("Testslow");
            }
        }
    }
}

