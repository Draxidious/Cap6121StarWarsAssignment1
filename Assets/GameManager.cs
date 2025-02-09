using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State = GameState.StartState;

    public static event Action<GameState> OnGameStateChange;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameState(State);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        OnGameStateChange?.Invoke(newState);
    }
    public void SetStartState() => UpdateGameState(GameState.StartState);
    public void SetTrainingState() => UpdateGameState(GameState.TrainingState);
	public void SetEndTrainingState() => UpdateGameState(GameState.EndTrainingState);
	public void SetLevel1State() => UpdateGameState(GameState.Level1State);
    public void SetLevel2State() => UpdateGameState(GameState.Level2State);
    public void SetLevel3State() => UpdateGameState(GameState.Level3State);
    public void SetBossBattleState() => UpdateGameState(GameState.BossBattleState);
    public void SetVictoryState() => UpdateGameState(GameState.VictoryState);
    public void SetDeathState() => UpdateGameState(GameState.DeathState);
}

public enum GameState
{
    StartState,
    TrainingState,
	EndTrainingState,
	Level1State,
    Level2State,
    Level3State,
    BossBattleState,
    VictoryState,
    DeathState
}