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
        players[0] = GameObject.Find("NetworkPlayer (Clone)");
        if (players.Length == 2)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        players[0].transform.position = new Vector3(14.52f, 0.37f,-15.52f);
        players[1].transform.position = new Vector3(-14.52f, 0.37f, 15.15f);
    }
}
