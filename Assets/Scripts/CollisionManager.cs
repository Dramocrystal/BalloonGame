using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private List<Balloon> balloons;
    private List<ArrowProjectile> arrows;

    public GameObject balloonPrefab;


    private float leftWallX = -6f;
    private float rightWallX = 6f;
    private float wallHeight = 3f;
    private float topBorder = 8f;
    private float ground = -3f;
    private float screenLeftX = -10f;
    private float screenRightX = 10f;

    private float directHitThreshold = 0.3f; // Adjust this value to determine what counts as a direct hit


    // Start is called before the first frame update
    void Start()
    {
        balloons = new List<Balloon>(FindObjectsOfType<Balloon>());
        arrows = new List<ArrowProjectile>();
        Debug.Log("Found " + balloons.Count + " balloons in the scene.");
    }

    // Update is called once per frame
    void Update()
    {
        // Update the list of arrows in the scene
        UpdateArrowsList();

        // Check balloon-balloon collisions
        for (int i = 0; i < balloons.Count; i++)
        {
            CheckWallCollisions(balloons[i]);

            for (int j = i + 1; j < balloons.Count; j++)
            {
                CheckBalloonCollision(balloons[i], balloons[j]);
            }
        }

        // Check arrow-balloon collisions
        for (int i = arrows.Count - 1; i >= 0; i--)
        {
            for (int j = balloons.Count - 1; j >= 0; j--)
            {
                CheckArrowBalloonCollision(arrows[i], balloons[j]);
            }
        }

        // Remove arrows that are out of bounds
        RemoveOutOfBoundsArrows();
    }

    void CheckBalloonCollision(Balloon b1, Balloon b2)
    {
        float dx = b2.position.x - b1.position.x;
        float dy = b2.position.y - b1.position.y;
        float distanceSquared = (dx * dx) + (dy * dy);
        float radiiSumSquared = (b1.radius + b2.radius) * (b1.radius + b2.radius);

        // Check if the distance is less than the sum of the radii (collision)
        if (distanceSquared < radiiSumSquared)
        {
            //Debug.Log("Collision detected between balloons at positions: " + b1.position + " and " + b2.position);
            ResolveBalloonCollision(b1, b2, Mathf.Sqrt(distanceSquared));
        }

    }

    void ResolveBalloonCollision(Balloon b1, Balloon b2, float distance)
    {
        // Calculate the normal vector at the point of collision
        Vector2 normal = (b2.position - b1.position).normalized;

        // Calculate relative velocity
        Vector2 relativeVelocity = b2.velocity - b1.velocity;

        // Calculate the normal component of the relative velocity
        float Vn_minus = Vector2.Dot(relativeVelocity, normal);

        // Coefficient of restitution (between 0 and 1)
        float epsilon = 0.95f; // Adjust this value as needed

        // Calculate the impulse magnitude
        float j = -(1 + epsilon) * Vn_minus / (1 / b1.mass + 1 / b2.mass);

        // Apply the impulse to both balloons
        b1.velocity -= (j / b1.mass) * normal;
        b2.velocity += (j / b2.mass) * normal;

        // Separate the balloons to prevent overlap
        float overlap = (b1.radius + b2.radius) - distance;
        if (overlap > 0)
        {
            float separationDistance = overlap * 0.5f; // Move both balloons half the overlap distance
            b1.position -= normal * separationDistance;
            b2.position += normal * separationDistance;
        }
    }


    void CheckWallCollisions(Balloon balloon)
    {

        Vector2 prevPosition = new Vector2(balloon.transform.position.x, balloon.transform.position.y) - balloon.velocity;

        // Check left wall (cliff)
        if (balloon.position.x - balloon.radius < leftWallX && balloon.position.y - balloon.radius - 0.1 < wallHeight)
        {
            if (prevPosition.y > wallHeight)
            {
                Debug.Log("Ballon came from top");
                balloon.velocity.y = Mathf.Abs(balloon.velocity.y);
                balloon.position.y = wallHeight + balloon.radius + 0.1f;
            }
            else

            {
                Debug.Log("Collision with left wall at position: " + balloon.position);
                balloon.velocity.x = Mathf.Abs(balloon.velocity.x); // Bounce off left wall
                balloon.position.x = leftWallX + balloon.radius; // Prevent sticking
            }

        }

        // Check right wall (cliff)
        if (balloon.position.x + balloon.radius > rightWallX && balloon.position.y - balloon.radius - 0.1 < wallHeight)
        {
            if (prevPosition.y > wallHeight)
            {
                Debug.Log("Ballon came from top");
                balloon.velocity.y = Mathf.Abs(balloon.velocity.y);
                balloon.position.y = wallHeight + balloon.radius + 0.1f;
            }
            else
            {
                //Debug.Log("Collision with right wall at position: " + balloon.position);
                balloon.velocity.x = -Mathf.Abs(balloon.velocity.x); // Bounce off right wall
                balloon.position.x = rightWallX - balloon.radius; // Prevent sticking
            }

        }



        // Check bottom wall (ground)
        if (balloon.position.y - balloon.radius - 0.1 < ground)
        {
            //Debug.Log("Collision with ground at position: " + balloon.position);
            balloon.velocity.y = Mathf.Abs(balloon.velocity.y); // Bounce off bottom wall
            balloon.position.y = ground + balloon.radius + 0.1f; // Prevent sticking
        }


        // Check out of scene (top)
        if (balloon.position.y - (2 * balloon.radius) > topBorder)
        {
            //Debug.Log("Collision with top wall at position: " + balloon.position);
            //destroy the ballon its out of the scene
            balloons.Remove(balloon);
            Destroy(balloon.gameObject);
            SpawnNewBalloon();
        }

        // Check out of screen right

        if (balloon.position.x > screenRightX)
        {
            //Debug.Log("Collision with top wall at position: " + balloon.position);
            //destroy the ballon its out of the scene
            balloons.Remove(balloon);
            Destroy(balloon.gameObject);
            SpawnNewBalloon();
        }

        //Check out of screen left

        if (balloon.position.x < screenLeftX)
        {
            //Debug.Log("Collision with top wall at position: " + balloon.position);
            //destroy the ballon its out of the scene
            balloons.Remove(balloon);
            Destroy(balloon.gameObject);
            SpawnNewBalloon();
        }


    }


    void CheckArrowBalloonCollision(ArrowProjectile arrow, Balloon balloon)
    {
        // Define the dimensions of the arrow
        float arrowHalfLength = 0.4f / 2f;
        float arrowHalfHeight = 0.1f / 2f;

        // Get the arrow's center position
        Vector2 arrowCenter = arrow.transform.position;

        // Get the balloon's center position
        Vector2 balloonCenter = balloon.transform.position;

        // Calculate the closest point on the arrow rectangle to the balloon's center
        Vector2 closestPointOnArrow = new Vector2(
            Mathf.Clamp(balloonCenter.x, arrowCenter.x - arrowHalfLength, arrowCenter.x + arrowHalfLength),
            Mathf.Clamp(balloonCenter.y, arrowCenter.y - arrowHalfHeight, arrowCenter.y + arrowHalfHeight)
        );

        // Calculate the distance from the balloon's center to the closest point on the arrow
        float distance = Vector2.Distance(closestPointOnArrow, balloonCenter);

        // Check if the distance is less than the radius of the balloon
        if (distance <= balloon.radius)
        {

            if (closestPointOnArrow.y > balloonCenter.y + directHitThreshold)
            {
                Debug.Log("top slide");
                ResolveGlancingBlow(arrow, balloon, closestPointOnArrow);
            }
            else if (closestPointOnArrow.y < balloonCenter.y - directHitThreshold)
            {
                Debug.Log("bottom slide");
                ResolveGlancingBlow(arrow, balloon, closestPointOnArrow);
            }
            else
            {
                Debug.Log("Direct hit");
                DestroyBalloonAndArrow(balloon, arrow);
                SpawnNewBalloon();
            }


        }
    }

    void ResolveGlancingBlow(ArrowProjectile arrow, Balloon balloon, Vector2 collisionPoint)
    {
        Vector2 normal = ((Vector2)balloon.transform.position - collisionPoint).normalized;
        Vector2 arrowVelocity = arrow.velocity;

        // Calculate impulse for the momentum transfer
        float impulse = Vector2.Dot(arrowVelocity - balloon.velocity, normal) * 2f / (1f / balloon.mass + 1f / 1f);

        // Update balloon velocity to simulate the push
        balloon.velocity += normal * impulse / balloon.mass;

        // Arrow velocity remains largely unchanged for a tangential hit, with slight change
        arrow.velocity -= normal * impulse * 0.05f; // Slight velocity change

        // Move the balloon slightly out of collision to avoid sticking
        float overlap = balloon.radius - Vector2.Distance(collisionPoint, balloon.transform.position);
        balloon.transform.position += (Vector3)(normal * overlap);

        //Debug.Log("Tangential blow resolved. Balloon velocity: " + balloon.velocity);
    }

    void UpdateArrowsList()
    {
        // Find all arrows in the scene
        ArrowProjectile[] sceneArrows = FindObjectsOfType<ArrowProjectile>();

        // Add new arrows to the list
        foreach (ArrowProjectile arrow in sceneArrows)
        {
            if (!arrows.Contains(arrow))
            {
                arrows.Add(arrow);
                //Debug.Log("New arrow detected and added to the list.");
            }
        }

        // Remove arrows that no longer exist in the scene
        arrows.RemoveAll(arrow => arrow == null);
    }

    void RemoveOutOfBoundsArrows()
    {
        for (int i = arrows.Count - 1; i >= 0; i--)
        {
            //out of screen
            if (arrows[i].transform.position.x + 0.2 > screenRightX ||
                arrows[i].transform.position.x - 0.2 < screenLeftX ||
                arrows[i].transform.position.y + 0.05 > topBorder ||
                arrows[i].transform.position.y - 0.05 < ground)
            {
                Destroy(arrows[i].gameObject);
                arrows.RemoveAt(i);
                //Debug.Log("Arrow removed (out of bounds)");
            }

            //Check if it hits the cliff wall/cliff top ground

            Vector2 prevPosition = new Vector2(arrows[i].transform.position.x, arrows[i].transform.position.y) - arrows[i].velocity;

            // Check left wall (cliff)
            if (arrows[i].transform.position.x - 0.2 < leftWallX && arrows[i].transform.position.y - 0.05 < wallHeight)
            {
                if (prevPosition.y > wallHeight)
                {
                    Debug.Log("arrow came from top");
                    Destroy(arrows[i].gameObject);
                    arrows.RemoveAt(i);
                }
                else

                {
                    Destroy(arrows[i].gameObject);
                    arrows.RemoveAt(i);
                }

            }

            //no need to check for the right wall (cliff)


        }
    }

    

    void DestroyBalloonAndArrow(Balloon balloon, ArrowProjectile arrow)
    {
        balloons.Remove(balloon);
        arrows.Remove(arrow);
        Destroy(balloon.gameObject);
        Destroy(arrow.gameObject);
        //Debug.Log("Direct hit! Balloon and arrow destroyed.");
    }

    void SpawnNewBalloon()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(leftWallX + 1, rightWallX - 1), Random.Range(ground + 1, wallHeight - 2), 0);
        GameObject newBalloonObject = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
        Balloon newBalloon = newBalloonObject.GetComponent<Balloon>();
        balloons.Add(newBalloon);
        //Debug.Log("New balloon spawned at: " + spawnPosition);
    }



}
