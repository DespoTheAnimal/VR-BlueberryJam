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


    private NetworkVariable<int> score = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            VRrigReference.Singleton.SetNetworkPlayer(this);
            gameObject.tag = "Player1";
        }
        else
        {
            gameObject.tag = "Player2";
        }

        score.OnValueChanged += ScoreChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner){
            root.position = VRrigReference.Singleton.root.position;
            root.rotation = VRrigReference.Singleton.root.rotation;

            head.position = VRrigReference.Singleton.head.position;
            head.rotation = VRrigReference.Singleton.head.rotation;

            leftHand.position = VRrigReference.Singleton.leftHand.position;
            leftHand.rotation = VRrigReference.Singleton.leftHand.rotation;

            rightHand.position = VRrigReference.Singleton.rightHand.position;
            rightHand.rotation = VRrigReference.Singleton.rightHand.rotation;

            //body.position = VRrigReference.Singleton.body.position;
            //body.rotation = VRrigReference.Singleton.body.rotation;

        }
    }

    public void IncrementScore()
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
    }
}
