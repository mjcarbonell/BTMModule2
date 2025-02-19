using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFollowPlayer : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the inspector
    public float speed = 5f; // Speed at which the cube follows the player
    public float stoppingDistance = 1.5f; // Distance at which the cube stops following

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Move towards the player if it's beyond the stopping distance
            if (distance > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
    }
}
