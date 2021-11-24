﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesHolderStroop : MonoBehaviour {
	// Values to store
	public static string stroopGameMode;
	public static int stroopTrialTime;
	public static int stroopNumberTrials;
	public static List<string> stroopSequence = new List<string>();
	public static List<int> stroopSequenceLevels = new List<int>();
	// Where to find the values
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

	void Start () {
		// Default values
		stroopGameMode = "Fixed";
		stroopTrialTime = 90;
		stroopNumberTrials = 1;
	}
	
	public void ChangeParameters() {
		// Update "time (one trial)"
		int.TryParse(inputTime.GetComponent<Text>().text, out stroopTrialTime);
		if (stroopTrialTime == 0){
			stroopTrialTime = 90;
		}
		// Update "number of trials"
		int.TryParse(inputNumberTrials.GetComponent<Text>().text, out stroopNumberTrials);
		if (stroopNumberTrials == 0){
			stroopNumberTrials = 1;
		}
		// Update "sequence"
		var Dropdowns = new[] { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6 };
		var DropdownsLevel = new[] { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6 };
		for (int i = 0; i < stroopNumberTrials; i++) {
			stroopSequence.Add(Dropdowns[i].GetComponent<Text>().text);
			stroopSequenceLevels.Add(int.Parse(DropdownsLevel[i].GetComponent<Text>().text));
		}
		// Update "game mode"
		if (ButtonRandom.GetComponent<Toggle>().isOn == true){
			stroopGameMode = "Random";
		}
		else{
			stroopGameMode = "Fixed";
		}
	}
}