using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject droidPrefab;
    [SerializeField] private TrainingMode trainingMode;

    private List<GameObject> spawnedDroids = new List<GameObject>();

    private void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    public void StartTraining()
    {
        List<Vector3> spawnLocations = trainingMode.getSpawnLocations();
        List<GameObject> droidObjects = trainingMode.getDroidObjects();
        for(int i = 0; i < spawnLocations.Count; i++)
        {
            SpawnDroid(spawnLocations[i], droidObjects[i]);
        }
    }

    public void Update()
    {
        if(spawnedDroids.Count > 0 && spawnedDroids[0] == null)
        {
            spawnedDroids.RemoveAt(0);
            StartTraining();
        }
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.TrainingState:
                StartTraining();
                break;
            default:
                break;
        }
    }

    public void SpawnDroid(Vector3 position, GameObject droid)
    {
        GameObject newDroid = Instantiate(droid, position, Quaternion.identity);
        newDroid.SetActive(true);
        spawnedDroids.Add(newDroid);
    }

    
}
