using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource safeMusic;
    public AudioSource battleMusic;

    private void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }
    private void GameManagerOnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Level1State:
            case GameState.Level2State:
            case GameState.Level3State:
            case GameState.BossBattleState:
                if (safeMusic.isPlaying && !battleMusic.isPlaying)
                {
                    safeMusic.Stop();
                    battleMusic.Play();
                }
                break;
            default:
                if(!safeMusic.isPlaying) safeMusic.Play();
                battleMusic.Stop();
                break;
        }

    }
}
