using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool isPaused = false;

    public static bool gameOver = false;
    [SerializeField] GameObject player;

    [SerializeField] RandomizedSpawner bettySpawner;
    [SerializeField] private GameObject betty;
    [SerializeField] private BettyAI bettyAI;
    [SerializeField] private float gracePeriod = 5f;

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
        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);
        winMenu.SetActive(false);

        if (doorSFXSource != null)
        {
            doorSFXSource.Play();
        }

    }

    void Update()
    {
        if (isPaused || gameOver)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;

        }
        else
        {
            Time.timeScale = 1.0f;
            AudioListener.pause = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
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
        bettyAI.Stun(gracePeriod);
        bettyAI.levelManager = this;

    }

    private void InitRecipe()
    {
        recipeSpawner.Spawn();
    }

    public void WinLevel()
    {
        gameOver = true;
        Time.timeScale = 0f;
        // show win UI
        winMenu.SetActive(true);
    }



    public void Lose()
    {
        gameOver = true;
        Time.timeScale = 0f;
        // show game over screen
        loseMenu.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
