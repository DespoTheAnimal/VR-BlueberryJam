using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagement : MonoBehaviour
{
    private int player1Score = 0;
    private int player2Score = 0;
    [SerializeField] private AudioSource audioSource;

    public int player1Kills = 0;
    public int player2Kills = 0;
    public TMP_Text p1Kills;
    public TMP_Text p2Kills;

    public GameObject Flag;
    public TMP_Text winner;
    public GameObject winnerPanel;

    private void Update() {
        player1Score = Flag.GetComponent<FlagLogic>().Player1Score;
        player2Score = Flag.GetComponent<FlagLogic>().Player2Score;
        //p1Kills.text = player1Kills.ToString();
        //p2Kills.text = player2Kills.ToString();

        Winner();
    }

    private void Winner(){
        if (player1Score > 2){
            audioSource.PlayOneShot(audioSource.clip);
            winnerPanel.SetActive(true);
            winner.text = "Player 1!";
            Time.timeScale = 0;
        } else if (player2Score > 2) {
            audioSource.PlayOneShot(audioSource.clip);
            winnerPanel.SetActive(true);
            winner.text = "Player 2";
            Time.timeScale = 0;
        }
    }
}
