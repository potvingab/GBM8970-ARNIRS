using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SFB;
using TMPro;
using System.Text.RegularExpressions;

public class VariablesHolder : MonoBehaviour {
    public static float speed;
    public static float GameSpeed; // pause or not
    public static string arduinoPort = "COM3";
	public static string fileName;
    public static string gameMode; 
    public static bool useMeta;
    public static bool useVisual;
    public static bool useAudio;
    public static float audioVolume;
    public static int numberOfObjects;
    public static List<string> sequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
	public static List<int> sequenceNBack = new List<int>(); // N of each N-back
    public static int numberTrials;
    public static string fixedFile = "Empty";

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
    public GameObject AudioVolume;
	public GameObject AudioVolumeField;
    public GameObject inputNumObjects;
    public GameObject ButtonLoadFixed;
	public GameObject checkSaved;
	public GameObject checkFixed;
	public GameObject errorText;
    public GameObject inputNumberTrials;
    public GameObject inputSpeed;
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
	public Dropdown DropdownNBack1;
	public Dropdown DropdownNBack2;
	public Dropdown DropdownNBack3;
	public Dropdown DropdownNBack4;
	public Dropdown DropdownNBack5;
	public Dropdown DropdownNBack6;
	public Dropdown DropdownNBack7;
	public Dropdown DropdownNBack8;
	public Dropdown DropdownNBack9;
	public Dropdown DropdownNBack10;
	public Dropdown DropdownNBack11;
	public Dropdown DropdownNBack12;
	public GameObject disableVolume;
	public GameObject disableObjects;

