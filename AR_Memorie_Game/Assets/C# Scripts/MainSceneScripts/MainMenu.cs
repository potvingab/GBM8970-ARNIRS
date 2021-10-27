using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool randomizer = false;

    public void PlayGame()
    {
        randomizer = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGameWithRandom()
    {
        randomizer = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
