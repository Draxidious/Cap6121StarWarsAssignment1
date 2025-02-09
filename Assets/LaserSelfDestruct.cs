using UnityEngine;

public class LaserSelfDestruct : MonoBehaviour
{
    [Header("Self-Destruction Timer")]
    public float lifetime = 15f; // Time in seconds before the laser destroys itself
    [SerializeField] public float laserDmg = 0f;

    void Start()
    {
        // Schedule destruction of the game object
        Destroy(gameObject, lifetime);
    }
}
