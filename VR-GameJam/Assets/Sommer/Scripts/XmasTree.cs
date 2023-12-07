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
        if(score.Value == 3){
            startTree.SetActive(false);
            endTree.SetActive(true);
            Debug.Log("Tree should be swapped");
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Present")){
            //Increment a coop score (network variable)
            IncrementScore();
            col.gameObject.tag("Untagged");
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
