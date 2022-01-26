using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sequence : MonoBehaviour {

	public static Sequence Instance;

	public static int numberTrials = 1;
	public GameObject inputField;
	public GameObject Dropdown2;
	public GameObject Dropdown3;
	public GameObject Dropdown4;
	public GameObject Dropdown5;
	public GameObject Dropdown6;
	public GameObject DropdownLevel2;
	public GameObject DropdownLevel3;
	public GameObject DropdownLevel4;
	public GameObject DropdownLevel5;
	public GameObject DropdownLevel6;

	void Awake()
    {
        Instance = this;
	}

	public void StoreNumber() {
		//string nb = inputField.GetComponent<Text>().text;
		string nb = inputField.GetComponent<TMP_InputField>().text;
		int.TryParse(nb, out numberTrials);

		if (numberTrials == 1){
			Dropdown2.SetActive(false);
			Dropdown3.SetActive(false);
			Dropdown4.SetActive(false);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
			DropdownLevel2.SetActive(false);
			DropdownLevel3.SetActive(false);
			DropdownLevel4.SetActive(false);
			DropdownLevel5.SetActive(false);
			DropdownLevel6.SetActive(false);
		}
		if (numberTrials == 2){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(false);
			Dropdown4.SetActive(false);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
			DropdownLevel2.SetActive(true);
			DropdownLevel3.SetActive(false);
			DropdownLevel4.SetActive(false);
			DropdownLevel5.SetActive(false);
			DropdownLevel6.SetActive(false);
		}
		if (numberTrials == 3){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(false);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
			DropdownLevel2.SetActive(true);
			DropdownLevel3.SetActive(true);
			DropdownLevel4.SetActive(false);
			DropdownLevel5.SetActive(false);
			DropdownLevel6.SetActive(false);
		}
		if (numberTrials == 4){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(true);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
			DropdownLevel2.SetActive(true);
			DropdownLevel3.SetActive(true);
			DropdownLevel4.SetActive(true);
			DropdownLevel5.SetActive(false);
			DropdownLevel6.SetActive(false);
		}
		if (numberTrials == 5){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(true);
			Dropdown5.SetActive(true);
			Dropdown6.SetActive(false);
			DropdownLevel2.SetActive(true);
			DropdownLevel3.SetActive(true);
			DropdownLevel4.SetActive(true);
			DropdownLevel5.SetActive(true);
			DropdownLevel6.SetActive(false);
		}
		if (numberTrials >= 6){
			numberTrials = 6;
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(true);
			Dropdown5.SetActive(true);
			Dropdown6.SetActive(true);
			DropdownLevel2.SetActive(true);
			DropdownLevel3.SetActive(true);
			DropdownLevel4.SetActive(true);
			DropdownLevel5.SetActive(true);
			DropdownLevel6.SetActive(true);
		}
	}
	
}
