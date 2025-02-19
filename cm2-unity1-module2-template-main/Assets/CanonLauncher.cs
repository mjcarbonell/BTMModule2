 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonLauncher : MonoBehaviour
{
    public GameObject cannonballPrefab; // Assign a cannonball prefab in the Inspector
    public Transform firePoint; // The point where the cannonball is fired
    public float launchForce = 10f; // Speed of the cannonball
    
    public float fireRate = 1f; // Delay between shots
    private float nextFireTime = 0f;
    void Update(){
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            FireCannonball();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireCannonball(){
        if (cannonballPrefab != null && firePoint != null)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firePoint.forward * launchForce, ForceMode.Impulse);
            }

            Destroy(cannonball, 5f); // Destroy cannonball after 5 seconds
        }
        else
        {
            Debug.LogWarning("Cannonball prefab or fire point is not assigned!");
        }
    }
}
