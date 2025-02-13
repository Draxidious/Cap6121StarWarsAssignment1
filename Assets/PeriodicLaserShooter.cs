using System.Collections;
using UnityEngine;

public class PeriodicLaserShooter : MonoBehaviour
{
    [Header("Laser Configuration")]
    public GameObject laserPrefab; // Prefab for the laser
    public Transform target; // Target GameObject the laser will shoot towards
    public float laserSpeed = 10f; // Speed of the laser
    public float returnLaserSpeed = 15f; // Speed of the laser when returning
    public float laserDamageToPlayer = 10f;

    [Header("Firing Configuration")]
    public float minFireInterval = 5f; // Minimum interval between shots
    public float maxFireInterval = 10f; // Maximum interval between shots
    public float firingRange = 20f; // Range within which the laser can be fired

    private Coroutine firingCoroutine;
    public bool inFuture = false;

    void Start()
    {
        // Start the periodic firing coroutine
        firingCoroutine = StartCoroutine(PeriodicFire());
    }

    void Update()
    {
        // Prevent firing when inFuture is true
        if (inFuture)
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = null;
            }
            return;
        }

        // Restart firing if it was previously stopped
        if (firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(PeriodicFire());
        }
    }

    IEnumerator PeriodicFire()
    {
        while (true)
        {
            // Check if the target is within range
            if (target != null && Vector3.Distance(transform.position, target.position) <= firingRange)
            {
                FireLaser();
            }

            // Wait for a random interval before firing again
            float interval = Random.Range(minFireInterval, maxFireInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    private void FireLaser()
    {
        if (laserPrefab == null || target == null)
        {
            Debug.LogWarning("LaserPrefab or Target is not assigned.");
            return;
        }

        // Instantiate the laser object at the current position
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);

        laser.GetComponent<LaserSelfDestruct>().laserDmg = laserDamageToPlayer;

        // Calculate direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Align the capsule's Y-axis with the direction it is traveling
        laser.transform.up = direction;

        // Set the laser's velocity to move it towards the target
        Rigidbody laserRigidbody = laser.GetComponent<Rigidbody>();
        if (laserRigidbody != null)
        {
            laserRigidbody.linearVelocity = direction * laserSpeed;
        }
        else
        {
            Debug.LogWarning("Laser prefab does not have a Rigidbody component.");
        }

        // Attach collision behavior to the laser
        LaserBehavior laserBehavior = laser.GetComponent<LaserBehavior>();
        if (laserBehavior == null)
        {
            laserBehavior = laser.AddComponent<LaserBehavior>();
        }

        // Configure the laser behavior
        laserBehavior.Initialize(this.gameObject, target.gameObject, returnLaserSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the firing range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRange);
    }
}

public class LaserBehavior : MonoBehaviour
{
    private GameObject originalSource;
    private GameObject currentTarget;
    
    private Rigidbody laserRigidbody;

    [Header("Reflecting Configuration")]
    public float returnSpeed;

    public void Initialize(GameObject source, GameObject target, float returnSpeed)
    {
        this.originalSource = source;
        this.currentTarget = target;
        this.returnSpeed = returnSpeed;
        this.laserRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {

        // If the laser hits an object with the tag, reverse direction
        if (other.CompareTag("Saber"))
        {
            // Original source is thing that shot it, and currentTarget is the thing that is NOW shooting it back
            Vector3 direction = (originalSource.transform.position - currentTarget.transform.position).normalized;

            // Align the capsule's Y-axis with the new direction
            transform.up = direction;

            // Set tag to laser
            gameObject.tag = "Laser";

            other.gameObject.GetComponent<AudioSource>()?.Play();

            // Set the new velocity
            if (laserRigidbody != null)
            {
                laserRigidbody.linearVelocity = direction * returnSpeed;
            }
        }
    }
}
