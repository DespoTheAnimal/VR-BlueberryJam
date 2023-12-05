using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningNPC : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] floor;
    [SerializeField] private GameObject[] npcs;
    [SerializeField] private GameObject container;
    private Vector3 spawnPos;
    private int amount = 1;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        NPCSpawn();
    }

    public void NPCSpawn(){
        foreach (MeshRenderer item in floor)
        {
            for (int i = 0; i < amount; i++)
            {
                int attempts = 20; // Number of attempts to find a free spot
                bool hasSpawned = false;

                while (attempts > 0 && !hasSpawned)
                {
                    float randomX = Random.Range(item.bounds.min.x, item.bounds.max.x);
                    float randomZ = Random.Range(item.bounds.min.z, item.bounds.max.z);
                    spawnPos = new Vector3(randomX, 1, randomZ); // Assuming 12.3f is the y-position for NPCs to be on the floor

                    if (IsPositionFree(spawnPos))
                    {
                        // Randomly select an NPC type from the npcs array
                        GameObject randomNpc = npcs[Random.Range(0, npcs.Length)];

                        // Generate a random rotation around the Y-axis
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                        // Instantiate the NPC and get the reference to the new instance
                        GameObject npcInstance = Instantiate(randomNpc, spawnPos, randomRotation, container.transform);
                        spawnedPositions.Add(spawnPos); // Add the spawn position to the list
                        hasSpawned = true;

                        // Add the NPCWander script component to the instantiated NPC
                        //NPCWander npcWander = npcInstance.AddComponent<NPCWander>();
                        //npcWander.SetBoundaries(item.bounds); // Pass the floor boundaries to the NPCWander script

                        // Now the NPCWander script will take over, moving the NPC within the given boundaries
                    }

                    attempts--;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsPositionFree(Vector3 position)
    {
        foreach (Vector3 existingPos in spawnedPositions)
        {
            if (Vector3.Distance(position, existingPos) < 1.0f) // Assuming a distance of 1.0 units to be "too close". Adjust as needed.
            {
                return false;
            }
        }
        return true;
    }
}
