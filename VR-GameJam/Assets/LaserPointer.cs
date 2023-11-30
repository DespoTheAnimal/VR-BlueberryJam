using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public LineRenderer laser;
    public float defaultLength = 10.0f; // Default length of the laser

    void Update()
    {
        if (laser.enabled)
        {
            UpdateLaser();
        }
    }

    public void PickedUp()
    {
        laser.enabled = true;
    }

    public void Dropped()
    {
        laser.enabled = false;
    }

    private void UpdateLaser()
    {
        RaycastHit hit;
        Vector3 start = Vector3.zero; // Start at the local position of the laser
        Vector3 end;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // Pull back the end point a bit to prevent it from going through the object
            end = hit.point - (transform.forward * 0.01f); // Adjust 0.01f as needed
        }
        else
        {
            end = transform.forward * defaultLength; // Set a default length for your laser
        }

        laser.SetPosition(0, start);
        laser.SetPosition(1, end);
    }
}




