using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;

public class ChooseFolder : MonoBehaviour {
	public GameObject inputFileName;

	public void OpenExplorer () {
		string[] directory = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", true);
		inputFileName.GetComponent<TMP_InputField>().text = directory[0] + "/" + DateTime.Now.ToString("yyyy_MM_dd_H_mm") + ".txt";
	}
}
