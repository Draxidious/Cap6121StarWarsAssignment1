using UnityEngine;

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
    }
}
