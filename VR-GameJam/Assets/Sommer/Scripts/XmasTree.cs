using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class XmasTree : NetworkBehaviour
{

    [SerializeField] private GameObject startTree;
    [SerializeField] private GameObject endTree;

    [SerializeField] private AudioClip wrappingPressent;
    private AudioSource audioSource;

    private NetworkVariable<int> score = new NetworkVariable<int>();

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnNetworkSpawn(){
        score.OnValueChanged += ScoreChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if(score.Value == 13){
            startTree.SetActive(false);
            endTree.SetActive(true);
            Debug.Log("Tree should be swapped");
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Present")){
            //Increment a coop score (network variable)
            IncrementScore();
            PlaySoundClientRPC();
            col.gameObject.tag = "Untagged";
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

    [ServerRpc]
    private void PlaySoundServerRPC()
    {
        PlaySoundClientRPC();
    }

    [ClientRpc]
    private void PlaySoundClientRPC()
    {
        audioSource.PlayOneShot(wrappingPressent);
    }
}
