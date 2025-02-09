using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject droidPrefab;
    [SerializeField] private TrainingMode trainingMode;
    [SerializeField] private TrainingMode Level1Mode;
    [SerializeField] private TrainingMode Level2Mode;
	[SerializeField] private TrainingMode Level3Mode;


	private List<GameObject> spawnedDroids = new List<GameObject>();

    public Player player;

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
	public void StartLevel1()
	{
		List<Vector3> spawnLocations = Level1Mode.getSpawnLocations();
		List<GameObject> droidObjects = Level1Mode.getDroidObjects();
		for (int i = 0; i < spawnLocations.Count; i++)
		{
			SpawnDroid(spawnLocations[i], droidObjects[i]);
		}
	}
	public void StartLevel2()
	{
		List<Vector3> spawnLocations = Level2Mode.getSpawnLocations();
		List<GameObject> droidObjects = Level2Mode.getDroidObjects();
		for (int i = 0; i < spawnLocations.Count; i++)
		{
			SpawnDroid(spawnLocations[i], droidObjects[i]);
		}
	}
	public void StartLevel3()
	{
		List<Vector3> spawnLocations = Level2Mode.getSpawnLocations();
		List<GameObject> droidObjects = Level2Mode.getDroidObjects();
		for (int i = 0; i < spawnLocations.Count; i++)
		{
			SpawnDroid(spawnLocations[i], droidObjects[i]);
		}
	}

	public void Update()
    {
        if(GameManager.instance.State == GameState.TrainingState && spawnedDroids.Count > 0 && spawnedDroids[0] == null)
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
                Debug.LogWarning("Training Started");
                StartTraining();
				break;
			case GameState.Level1State:
				StartLevel1();
				break;
            case GameState.Level2State:
                StartLevel2();
                break;
			case GameState.Level3State:
				StartLevel3();
				break;
			default:
                print("DESTROY ALL DROIDS: " + spawnedDroids);
                foreach(GameObject obj in spawnedDroids)
                {
                    if(obj!=null)
                    {
                        Destroy(obj);
                    }
                }
                spawnedDroids.Clear();
                break;
        }
        
    }

    public void SpawnDroid(Vector3 position, GameObject droid)
    {
        GameObject newDroid = Instantiate(droid, position, Quaternion.identity);
        newDroid.SetActive(true);
        spawnedDroids.Add(newDroid);
        player.droids.Add(newDroid.GetComponent<droidTopLevel>().droid);
    }

    
}
