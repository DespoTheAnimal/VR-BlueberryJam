using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMicAdjust : MonoBehaviour
{
    public float maxValue = 50;
    [SerializeField] private Slider slider;
    public AudioLoudnessDetect detect;


    void Start()
    {
        slider.maxValue = maxValue;
    }

    void Update()
    {
        //AdjustSliderWithMicInput();
        float loudness = detect.GetLoudnessFromMicrophone() * 100;
        slider.value = loudness;
    }
}
