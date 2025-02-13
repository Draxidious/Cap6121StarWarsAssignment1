using UnityEngine;
using System.Diagnostics;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startStateMenu;
    [SerializeField] private GameObject trainingStateMenu;
    [SerializeField] private GameObject deadStateMenu;
    [SerializeField] private GameObject victoryStateMenu;
	private void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        startStateMenu.SetActive(state == GameState.StartState);
        trainingStateMenu.SetActive(state == GameState.TrainingState);
        deadStateMenu.SetActive(state == GameState.DeathState);
        victoryStateMenu.SetActive(state == GameState.VictoryState);
        //UnityEngine.Debug.LogWarning("Menu Manager");
        //StackTrace stackTrace = new StackTrace();
        //for (int i = 1; i < stackTrace.FrameCount; i++) // Start from 1 to skip this method
        //{
        //	StackFrame frame = stackTrace.GetFrame(i);
        //	string methodName = frame.GetMethod().Name;
        //	UnityEngine.Debug.LogWarning($"Event triggered by: {methodName}");
        //	UnityEngine.Debug.LogWarning(frame.GetMethod().DeclaringType.Name);
        //}
        //You can stop here if you only need the immediate caller
        //break; 
    }
	}

