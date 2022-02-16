using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class VariablesHolder : MonoBehaviour
{
    public static float speed;
    public static float GameSpeed;
    public static string arduinoPort = "COM3";
    public static string fileName;
    public static string gameMode;
    public static bool useMeta;
    public static bool useVisual;
    public static bool useAudio;
    public static int nBackNumber;
    public static bool realistCheck;

    public GameObject inputFileName;
    public GameObject inputArduinoPort;
    public GameObject errorMessageFileName;
    public GameObject FileNameNBackPage;
    public GameObject OptionsNBackPage;
    public GameObject ButtonRandom;
    public GameObject ButtonFixed;
    public GameObject ToggleMeta;
    public GameObject ToggleVisual;
    public GameObject ToggleAudio;
    public Slider NBackSlider;
    public Slider NBackLevelSlider;
    public GameObject ToggleReal;

    void Start()
    {
        speed = 1;
        GameSpeed = 0;
        nBackNumber = 2;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void UpdateNBack()
    {
        nBackNumber = (int)NBackSlider.value;
    }

    public void UpdateNBackLevel()
    {
        nBackNumber = (int)NBackLevelSlider.value;
    }

    public void ChangeParameters()
    {
        // Update "game mode"
        useVisual = ToggleVisual.GetComponent<Toggle>().isOn;
        useAudio = ToggleAudio.GetComponent<Toggle>().isOn;
        Debug.Log(useVisual);
        Debug.Log(useAudio);
        if (ButtonRandom.GetComponent<Toggle>().isOn == true)
        {
            gameMode = "Random";
        }
        else
        {
            gameMode = "Fixed";
        }
        Debug.Log("Game mode: " + gameMode);
        //TimeSpawner.CreateCheckpoint("EndOfMenu");
        useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
        realistCheck = ToggleMeta.GetComponent<Toggle>().isOn;
        Debug.Log("Real: " + realistCheck.ToString());

    }

    public void ChangeFileNameAndPort()
    {
        // Update "file name"
        fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
        Debug.Log("File name: " + fileName);
        // Update "Arduino port"
        arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
        arduinoPort = Regex.Replace(arduinoPort, "[^A-Za-z0-9 -]", "");
        Debug.Log("Arduino port: " + arduinoPort);
        Debug.Log(arduinoPort == "COM3");
        Debug.Log(arduinoPort.Length);
        // Check if valid inputs
        if ((fileName.Contains("/")) && (arduinoPort.Contains("COM")))
        {
            errorMessageFileName.SetActive(false);
            FileNameNBackPage.SetActive(false);
            OptionsNBackPage.SetActive(true);
        }
        else
        {
            errorMessageFileName.SetActive(true);
        }
    }
}