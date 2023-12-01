using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGoUnalive : MonoBehaviour
{
    public int hitNum = 0;
    public Vector3 SpawnPosP1 = new Vector3(10f, 0f, 5f);
    public Vector3 SpawnPosP2 = new Vector3(-10f, 0f, -5f);
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
        hitNum++;
        other.transform.position = spawn;
    }
}
