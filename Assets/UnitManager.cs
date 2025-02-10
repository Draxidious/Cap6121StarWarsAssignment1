using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject droidPrefab;
    [SerializeField] private TrainingMode trainingMode;
    [SerializeField] private TrainingMode Level1Mode;
    [SerializeField] private TrainingMode Level2Mode;
    [SerializeField] private TrainingMode Level3Mode;
	[SerializeField] private TrainingMode BossMode;


	public List<GameObject> spawnedDroids = new List<GameObject>();

    public Player player;

    private void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    public void StartTraining()
    {
        List<Vector3> spawnLocations = trainingMode.getSpawnLocations();
        List<GameObject> droidObjects = trainingMode.getDroidObjects();
        for (int i = 0; i < spawnLocations.Count; i++)
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
        List<Vector3> spawnLocations = Level3Mode.getSpawnLocations();
        List<GameObject> droidObjects = Level3Mode.getDroidObjects();
        for (int i = 0; i < spawnLocations.Count; i++)
        {
            SpawnDroid(spawnLocations[i], droidObjects[i]);
        }
    }
	public void StartBoss()
	{
		List<Vector3> spawnLocations = BossMode.getSpawnLocations();
		List<GameObject> droidObjects = BossMode.getDroidObjects();
		for (int i = 0; i < spawnLocations.Count; i++)
		{
			SpawnDroid(spawnLocations[i], droidObjects[i]);
		}
	}


	public void Update()
    {
        if (spawnedDroids.Count > 0)
        {
            // Check to see if all droids died
            bool alive = false;
            foreach (GameObject obj in spawnedDroids)
            {
                if (obj != null) alive = true;
            }

            if (!alive)
            {
				//player.killDroids();
				spawnedDroids.Clear();
                spawnedDroids = new List<GameObject>();
                //Debug.LogWarning("we got hereeeee: " + GameManager.instance.State);
                switch (GameManager.instance.State)
                {
                    case GameState.TrainingState:
                        print("Starting Training");
                        GameManager.instance.SetTrainingState();
                        break;

                    case GameState.Level1State:
                        print("Starting Level2");
                        GameManager.instance.SetLevel2State();
                        break;
                    case GameState.Level2State:
                        GameManager.instance.SetLevel3State();
                        break;
                    case GameState.Level3State:
						GameManager.instance.SetBossBattleState();
						break;
					case GameState.BossBattleState:
						GameManager.instance.SetVictoryState();
						player.Reset();

						break;
					default:
                        break;

                }

            }
            if(player.health <= 0)
            {
				player.killDroids();

				foreach (GameObject obj in spawnedDroids)
				{
					if (obj != null)
					{
						Destroy(obj);
					}
				}
				spawnedDroids.Clear ();
				GameManager.instance.SetDeathState();
				player.Reset();
			}
        
        }
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.TrainingState:
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
			case GameState.BossBattleState:
				StartBoss();
				break;
			default:
                player.killDroids();
                print("DESTROY ALL DROIDS: " + spawnedDroids);

				foreach (GameObject obj in spawnedDroids)
                {
                    if (obj != null)
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
