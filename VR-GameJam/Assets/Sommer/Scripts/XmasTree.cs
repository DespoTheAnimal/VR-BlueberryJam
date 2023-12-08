using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro; // For TextMeshPro

public class XmasTree : NetworkBehaviour
{
    [SerializeField] private GameObject startTree;
    [SerializeField] private GameObject endTree;

    [SerializeField] private AudioClip wrappingPressent;
    private AudioSource audioSource;

    [SerializeField] private TextMeshProUGUI scoreText; // Original UI Text element
    [SerializeField] private TextMeshProUGUI scoreTexT; // Original UI Text element
    [SerializeField] private TextMeshProUGUI victoryText1; // New UI Text element for after finding all presents
    [SerializeField] private TextMeshProUGUI victoryText2; // New UI Text element for after finding all presents
    private const int totalPresentsNeeded = 1; // Total presents needed to win

    private NetworkVariable<int> score = new NetworkVariable<int>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreText(); // Initial score text update
        victoryText1.gameObject.SetActive(false); // Initially hide new victory text
        victoryText2.gameObject.SetActive(false); // Initially hide new victory text
    }

    public override void OnNetworkSpawn()
    {
        score.OnValueChanged += ScoreChanged;
    }

    void Update()
    {
        if (score.Value == totalPresentsNeeded)
        {
            startTree.SetActive(false);
            endTree.SetActive(true);
            scoreText.gameObject.SetActive(false); // Disable original score text
            scoreTexT.gameObject.SetActive(false); // Disable original score text
            victoryText1.gameObject.SetActive(true); // Enable new victory text
            victoryText2.gameObject.SetActive(true); // Enable new victory text
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