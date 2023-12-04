using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    GameObject[] players = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        players[0] = GameObject.Find("Player 1");
        players[1] = GameObject.Find("Player 2");

        if (players.Length == 2)
        {
            StartGames();
        }
    }

    private void StartGames()
    {
        players[0].transform.position = new Vector3(14.52f, 0.37f,-15.52f);
        players[1].transform.position = new Vector3(-14.52f, 0.37f, 15.15f);
    }
}
