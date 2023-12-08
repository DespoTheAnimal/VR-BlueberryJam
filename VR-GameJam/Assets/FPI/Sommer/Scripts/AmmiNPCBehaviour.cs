using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmiNPCBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip, spawnTime;
    [SerializeField] private Transform player;
    private Animator anim;
    private GameObject objectManager;
    private FPIGameManager gameManager;
    private bool isAlive;
    private float spawnTimer = 5, direction = 1;
    void Start()
    {
        objectManager = GameObject.Find("GameManager");
        gameManager = objectManager.GetComponent<FPIGameManager>();

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameObject playerObjectByName = GameObject.Find("Player");
        if (playerObjectByName != null)
        {
            player = playerObjectByName.transform;
        }
        anim.SetBool("isAlive", true);
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
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
            if(spawnTimer <= 0){
                //Call instantiate method
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isAlive){
            isAlive = false;
            anim.SetBool("isAlive", false);
            gameManager.IncrementPlayerScore();
            Destroy(this, 10);
        }
        if(collision.gameObject.CompareTag("Bullet")){
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

}
