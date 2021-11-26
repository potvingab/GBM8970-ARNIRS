using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ChooseFolder : MonoBehaviour {
	public GameObject inputFileName;

	public void OpenExplorer () {
		string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
		Debug.Log(directory);
		inputFileName.GetComponent<TMP_InputField>().text = directory + "\\" + DateTime.Now.ToString("yyyy_MM_dd_H_mm") + ".txt";
	}
}
