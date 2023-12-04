using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;

public class BulletGoUnalive : MonoBehaviour
{
    private Vector3 SpawnPosP1 = new Vector3(14.52f,0.37f,-15.52f);
    private Vector3 SpawnPosP2 = new Vector3(-14.56f,0.37f,15.15f);
    public void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "SafeZone"){
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player1")
        {
            PlayerHit(other.gameObject.transform.parent.gameObject, SpawnPosP1);
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player1Kills++;
        } else if (other.gameObject.tag == "Player2")
        {
            PlayerHit(other.gameObject, SpawnPosP2);
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player2Kills++;
        }
    }

    public void PlayerHit(GameObject other, Vector3 spawn) {
        other.GetComponent<clientNetworkTransform>().Teleport(spawn, transform.rotation, transform.localScale);
    }
}
