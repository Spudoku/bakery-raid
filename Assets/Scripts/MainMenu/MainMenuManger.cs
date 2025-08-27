using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManger : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI;
    [SerializeField] int mainLevelBuildIndex = 1;


    void Awake()
    {
        // tutorial menu

        tutorialUI.SetActive(false);

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
    }
    #endregion
}
