using System;
using UnityEngine;

public class StartGameBehaviour : MonoBehaviour
{

    private GameObject[] players = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        players[0] = GameObject.Find("NetworkPlayer(Clone)");
        players[1] = GameObject.Find("NetworkPlayer(Clone)(Clone)");

        if (players.Length == 2)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        players[0].transform.position = new Vector3(14.52f, 0.37f,-15.52f);
        players[0].tag = "Player1";
        players[1].transform.position = new Vector3(-14.52f, 0.37f, 15.15f);
        players[1].tag = "Player2";
    }
}
