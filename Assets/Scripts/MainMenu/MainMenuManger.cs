using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI;
    [SerializeField] int mainLevelBuildIndex = 1;

    [SerializeField] AudioSource menuMusic;


    void Awake()
    {
        Debug.Log("[MainMenuManager.Awake] awake called!");

        tutorialUI.SetActive(false);

    }

    void OnEnable()
    {
        menuMusic.Play();
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
