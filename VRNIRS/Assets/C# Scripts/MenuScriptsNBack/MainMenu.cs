using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayNBack()
    {
        SceneManager.LoadScene("N-back");
    }

    public void PlayStroop()
    {
        SceneManager.LoadScene("Stroop");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
