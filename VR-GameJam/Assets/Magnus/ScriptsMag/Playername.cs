using System.Threading;
using UnityEngine;

public class Playername : MonoBehaviour
{
    float waitTimer = 5f;

    public void CreatePlayer1()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            GameObject newPlayer = GameObject.Find("NetworkPlayer(Clone)");
            newPlayer.name = "Player 1";
            newPlayer.tag = "Player1";
        }
        waitTimer = 5f;
    }

    public void CreatePlayer2()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0 )
        {
            GameObject newPlayer = GameObject.Find("NetworkPlayer(Clone)");
            newPlayer.name = "Player 2";
            newPlayer.tag = "Player2";
        }
        waitTimer = 5f;
    }


}
