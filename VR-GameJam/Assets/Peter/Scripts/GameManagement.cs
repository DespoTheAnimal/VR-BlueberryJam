using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameManagement : NetworkBehaviour
{
    private NetworkVariable<int> player1Score = new NetworkVariable<int>();
    private NetworkVariable<int> player2Score = new NetworkVariable<int>();
    public NetworkVariable<int> player1Kills = new NetworkVariable<int>();
    public NetworkVariable<int> player2Kills = new NetworkVariable<int>();

    [SerializeField] private AudioSource audioSource;
    public TMP_Text p1KillsText;
    public TMP_Text p2KillsText;

    public GameObject Flag;
    public TMP_Text winner;
    public GameObject winnerPanel;

    private void Start()
    {
        player1Kills.OnValueChanged += UpdatePlayer1Kills;
        player2Kills.OnValueChanged += UpdatePlayer2Kills;
    }

    private void Update()
    {
        if (IsServer)
        {
            player1Score.Value = Flag.GetComponent<FlagLogic>().Player1Score;
            player2Score.Value = Flag.GetComponent<FlagLogic>().Player2Score;

            Winner();
        }

        p1KillsText.text = player1Kills.Value.ToString();
        p2KillsText.text = player2Kills.Value.ToString();
    }

    private void UpdatePlayer1Kills(int oldValue, int newValue)
    {
        p1KillsText.text = newValue.ToString();
    }

    private void UpdatePlayer2Kills(int oldValue, int newValue)
    {
        p2KillsText.text = newValue.ToString();
    }

    private void Winner()
    {
        if (player1Score.Value > 2)
        {
            audioSource.PlayOneShot(audioSource.clip);
            winnerPanel.SetActive(true);
            winner.text = "Player 1!";
            //Time.timeScale = 0;
            // Implement additional logic for when the game ends
        }
        else if (player2Score.Value > 2)
        {
            audioSource.PlayOneShot(audioSource.clip);
            winnerPanel.SetActive(true);
            winner.text = "Player 2";
            //Time.timeScale = 0;
            // Implement additional logic for when the game ends
        }
    }

    [ServerRpc]
    public void IncreasePlayer1KillsServerRpc()
    {
        player1Kills.Value++;
    }

    [ServerRpc]
    public void IncreasePlayer2KillsServerRpc()
    {
        player2Kills.Value++;
    }
}
