using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetting : NetworkBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    //[SerializeField] private Transform body;
    //public GameObject xRorigin;

    //private NetworkVariable<int> score = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            VRrigReference.Singleton.SetNetworkPlayer(this);
            /*gameObject.transform.GetChild(0).tag = "Player1";
            xRorigin = GameObject.Find("Player");
            xRorigin.GetComponent<CharacterController>().detectCollisions = false;
            xRorigin.GetComponent<CharacterController>().attachedRigidbody.detectCollisions = false;  
            //xRorigin.tag = "Player3";*/
            GameObject xrLocal = GameObject.Find("Player");
        }
        else
        {
            /*gameObject.tag = "Player2";
            xRorigin = GameObject.Find("Player");*/
            //xRorigin.tag = "Player4";
        }

        score.OnValueChanged += ScoreChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner){
            root.position = VRrigReference.Singleton.root.position;
            root.rotation = VRrigReference.Singleton.root.rotation;

            head.position = VRrigReference.Singleton.head.position + new Vector3(0,-0.5f,0);
            head.rotation = VRrigReference.Singleton.head.rotation;

            leftHand.position = VRrigReference.Singleton.leftHand.position;
            leftHand.rotation = VRrigReference.Singleton.leftHand.rotation;

            rightHand.position = VRrigReference.Singleton.rightHand.position;
            rightHand.rotation = VRrigReference.Singleton.rightHand.rotation;

            //body.position = VRrigReference.Singleton.body.position;
            //body.rotation = VRrigReference.Singleton.body.rotation;

        }
    }

    /*public void IncrementScore()
    {
        IncrementScoreServerRPC();
    }

    [ServerRpc]
    public void IncrementScoreServerRPC()
    {
        score.Value++;
    }

    private void ScoreChanged(int oldValue, int currentValue)
    {
        print(currentValue);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        score.OnValueChanged -= ScoreChanged;
    }*/
}
