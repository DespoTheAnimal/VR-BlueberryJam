using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMicAdjust : MonoBehaviour
{
    public float maxValue = 50;
    [SerializeField] private Slider slider;
    public AudioLoudnessDetect detect;
    public AudioSource audioSource;


    void Start()
    {
        slider.maxValue = maxValue;
    }

    void Update()
    {
        float loudness = detect.GetLoudnessFromMicrophone() * 100;
        slider.value = loudness;
        float pitch = AnalyzePitch(audioSource);
        Debug.Log(pitch);
    }

    float AnalyzePitch(AudioSource audioSource)
    {
        // Get the audio clip
        AudioClip clip = detect.clip;

        // Create an array to hold the audio samples
        int sampleCount = 1024; // You can adjust this value as needed
        float[] samples = new float[sampleCount];

        // Get the audio samples from the audio clip
        clip.GetData(samples, 0);

        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] *= 0.5f * (1 - Mathf.Cos(2 * Mathf.PI * i / (samples.Length - 1)));
        }

        // Perform the FFT on the audio samples
        float[] fftResults = PerformFFT(samples);

        // Find the index of the maximum value in the FFT results
        int maxIndex = 0;
        float maxValue = 0;
        for (int i = 0; i < fftResults.Length; i++)
        {
            if (fftResults[i] > maxValue)
            {
                maxValue = fftResults[i];
                maxIndex = i;
            }
        }

        // Calculate the pitch based on the index of the maximum value
        float pitch = maxIndex * clip.frequency / samples.Length;

        return pitch;
    }

    float[] PerformFFT(float[] data)
    {
        int n = data.Length;

        // Check if n is a power of 2
        if ((n & (n - 1)) != 0)
        {
            Debug.LogError("data length " + n + " is not a power of 2, cannot perform FFT.");
            return null;
        }

        // Initialize the real and imaginary parts of the FFT results
        float[] real = new float[n];
        float[] imag = new float[n];

        // Perform the FFT
        for (int m = 0; m < n; m++)
        {
            for (int k = 0; k < n; k++)
            {
                real[m] += data[k] * Mathf.Cos(2 * Mathf.PI * m * k / n);
                imag[m] -= data[k] * Mathf.Sin(2 * Mathf.PI * m * k / n);
            }
        }

        // Calculate the absolute values of the FFT results
        float[] result = new float[n];
        for (int i = 0; i < n; i++)
        {
            result[i] = Mathf.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
        }

        return result;
    }
}
