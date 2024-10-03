using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public float radius;
    public float mass;

    // Maximum upward velocity the balloon can regain after a collision
    public float maxYVelocity = 2f;
    // How quickly the balloon regains its upward velocity
    public float upwardForce = 0.1f;

    void Start()
    {
        position = transform.position;
        velocity = new Vector2(0, 1); // Start with an upward velocity
    }

    void Update()
    {
        ApplyRandomAirCurrent();
        RecoverYVelocity(); // Gradually restore upward velocity
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    void ApplyRandomAirCurrent()
    {
        // Apply a slight random force to simulate chaotic air movement
        velocity.x += Random.Range(-0.05f, 0.05f);
    }

    void RecoverYVelocity()
    {
        // Gradually regain upward velocity if it's below the maximum
        if (velocity.y < maxYVelocity)
        {
            velocity.y += upwardForce * Time.deltaTime;
        }

        // Cap the y-velocity to the maximum limit
        if (velocity.y > maxYVelocity)
        {
            velocity.y = maxYVelocity;
        }
    }
}