    void Start()
    {
        speed = 1;
        GameSpeed = 0;
        inputNumObjects.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            FieldValueChangedNum(inputNumObjects.GetComponent<TMP_InputField>());
        	});
        ButtonFixed.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            ButtonFixedValueChanged(ButtonFixed.GetComponent<Toggle>(), ButtonLoadFixed);
        	});
        inputNumberTrials.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            FieldValueChangedTrials(inputNumberTrials.GetComponent<TMP_InputField>());
        	});
        inputSpeed.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            FieldValueChangedSpeed(inputSpeed.GetComponent<TMP_InputField>());
        	});
		ToggleAudio.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            ButtonAudioValueChanged(ToggleAudio.GetComponent<Toggle>());
        	});
		AudioVolume.GetComponent<Slider>().onValueChanged.AddListener(delegate {
            SliderValueChanged(AudioVolume.GetComponent<Slider>());
        	});
		AudioVolumeField.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            SliderFieldValueChanged(AudioVolumeField.GetComponent<TMP_InputField>());
        	});
    }

    void FieldValueChangedNum(TMP_InputField inp)
    {
		var numOb = 1;
		int.TryParse(inp.text, out numOb);
		inp.text = Math.Min(15, numOb).ToString();
    }

    void FieldValueChangedTrials(TMP_InputField inp)
    {
		var numTrials = 1;
		int.TryParse(inp.text, out numTrials);
		inp.text = Math.Min(12, numTrials).ToString();
		SequenceNBack.Instance.StoreNumber();
    }

    void FieldValueChangedSpeed(TMP_InputField inp)
    {
		float sp;
		float.TryParse(inp.text, out sp);
		inp.text = Math.Max(0.01, sp).ToString();
        if (!inp.text.Contains("."))
        {
            inp.text += ".00";
        }
        else{
            if (inp.text.Split('.')[1].Length < 2){
                inp.text += "0";
            }
        }
    }

	void ButtonFixedValueChanged(Toggle tog, GameObject loadFixed)
	{
		if (tog.isOn)
		{
			loadFixed.GetComponent<Button>().interactable = true;
			disableObjects.SetActive(true);
			//If fixed and no file -> 15 objects
			inputNumObjects.GetComponent<TMP_InputField>().text = "15";
		}
		else
		{
			loadFixed.GetComponent<Button>().interactable = false;
			disableObjects.SetActive(false);
		}
	}

	void ButtonAudioValueChanged(Toggle tog)
	{
		if (tog.isOn)
		{
			disableVolume.SetActive(false);
		}
		else
		{
			disableVolume.SetActive(true);
		}
	}

	// Volume slider
	void SliderValueChanged(Slider slid)
	{
		Debug.Log(slid.value.ToString());
		AudioVolumeField.GetComponent<TMP_InputField>().text = slid.value.ToString();
	}
	void SliderFieldValueChanged(TMP_InputField inp)
	{
		Debug.Log(inp.text);
		var val =  Math.Min(1, float.Parse(inp.text));
		val =  Math.Max(0, float.Parse(inp.text));
		inp.text = val.ToString();
		AudioVolume.GetComponent<Slider>().value = val;
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ChangeParameters()
    {
        useVisual = ToggleVisual.GetComponent<Toggle>().isOn;
        useAudio = ToggleAudio.GetComponent<Toggle>().isOn;
        audioVolume = AudioVolume.GetComponent<Slider>().value;
        AudioListener.volume = audioVolume;
        Debug.Log("Use Visual: " + useVisual);
        Debug.Log("Use Audio: " + useAudio);
        Debug.Log("Audio Volume: " + audioVolume);
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
        //TimeSpawner.CreateCheckpoint("EndOfMenu");
        useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
        // Update "number of trials"
		int.TryParse(inputNumberTrials.GetComponent<TMP_InputField>().text, out numberTrials);
		if (numberTrials == 0)
		{
			numberTrials = 1;
		}
        Debug.Log("Number trials: " + numberTrials);
        // Update "number objects (one trial)"
		int.TryParse(inputNumObjects.GetComponent<TMP_InputField>().text, out numberOfObjects);
		if (numberOfObjects == 0)
		{
			numberOfObjects = 15;
		}
        Debug.Log("Number objects: " + numberOfObjects);
        // Update "speed"
        float.TryParse(inputSpeed.GetComponent<TMP_InputField>().text, out speed);
        if (speed == 0)
		{
			speed = 1;
		}
        Debug.Log("Speed: " + speed);
        // Update "sequence"
		var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsNBack = new[] { DropdownNBack1, DropdownNBack2, DropdownNBack3, DropdownNBack4, DropdownNBack5, DropdownNBack6, DropdownNBack7, DropdownNBack8, DropdownNBack9, DropdownNBack10, DropdownNBack11, DropdownNBack12 };
		sequence = new List<string>();
		sequenceNBack = new List<int>();
        for (int i = 0; i < numberTrials; i++)
		{
			sequence.Add(Dropdowns[i].options[Dropdowns[i].value].text);
			sequenceNBack.Add(int.Parse(DropdownsNBack[i].options[DropdownsNBack[i].value].text));
		}
        Debug.Log("Sequence: " + String.Join(", ", sequence.ToArray()));
		Debug.Log("Sequence N-Back: " + String.Join(", ", sequenceNBack.Select(x => x.ToString()).ToArray()) );
        Debug.Log("Chosen Objects: " + String.Join(", ", BoolArrayHolder.assetsChecks.Select(x => x.ToString()).ToArray()));
    }

    public void ChangeFileNameAndPort() {
		// Update "file name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);
		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
        arduinoPort = Regex.Replace(arduinoPort, "[^A-Za-z0-9 -]", "");
        Debug.Log("Arduino port: " + arduinoPort);
        TimeSpawner.TriggerArduino("C");
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

    public void SaveParameters()
	{
        // Update the values of the parameters
		ChangeParameters();
		// Create a string that contains the name and value of all the parameters
		string[] parameters = {"AR N-Back Study Parameters", "Number of objects:" + numberOfObjects.ToString(), "Number Trials:" + numberTrials.ToString(), "Sequence:" + String.Join(",", sequence.ToArray()), "Sequence N:" + String.Join(",", sequenceNBack.Select(x => x.ToString()).ToArray()), "Game Mode:" + gameMode, "Speed:" + speed.ToString(), "Use Visual:" + useVisual.ToString(), "Use Audio:" + useAudio.ToString(), "Chosen Objects:" + String.Join(",", BoolArrayHolder.assetsChecks.Select(x => x.ToString()).ToArray()), "Volume:" + audioVolume.ToString()};
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
				// Load the "number of objects (one trial)"
				inputNumObjects.GetComponent<TMP_InputField>().text = parameters[1].Split(':')[1];
				// Load the "number of trials" and show the right number of dropdowns
				int.TryParse(parameters[2].Split(':')[1], out numberTrials);
				inputNumberTrials.GetComponent<TMP_InputField>().text = numberTrials.ToString();
				SequenceNBack.Instance.StoreNumber();
				// Load the "sequence" and "sequence N"
				string[] seq = parameters[3].Split(':')[1].Split(',');
				string[] seqNBack = parameters[4].Split(':')[1].Split(',');
				var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
				var DropdownsNBack = new[] { DropdownNBack1, DropdownNBack2, DropdownNBack3, DropdownNBack4, DropdownNBack5, DropdownNBack6, DropdownNBack7, DropdownNBack8, DropdownNBack9, DropdownNBack10, DropdownNBack11, DropdownNBack12 };
				for (int i = 0; i < seq.Length; i++) {
					Dropdowns[i].value = Dropdowns[i].options.FindIndex(option => option.text == seq[i]);
					DropdownsNBack[i].value = DropdownsNBack[i].options.FindIndex(option => option.text == seqNBack[i]);
				}
				// Load the "game mode"
				if (parameters[5].Split(':')[1] == "Random"){
					ButtonRandom.GetComponent<Toggle>().isOn = true;
				}
				else
				{
					ButtonRandom.GetComponent<Toggle>().isOn = false;
				}
				// Load the "speed"
				inputSpeed.GetComponent<TMP_InputField>().text = parameters[6].Split(':')[1];
				// Load "use visual"
				if (parameters[7].Split(':')[1] == "True"){
					ToggleVisual.GetComponent<Toggle>().isOn = true;
				}
				else
				{
					ToggleVisual.GetComponent<Toggle>().isOn = false;
				}
				// Load "use audio"
				if (parameters[8].Split(':')[1] == "True"){
					ToggleAudio.GetComponent<Toggle>().isOn = true;
				}
				else
				{
					ToggleAudio.GetComponent<Toggle>().isOn = false;
				}
				// Load "chosen objects"
				BoolArrayHolder.assetsChecks = parameters[9].Split(':')[1].Split(',').Select(s => s == "True").ToArray();
				// Load "volume"
				AudioVolume.GetComponent<Slider>().value = float.Parse(parameters[10].Split(':')[1]);
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
				errorText.GetComponent<Text>().text = "Error: The fixed sequence file is not valid. Read the instruction manual for more information.";
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
        bool success = true;
        try
        {
            string[] lines = fixedFile.Split('\n');
            string[] firstRowcols = fixedFile.Split('\n')[0].Split(';');
                       
            int numberOfElements = Convert.ToInt32(firstRowcols[1]);
            Debug.Log(numberOfElements);
			inputNumObjects.GetComponent<TMP_InputField>().text = numberOfElements.ToString();
            int numberOfLines = lines.Length;
            for (int line = 1; line < numberOfLines; line++)
            {
                string[] col = lines[line].Split(';');
                
                if (lines[line].Count(c => (c == ';')) != numberOfElements + 1 || col[numberOfElements + 1].Contains("END") == false ||
                    col[0].Contains("Level") == false || col[0].Contains(line.ToString()) == false)
                {
                    success = false;
                }
            }
        }
        catch
        {
            success = false;
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
			(lines.Count() == 11) && 
			(Regex.Replace(lines[0], @"\s", "") == "ARN-BackStudyParameters") && 
			(lines[1].Split(':')[0] == "Number of objects") &&
			(Convert.ToInt32(lines[1].Split(':')[1]) > 0) &&
			(lines[2].Split(':')[0] == "Number Trials") &&
			(Convert.ToInt32(lines[2].Split(':')[1]) > 0) &&
			(Convert.ToInt32(lines[2].Split(':')[1]) < 13) &&
			(lines[3].Split(':')[0] == "Sequence") &&
			(lines[3].Split(':')[1].Split(',').Count() == Convert.ToInt32(lines[2].Split(':')[1])) &&
			(lines[4].Split(':')[0] == "Sequence N") &&
			(lines[4].Split(':')[1].Split(',').Count() == Convert.ToInt32(lines[2].Split(':')[1])) &&
			(lines[5].Split(':')[0] == "Game Mode") &&
			((lines[5].Split(':')[1] == "Random") || (lines[5].Split(':')[1] == "Fixed")) &&
			(lines[6].Split(':')[0] == "Speed") &&
			(float.Parse(lines[6].Split(':')[1]) > 0) &&
			(lines[7].Split(':')[0] == "Use Visual") &&
			((lines[7].Split(':')[1] == "True") || (lines[7].Split(':')[1] == "False")) &&
			(lines[8].Split(':')[0] == "Use Audio") &&
			((lines[8].Split(':')[1] == "True") || (lines[8].Split(':')[1] == "False")) &&
			(lines[9].Split(':')[0] == "Chosen Objects") &&
			(lines[9].Split(':')[1].Split(',').Count() == 9) &&
			(lines[10].Split(':')[0] == "Volume") &&
			((float.Parse(lines[10].Split(':')[1]) >= 0) && (float.Parse(lines[10].Split(':')[1]) <= 1))
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