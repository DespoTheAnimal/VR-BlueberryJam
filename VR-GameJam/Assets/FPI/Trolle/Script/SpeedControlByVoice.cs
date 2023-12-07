using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;

public class SpeedControlByVoice : MonoBehaviour
{
    public float lowPitchThreshold = 100f; // Example threshold, adjust as needed
    public float highPitchThreshold = 300f; // Example threshold, adjust as needed

    public void AdjustSpeed(string command)
    {
        float pitch = AnalyzePitch(); // Implement actual pitch detection here

        if (command.Contains("slowmotion") && pitch < lowPitchThreshold)
        {
            Time.timeScale = 0.5f; // Slower speed for low pitch
        }
        else if (command.Contains("speed") && pitch > highPitchThreshold)
        {
            Time.timeScale = 2.0f; // Faster speed for high pitch
        }
        else
        {
            Time.timeScale = 1f; // Normal speed
        }
    }

    private float AnalyzePitch()
    {
        // Implement actual pitch detection logic
        return 150f; // Placeholder value
    }
}
