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
    // Start is called before the first frame update

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    public void IncrementPlayerScore(){
        playerScore ++;
        textPlayerScore.text = playerScore.ToString();
        Debug.Log(playerScore);
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void YouWon(){
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Update(){
        if(playerScore >= 10){
            YouWon();
        }
    }
}
