using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sequence : MonoBehaviour {

	public static Sequence Instance;

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
	public GameObject DropdownLevel1;
	public GameObject DropdownLevel2;
	public GameObject DropdownLevel3;
	public GameObject DropdownLevel4;
	public GameObject DropdownLevel5;
	public GameObject DropdownLevel6;
	public GameObject DropdownLevel7;
	public GameObject DropdownLevel8;
	public GameObject DropdownLevel9;
	public GameObject DropdownLevel10;
	public GameObject DropdownLevel11;
	public GameObject DropdownLevel12;

	void Awake()
    {
        Instance = this;
	}

	void Start()
    {
		var Dropdowns = new List<GameObject>() { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsLevel = new List<GameObject>() { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6, DropdownLevel7, DropdownLevel8, DropdownLevel9, DropdownLevel10, DropdownLevel11, DropdownLevel12 };
		for (int i = 0; i < Dropdowns.Count; i++) {
			int x = i; // https://answers.unity.com/questions/1341897/buttononclickaddlistener-in-for-loop.html
			Dropdowns[x].GetComponent<Dropdown>().onValueChanged.AddListener(delegate {
            DropdownValueChanged(Dropdowns[x].GetComponent<Dropdown>(), DropdownsLevel[x]);
        	});
		}
    }

	// Hide the Level Dropdowns if "Single Task (Walk)" selected
	void DropdownValueChanged(Dropdown drop, GameObject droplevel)
    {
		if (drop.options[drop.value].text == "Single Task (Walk)")
		{
        	droplevel.SetActive(false);
		}
		else
		{
			droplevel.SetActive(true);
		}
    }

	public void StoreNumber() {
		var Dropdowns = new List<GameObject>() { Dropdown1, Dropdown2, Dropdown3, Dropdown4, Dropdown5, Dropdown6, Dropdown7, Dropdown8, Dropdown9, Dropdown10, Dropdown11, Dropdown12 };
		var DropdownsLevel = new List<GameObject>() { DropdownLevel1, DropdownLevel2, DropdownLevel3, DropdownLevel4, DropdownLevel5, DropdownLevel6, DropdownLevel7, DropdownLevel8, DropdownLevel9, DropdownLevel10, DropdownLevel11, DropdownLevel12 };
	
		string nb = inputField.GetComponent<TMP_InputField>().text;
		int.TryParse(nb, out numberTrials);

		if (numberTrials > 12){
			numberTrials = 12;
		}

		// Show the right number of Dropdowns
		for (int i = 0; i < Dropdowns.Count; i++){
			Dropdowns[i].SetActive(false);
			DropdownsLevel[i].SetActive(false);
		}
		for (int i = 0; i < numberTrials; i++){
			Dropdowns[i].SetActive(true);
			DropdownsLevel[i].SetActive(true);
		}
	}
	
}
