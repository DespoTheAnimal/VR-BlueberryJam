using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagement : MonoBehaviour
{
    private int player1Score = 0;
    private int player2Score = 0;

    public GameObject Flag;
    public TMP_Text winner;
    public GameObject winnerPanel;

    private void Update() {
        player1Score = Flag.GetComponent<FlagLogic>().Player1Score;
        player2Score = Flag.GetComponent<FlagLogic>().Player2Score;

        Winner();
    }

    private void Winner(){
        if (player1Score > 2){
            winnerPanel.SetActive(true);
            winner.text = "Player 1!";
            Time.timeScale = 0;
        } else if (player2Score > 2) {
            winnerPanel.SetActive(true);
            winner.text = "Player 2";
            Time.timeScale = 0;
        }
    }


}
