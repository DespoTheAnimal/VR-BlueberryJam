using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmiSpawnNPC : MonoBehaviour
{
    [SerializeField] private MeshRenderer floor;
    [SerializeField] private GameObject npc;
    [SerializeField] private GameObject container;
    private Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        NPCSpawn();
    }

    public void NPCSpawn()
    {
        int attempts = 10; // Number of attempts to find a free spot
        bool hasSpawned = false;

        while (attempts > 0 && !hasSpawned)
        {
            float randomX = Random.Range(floor.bounds.min.x, floor.bounds.max.x);
            float randomZ = Random.Range(floor.bounds.min.z, floor.bounds.max.z);
            spawnPos = new Vector3(randomX, 0, randomZ); // Adjust the Y coordinate according to your need

            GameObject npcInstance = Instantiate(npc, spawnPos, Quaternion.identity, container.transform);
            hasSpawned = true;

            attempts--;
        }
    }

    // private bool IsPositionFree(Vector3 position)
    // {
    //     return true;
    // }
}