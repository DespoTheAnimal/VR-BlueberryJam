using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;
using Unity.Netcode;

public class BulletGoUnalive : NetworkBehaviour
{
    private Vector3 SpawnPosP1 = new Vector3(14.52f,0.37f,-15.52f);
    private Vector3 SpawnPosP2 = new Vector3(-14.56f,0.37f,15.15f);

    GameObject happened; 
    public void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "SafeZone"){
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player1")
        {
            //PlayerHitServerRPC(other.gameObject.transform.parent.gameObject, SpawnPosP1);
            //other.gameObject.transform.position = PlayerHitServerRPC(SpawnPosP1);
            happened = other.gameObject;
            PlayerHitServerRPC(happened.tag);
            
            
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player1Kills++;
        } else if (other.gameObject.tag == "Player2")
        {
            //PlayerHitServerRPC(SpawnPosP2);
            happened = other.gameObject;
            PlayerHitServerRPC(happened.tag);
            GameObject.FindWithTag("GameManager").GetComponent<GameManagement>().player2Kills++;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHitServerRPC(string spawn) {
        //other.GetComponent<clientNetworkTransform>().Teleport(spawn, transform.rotation, transform.localScale);
        if (spawn == "Player1")
        {
            happened.transform.position = SpawnPosP1;
        }
        else if (spawn == "Player2")
        {
            happened.transform.position = SpawnPosP2;
        }
    }
}
