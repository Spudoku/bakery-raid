using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool isPaused = false;
    [SerializeField] GameObject player;

    [SerializeField] RandomizedSpawner bettySpawner;
    [SerializeField] private GameObject betty;
    [SerializeField] private BettyAI bettyAI;

    [Header("UI stuff")]
    [SerializeField] private GameObject PauseMenu;
    void Start()
    {
        StartLevel();
        isPaused = true;
    }

    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    private void StartLevel()
    {
        InitBetty();
    }
    private void InitBetty()
    {
        GameObject spawnerPrefab = bettySpawner.prefab;
        if (spawnerPrefab != null && spawnerPrefab.TryGetComponent<BettyAI>(out var bettyPrefabAI))
        {
            bettyPrefabAI.target = player;
        }

        betty = bettySpawner.Spawn();
        bettyAI = betty.GetComponent<BettyAI>();

    }


}
