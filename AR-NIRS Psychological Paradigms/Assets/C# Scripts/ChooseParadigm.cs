using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseParadigm : MonoBehaviour {
	public GameObject ParadigmChoicePage;
	public GameObject FileNameNBackPage;
	public GameObject FileNameStroopPage;
	public GameObject Dropdown;

	// Load the right "file name" page according to the chosen paradigm
	public void ChooseParadi () {
		ParadigmChoicePage.SetActive(false);
		string mode = Dropdown.GetComponent<Text>().text;
		if (mode == "N-Back"){
			FileNameNBackPage.SetActive(true);
		}
		if (mode == "Stroop"){
			FileNameStroopPage.SetActive(true);
		}
		
	}

}
