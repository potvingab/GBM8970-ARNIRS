using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VariablesHolderStroop : MonoBehaviour {
	// Values to store (with default values)
	public static string stroopGameMode; // "Random" or "Fixed"
	public static int stroopTrialTime; // Trial time in seconds
	public static int stroopNumberTrials;
	public static List<string> stroopSequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
	public static List<int> stroopSequenceLevels = new List<int>();
	public static string arduinoPort = "COM3";
	public static string fileName;
	public static bool useMeta;
	// Where to find the values (Options scene)
	public GameObject inputTime;
	public GameObject inputNumberTrials;
	public GameObject Dropdown1;
	public GameObject Dropdown2;
	public GameObject Dropdown3;
	public GameObject Dropdown4;
	public GameObject Dropdown5;
	public GameObject Dropdown6;
	public GameObject DropdownLevel1;
	public GameObject DropdownLevel2;
	public GameObject DropdownLevel3;
	public GameObject DropdownLevel4;
	public GameObject DropdownLevel5;
	public GameObject DropdownLevel6;
	public GameObject ButtonRandom;
	public GameObject ButtonFixed;
	public GameObject ToggleMeta;
	// Where to find the values (FileName scene)
	public GameObject inputFileName;
	public GameObject inputArduinoPort;
	// Pages of the scene
	public GameObject paradigmChoicePage;
	public GameObject FileNameNBackPage;
	public GameObject FileNameStroopPage;
	public GameObject OptionsNBackPage;
	public GameObject OptionsStroopPage;
	public GameObject Options3DPage;

	void Awake(){
		paradigmChoicePage.SetActive(true);
		FileNameNBackPage.SetActive(false);
		FileNameStroopPage.SetActive(false);
		OptionsNBackPage.SetActive(false);
		OptionsStroopPage.SetActive(false);
		Options3DPage.SetActive(false);
	}
	
	public void ChangeParameters() {
		// Update "time (one trial)"
		int.TryParse(inputTime.GetComponent<Text>().text, out stroopTrialTime);
		if (stroopTrialTime == 0){
			stroopTrialTime = 90;
		}
		Debug.Log("Trial time: " + stroopTrialTime);
		// Update "number of trials"
		int.TryParse(inputNumberTrials.GetComponent<Text>().text, out stroopNumberTrials);
		if (stroopNumberTrials == 0){
			stroopNumberTrials = 1;
		}
		Debug.Log("Number of trials: " + stroopNumberTrials);
		// Update "sequence"
		var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6 };
		var DropdownsLevel = new[] { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6 };
		for (int i = 0; i < stroopNumberTrials; i++) {
			stroopSequence.Add(Dropdowns[i].GetComponent<Text>().text);
			stroopSequenceLevels.Add(int.Parse(DropdownsLevel[i].GetComponent<Text>().text));
		}
		Debug.Log("Sequence: " + String.Join(", ", stroopSequence.ToArray()));
		Debug.Log("Sequence levels: " + String.Join(", ", stroopSequenceLevels.Select(x => x.ToString()).ToArray()));
		// Update "game mode"
		if (ButtonRandom.GetComponent<Toggle>().isOn == true){
			stroopGameMode = "Random";
		}
		else{
			stroopGameMode = "Fixed";
		}
		Debug.Log("Game mode: " + stroopGameMode);
		Response.CreateCheckpoint("EndOfMenu");
		useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
	}

	public void ChangeFileNameAndPort() {
		// Update "file name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);
		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("Arduino port: " + arduinoPort);
	}
}
