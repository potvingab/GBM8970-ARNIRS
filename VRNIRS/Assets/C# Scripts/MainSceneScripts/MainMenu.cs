using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool randomizer = false;

    public void PlayNBack()
    {
        randomizer = false;
        SceneManager.LoadScene("N-back");
    }

    public void PlayNBackRandom()
    {
        randomizer = true;
        SceneManager.LoadScene("N-back");
    }

    public void PlayStroop()
    {
        randomizer = false;
        SceneManager.LoadScene("Stroop");
    }

    public void PlayStroopRandom()
    {
        randomizer = true;
        SceneManager.LoadScene("Stroop");
    }


    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
