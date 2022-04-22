using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void OpenMenu()
    {
        if (SaveManager.instance.hasLoaded)
        {
            gameObject.SetActive(true);
        }  
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }

    public void PlaySample()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PlayGame2()
    {
        SceneManager.LoadScene("level2");
    }
}
