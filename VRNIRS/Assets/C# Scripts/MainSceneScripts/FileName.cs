using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class FileName : MonoBehaviour {

    public static string nameOfFile;
    public TextMeshProUGUI nameOfFileTMP;
    public static string nameOfFileOriginal;
    public static string timeStamp;
    public static string mode;


    public void GenerateFileName()
    {
        nameOfFileOriginal = nameOfFileTMP.text;
        timeStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        nameOfFile = nameOfFileTMP.text + "_" + timeStamp;
        Debug.Log(nameOfFile);

        string path = Application.dataPath + "/Saves";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        nameOfFile = path + "/" + nameOfFile;


     
    }

    public void AddTestType(string testType)
    {
        if (testType == "F")
        {
            mode = "FIXED";
        }
        else
        {
            mode = "RANDOM";
        }
        nameOfFile = nameOfFile + "_" + testType + ".csv";
        Debug.Log(nameOfFile);
    }

    public void RemoveTestType()
    {
        nameOfFile = nameOfFile.Substring(0,nameOfFile.Length - 6);
        Debug.Log(nameOfFile);
    }
}
