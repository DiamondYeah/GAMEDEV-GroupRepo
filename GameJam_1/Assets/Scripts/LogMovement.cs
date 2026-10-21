using UnityEngine;

public class LogMovement : MonoBehaviour
{
    float speed;
    float travelDistance;

    Vector3 direction;
    Vector3 startPosition;

    bool initialized;
    public Vector3 Velocity => direction * speed;

    public void Initialize(float moveSpeed, Vector3 moveDirection, float distanceToTravel)
    {
        speed = moveSpeed;
        direction = moveDirection.normalized;
        travelDistance = distanceToTravel;
        startPosition = transform.position;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;

        transform.position += Velocity * Time.deltaTime;

        // Destroy the log if it has traveled the specified distance
        if (Vector3.Distance(transform.position, startPosition) >= travelDistance)
        {
            Destroy(gameObject);
        }
    }
}
