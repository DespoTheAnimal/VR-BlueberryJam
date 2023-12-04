using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject[] players = new GameObject[2];

    // Update is called once per frame
    void Update()
    {
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
