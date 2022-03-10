﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;
using System.Text.RegularExpressions;

public class VariablesHolderStroop : MonoBehaviour 
{
	// Values to store (with default values)
	public static string stroopGameMode; // "Random" or "Fixed"
	public static int stroopTrialTime; // Trial time in seconds
	public static int stroopNumberTrials;
	public static List<string> stroopSequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
	public static List<int> stroopSequenceLevels = new List<int>();
	public static string arduinoPort = "COM3";
	public static string fileName;
	public static bool useMeta;
	public static string fixedFile = "Empty";
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
	public GameObject ButtonLoadFixed;
	public GameObject checkSaved;
	public GameObject checkFixed;
	public GameObject errorText;
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

	void Awake()
	{
		paradigmChoicePage.SetActive(true);
		FileNameNBackPage.SetActive(false);
		FileNameStroopPage.SetActive(false);
		OptionsNBackPage.SetActive(false);
		OptionsStroopPage.SetActive(false);
		Options3DPage.SetActive(false);
	}

	void Start()
	{
		inputNumberTrials.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            FieldValueChanged(inputNumberTrials.GetComponent<TMP_InputField>());
        	});
		ButtonFixed.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            ButtonFixedValueChanged(ButtonFixed.GetComponent<Toggle>(), ButtonLoadFixed);
        	});
	}

	void FieldValueChanged(TMP_InputField inp)
    {
		var numTrials = 1;
		int.TryParse(inp.text, out numTrials);
		inp.text = Math.Min(12, numTrials).ToString();
		Sequence.Instance.StoreNumber();
    }

	void ButtonFixedValueChanged(Toggle tog, GameObject loadFixed)
	{
		if (tog.isOn)
		{
			loadFixed.GetComponent<Button>().interactable = true;
		}
		else
		{
			loadFixed.GetComponent<Button>().interactable = false;
		}
	}
	
	public void ChangeParameters() 
	{
		// Update "time (one trial)"
		int.TryParse(inputTime.GetComponent<TMP_InputField>().text, out stroopTrialTime);
		if (stroopTrialTime == 0)
		{
			stroopTrialTime = 90;
		}
		Debug.Log("Trial time: " + stroopTrialTime);
		// Update "number of trials"
		int.TryParse(inputNumberTrials.GetComponent<TMP_InputField>().text, out stroopNumberTrials);
		if (stroopNumberTrials == 0)
		{
			stroopNumberTrials = 1;
		}
		Debug.Log("Number of trials: " + stroopNumberTrials);
		// Update "sequence"
		var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsLevel = new[] { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6, DropdownLevel7, DropdownLevel8, DropdownLevel9, DropdownLevel10, DropdownLevel11, DropdownLevel12 };
		stroopSequence = new List<string>();
		stroopSequenceLevels = new List<int>();

        // Obligation de faire le baseline en premier
        stroopSequence.Add("Control");
        stroopSequenceLevels.Add(0);
        for (int i = 0; i < stroopNumberTrials; i++)
		{
			stroopSequence.Add(Dropdowns[i].options[Dropdowns[i].value].text);
			stroopSequenceLevels.Add(int.Parse(DropdownsLevel[i].options[DropdownsLevel[i].value].text));
		}
		Debug.Log("Sequence: " + String.Join(", ", stroopSequence.ToArray()));
		Debug.Log("Sequence levels: " + String.Join(", ", stroopSequenceLevels.Select(x => x.ToString()).ToArray()));
		// Update "game mode"
		if (ButtonRandom.GetComponent<Toggle>().isOn == true)
		{
			stroopGameMode = "Random";
		}
		else
		{
			stroopGameMode = "Fixed";
		}
		Debug.Log("Game mode: " + stroopGameMode);
		useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
	}

	public void ChangeFileNameAndPort() 
	{
		// Update "file name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);
		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("Arduino port: " + arduinoPort);
        Response.TriggerArduino("C");
        // Check if valid inputs
        // Mettre en commentaire ce qui suit si on utilise l'Arduino
        if ((fileName.Contains("/")))
        {
           errorMessageFileName.SetActive(false);
           FileNameStroopPage.SetActive(false);
           OptionsStroopPage.SetActive(true);
        }
        else
        {
           errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename";
           errorMessageFileName.SetActive(true);
        }

        // Enlever commentaire si on utilise l'Arduino (et mettre le if en haut en commentaire)
        // try
        // {
        //     if (!Response.serialPort.IsOpen)
        //         Response.serialPort.Open();
        //     if ((fileName.Contains("/")))
        //     {
        //         errorMessageFileName.SetActive(false);
        //         FileNameStroopPage.SetActive(false);
        //         OptionsStroopPage.SetActive(true);
        //     }
        //     else
        //     {
        //         errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename";
        //         errorMessageFileName.SetActive(true);
        //     }
        // }
        // catch (IOException ioex)
        // {
        //     errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename and port. \n IO Port Exception: " + ioex.Message;
        //     errorMessageFileName.SetActive(true);
        // }
    }

	public void SaveParameters()
	{
		// Update the values of the parameters
		ChangeParameters();
		// Create a string that contains the name and value of all the parameters
		string[] parameters = {"AR Stroop Study Parameters", "Trial Time:" + stroopTrialTime.ToString(), "Number Trials:" + stroopNumberTrials.ToString(), "Sequence:" + String.Join(",", stroopSequence.ToArray()), "Sequence Levels:" + String.Join(",", stroopSequenceLevels.Select(x => x.ToString()).ToArray()), "Game Mode:" + stroopGameMode, "Use Meta:" + useMeta.ToString()};
		// Open the file explorer (to choose the path and name of the file)
		var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "parameters", "txt");
		// Write the file
		if (!string.IsNullOrEmpty(path)) {
            File.WriteAllText(path, string.Join("\n", parameters));
			checkSaved.SetActive(true);
        }
		else
		{
			checkSaved.SetActive(false);
		}
	}

	public void LoadParameters()
	{
		// Open the file explorer (to select the file)
		var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
		// Read the file
		if (path.Length > 0)
		{
			var possibleParameters = File.ReadAllText(path[0]);
			if (CheckValidFileParameters(possibleParameters))
			{
				errorText.SetActive(false);
				// Read all the parameters
				string[] parameters = possibleParameters.Split('\n');
				// Load the "time (one trial)"
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
			else
			{
				errorText.GetComponent<Text>().text = "Error: The parameters file is not valid. Read the instruction manual for more information.";
				errorText.SetActive(true);
			}
        }
		else
		{
			errorText.GetComponent<Text>().text = "Error: Please select a .txt file.";
			errorText.SetActive(true);
		}
	}

	public void SelectFixedFile()
	{
		var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false)[0];
		Debug.Log("Fixed file: " + path);
		if (path.Length > 0)
		{
			var possibleFixedFile = File.ReadAllText(path);
			if (CheckValidFileFixed(possibleFixedFile))
			{
				fixedFile = possibleFixedFile;
				checkFixed.SetActive(true);
				errorText.SetActive(false);
			}
			else
			{
                checkFixed.SetActive(false);
				errorText.GetComponent<Text>().text = "Error: The fixed colors sequence file is not valid. Read the instruction manual for more information.";
				errorText.SetActive(true);
			}
		}
		else
		{
			checkFixed.SetActive(false);
			errorText.GetComponent<Text>().text = "Error: Please select a .txt file.";
			errorText.SetActive(true);
		}
	}

	public bool CheckValidFileFixed(string fixedFile)
	{
		bool success = false;
		string[] lines = fixedFile.Split('\n');
        if (
			(lines.Count() == 8) && 
			(Regex.Replace(lines[0], @"\s", "") == "Niveau1") && 
			(Regex.Replace(lines[2], @"\s", "") == "Niveau2") && 
			(Regex.Replace(lines[4], @"\s", "") == "Niveau3") && 
			(Regex.Replace(lines[6], @"\s", "") == "Niveau4") &&
			(lines[1].Count(c => (c == ';')) * 2 + 3 == Regex.Replace(lines[1], @"\s", "").Count()) &&
			(lines[3].Count(c => (c == ';')) * 2 + 3 == Regex.Replace(lines[3], @"\s", "").Count()) &&
			(lines[5].Count(c => (c == ';')) * 4 + 3 == lines[5].Count(c => (c == ',')) * 4 + 3) && 
			(lines[5].Count(c => (c == ',')) * 4 + 3 == Regex.Replace(lines[5], @"\s", "").Count()) &&
			(lines[7].Count(c => (c == ';')) * 6 + 3 == (lines[7].Count(c => (c == ','))) * 6 / 2 + 3) && 
			((lines[7].Count(c => (c == ','))) * 6 / 2 + 3 == Regex.Replace(lines[7], @"\s", "").Count()) &&
			(fixedFile.All(c => "Niveau01234RGB;,END\n ".Contains(c)))
			)
		{
			success = true;
		}
		return success;
	}

	public bool CheckValidFileParameters(string paramFile)
	{
		bool success = false;
		string[] lines = paramFile.Split('\n');
		try
		{
			if (
			(lines.Count() == 7) && 
			(Regex.Replace(lines[0], @"\s", "") == "ARStroopStudyParameters") && 
			(lines[1].Split(':')[0] == "Trial Time") &&
			(Convert.ToInt32(lines[1].Split(':')[1]) > 0) &&
			(lines[2].Split(':')[0] == "Number Trials") &&
			(Convert.ToInt32(lines[2].Split(':')[1]) > 0) &&
			(Convert.ToInt32(lines[2].Split(':')[1]) < 13) &&
			(lines[3].Split(':')[0] == "Sequence") &&
			(lines[3].Split(':')[1].Split(',').Count() == Convert.ToInt32(lines[2].Split(':')[1])+1) &&
			(lines[4].Split(':')[0] == "Sequence Levels") &&
			(lines[4].Split(':')[1].Split(',').Count() == Convert.ToInt32(lines[2].Split(':')[1])+1) &&
			(lines[5].Split(':')[0] == "Game Mode") &&
			((lines[5].Split(':')[1] == "Random") || (lines[5].Split(':')[1] == "Fixed"))
			)
			{
				success = true;
			}
		}
		catch
		{
			success = false;
		}
		return success;
	}
}
