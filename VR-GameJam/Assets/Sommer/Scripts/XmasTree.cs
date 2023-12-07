using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class XmasTree : NetworkBehaviour
{

    [SerializeField] private GameObject startTree;
    [SerializeField] private GameObject endTree;

    private NetworkVariable<int> score = new NetworkVariable<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnNetworkSpawn(){
        score.OnValueChanged += ScoreChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if(score.Value == 1){
            startTree.SetActive(false);
            startTree.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Present")){
            //Increment a coop score (network variable)
            IncrementScore();
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
}
