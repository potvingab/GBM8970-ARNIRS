using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

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
	public Dropdown Dropdown1;
	public Dropdown Dropdown2;
	public Dropdown Dropdown3;
	public Dropdown Dropdown4;
	public Dropdown Dropdown5;
	public Dropdown Dropdown6;
	public Dropdown Dropdown7;
	public Dropdown Dropdown8;
	public Dropdown Dropdown9;
	public Dropdown Dropdown10;
	public Dropdown Dropdown11;
	public Dropdown Dropdown12;
	public Dropdown DropdownLevel1;
	public Dropdown DropdownLevel2;
	public Dropdown DropdownLevel3;
	public Dropdown DropdownLevel4;
	public Dropdown DropdownLevel5;
	public Dropdown DropdownLevel6;
	public Dropdown DropdownLevel7;
	public Dropdown DropdownLevel8;
	public Dropdown DropdownLevel9;
	public Dropdown DropdownLevel10;
	public Dropdown DropdownLevel11;
	public Dropdown DropdownLevel12;
	public GameObject ButtonRandom;
	public GameObject ButtonFixed;
	public GameObject ToggleMeta;
	// Where to find the values (FileName scene)
	public GameObject inputFileName;
	public GameObject inputArduinoPort;
	public GameObject errorMessageFileName;
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

	void Start(){
		inputNumberTrials.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            FieldValueChanged(inputNumberTrials.GetComponent<TMP_InputField>());
        	});
	}

	void FieldValueChanged(TMP_InputField inp)
    {
		var numTrials = 1;
		int.TryParse(inp.text, out numTrials);
		inp.text = Math.Min(12, numTrials).ToString();
		Sequence.Instance.StoreNumber();
    }
	
	public void ChangeParameters() {
		// Update "time (one trial)"
		int.TryParse(inputTime.GetComponent<TMP_InputField>().text, out stroopTrialTime);
		if (stroopTrialTime == 0){
			stroopTrialTime = 90;
		}
		Debug.Log("Trial time: " + stroopTrialTime);
		// Update "number of trials"
		int.TryParse(inputNumberTrials.GetComponent<TMP_InputField>().text, out stroopNumberTrials);
		if (stroopNumberTrials == 0){
			stroopNumberTrials = 1;
		}
		Debug.Log("Number of trials: " + stroopNumberTrials);
		// Update "sequence"
		var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsLevel = new[] { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6, DropdownLevel7, DropdownLevel8, DropdownLevel9, DropdownLevel10, DropdownLevel11, DropdownLevel12 };
		stroopSequence = new List<string>();
		stroopSequenceLevels = new List<int>();
		for (int i = 0; i < stroopNumberTrials; i++) {
			stroopSequence.Add(Dropdowns[i].options[Dropdowns[i].value].text);
			stroopSequenceLevels.Add(int.Parse(DropdownsLevel[i].options[DropdownsLevel[i].value].text));
		}
		Debug.Log("Sequence: " + String.Join(", ", stroopSequence.ToArray()));
		Debug.Log("Sequence levels: " + String.Join(", ", stroopSequenceLevels.Select(x => x.ToString()).ToArray()) );
		// Update "game mode"
		if (ButtonRandom.GetComponent<Toggle>().isOn == true){
			stroopGameMode = "Random";
		}
		else{
			stroopGameMode = "Fixed";
		}
		Debug.Log("Game mode: " + stroopGameMode);
		useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
	}

	public void ChangeFileNameAndPort() {
		// Update "file name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);
		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("Arduino port: " + arduinoPort);
		// Check if valid inputs
		// Mettre en commentaire ce qui suit si on utilise l'Arduino
		if ((fileName.Contains("/"))) {
			errorMessageFileName.SetActive(false);
			FileNameStroopPage.SetActive(false);
			OptionsStroopPage.SetActive(true);
		}
		else{
			errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename";
			errorMessageFileName.SetActive(true);
		}

		// Enlever commentaire si on utilise l'Arduino (et mettre le if en haut en commentaire)
		// try
		// {
		// 	Response.serialPort.Open();
		// 	if ((fileName.Contains("/"))) {
		// 		errorMessageFileName.SetActive(false);
		// 		FileNameStroopPage.SetActive(false);
		// 		OptionsStroopPage.SetActive(true);
		// 		Response.serialPort.Open();
		// 	}
		// 	else{
		// 		errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename";
		// 		errorMessageFileName.SetActive(true);
		// 	}
		// }
		// catch(IOException ioex)
		// {
		// 	errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename and port. \n IO Port Exception: " + ioex.Message;
		// 	errorMessageFileName.SetActive(true);
		// }
	}

	public void SaveParameters(){
		// Update the values of the parameters
		ChangeParameters();
		// Create a string that contains the name and value of all the parameters
		string[] parameters = {"AR Stroop Study Parameters", "Trial Time:" + stroopTrialTime.ToString(), "Number Trials:" + stroopNumberTrials.ToString(), "Sequence:" + String.Join(",", stroopSequence.ToArray()), "Sequence Levels:" + String.Join(",", stroopSequenceLevels.Select(x => x.ToString()).ToArray()), "Game Mode:" + stroopGameMode, "Use Meta:" + useMeta.ToString()};
		// Open the file explorer (to choose the path and name of the file)
		var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "parameters", "txt");
		// Write the file
		if (!string.IsNullOrEmpty(path)) {
            File.WriteAllText(path, string.Join("\n", parameters));
        }
	}

	public void LoadParameters(){
		// Open the file explorer (to select the file)
		var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
		// Read the file
		if (path.Length > 0) {
			// Read all the parameters
            string allParameters = File.ReadAllText(path[0]);
			string[] parameters = allParameters.Split('\n');
			// Load the time (one trial)"
			inputTime.GetComponent<TMP_InputField>().text = parameters[1].Split(':')[1];
			// Load the "number of trials" and show the right number of dropdowns
			int.TryParse(parameters[2].Split(':')[1], out stroopNumberTrials);
			inputNumberTrials.GetComponent<TMP_InputField>().text = stroopNumberTrials.ToString();
			Sequence.Instance.StoreNumber();
			// Load the "sequence" and "sequence levels"
			string[] seq = parameters[3].Split(':')[1].Split(',');
			string[] seqLevels = parameters[4].Split(':')[1].Split(',');
			var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
			var DropdownsLevel = new[] { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6, DropdownLevel7, DropdownLevel8, DropdownLevel9, DropdownLevel10, DropdownLevel11, DropdownLevel12 };
			for (int i = 0; i < seq.Length; i++) {
				Dropdowns[i].value = Dropdowns[i].options.FindIndex(option => option.text == seq[i]);
				DropdownsLevel[i].value = DropdownsLevel[i].options.FindIndex(option => option.text == seqLevels[i]);
			}
			// Load the "game mode"
			if (parameters[5].Split(':')[1] == "Random"){
				ButtonRandom.GetComponent<Toggle>().isOn = true;
			}
			else
			{
				ButtonRandom.GetComponent<Toggle>().isOn = false;
			}
			if (parameters[6].Split(':')[1] == "True"){
				ToggleMeta.GetComponent<Toggle>().isOn = true;
			}
			else
			{
				ToggleMeta.GetComponent<Toggle>().isOn = false;
			}
        }
	}
}
