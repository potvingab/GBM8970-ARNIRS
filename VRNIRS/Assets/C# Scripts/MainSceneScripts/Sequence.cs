using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sequence : MonoBehaviour {

	public static int numberTrials = 1;
	public GameObject inputField;
	public GameObject Dropdown2;
	public GameObject Dropdown3;
	public GameObject Dropdown4;
	public GameObject Dropdown5;
	public GameObject Dropdown6;

	public void StoreNumber() {
		string nb = inputField.GetComponent<Text>().text;
		int.TryParse(nb, out numberTrials);

		if (numberTrials == 1){
			Dropdown2.SetActive(false);
			Dropdown3.SetActive(false);
			Dropdown4.SetActive(false);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
		}
		if (numberTrials == 2){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(false);
			Dropdown4.SetActive(false);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
		}
		if (numberTrials == 3){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(false);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
		}
		if (numberTrials == 4){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(true);
			Dropdown5.SetActive(false);
			Dropdown6.SetActive(false);
		}
		if (numberTrials == 5){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(true);
			Dropdown5.SetActive(true);
			Dropdown6.SetActive(false);
		}
		if (numberTrials >= 6){
			Dropdown2.SetActive(true);
			Dropdown3.SetActive(true);
			Dropdown4.SetActive(true);
			Dropdown5.SetActive(true);
			Dropdown6.SetActive(true);
		}
	}
	
}
