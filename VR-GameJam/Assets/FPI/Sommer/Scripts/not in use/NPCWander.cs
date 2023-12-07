using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWander : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Movement speed of the NPC

        private float moveTimer; // Timer to track movement
        private float waitTimer; // Timer to track waiting
        private Vector3 moveDirection; // Current movement direction
        Animator animator;

    private Bounds walkingBounds;

    public void SetBoundaries(Bounds bounds)
    {
        walkingBounds = bounds;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        PickNewDirectionAndTimers();
            GetComponent<Rigidbody>().isKinematic = true;
        
    }

     void Update()
    {
            if (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
                if (moveTimer <= 0) // If move timer has run out, start the wait timer
                {
                    waitTimer = Random.Range(5.0f, 10.0f); // Set a new random wait timer
                }
                else
                {
                    MoveNPC();
                }
            }
            else if (waitTimer > 0)
            {
                animator.SetBool("Walking", false);
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    PickNewDirectionAndTimers();
                }
            }
    }

    void MoveNPC()
    {
        Vector3 newPos = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Only perform the collision check if we're still within the move timer
        if (moveTimer > 0)
        {
            // Check for collisions with NPC layer only
            int npcLayer = LayerMask.NameToLayer("NPC");
            int layerMask = 1 << npcLayer;
            Collider[] hitColliders = Physics.OverlapSphere(newPos, 0.5f, layerMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject) // Exclude self-collision
                {                  
                    PickNewDirectionAndTimers();
                    return; // Exit the method early to avoid moving into the other NPC
                }
            }
        }

        // If no collisions are detected and we're within the walking bounds, move the NPC
        if (newPos.x >= walkingBounds.min.x && newPos.x <= walkingBounds.max.x &&
            newPos.z >= walkingBounds.min.z && newPos.z <= walkingBounds.max.z)
        {
            transform.position = newPos;
            transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        }
        else
        {
            PickNewDirectionAndTimers();
        }
    }

    void PickNewDirectionAndTimers()
    {
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        moveTimer = Random.Range(1.0f, 2.0f); // Random move time between seconds
        waitTimer = Random.Range(6.0f, 18.0f);

        // Update the IsWalking parameter based on movement
        animator.SetBool("Walking", moveTimer > 0);
    }
}
