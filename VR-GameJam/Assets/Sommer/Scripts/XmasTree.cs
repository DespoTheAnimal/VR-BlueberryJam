using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro; // Add this for TextMeshPro

public class XmasTree : NetworkBehaviour
{
    [SerializeField] private GameObject startTree;
    [SerializeField] private GameObject endTree;

    [SerializeField] private AudioClip wrappingPressent;
    private AudioSource audioSource;

    [SerializeField] private TextMeshProUGUI scoreText; // UI Text element reference
    [SerializeField] private TextMeshProUGUI scoreTexT; // UI Text element reference
    private const int totalPresentsNeeded = 8; // Total presents needed to win

    private NetworkVariable<int> score = new NetworkVariable<int>();

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreText(); // Initial score text update

    }

    public override void OnNetworkSpawn()
    {
        score.OnValueChanged += ScoreChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (score.Value == totalPresentsNeeded)
        {
            startTree.SetActive(false);
            endTree.SetActive(true);
            Debug.Log("Tree should be swapped");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Present"))
        {
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
        UpdateScoreText(); // Update score text when the score changes
    }

    // Update the score text UI
    private void UpdateScoreText()
    {
        int presentsRemaining = totalPresentsNeeded - score.Value;
        scoreText.text = $"You have found {score.Value} present(s). You need {presentsRemaining} more to save Christmas!";
        scoreTexT.text = $"You have found {score.Value} present(s). You need {presentsRemaining} more to save Christmas!";
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

