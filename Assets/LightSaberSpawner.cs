using UnityEngine;

public class LightSaberSpawner : MonoBehaviour
{
    public GameObject saber1;
    public GameObject saber2;
    public Transform spawnLocation1;
    public Transform spawnLocation2;

    public void SpawnSabers()
    {
        if (saber1 != null && spawnLocation1 != null)
        {
            Instantiate(saber1, spawnLocation1.position, Quaternion.Euler(0, 0, 0));
        }

        if (saber2 != null && spawnLocation2 != null)
        {
            Instantiate(saber2, spawnLocation2.position, Quaternion.Euler(0, 0, 0));
        }
    }
}
