using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private Transform player;
    private float moveSpeed = 2.0f, directionChangeInterval = 2.0f, direction = 1, timer, respawnTimer = 10f;
    private bool isAlive = false;
    private Animator anim;

    void Start()
    {

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    
        GameObject playerObjectByName = GameObject.Find("Player");
        
        if (playerObjectByName != null)
        {
            player = playerObjectByName.transform;
        }
        timer = directionChangeInterval;
        isAlive = true;
        anim.SetBool("isAlive", true);
    }

    void Update()
    {
        LookAtPlayer();
        NPCMovement();
        Respawn();
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

    private void Respawn(){
        if(!isAlive){
            respawnTimer -= Time.deltaTime;
        }
        if(respawnTimer <= 1f){
            anim.SetBool("isAlive", true);
        }if(respawnTimer <= 0){
            isAlive = true;
            respawnTimer = 10f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isAlive){
            isAlive = false;
            anim.SetBool("isAlive", false);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
