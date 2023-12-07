using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;

public class VoiceActivationSlowMotion : MonoBehaviour
{
    [SerializeField] private Wit wit;
    //[SerializeField] private SpeedAdjuster speedAdjuster;

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
        if (response != null)
        {
            string text = response["text"].Value.ToLower();
            if (text.Contains("speed") || text.Contains("slowmotion"))
            {
                //speedAdjuster.AdjustSpeed(text);
            }
        }
    }
}
