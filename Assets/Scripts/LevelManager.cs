using UnityEditor.SearchService;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] GameObject player;

    [SerializeField] RandomizedSpawner bettySpawner;
    [SerializeField] private GameObject betty;
    [SerializeField] private BettyAI bettyAI;

    [SerializeField] private RandomizedSpawner recipeSpawner;

    [Header("Sounds")]
    [SerializeField] AudioSource doorSFXSource;

    [Header("UI stuff")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;

    [Header("Scenes")]
    [SerializeField] private int mainMenuBuildIndex;
    void Start()
    {
        StartLevel();
        isPaused = false;
        if (doorSFXSource != null)
        {
            doorSFXSource.Play();
        }

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
        InitRecipe();
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

    private void InitRecipe()
    {
        recipeSpawner.Spawn();
    }

    public static void WinLevel()
    {
        Time.timeScale = 0f;
        // show win UI
    }

    public static void Lose()
    {
        Time.timeScale = 0f;
        // show game over screen
    }


}
