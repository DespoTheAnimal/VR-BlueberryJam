using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    [SerializeField] private Transform player;

    public float moveSpeed = 2.0f;
    public float directionChangeInterval = 2.0f;

    private float direction = 1; // 1 for right, -1 for left
    private float timer;

    private bool isAlive = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject playerObjectByName = GameObject.Find("Player");
        if (playerObjectByName != null)
        {
            player = playerObjectByName.transform;
        }
        timer = directionChangeInterval;
        isAlive = true;
    }

    void Update()
    {
        LookAtPlayer();
        NPCMovement();
    }

    void LookAtPlayer()
    {
        // Check if the player is not null
        if (player != null && isAlive)
        {
            // Calculate the direction from the NPC to the player
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // This ensures the NPC only rotates around the y axis

            // Create a new rotation towards the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            // Apply the rotation to the NPC
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void NPCMovement()
    {
        if(isAlive){
            timer -= Time.deltaTime;

            // Check if it's time to change direction
            if (timer <= 0)
            {
                // Reset the timer
                timer = directionChangeInterval;

                // Randomly choose a new direction
                direction = Random.Range(0, 2) == 0 ? -1 : 1;
            }

        // Move the NPC side to side
            transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet")){
            isAlive = false;
            audioSource.clip = clip;
            audioSource.Play();
            transform.Rotate(0, 90, 0);
        }
    }
}
