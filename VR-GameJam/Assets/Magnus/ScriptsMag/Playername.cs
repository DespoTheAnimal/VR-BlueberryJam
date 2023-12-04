using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Playername : MonoBehaviour
{
    [SerializeField] StartGame _st;
    GameObject newPlayer; 
    GameObject newPlayer2;
    void Update()
    {
        if(newPlayer == null)
        {
            newPlayer = GameObject.Find("NetworkPlayer(Clone)");
        }
        else if (newPlayer != null)
        {
            newPlayer.name = "Player 1";
            newPlayer.tag = "Player1";
           _st.players[0] = newPlayer;
        }
       /* if (newPlayer2 == null)
        {
            newPlayer2 = GameObject.Find("n")
        }*/
    }

    /*public void CreatePlayer2()
    {
        while (true)
        {
            GameObject newPlayer = GameObject.Find("NetworkPlayer(Clone)");
        else if()
            {
                newPlayer.name = "Player 2";
                newPlayer.tag = "Player2";
                _st.players[1] = newPlayer;
                break;
            }
        }
    }*/


}
