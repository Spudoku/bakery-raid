using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI;
    [SerializeField] int mainLevelBuildIndex = 1;

    [SerializeField] AudioSource menuMusic;


    void Awake()
    {


        tutorialUI.SetActive(false);

    }

    void OnEnable()
    {
        Debug.Log("[MainMenuManager.OnEnable] awake called!");

    }

    void Update()
    {
        if (menuMusic != null && !menuMusic.isPlaying)
        {
            menuMusic.Play();
            Debug.Log("Menu music started playing.");
        }
    }




    #region buttons
    public void OnTutorialOpenClick()
    {
        Debug.Log("Should open the tutorial now");
        tutorialUI.SetActive(true);
    }

    public void OnTutorialCloseClick()
    {
        Debug.Log("Should close the tutorial now");
        tutorialUI.SetActive(false);
    }

    public void LoadMainLevel()
    {
        SceneManager.LoadScene(mainLevelBuildIndex);
        menuMusic.Stop();
    }
    #endregion
}
