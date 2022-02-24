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

public class VariablesHolder : MonoBehaviour {
    public static float speed;
    public static float GameSpeed; // pause or not
    public static string arduinoPort = "COM3";
	public static string fileName;
    public static string gameMode; 
    public static bool useMeta;
    public static bool useVisual;
    public static bool useAudio;
    public static int numberOfObjects;
    public static List<string> sequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
	public static List<int> sequenceNBack = new List<int>(); // N of each N-back
    public static int numberTrials;
    //public static int nBackNumber = 2; // a supprimer bientot

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
			loadFixed.SetActive(true);
		}
		else
		{
			loadFixed.SetActive(false);
		}
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ChangeParameters()
    {
        
        useVisual = ToggleVisual.GetComponent<Toggle>().isOn;
        useAudio = ToggleAudio.GetComponent<Toggle>().isOn;
        Debug.Log(useVisual);
        Debug.Log(useAudio);
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
			speed = 1.00;
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
        // a faire
    }

    public void LoadParameters()
	{
        // a faire
    }

    public void SelectFixedFile()
	{
        // a faire
    }

    public bool CheckValidFileFixed(string fixedFile)
	{
        // a faire
        return true;
    }

    public void CheckValidFileParameters()
	{
        // a faire
	}
}