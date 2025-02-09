using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public Player player;
    public AudioSource oof;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LaserInitial"))
        {
            player.health -= other.gameObject.GetComponent<LaserSelfDestruct>().laserDmg;
            oof.Play();
        }
    }
}
