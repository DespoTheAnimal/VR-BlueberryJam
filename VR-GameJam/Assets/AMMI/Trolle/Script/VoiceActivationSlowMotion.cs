using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;
using UnityEngine.InputSystem;

public class VoiceActivationSlowMotion : MonoBehaviour
{
    [SerializeField] private Wit wit;
    public AudioLoudnessDetect detect;
    public float loudnessSens = 100;
    public float threshold = 2.5f;
    [SerializeField] private InputActionProperty actVoice;

    void Start()
    {
        actVoice.action.performed += startVoice;
        /*if (actVoice.action.WasPerformedThisFrame())
        {
            Debug.Log("Voice");
            wit.Activate();
        }*/
    }

    private void OnEnable()
    {
        // wit.events.onResponse.AddListener(OnWitResponse);
    }

    private void OnDisable()
    {
        //wit.events.onResponse.RemoveListener(OnWitResponse);
    }

    private void startVoice(InputAction.CallbackContext ctx)
    {
        Debug.Log("test1");
        if(wit.Active == false)
        {
            wit.Activate();
            Debug.Log("test2");
        }

        
    }



     public void OnWitResponse(WitResponseNode response)
    {
        if (response != null)
        {
            string text = response["text"].Value.ToLower();
            if (text.Contains("speed"))
            {
                //StartSpeedramp(text);
            }
            else if (text.Contains("slow"))
            {

            }
        }
    } 

    public void StartSpeedramp(string[] input)
    {
        float loudness = detect.GetLoudnessFromMicrophone() * loudnessSens;
        Debug.Log(loudness);
        if (input[0] == "Speed" && loudness > threshold)
        {
            Time.timeScale = 1.5f;
            Debug.Log("Testspeed");
            wit.Deactivate();
        }
        else if (input[0] == "slow" && loudness > threshold)
        {
            Time.timeScale = 0.5f;
            Debug.Log("Testslow");
            wit.Deactivate();
        }
    }
}


