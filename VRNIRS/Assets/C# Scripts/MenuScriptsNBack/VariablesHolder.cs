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
    public static int numberOfTutorial;
    public static int sizeOfArray;
   // public static string[] lines = fixedFile.Split('\n');

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
	public Dropdown[] Dropdowns;
	public Dropdown[] DropdownsNBack;
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
		var numOb = 2;
		int.TryParse(inp.text, out numOb);
		numOb = Math.Max(2, numOb);
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
        else
		{
            if (inp.text.Split('.')[1].Length < 2)
			{
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
            string allFile;
            int numberTrialFile;
            try
            {
                //If fixed and no file -> read file par default
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
                //numberOfTutorial = Convert.ToInt16((allFile).Split('\n')[0].Split(';')[3]);
                numberTrialFile = allFile.Split('\n').Length - Convert.ToInt16((allFile).Split('\n')[0].Split(';')[3])-1;
                
                Debug.Log("ntuto:" + Convert.ToInt16((allFile).Split('\n')[0].Split(';')[3]));
                errorText.GetComponent<Text>().text = "According to the fixed sequence, there should be "+ numberTrialFile + " levels, without counting the single tasks (Walk) ";
                errorText.SetActive(true);

            }
            catch {
                //ne doit pas continuer
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
		AudioVolumeField.GetComponent<TMP_InputField>().text = slid.value.ToString("f1");
	}
	void SliderFieldValueChanged(TMP_InputField inp)
	{
		Debug.Log(inp.text);
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
            useMeta = ToggleMeta.GetComponent<Toggle>().isOn;
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

            while (Dropdowns[j].options[Dropdowns[j].value].text.Contains("Single Task (Walk)") && j < numberTrials) { j++; }

            for (int i = 0; i < numberOfTutorial; i++)
            {
                sequenceNBack.Add(int.Parse(DropdownsNBack[j].options[DropdownsNBack[j].value].text));
                sequence.Add("Tutorial");
                Debug.Log(sequence[i]);
                Debug.Log(sequenceNBack[i]);
            }
            for (int i = 0; i < numberTrials; i++)
            {
                sequence.Add(Dropdowns[i].options[Dropdowns[i].value].text);
                sequenceNBack.Add(int.Parse(DropdownsNBack[i].options[DropdownsNBack[i].value].text));
                Debug.Log("sEQUENCE nBACK:" + int.Parse(DropdownsNBack[i].options[DropdownsNBack[i].value].text));
                Debug.Log(sequenceNBack[i+numberOfTutorial]);
            }
            //Debug.Log("Sequence: " + String.Join(", ", sequence.ToArray()));
            Debug.Log("Sequence N-Back: ");



            int numberOfSingleWalk = 0;
            Debug.Log("NumberOftrial: " + numberTrials);
            for (int i = 0; i < sizeOfArray; i++)
            {
                Debug.Log("lA  " + sequence[i]);
                if (sequence[i].Contains("Single Task (Walk)"))
                {
                    numberOfSingleWalk++;
                    Debug.Log("ici " + numberOfSingleWalk);
                }
            }
            if (gameMode == "Fixed")
            {
                Debug.Log("NTUTORIALFILE: " + (numberTrials - numberOfSingleWalk));
                Debug.Log("Number trials: " + (allFile.Split('\n').Length - numberOfTutorial - 1));
                //Debug.Log("Number trials: " + allFile.Split('\n').Length);
                //Debug.Log("Number trials: " + numberOfTutorial);
                if (numberTrials - numberOfSingleWalk > allFile.Split('\n').Length - numberOfTutorial - 1)
                {
                    Debug.Log("merde");

                    throw new Exception();
                }
            }

        }
        catch
        {
            errorText.GetComponent<Text>().text = "Error: The parameters are not valid. Read the instruction manual for more information.";
            errorText.SetActive(true);

        }
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
                numberOfObjects = Convert.ToInt16((fixedFile).Split('\n')[0].Split(';')[1]);
                inputNumObjects.GetComponent<TMP_InputField>().text = (fixedFile).Split('\n')[0].Split(';')[1];
             
                errorText.GetComponent<Text>().text = "According to the fixed sequence, there should be " + (fixedFile.Split('\n').Length - Convert.ToInt16((fixedFile).Split('\n')[0].Split(';')[3]) - 1) + " levels, without counting the single tasks ( Walk) ";
                errorText.SetActive(true);
            }
			else
			{
                checkFixed.SetActive(false);
				errorText.GetComponent<Text>().text = "Error: The fixed sequence file is not valid. Read the instruction manual for more information.";
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
        bool success = true;
        try
        {
            string[] lines = fixedFile.Split('\n');
            string[] firstRowcols = fixedFile.Split('\n')[0].Split(';');
                       
            int numberOfElements = Convert.ToInt32(firstRowcols[1]);
            int numberofTutorialFile = Convert.ToInt32(firstRowcols[3]);
            int numberOfLevel = lines.Length - numberofTutorialFile;

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
			(lines[3].Split(':')[1].Split(',').Count() == (Convert.ToInt32(lines[2].Split(':')[1]))+numberOfTutorial) &&
			(lines[4].Split(':')[0] == "Sequence N") &&
			(lines[4].Split(':')[1].Split(',').Count() ==( Convert.ToInt32(lines[2].Split(':')[1])) + numberOfTutorial)  &&
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