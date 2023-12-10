using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningNPC : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] floor;
    [SerializeField] private GameObject npcs;
    [SerializeField] private GameObject container;
    private Vector3 spawnPos;
    private int amount = 10;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    void Start()
    {
        NPCSpawn();
    }

    public void NPCSpawn(){
        foreach (MeshRenderer item in floor)
        {
            for (int i = 0; i < amount; i++)
            {
                int attempts = 10; // Number of attempts to find a free spot
                bool hasSpawned = false;

                while (attempts > 0 && !hasSpawned)
                {
                    float randomX = Random.Range(item.bounds.min.x, item.bounds.max.x);
                    float randomZ = Random.Range(item.bounds.min.z, item.bounds.max.z);
                    spawnPos = new Vector3(randomX, -0.1f, randomZ);

                    if (IsPositionFree(spawnPos))
                    {
                        GameObject randomNpc = npcs;
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                        GameObject npcInstance = Instantiate(randomNpc, spawnPos, randomRotation, container.transform);
                        spawnedPositions.Add(spawnPos);
                        hasSpawned = true;
                    }

                    attempts--;
                }
            }
        }
    }

    private bool IsPositionFree(Vector3 position)
    {
        foreach (Vector3 existingPos in spawnedPositions)
        {
            if (Vector3.Distance(position, existingPos) < 1.0f)
            {
                return false;
            }
        }
        return true;
    }
}