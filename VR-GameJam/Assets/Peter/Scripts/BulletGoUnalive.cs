using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGoUnalive : MonoBehaviour
{
    private Vector3 SpawnPosP1 = new Vector3(14.5162945,0.365164995,-15.518199);
    private Vector3 SpawnPosP2 = new Vector3(-14.5612411,0.365164995,15.1473541);
    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player1")
        {
            PlayerHit(other.gameObject, SpawnPosP1);
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player1Kills++;
        } else if (other.gameObject.tag == "Player2")
        {
            PlayerHit(other.gameObject, SpawnPosP2);
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player2Kills++;
        }
    }

    public void PlayerHit(GameObject other, Vector3 spawn) {
        other.transform.position = spawn;
    }
}
