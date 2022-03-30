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
    // Values to store (with default values)
    public static float speed;
    public static float GameSpeed; // Pause or not
    public static string arduinoPort = "COM3";
	public static string fileName;
    public static string gameMode; 
    public static bool useVisual;
    public static bool useAudio;
    public static float audioVolume;
    public static int numberOfObjects;
    public static List<string> sequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
	public static List<int> sequenceNBack = new List<int>(); // N of each N-back
    public static int numberTrials;
    public static string fixedFile = "Empty";
    public static int numberOfTutorial;
    public static int sizeOfArray;
    public static bool errorInMenu = false;
    // Where to find the values (Options scene)
    public GameObject inputFileName;
	public GameObject inputArduinoPort;
	public GameObject errorMessageFileName;
    public GameObject FileNameNBackPage;
	public GameObject OptionsNBackPage;
    public GameObject ButtonRandom;
    public GameObject ButtonFixed;
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
	public Dropdown[] Dropdowns;
	public Dropdown[] DropdownsNBack;
	public GameObject disableVolume;
	public GameObject disableObjects;

    // Add listeners to modifiy the values of the input UI in real time
    void Start()
    {
        speed = 1;
        GameSpeed = 0;
        inputNumObjects.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            NumValueChanged(inputNumObjects.GetComponent<TMP_InputField>());
        	});
        ButtonFixed.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            ButtonFixedValueChanged(ButtonFixed.GetComponent<Toggle>(), ButtonLoadFixed);
        	});
        inputNumberTrials.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            TrialsValueChanged(inputNumberTrials.GetComponent<TMP_InputField>());
        	});
        inputSpeed.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate {
            SpeedValueChanged(inputSpeed.GetComponent<TMP_InputField>());
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

    // On deselect, if the value of "Number of Objectfs" is smaller than 2 (min), change it to 2
    void NumValueChanged(TMP_InputField inp)
    {
		int numOb = int.Parse(inp.text);
		inp.text = Math.Max(2, numOb).ToString();
    }

    // On deselect, display the number of dropdowns according to the number of trials
	// If the value of "Number of Trials" is smaller than 1 (min), change it to 1
	// If the value is larger than 12 (max), change it to 12
    void TrialsValueChanged(TMP_InputField inp)
    {
		int numTrials = int.Parse(inp.text);
        numTrials = Math.Max(1, numTrials);
		inp.text = Math.Min(12, numTrials).ToString();
		SequenceNBack.Instance.StoreNumber();
    }

    // On deselect, if the value of "Speed" is smaller than 0.01 (min), change it to 0.01
    // Also add decimals if there are not (.00)
    void SpeedValueChanged(TMP_InputField inp)
    {
		float sp = float.Parse(inp.text);
		inp.text = Math.Max(0.01, sp).ToString();
        if (!inp.text.Contains("."))
        {
            inp.text += ".00";
        }
        else
		{
            if (inp.text.Split('.')[1].Length < 2)
			{
                inp.text += "0";
            }
        }
    }

    // If the Fixed button is selected, the button "Load Fixed File" is disabled
	void ButtonFixedValueChanged(Toggle tog, GameObject loadFixed)
	{
		if (tog.isOn)
		{
			loadFixed.GetComponent<Button>().interactable = true;
            disableObjects.SetActive(true);
            string allFile;
            int numberTrialFile;
            try
            {
                // If fixed and no file -> read file by default
                if (fixedFile == "Empty")
                {
                    TextAsset txt = (TextAsset)Resources.Load("FixedSequenceNBack", typeof(TextAsset));
                    allFile = txt.text;
                }
                else
                {
                    allFile = VariablesHolder.fixedFile;
                }
                numberOfObjects = Convert.ToInt16((allFile).Split('\n')[0].Split(';')[1]);
                inputNumObjects.GetComponent<TMP_InputField>().text = (allFile).Split('\n')[0].Split(';')[1];
                numberTrialFile = allFile.Split('\n').Length - Convert.ToInt16((allFile).Split('\n')[0].Split(';')[3])-1;
                errorText.GetComponent<Text>().text = "Warning: According to the fixed sequence, there should be a maximum " + numberTrialFile + " N-back levels. ";
                errorText.SetActive(true);
            }
            catch {
                errorText.GetComponent<Text>().text = "Error: The parameters are not valid. Read the instruction manual for more information.";
                errorText.SetActive(true);
            }
        }
		else
		{
			loadFixed.GetComponent<Button>().interactable = false;
			disableObjects.SetActive(false);
		}
	}

    // If the "Use Audio" toggle is not selected, the volume slider is disabled
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

	// Synchronize the volume slider and input field
	void SliderValueChanged(Slider slid)
	{
		AudioVolumeField.GetComponent<TMP_InputField>().text = slid.value.ToString("f1");
	}
	void SliderFieldValueChanged(TMP_InputField inp)
	{
		var val =  Math.Min(100, float.Parse(inp.text));
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
        try
        {
            string allFile = "EmptyForNow";
            useVisual = ToggleVisual.GetComponent<Toggle>().isOn;
            useAudio = ToggleAudio.GetComponent<Toggle>().isOn;
            audioVolume = AudioVolume.GetComponent<Slider>().value;
            AudioListener.volume = audioVolume / 100;
            Debug.Log("Use Visual: " + useVisual);
            Debug.Log("Use Audio: " + useAudio);
            Debug.Log("Audio Volume: " + audioVolume);
         
            //TimeSpawner.CreateCheckpoint("EndOfMenu");
            // Update "number of trials"
            int.TryParse(inputNumberTrials.GetComponent<TMP_InputField>().text, out numberTrials);
            if (numberTrials == 0)
            {
                numberTrials = 1;
            }

            //Debug.Log("Chosen Objects: " + String.Join(", ", BoolArrayHolder.assetsChecks.Select(x => x.ToString()).ToArray()));
            // Update "game mode"
            if (ButtonRandom.GetComponent<Toggle>().isOn == true)
            {
                gameMode = "Random";
                numberOfTutorial = 7; //Default value
            }
            else
            {
                gameMode = "Fixed";

                //If fixed and no file -> read file par default
                if (fixedFile == "Empty")
                {
                    TextAsset txt = (TextAsset)Resources.Load("FixedSequenceNBack", typeof(TextAsset));
                    allFile = txt.text;
                }
                else
                {
                    allFile = fixedFile;
                }
                numberOfTutorial = Convert.ToInt16((allFile).Split('\n')[0].Split(';')[3]);
            }
            Debug.Log("Game mode: " + gameMode);
            Debug.Log("Number trials: " + numberTrials);
            Debug.Log("Number trials: " + numberOfTutorial);
            Debug.Log("Number trials: " + sizeOfArray);
            sizeOfArray = numberTrials + numberOfTutorial;
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
            sequence = new List<string>();
            sequenceNBack = new List<int>();

            int j = 0;
            while (Dropdowns[j].options[Dropdowns[j].value].text.Contains("Single Task (Walk)") && j < numberTrials)
            {
                j++; 
            }

            for (int i = 0; i < numberOfTutorial; i++)
            {
                sequenceNBack.Add(int.Parse(DropdownsNBack[j].options[DropdownsNBack[j].value].text));
                sequence.Add("Tutorial");
            }
            for (int i = 0; i < numberTrials; i++)
            {
                sequence.Add(Dropdowns[i].options[Dropdowns[i].value].text);
                sequenceNBack.Add(int.Parse(DropdownsNBack[i].options[DropdownsNBack[i].value].text));
            }
            //Debug.Log("Sequence: " + String.Join(", ", sequence.ToArray()));
            Debug.Log("Sequence N-Back: ");

            int numberOfSingleWalk = 0;
            Debug.Log("NumberOftrial: " + numberTrials);
            for (int i = 0; i < sizeOfArray; i++)
            {
                
                if (sequence[i].Contains("Single Task (Walk)"))
                {
                    numberOfSingleWalk++;
                }
            }
            errorInMenu = false;

            if (gameMode == "Fixed")
            {
                if (numberTrials - numberOfSingleWalk > allFile.Split('\n').Length - numberOfTutorial - 1)
                {
                    //errorInMenu = true;
                    throw new Exception();
                }
            }
        }
        catch
        {
            errorText.GetComponent<Text>().text = "Error: The parameters are not valid. Read the instruction manual for more information.";
            errorText.SetActive(true);
            errorInMenu = true;
        }
    }

 	public void ChangeFileNameAndPort() 
	{
		// Update "File name"
		fileName = inputFileName.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("File name: " + fileName);

		// Update "Arduino port"
		arduinoPort = inputArduinoPort.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log("Arduino port: " + arduinoPort);

        // Check if valid inputs
        // Mettre en commentaire ce qui suit si on utilise l'Arduino
        //if ((fileName.Contains("/")))
        //{
        //   errorMessageFileName.SetActive(false);
        //   FileNameStroopPage.SetActive(false);
        //   OptionsStroopPage.SetActive(true);
        //}
        //else
        //{
        //   errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename";
        //   errorMessageFileName.SetActive(true);
        //}

        // Enlever commentaire si on utilise l'Arduino (et mettre le if en haut en commentaire)
        try
        {
            if (!Response.serialPort.IsOpen)
                Response.serialPort.Open();
            if ((fileName.Contains("/")))
            {
                errorMessageFileName.SetActive(false);
                FileNameNBackPage.SetActive(false);
                OptionsNBackPage.SetActive(true);
            }
            else
            {
                errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid filename. Read the manual for more information.";
                errorMessageFileName.SetActive(true);
            }
        }
        catch (IOException ioex)
        {
            errorMessageFileName.GetComponent<Text>().text = "Error: Please choose a valid port. Read the manual for more information. \n IO Port Exception: " + ioex.Message;
            errorMessageFileName.SetActive(true);
        }
    }

    public void SaveParameters()
	{
        // Update the values of the parameters
		ChangeParameters();
		// Remove the Tutorials from the sequence (will be added later)
        List<string> seqNoTuto = new List<string>(sequence);
        var numTuto = seqNoTuto.RemoveAll(x => x == "Tutorial");
        List<int> seqNNoTuto = new List<int>(sequenceNBack);
        seqNNoTuto.RemoveRange(0, numTuto);
        // Create a string that contains the name and value of all the parameters
		string[] parameters = {"AR N-Back Study Parameters", "Number of objects:" + numberOfObjects.ToString(), "Number Trials:" + numberTrials.ToString(), "Sequence:" + String.Join(",", seqNoTuto.ToArray()), "Sequence N:" + String.Join(",", seqNNoTuto.Select(x => x.ToString()).ToArray()), "Game Mode:" + gameMode, "Speed:" + speed.ToString(), "Use Visual:" + useVisual.ToString(), "Use Audio:" + useAudio.ToString(), "Chosen Objects:" + String.Join(",", BoolArrayHolder.assetsChecks.Select(x => x.ToString()).ToArray()), "Volume:" + audioVolume.ToString()};
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
				for (int i = 0; i < seq.Length; i++) {
					Dropdowns[i].value = Dropdowns[i].options.FindIndex(option => option.text == seq[i]);
					DropdownsNBack[i].value = DropdownsNBack[i].options.FindIndex(option => option.text == seqNBack[i]);
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
				// Load the "speed"
				inputSpeed.GetComponent<TMP_InputField>().text = parameters[6].Split(':')[1];
				// Load "use visual"
				if (parameters[7].Split(':')[1] == "True")
                {
					ToggleVisual.GetComponent<Toggle>().isOn = true;
				}
				else
				{
					ToggleVisual.GetComponent<Toggle>().isOn = false;
				}
				// Load "use audio"
				if (parameters[8].Split(':')[1] == "True")
                {
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
                //errorInMenu = false;
            }
			else
			{
				errorText.GetComponent<Text>().text = "Error: The parameters file is not valid. Read the instruction manual for more information.";
				errorText.SetActive(true);
                //errorInMenu = true;
            }
        }
		catch
		{
			errorText.GetComponent<Text>().text = "Error: Please select a .txt file.";
			errorText.SetActive(true);
            //errorInMenu = true;
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
                numberOfObjects = Convert.ToInt16((fixedFile).Split('\n')[0].Split(';')[1]);
                inputNumObjects.GetComponent<TMP_InputField>().text = (fixedFile).Split('\n')[0].Split(';')[1];

                errorText.GetComponent<Text>().text = "Warning: According to the fixed sequence, there should be a maximum " + +(fixedFile.Split('\n').Length - Convert.ToInt16((fixedFile).Split('\n')[0].Split(';')[3]) - 1) + " N-back levels. "; 
                errorText.SetActive(true);
                //errorInMenu = false;
            }
			else
			{
                checkFixed.SetActive(false);
				errorText.GetComponent<Text>().text = "Error: The fixed sequence file is not valid. Read the instruction manual for more information.";
				errorText.SetActive(true);
               //errorInMenu = true;
            }
        }
        catch
		{
			checkFixed.SetActive(false);
			errorText.GetComponent<Text>().text = "Error: Please select a .txt file.";
			errorText.SetActive(true);
            //errorInMenu = true;
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
            int numberofTutorialFile = Convert.ToInt32(firstRowcols[3]);
            //int numberOfLevel = lines.Length - numberofTutorialFile;

            //Debug.Log(numberOfElements);
            //inputNumObjects.GetComponent<TMP_InputField>().text = numberOfElements.ToString();
            //int numberOfLines = lines.Length;
            for (int line = 1; line < lines.Length; line++)
            {
                string[] col = lines[line].Split(';');

                if (col[1].Contains("walk") == false && (lines[line].Count(c => (c == ';')) != numberOfElements + 1 || col[numberOfElements + 1].Contains("END") == false))
                {
                    
                    if (line < numberofTutorialFile && (col[0].Contains(line.ToString()) == false || col[0].Contains("Tutorial") == false || col.All(c => "Tutorial1234567890;END\n".Contains(c))))
                    {
                        success = false;
                    }
                    else if (col[0].Contains((line + numberofTutorialFile).ToString()) == false || col[0].Contains("Level") == false || col.All(c => "Level1234567890;END\n".Contains(c)))
                    {
                        success = false;
                    }
                }
                Debug.Log(success);
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
			((float.Parse(lines[10].Split(':')[1]) >= 0) && (float.Parse(lines[10].Split(':')[1]) <= 100))
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