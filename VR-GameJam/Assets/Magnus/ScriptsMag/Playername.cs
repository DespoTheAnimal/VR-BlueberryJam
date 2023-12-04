using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Playername : MonoBehaviour
{
    [SerializeField] StartGame _st;
    public void CreatePlayer1()
    {
        while (true)
        {
            GameObject newPlayer = GameObject.Find("NetworkPlayer(Clone)");
            newPlayer.name = "Player 1";
            newPlayer.tag = "Player1";
            if (newPlayer != null)
            {
                newPlayer = _st.players[0];
                break;
            }
        }


    }

    public void CreatePlayer2()
    {
        while (true)
        {
            GameObject newPlayer = GameObject.Find("NetworkPlayer(Clone)");
            newPlayer.name = "Player 2";
            newPlayer.tag = "Player2";
            if (newPlayer != null)
            {
                newPlayer = _st.players[1];
                break;
            }
        }
    }


}
