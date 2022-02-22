using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SequenceNBack : MonoBehaviour {

	public static SequenceNBack Instance;

	public static int numberTrials = 1;
	public GameObject inputField;
	public GameObject Dropdown1;
	public GameObject Dropdown2;
	public GameObject Dropdown3;
	public GameObject Dropdown4;
	public GameObject Dropdown5;
	public GameObject Dropdown6;
	public GameObject Dropdown7;
	public GameObject Dropdown8;
	public GameObject Dropdown9;
	public GameObject Dropdown10;
	public GameObject Dropdown11;
	public GameObject Dropdown12;
	public GameObject DropdownNBack1;
	public GameObject DropdownNBack2;
	public GameObject DropdownNBack3;
	public GameObject DropdownNBack4;
	public GameObject DropdownNBack5;
	public GameObject DropdownNBack6;
	public GameObject DropdownNBack7;
	public GameObject DropdownNBack8;
	public GameObject DropdownNBack9;
	public GameObject DropdownNBack10;
	public GameObject DropdownNBack11;
	public GameObject DropdownNBack12;

	void Awake()
    {
        Instance = this;
	}

	void Start()
    {
		var Dropdowns = new List<GameObject>() { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsNBack = new List<GameObject>() { DropdownNBack1, DropdownNBack2, DropdownNBack3, DropdownNBack4, DropdownNBack5, DropdownNBack6, DropdownNBack7, DropdownNBack8, DropdownNBack9, DropdownNBack10, DropdownNBack11, DropdownNBack12 };
		for (int i = 0; i < Dropdowns.Count; i++) {
			int x = i; // https://answers.unity.com/questions/1341897/buttononclickaddlistener-in-for-loop.html
			Dropdowns[x].GetComponent<Dropdown>().onValueChanged.AddListener(delegate {
            DropdownValueChanged(Dropdowns[x].GetComponent<Dropdown>(), DropdownsNBack[x]);
        	});
		}
    }

	// Hide the NBack Dropdowns if "Single Task (Walk)" selected
	void DropdownValueChanged(Dropdown drop, GameObject dropNBack)
    {
		if (drop.options[drop.value].text == "Single Task (Walk)")
		{
        	dropNBack.SetActive(false);
		}
		else
		{
			dropNBack.SetActive(true);
		}
    }

	public void StoreNumber() {
		var Dropdowns = new List<GameObject>() { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsNBack = new List<GameObject>() { DropdownNBack1, DropdownNBack2, DropdownNBack3, DropdownNBack4, DropdownNBack5, DropdownNBack6, DropdownNBack7, DropdownNBack8, DropdownNBack9, DropdownNBack10, DropdownNBack11, DropdownNBack12 };
	
		string nb = inputField.GetComponent<TMP_InputField>().text;
		int.TryParse(nb, out numberTrials);

		// Show the right number of Dropdowns
		for (int i = 0; i < Dropdowns.Count; i++){
			Dropdowns[i].SetActive(false);
			DropdownsNBack[i].SetActive(false);
		}
		for (int i = 0; i < numberTrials; i++){
			Dropdowns[i].SetActive(true);
			DropdownsNBack[i].SetActive(true);
		}
	}
	
}
