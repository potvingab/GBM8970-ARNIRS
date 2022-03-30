using System.Collections;
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
        // 0: Question
        // 1: Response
        // Enlever commentaire si on utilise l'Arduino
        //try
        //{
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            serialPort.WriteLine(line);
            ARCheckpoint("Trigger sent");
        //    Questions.Instance.errorTextInstruc.gameObject.SetActive(false);
        //    Questions.Instance.errorTextGame.gameObject.SetActive(false);
        //    Questions.Instance.errorButtonInstruc.gameObject.SetActive(false);
        //    Questions.Instance.errorButtonGame.gameObject.SetActive(false);
        //    Questions.Instance.errorBgInstruc.gameObject.SetActive(false);
        //    Questions.Instance.errorBgGame.gameObject.SetActive(false);
        //}
        //catch
        //{
        //    Questions.Instance.errorTextInstruc.gameObject.SetActive(true);
        //    Questions.Instance.errorTextGame.gameObject.SetActive(true);
        //    Questions.Instance.errorButtonInstruc.gameObject.SetActive(true);
        //    Questions.Instance.errorButtonGame.gameObject.SetActive(true);
        //    Questions.Instance.errorBgInstruc.gameObject.SetActive(true);
        //    Questions.Instance.errorBgGame.gameObject.SetActive(true);
        //    Questions.Instance.errorTextGame.GetComponent<Text>().text = "Error: The Arduino seems disconnected. Read the instruction manual for more information.";
        //    Questions.Instance.errorTextInstruc.GetComponent<Text>().text = "Error: The Arduino seems disconnected. Read the instruction manual for more information.";
        //}
    }

    public static void CreateCheckpoint(string nom)
    {
        String name = VariablesHolderStroop.fileName;
        int index = name.IndexOf(".txt");
        String masterFileName = name.Insert(index, "_Master");
        using (StreamWriter sw = File.AppendText(masterFileName))
        {
            sw.Write("Checkpoint; " + nom + "; ");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff") + "\n");
        }
        ARCheckpoint("Event received");
    }
    public static void ArduinoCheckpoint(string nom)
    {
        String name = VariablesHolderStroop.fileName;
        int index = name.IndexOf(".txt");
        String arduinoFileName = name.Insert(index, "_Test_synchro_Arduino");
        if (!serialPort.IsOpen)
            serialPort.Open();
        string delay = serialPort.ReadLine();
        using (StreamWriter sw = File.AppendText(arduinoFileName))
        {
            sw.Write("Arduino Delay; " + nom + "; " + delay + " μs" + "\n");
        }
    }

    public static void ARCheckpoint(string nom)
    {
        String name = VariablesHolderStroop.fileName;
        int index = name.IndexOf(".txt");
        String arFileName = name.Insert(index, "_Test_synchro_AR");
        using (StreamWriter sw = File.AppendText(arFileName))
        {
            sw.Write("Checkpoint; " + nom + "; ");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff") + "\n");
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
        CreateCheckpoint("Participant's response: " + color + "; Response time: " + (Questions.timeEndQuestion - Questions.timeStartQuestion).TotalSeconds + "sec");
        Response.TriggerArduino("0");

        // Create and show a new random question
        switch (VariablesHolderStroop.sequenceLevels[Questions.currentIndexSeq])
        {
            case 0:
                Difficulty.Instance.BaseLine();
                break;
                
            case 1:
                Difficulty.Instance.backgroundColor();
                break;

            case 2:
                Difficulty.Instance.blackText();
                break;

            case 3:
                Difficulty.Instance.inkColor();
                break;

            case 4:
                Difficulty.Instance.randomRectangle();
                break;
        }
    }
}