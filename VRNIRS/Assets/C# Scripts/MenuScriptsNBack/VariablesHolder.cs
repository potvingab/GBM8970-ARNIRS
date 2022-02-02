﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesHolder : MonoBehaviour {
    public static float speed;
    public static float GameSpeed;
    public static string arduinoPort = "COM3";
	public static string fileName;
    public static string gameMode; 
    public static bool useMeta;
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

    public void ChangeParameters() {
        // Update "game mode"
		if (ButtonRandom.GetComponent<Toggle>().isOn == true){
			gameMode = "Random";
		}
		else{
			gameMode = "Fixed";
		}
		Debug.Log("Game mode: " + gameMode);
		Response.CreateCheckpoint("EndOfMenu");
		useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
        realistCheck = ToggleMeta.GetComponent<Toggle>().isOn;
        Debug.Log("Real: " + realistCheck.ToString());
    }

    public void ChangeFileNameAndPort() {
		// Update "file name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);
		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("Arduino port: " + arduinoPort);
		// Check if valid inputs
		if ((fileName.Contains("\\")) && (arduinoPort.Contains("COM"))) {
			errorMessageFileName.SetActive(false);
			FileNameNBackPage.SetActive(false);
			OptionsNBackPage.SetActive(true);
        }
		else{
			errorMessageFileName.SetActive(true);
		}
	}
}