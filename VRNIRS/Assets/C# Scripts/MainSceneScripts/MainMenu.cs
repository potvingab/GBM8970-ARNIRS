using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool randomizer = false;
    public static string mode;

    public void PlayNBack()
    {
        if (mode == "RANDOM"){
            randomizer = true;
        }
        else{
            randomizer = false;
        }
        SceneManager.LoadScene("N-back");
    }

    // public void PlayNBackRandom()
    // {
    //     randomizer = true;
    //     SceneManager.LoadScene("N-back");
    // }

    public void PlayStroop()
    {
        if (mode == "RANDOM"){
            randomizer = true;
        }
        else{
            randomizer = false;
        }
        SceneManager.LoadScene("Stroop");
    }

    // public void PlayStroopRandom()
    // {
    //     randomizer = true;
    //     SceneManager.LoadScene("Stroop");
    // }


    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
