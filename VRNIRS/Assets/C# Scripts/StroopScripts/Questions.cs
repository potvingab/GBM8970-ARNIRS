using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Questions : MonoBehaviour
{
    // An instance is needed to use the method "CreateNewRandomQuestion" in other scripts
    public static Questions Instance;

    // Objects in participant's view
    public GameObject questionHolder;
    public GameObject redButton;
    public GameObject greenButton;
    public GameObject blueButton;
    public GameObject instructions;

    // Objectfs in searcher's view
    public GameObject timer;
    public GameObject totalResults;
    public Button buttonContinue;
    public GameObject correctAnswersShown;
    public GameObject selectedAnswersShown;
    public GameObject averageResponseTime;

    // Parameters from the menu scene
    public static float timeValue = VariablesHolderStroop.stroopTrialTime;
    public static int currentIndexSeq = 0;

    // New variables used
    public static List<string> possibleQuestions = new List<string>{ "GREEN", "RED", "BLUE" };
    public static Color[] possibleColors = { Color.green, Color.red, Color.blue };
    public static int indexRandQuestion;
    public static int indexRandColor;
    public static List<string> selectedAnswers = new List<string>(); // Answers selected by the participant
    public static List<string> correctAnswers = new List<string>(); // Correct answers (created by CreateNewRandomQuestion)
    public static int numCorrectAnswers = 0;
    public static int numTotalAnswers = 0;
    public static bool flagBeginTimer = false;
    public static List<double> responseTimes = new List<double>();
    public static DateTime timeStartQuestion;
    public static DateTime timeEndQuestion;
    public Image BackgroundImage; // New variable level 1 
    public Image Rectangle; // New variable level 4

    // Pages of the scene
    public GameObject canvasChercheurInstructions;
    public GameObject canvasChercheurJeu;
    public GameObject canvasParticipantInstructions;
    public GameObject canvasParticipantJeu;

    // An instance is needed to use the method "CreateNewRandomQuestion" in other scripts
    void Awake()
    {
        Instance = this;
        canvasChercheurInstructions.SetActive(true);
        canvasParticipantInstructions.SetActive(true);
		canvasChercheurJeu.SetActive(false);
        canvasParticipantJeu.SetActive(false);
        if (VariablesHolderStroop.useMeta == false){
            GameObject metaCamera = GameObject.Find("MetaCameraRig");
            GameObject metaHands = GameObject.Find("MetaHands");
            Destroy(metaCamera);
            Destroy(metaHands);
        }
    }

    // Level 0: Negative Control (pas encore dans le jeu)
    public void negativeControl()
    {
        Debug.Log("negativeControl");
        // setActive the right components
        BackgroundImage.gameObject.SetActive(false);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        // Sample random indices between 0 and 2
        indexRandQuestion = UnityEngine.Random.Range(0, 3);
        // The color of the text is the same as the text
        indexRandColor = indexRandQuestion;
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexRandQuestion];
        Debug.Log(possibleQuestions[indexRandQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexRandQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandQuestion] + " ");
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexRandColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexRandColor];
        timeStartQuestion = DateTime.Now;
    }

    public void playLevel()
    // Called by the "Start" button or "Continue" button
    // Play the right level according to the sequence
    {
        if (currentIndexSeq < VariablesHolderStroop.stroopNumberTrials){
            if (VariablesHolderStroop.stroopSequence[currentIndexSeq] != "Single Task (Walk)")
            {
                // Set active the right objects
                canvasChercheurInstructions.SetActive(false);
                canvasParticipantInstructions.SetActive(false);
                canvasChercheurJeu.SetActive(true);
                canvasParticipantJeu.SetActive(true);
                greenButton.gameObject.SetActive(true);
                redButton.gameObject.SetActive(true);
                blueButton.gameObject.SetActive(true);
                correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "Correct Answers: ";
                selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "Selected Answers: ";
                averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Time";
                totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = "Results";
                buttonContinue.gameObject.SetActive(false);
                // Prepare the right level
                Response.CreateCheckpoint("Level: " + VariablesHolderStroop.stroopSequence[currentIndexSeq] + " " + VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq].ToString());
                switch (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq])
                {
                    case 1:
                        backgroundColor();
                        break;

                    case 2:
                        blackText();
                        break;

                    case 3:
                        inkColor();
                        break;

                    case 4:
                        randomRectangle();
                        break;
                }
                // Start the timer ("Update" function is executed)
                flagBeginTimer = true;
            }
            else
            {
                // Do things for single task (for now, only write "single task")
                Response.CreateCheckpoint("Level: Single Task (Walk)");
                buttonContinue.gameObject.SetActive(false);
                selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "SINGLE TASK";
                BackgroundImage.gameObject.SetActive(false);
                Rectangle.gameObject.SetActive(false);
                questionHolder.gameObject.SetActive(true);
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "SINGLE TASK";
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Color.black;
                greenButton.gameObject.SetActive(false);
                redButton.gameObject.SetActive(false);
                blueButton.gameObject.SetActive(false);
                flagBeginTimer = true;
            }
        }
        else
        {
            // Do things for final screen after all levels (for now, only write "END")
            Response.CreateCheckpoint("Final screen");
            buttonContinue.gameObject.SetActive(false);
            selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "END";
            BackgroundImage.gameObject.SetActive(false);
            Rectangle.gameObject.SetActive(false);
            questionHolder.gameObject.SetActive(true);
            questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "END";
            questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
            questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Color.black;
            greenButton.gameObject.SetActive(false);
            redButton.gameObject.SetActive(false);
            blueButton.gameObject.SetActive(false);
        }
    }

    public void backgroundColor()
    {
        // Level 1: Background Color
        // TODO: Instructions = "Select the color of the rectangle.\n Are you ready?"
        Debug.Log("backgroundColor");

        // setActive the right components
        BackgroundImage.gameObject.SetActive(true);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(false);

        // Sample random indices between 0 and 2
        indexRandColor = UnityEngine.Random.Range(0, 3);
        // Change the color of the backgroundColor to the random color
        BackgroundImage.color = possibleColors[indexRandColor];
        Debug.Log(possibleColors[indexRandColor]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexRandColor]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandColor] + " ");

        Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexRandColor]);
        Response.TriggerArduino("0");
    }

    public void blackText()
    {
        // Level 2: Black Text
        // TODO: Instructions = "Select the written color.\n Are you ready?"
        Debug.Log("blackText");

        // setActive the right components
        BackgroundImage.gameObject.SetActive(false);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        // Sample random indices between 0 and 2
        indexRandQuestion = UnityEngine.Random.Range(0, 3);
        // Change the color of questionHolder to the black
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexRandColor];
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexRandQuestion];
        Debug.Log(possibleQuestions[indexRandQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexRandQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandQuestion] + " ");
        timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexRandQuestion]);
        Response.TriggerArduino("0");
    }

    //Level 3
    public void inkColor()
    {
        // Level 3: Ink Color (not the written color)
        // TODO: Instructions = "Select the color of the ink that the letters are printed in and not the written color.\n Are you ready?"
        Debug.Log("CreateQuestion");
        
        // Set Active the right components
        BackgroundImage.gameObject.SetActive(false);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        // Sample random indices between 0 and 2
        indexRandQuestion = UnityEngine.Random.Range(0, 3);
        indexRandColor = UnityEngine.Random.Range(0, 3);
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexRandQuestion];
        Debug.Log(possibleQuestions[indexRandQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexRandColor]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandColor] + " ");
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexRandColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexRandColor];
        timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexRandColor]);
        Response.TriggerArduino("0");
    }
    //Level 4
    public void randomRectangle()
    {
        // Level 4: Ink Color by default, Written Color if rectangle
        // TODO: Instructions = "By default, select the color of the ink that the letters are printed in and not the written color.\n If the text is framed, select the written color.\n Are you ready?"
        Debug.Log("randomRectangle");
       
        // Set Active the right components
        BackgroundImage.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        // Sample random indices either true or false 
        bool randomBool = UnityEngine.Random.Range(0, 2) > 0;
        Rectangle.gameObject.SetActive(randomBool);
        Debug.Log(randomBool);
        // Sample random indices between 0 and 2
        indexRandQuestion = UnityEngine.Random.Range(0, 3);
        indexRandColor = UnityEngine.Random.Range(0, 3);
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexRandQuestion];
        Debug.Log(possibleQuestions[indexRandQuestion]);
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexRandColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexRandColor];
        if (randomBool == true)
        {
            // Add the color as the correct answer to the list correctAnswers
            correctAnswers.Add(possibleQuestions[indexRandColor]);
            Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexRandColor]);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandColor] + " ");
        }
        else {
            // Add the text as the correct answer to the list correctAnswers
            correctAnswers.Add(possibleQuestions[indexRandQuestion]);
            Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexRandQuestion]);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandQuestion] + " ");

        }
        timeStartQuestion = DateTime.Now;
        Response.TriggerArduino("0");
    }

    void Update()
    {
        if (flagBeginTimer == true)
        {
            Debug.Log(timeValue);
            // Each second, if there's still time on the timer, print the time and decrease it
            if (timeValue > 0)
            {
                timer.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Time left: {0:00}", Mathf.FloorToInt(timeValue));
                timeValue -= Time.deltaTime;
            }
            // If there's not time left
            else
            {
                // Compare the correct and selected answers, and compute the result (numCorrectAnswers/numTotalAnswers)
                for (int i=0; i<selectedAnswers.Count; i++)
                {
                    if (correctAnswers[i] == selectedAnswers[i])
                    {
                        numCorrectAnswers += 1;
                    }
                }
                // Show the result (researcher's view)
                totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Results: {0:00}/{1:00}", numCorrectAnswers, numTotalAnswers);
                Response.CreateCheckpoint("Result: " + numCorrectAnswers + "/" + numTotalAnswers);
                if (responseTimes.Count==0)
                {
                    timeEndQuestion = DateTime.Now;
                    responseTimes.Add((Questions.timeEndQuestion - Questions.timeStartQuestion).TotalSeconds);
                }
                averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Time (sec): " + Queryable.Average(responseTimes.AsQueryable()).ToString();
                Response.CreateCheckpoint("Average Response Time: " + Queryable.Average(responseTimes.AsQueryable()).ToString());
                // Show the button "Continue" (researcher's view)
                buttonContinue.gameObject.SetActive(true);
                // Change the text of the questionHolder (player's view)
                BackgroundImage.gameObject.SetActive(false);
                Rectangle.gameObject.SetActive(false);
                questionHolder.gameObject.SetActive(true);
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "WAIT FOR NEXT LEVEL";
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Color.black;
                greenButton.gameObject.SetActive(false);
                redButton.gameObject.SetActive(false);
                blueButton.gameObject.SetActive(false);
                // Reset the answers
                selectedAnswers = new List<string>(); // Answers selected by the participant
                correctAnswers = new List<string>(); // Correct answers (created by CreateNewRandomQuestion)
                responseTimes = new List<double>();
                numCorrectAnswers = 0;
                numTotalAnswers = 0;
                // Play the next level in the sequence next time
                currentIndexSeq += 1;
                // Change the flag to compute the result only one time
                flagBeginTimer = false;
                timeValue = VariablesHolderStroop.stroopTrialTime;
            }
        }
    }
}