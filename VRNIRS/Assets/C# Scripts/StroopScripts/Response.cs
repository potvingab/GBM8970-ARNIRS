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

    public static SerialPort serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    public GameObject selectedAnswersShown; // List of selected answers shown in the searcher's view
    public GameObject cube; // Button selected by the participant (RED, BLUE or GREEN)
    public string color;


    private HandFeature _handFeature; //follow the hand during a grab
    private GameObject _selectedGameObject;
    //private string color;


    //public static string color; // Color of the button (RED, BLUE or GREEN)       

    protected override void Engage() //when it's in the zone, hand close
    {
        _handFeature = GrabbingHands[0];

        if (_handFeature == null)

        {
            _selectedGameObject = Instantiate(cube); //create a clone to move  object

        }
        Debug.Log("grabbed");
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

        string fileName = @"C:\Users\achil\TempsVRNIRS.txt";
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.Write("\n Checkpoint; " + nom + " ;");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff"));
        }
    }



    public void changeText()
    {
        Debug.Log("changetext");
        // Find the color of the selected button
        //color = "blue";
        //color = cube.GetComponent<TMPro.TextMeshProUGUI>().text;
        //colors = color.text;
        Debug.Log(color);
        // Add the answer selected by the participant (RED, BLUE or GREEN) to the list of selectedAnswers
        Questions.selectedAnswers.Add(color);
        // Add the selected answer to the list shown in the searcher's view
        selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (color + " ");
        CreateCheckpoint("Reponse" + color);
        TriggerArduino("1");
        // Increase the total number of answers
        Questions.numTotalAnswers += 1;
        // Create and show a new random question
        Questions.Instance.CreateNewRandomQuestion();
    }

}

