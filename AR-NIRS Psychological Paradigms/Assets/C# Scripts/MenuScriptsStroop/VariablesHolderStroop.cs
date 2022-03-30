using System;
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
	public static string gameMode; // "Random" or "Fixed"
	public static int trialTime; // Trial time in seconds
	public static int numberTrials;
	public static List<string> sequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
	public static List<int> sequenceLevels = new List<int>();
	public static string arduinoPort = "COM3";
	public static string fileName;
	public static string fixedFile = "Empty";
	// Where to find the values (Options scene)
	public GameObject inputTime;
	public GameObject inputNumberTrials;
	public Dropdown[] Dropdowns;
	public Dropdown[] DropdownsLevel;
	public GameObject ButtonRandom;
	public GameObject ButtonFixed;
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

	// Add listeners to modifiy the values of the input UI in real time
	void Start()
	{
		inputTime.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            TimeValueChanged(inputTime.GetComponent<TMP_InputField>());
        	});
		inputNumberTrials.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            NumTrialsValueChanged(inputNumberTrials.GetComponent<TMP_InputField>());
        	});
		ButtonFixed.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            ButtonFixedValueChanged(ButtonFixed.GetComponent<Toggle>(), ButtonLoadFixed);
        	});
	}

	// On deselect, if the value of "Time" is smaller than 1 (min), change it to 1
	void TimeValueChanged(TMP_InputField inp)
    {
		int time = int.Parse(inp.text);
		inp.text = Math.Max(1, time).ToString();
    }

	// On deselect, display the number of dropdowns according to the number of trials
	// If the value of "Number of Trials" is smaller than 1 (min), change it to 1
	// If the value is larger than 12 (max), change it to 12
	void NumTrialsValueChanged(TMP_InputField inp)
    {
		int numTrials = int.Parse(inp.text);
		numTrials = Math.Max(1, numTrials);
		inp.text = Math.Min(12, numTrials).ToString();
		Sequence.Instance.StoreNumber();
    }

	// If the Fixed button is selected, the button "Load Fixed File" is disabled
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
		int.TryParse(inputTime.GetComponent<TMP_InputField>().text, out trialTime);
		if (trialTime == 0)
		{
			trialTime = 90;
		}
		Debug.Log("Trial time: " + trialTime);

		// Update "number of trials"
		int.TryParse(inputNumberTrials.GetComponent<TMP_InputField>().text, out numberTrials);
		if (numberTrials == 0)
		{
			numberTrials = 1;
		}
		Debug.Log("Number of trials: " + numberTrials);

		// Update "sequence"
        // Control is done first
        sequence.Add("Control");
        sequenceLevels.Add(0);
        for (int i = 0; i < numberTrials; i++)
		{
			sequence.Add(Dropdowns[i].options[Dropdowns[i].value].text);
			sequenceLevels.Add(int.Parse(DropdownsLevel[i].options[DropdownsLevel[i].value].text));
		}
		Debug.Log("Sequence: " + String.Join(", ", sequence.ToArray()));
		Debug.Log("Sequence levels: " + String.Join(", ", sequenceLevels.Select(x => x.ToString()).ToArray()));

		// Update "game mode"
		if (ButtonRandom.GetComponent<Toggle>().isOn == true)
		{
			gameMode = "Random";
		}
		else
		{
			gameMode = "Fixed";
		}
		Debug.Log("Game mode: " + gameMode);
	}

	public void ChangeFileNameAndPort() 
	{
		// Update "File name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);

		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
		arduinoPort = Regex.Replace(arduinoPort, "[^A-Za-z0-9 -]", "");
		Debug.Log("Arduino port: " + arduinoPort);

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
        //         errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename. Read the manual for more information.";
        //         errorMessageFileName.SetActive(true);
        //     }
        // }
        // catch (IOException ioex)
        // {
        //     errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid port. Read the manual for more information. \n IO Port Exception: " + ioex.Message;
        //     errorMessageFileName.SetActive(true);
        // }
    }

	public void SaveParameters()
	{
		// Update the values of the parameters
		ChangeParameters();

		// Remove the Control from the sequence (will be added later)
		List<string> seqNoControl = new List<string>(sequence);
		seqNoControl.RemoveAt(0);
		List<int> seqLNoControl = new List<int>(sequenceLevels);
		seqLNoControl.RemoveAt(0);

		// Create a string that contains the name and value of all the parameters
		string[] parameters = {"AR Stroop Study Parameters", "Trial Time:" + trialTime.ToString(), "Number Trials:" + numberTrials.ToString(), "Sequence:" + String.Join(",", seqNoControl.ToArray()), "Sequence Levels:" + String.Join(",", seqLNoControl.Select(x => x.ToString()).ToArray()), "Game Mode:" + gameMode};
		
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
		try
		{
			// Open the file explorer (to select the file)
			var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
			
			// Read the file
			var possibleParameters = File.ReadAllText(path[0]);
			if (CheckValidFileParameters(possibleParameters))
			{
				
				// Read all the parameters
				string[] parameters = possibleParameters.Split('\n');
				// Load the "time (one trial)"
				inputTime.GetComponent<TMP_InputField>().text = parameters[1].Split(':')[1];
				// Load the "number of trials" and show the right number of dropdowns
				int.TryParse(parameters[2].Split(':')[1], out numberTrials);
				inputNumberTrials.GetComponent<TMP_InputField>().text = numberTrials.ToString();
				Sequence.Instance.StoreNumber();
				// Load the "sequence" and "sequence levels"
				string[] seq = parameters[3].Split(':')[1].Split(',');
				string[] seqLevels = parameters[4].Split(':')[1].Split(',');
				for (int i = 0; i < seq.Length; i++) {
					Dropdowns[i].value = Dropdowns[i].options.FindIndex(option => option.text == seq[i]);
					DropdownsLevel[i].value = DropdownsLevel[i].options.FindIndex(option => option.text == seqLevels[i]);
				}
				// Load the "game mode"
				if (parameters[5].Split(':')[1] == "Random")
				{
					ButtonRandom.GetComponent<Toggle>().isOn = true;
				}
				else
				{
					ButtonRandom.GetComponent<Toggle>().isOn = false;
				}
			}
			else
			{
				errorText.GetComponent<Text>().text = "Error: The parameters file is not valid. Read the instruction manual for more information.";
				errorText.SetActive(true);
			}
        }
		catch
		{
			errorText.GetComponent<Text>().text = "Error: Please select a .txt file.";
			errorText.SetActive(true);
		}
	}

	public void SelectFixedFile()
	{
		try
		{
			var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false)[0];
			Debug.Log("Fixed file: " + path);
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
		catch
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
			(Regex.Replace(lines[0], @"\s", "") == "Level 1") && 
			(Regex.Replace(lines[2], @"\s", "") == "Level 2") && 
			(Regex.Replace(lines[4], @"\s", "") == "Level 3") && 
			(Regex.Replace(lines[6], @"\s", "") == "Level 4") &&
			(lines[1].Count(c => (c == ';')) * 2 + 3 == Regex.Replace(lines[1], @"\s", "").Count()) &&
			(lines[3].Count(c => (c == ';')) * 2 + 3 == Regex.Replace(lines[3], @"\s", "").Count()) &&
			(lines[5].Count(c => (c == ';')) * 4 + 3 == lines[5].Count(c => (c == ',')) * 4 + 3) && 
			(lines[5].Count(c => (c == ',')) * 4 + 3 == Regex.Replace(lines[5], @"\s", "").Count()) &&
			(lines[7].Count(c => (c == ';')) * 6 + 3 == (lines[7].Count(c => (c == ','))) * 6 / 2 + 3) && 
			((lines[7].Count(c => (c == ','))) * 6 / 2 + 3 == Regex.Replace(lines[7], @"\s", "").Count()) &&
			(fixedFile.All(c => "Level01234RGB;,END\n ".Contains(c)))
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
			(lines.Count() == 6) && 
			(Regex.Replace(lines[0], @"\s", "") == "ARStroopStudyParameters") && 
			(lines[1].Split(':')[0] == "Trial Time") &&
			(Convert.ToInt32(lines[1].Split(':')[1]) > 0) &&
			(lines[2].Split(':')[0] == "Number Trials") &&
			(Convert.ToInt32(lines[2].Split(':')[1]) > 0) &&
			(Convert.ToInt32(lines[2].Split(':')[1]) < 13) &&
			(lines[3].Split(':')[0] == "Sequence") &&
			(lines[3].Split(':')[1].Split(',').Count() == Convert.ToInt32(lines[2].Split(':')[1])) &&
			(lines[4].Split(':')[0] == "Sequence Levels") &&
			(lines[4].Split(':')[1].Split(',').Count() == Convert.ToInt32(lines[2].Split(':')[1])) &&
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
