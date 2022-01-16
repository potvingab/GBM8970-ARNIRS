﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta;
using Meta.HandInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using System;
using System.IO.Ports;

public class Response : Interaction
{
	// Parameter from the menu scene
	public static string fileName = VariablesHolderStroop.fileName; 
    public static SerialPort serialPort = new SerialPort(VariablesHolderStroop.arduinoPort, 9600, Parity.None, 8, StopBits.One); // Arduino's port
    // New variables
    public GameObject selectedAnswersShown; // List of selected answers shown in the searcher's view
    public GameObject cube; // Button selected by the participant (RED, BLUE or GREEN)
    public string color; // Color of the button (RED, BLUE or GREEN)
    // New variables for HandTracking
    private HandFeature _handFeature; // Follow the hand during a grab
    private GameObject _selectedGameObject;

    protected override void Engage() // When it's in the zone, hand close
    {
        _handFeature = GrabbingHands[0];
        changeText();
    }

    protected override void Disengage()
    {
        if (_handFeature == null || _selectedGameObject == null)
        {
            return;
        }
        _selectedGameObject.SendMessage("detach");
        _selectedGameObject = null;

    }

    protected override void Manipulate()
    {
        return;
    }

    public static void TriggerArduino(string line)
    {
        serialPort.Open();
        serialPort.WriteLine(line);
        serialPort.Close();
    }

    public static void CreateCheckpoint(string nom)
    {
        //string fileName = @"C:\Users\achil\TempsVRNIRS.txt";
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.Write("\n Checkpoint; " + nom + " ;");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff"));
        }
    }

    public void changeText()
    {
        Debug.Log("ChangeText");
        Questions.timeEndQuestion = DateTime.Now;
        Questions.responseTimes.Add((Questions.timeEndQuestion - Questions.timeStartQuestion).TotalSeconds);
        // Add the answer selected by the participant (RED, BLUE or GREEN) to the list of selectedAnswers
        Questions.selectedAnswers.Add(color);
        // Add the selected answer to the list shown in the searcher's view
        selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (color + " ");
        // Increase the total number of answers
        Questions.numTotalAnswers += 1;
        // Create and show a new random question
        Questions.Instance.CreateNewRandomQuestion();
        CreateCheckpoint("Response: " + color);
        TriggerArduino("1");
    }

}
