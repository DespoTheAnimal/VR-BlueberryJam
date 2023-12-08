using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmiNPCBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip, spawnSoundCue;
    [SerializeField] private Transform player;
    private Animator anim;
    private GameObject objectManager, spawningObject;
    private FPIGameManager gameManager;
    private AmmiSpawnNPC ammiSpawn;
    private bool isAlive, shouldSpawn;
    private float spawnTimer = 3, direction = 1;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectManager = GameObject.Find("GameManager");
        gameManager = objectManager.GetComponent<FPIGameManager>();

        spawningObject = GameObject.Find("NPC_Spawner");
        ammiSpawn = spawningObject.GetComponent<AmmiSpawnNPC>();

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameObject playerObjectByName = GameObject.Find("Player");
        if (playerObjectByName != null)
        {
            player = playerObjectByName.transform;
        }
        anim.SetBool("isAlive", true);
        isAlive = true;
        shouldSpawn = true;
        audioSource.clip = spawnSoundCue;
        audioSource.Play();
    }

    void Update()
    {
        LookAtPlayer();
        SpawnNextNPC();
        if(!isAlive){
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void LookAtPlayer(){
        if (player != null && isAlive)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            // Create a rotation towards the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            // Apply the rotation to the NPC
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void SpawnNextNPC(){
        if(!isAlive){
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0 && shouldSpawn){
                shouldSpawn = false;
                ammiSpawn.NPCSpawn();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isAlive){
            isAlive = false;
            anim.SetBool("isAlive", false);
            gameManager.IncrementPlayerScore();
            Destroy(gameObject, 10);
        }
        if(collision.gameObject.CompareTag("Bullet")){
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

}
