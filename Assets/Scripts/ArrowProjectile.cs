using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float initialVelocity = 10f;  // Initial speed of the arrow
    public float angle = 45f;  // Initial angle in degrees
    public Vector2 velocity;
    private float gravity = 9.81f;
    private float time;

    void Start()
    {
        // Convert angle to radians for Mathf trig functions
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Set initial velocity based on angle
        velocity = new Vector2(initialVelocity * Mathf.Cos(angleInRadians), initialVelocity * Mathf.Sin(angleInRadians));

        time = 0f;
    }

    void Update()
    {
        time += Time.deltaTime;

        // Update horizontal (vx) remains constant, vertical (vy) decreases due to gravity
        float vx = velocity.x;
        float vy = velocity.y - gravity * time;

        // Move arrow based on kinematics
        transform.position += new Vector3(-vx, vy, 0) * Time.deltaTime;
    }
}
