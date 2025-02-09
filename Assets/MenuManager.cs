using UnityEngine;
using System.Diagnostics;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startStateMenu;
    private void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        startStateMenu.SetActive(state == GameState.StartState);
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

