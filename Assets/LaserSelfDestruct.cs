using UnityEngine;

public class LaserSelfDestruct : MonoBehaviour
{
    [Header("Self-Destruction Timer")]
    public float lifetime = 15f; // Time in seconds before the laser destroys itself

    void Start()
    {
        // Schedule destruction of the game object
        Destroy(gameObject, lifetime);
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "enemy" tag
        if (other.CompareTag("Enemy"))
        {
            // Destroy the laser
            Destroy(gameObject);
        }
    }
}
