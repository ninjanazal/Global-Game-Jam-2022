using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public levelsInfos levelInfo;

    public GameObject levelSelection;
    public GameObject levels;
    public GameObject Cover;

    private bool start = true;


    private void Start()
    {
        if (levelInfo.level_one > 0)
        {
            levels.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            levels.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }

        Cover.SetActive(true);
    }

    private void Update()
    {
        if ( start )
        {
            if ( Input.anyKeyDown )
            {
                start = false;
                Cover.SetActive(false);
            }
        }
    }

    public void OpenLevelSelection()
    {
        levelSelection.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        levelSelection.SetActive(false);
    }

    public void StartLevel(int level)
    {
        SceneManager.LoadSceneAsync(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
