using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SFB;

public class ChooseFolder : MonoBehaviour {
	public GameObject inputFileName;
	public GameObject errorText;

	// Open the file explorer and save the chosen file path as the "inputFileName" variable. Add the time and date as filename.
	public void OpenExplorer () {
		string[] directory = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);
		if (!String.IsNullOrEmpty(directory[0]))
		{
			inputFileName.GetComponent<TMP_InputField>().text = directory[0] + "/" + DateTime.Now.ToString("yyyy_MM_dd_H_mm") + ".txt";
			errorText.SetActive(false);
		}
		else
		{
			errorText.GetComponent<Text>().text = "Error: Please select a valid folder.";
			errorText.SetActive(true);
		}
	}
}
