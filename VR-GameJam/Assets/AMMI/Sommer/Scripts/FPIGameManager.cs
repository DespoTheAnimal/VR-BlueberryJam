using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPIGameManager : MonoBehaviour
{
    private int playerScore = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private TextMeshProUGUI textPlayerScore;

    void Start()
    {
        // Ensure the AudioSource component is attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    public void IncrementPlayerScore()
    {
        playerScore += 1;
        if(playerScore < 10){
            UpdateScoreText();
        }else if(playerScore >= 10){
            YouWon();
        }
    }

    private void UpdateScoreText()
    {
        // Update the TextMeshProUGUI text with the current score
        textPlayerScore.text = "Kills: " + playerScore.ToString();
    }

    public void YouWon()
    {
        // Update the TextMeshProUGUI text for winning
        textPlayerScore.text = "Conratulations you have killed enough to proceed from the tutorial stage";
        // Play the audio clip
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Update()
    {
        // Check if the winning condition is met
    }
}

