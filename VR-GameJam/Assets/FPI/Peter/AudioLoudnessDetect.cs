using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetect : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip(){
        string mic = Microphone.devices[0];
        clip = Microphone.Start(mic, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone(){
        return GetLoudnessFromClip(Microphone.GetPosition(Microphone.devices[0]), clip);
    }
    public float GetLoudnessFromClip(int clipPosition, AudioClip clip){
        int startPosition = clipPosition - sampleWindow;

        if (startPosition == 0) {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);


        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++){
            //Get absolute value of data
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness/sampleWindow;
    }
}
