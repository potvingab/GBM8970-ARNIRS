using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

public class TestFichierTemps : MonoBehaviour {

    public static string nameOfFileTemps;
    public TextMeshProUGUI nameOfFileTMP;
    public static string nameOfFileTempsOriginal;
    public static string timeStamp_test;
    public static string mode;
    public Stopwatch stopWatch;

    public void GenerateFichierTemps()
    {
        stopWatch = new Stopwatch();
        stopWatch.Start();

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

            Thread.Sleep(100);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            stopWatch.Start();

            Byte[] temps_init_stopW = new System.Text.UTF8Encoding(true).GetBytes("\n verif "+elapsedTime);
            fs.Write(temps_init_stopW, 0, temps_init_stopW.Length);
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

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            stopWatch.Start();

            sw.Write("\n verif " + elapsedTime);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
