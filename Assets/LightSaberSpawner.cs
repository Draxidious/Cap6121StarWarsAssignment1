using UnityEngine;

public class LightSaberSpawner : MonoBehaviour
{
    public GameObject saber1;
    public GameObject saber2;
    public Transform spawnLocation1;
    public Transform spawnLocation2;
    GameObject lightsaber1;
    GameObject lightsaber2;

    public void SpawnSabers()
    {
        if (saber1 != null && spawnLocation1 != null)
        {
			lightsaber1 = Instantiate(saber1, spawnLocation1.position, Quaternion.Euler(0, 0, 0));
        }

        if (saber2 != null && spawnLocation2 != null)
        {
            lightsaber2 = Instantiate(saber2, spawnLocation2.position, Quaternion.Euler(0, 0, 0));
        }
    }
    public void DespawnSabers()
    {
        if(lightsaber1 != null)
        {
            Destroy(lightsaber1);
        }
        if(lightsaber2 != null)
        {
            Destroy(lightsaber2);
        }
    }
}
