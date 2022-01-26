using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class TestFichierTemps : MonoBehaviour {

    public static string nameOfFileTemps;
    public TextMeshProUGUI nameOfFileTMP;
    public static string nameOfFileTempsOriginal;
    public static string timeStamp_test;
    public static string mode;

    public void GenerateFichierTemps()
    {
        Console.Write("allo");
        nameOfFileTempsOriginal = nameOfFileTMP.text;
        timeStamp_test = DateTime.Now.ToString("0-HHmmssfff");
        nameOfFileTemps = nameOfFileTMP.text + "_" + timeStamp_test;
        Debug.Log(nameOfFileTemps);

        string path = Application.dataPath + "/Saves";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        nameOfFileTemps = path + "/" + nameOfFileTemps;

        string fileName = @"C:\Users\achil\TempsVRNIRS.txt";



        // Check if file already exists. If yes, delete it.     
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        // Create a new file     
        using (FileStream fs = File.Create(fileName))
        {
            // Add some text to file    
            Byte[] title = new System.Text.UTF8Encoding(true).GetBytes("Début enregistrement : ");
            fs.Write(title, 0, title.Length);

            Byte[] temps_init = new System.Text.UTF8Encoding(true).GetBytes(DateTime.Now.ToString("H:mm:ss.fff"));
            fs.Write(temps_init, 0, temps_init.Length);
        }


    }
    // Use this for initialization
    public void CreateCheckpoint(string nom) {

        string fileName = @"C:\Users\achil\TempsVRNIRS.txt";
        using (StreamWriter sw = File.AppendText(fileName))
        {
            // Add some text to file    
            //Byte[] title = new System.Text.UTF8Encoding(true).GetBytes("\n Checkpoint ");
            //sw.Write(title, 0, title.Length);
            sw.Write("\n Checkpoint " + nom + " ");

            //Byte[] temps_init = new System.Text.UTF8Encoding(true).GetBytes(DateTime.Now.ToString("H:mm:ss zzz"));
            //sw.Write(temps_init, 0, temps_init.Length);
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff"));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
