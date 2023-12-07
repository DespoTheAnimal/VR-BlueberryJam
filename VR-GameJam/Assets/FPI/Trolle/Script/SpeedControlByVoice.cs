using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;

public class SpeedControlByVoice : MonoBehaviour
{
   /* [SerializeField] private Wit wit;
    public float lowPitchThreshold = 100f; // Threshold for low pitch (in Hz)
    public float highPitchThreshold = 300f; // Threshold for high pitch (in Hz)

    private void Start()
    {
        wit.events.onResponse.AddListener(OnWitResponse);
    }

    private void OnWitResponse(WitResponseNode response)
    {
        if (response != null)
        {
            string text = response["text"].Value;
            if (text.ToLower().Contains("speed"))
            {
                float pitch = AnalyzePitch(); // You need to implement this method
                AdjustGameSpeed(pitch);
            }
        }
    }

    private float AnalyzePitch()
    {
        // Implement pitch analysis logic here
        // Return the pitch in Hertz
    }

    private void AdjustGameSpeed(float pitch)
    {
        if (pitch < lowPitchThreshold)
        {
            // Low pitch, slow down the game
            Time.timeScale = 0.5f;
        }
        else if (pitch > highPitchThreshold)
        {
            // High pitch, speed up the game
            Time.timeScale = 1.5f;
        }
        else
        {
            // Normal pitch, normal game speed
            Time.timeScale = 1f;
        }
    }

    private void OnDestroy()
    {
        if (wit != null)
        {
            wit.events.onResponse.RemoveListener(OnWitResponse);
        }
    }*/
}
