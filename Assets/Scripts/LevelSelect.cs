using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }

    public void PlaySample()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
