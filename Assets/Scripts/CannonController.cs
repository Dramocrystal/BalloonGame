using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject arrowPrefab;
    public Transform firePoint;
    public float shootForce = 20f;

    private float angle = 0f;
    private const float angleChange = 10f;
    void Start()
    {
        angle = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Adjust angle with W, S, Up Arrow, and Down Arrow keys
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            angle -= angleChange * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            angle += angleChange * Time.deltaTime;
        }

        // Clamp angle to prevent it from going too low or too high
        angle = Mathf.Clamp(angle, -75f, 30f);

        // Debug.Log("Cannon Angle: " + angle);
        // Rotate the cannon
        transform.eulerAngles = new Vector3(0, 0, angle);

        // Fire the arrow with space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootForce = Random.Range(5, 26);
            FireArrow();
        }
    }


    void FireArrow()
    {
        // Instantiate the arrow
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrow.GetComponent<ArrowProjectile>();
        arrowProjectile.angle = -angle;
        arrowProjectile.initialVelocity = shootForce;
    }
}
