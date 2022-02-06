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
    public GameObject instructionLevel;
    public AudioSource beep;

    // Objectfs in searcher's view
    public GameObject timer;
    public GameObject totalResults;
    public Button buttonContinue;
    public Button buttonRestart;
    public GameObject correctAnswersShown;
    public GameObject selectedAnswersShown;
    public GameObject averageResponseTime;
    public Button playButton;
    public Button instructionButton;
    public GameObject textLevel;
    public GameObject textCalibraton;
    public Button buttonQuit;
    public Button buttonNew;

    // Parameters from the menu scene
    public static float timeValue = VariablesHolderStroop.stroopTrialTime;
    public static int currentIndexSeq = 0;

    // New variables used
    public static List<string> possibleQuestions = new List<string>{ "GREEN", "RED", "BLUE" };
    public static Color[] possibleColors = { Color.green, Color.red, Color.blue };
    public static int indexQuestion;
    public static int indexColor;
    public static bool bool_Square;

    public static int number_questions_fs;
    public static int n_question_fixed = 0;
    public static int line = 0;

    public static string[] question;
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
        indexQuestion = UnityEngine.Random.Range(0, 3);
        // The color of the text is the same as the text
        indexColor = indexQuestion;
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexQuestion];
        Debug.Log(possibleQuestions[indexQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexQuestion] + " ");
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexColor];
        timeStartQuestion = DateTime.Now;
    }

    public void playLevel()
    {
        if(currentIndexSeq < VariablesHolderStroop.stroopNumberTrials)
        {
            // Set active the right objects
            canvasChercheurInstructions.SetActive(false);
            canvasParticipantInstructions.gameObject.SetActive(false);
            canvasChercheurJeu.SetActive(true);
            canvasParticipantJeu.SetActive(true);
            greenButton.gameObject.SetActive(true);
            redButton.gameObject.SetActive(true);
            blueButton.gameObject.SetActive(true);
            questionHolder.gameObject.SetActive(true);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "Correct Answers: ";
            selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "Selected Answers: ";
            averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Time: ";
            totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = "Results: ";
            buttonContinue.gameObject.SetActive(false);
            buttonRestart.gameObject.SetActive(false);
            buttonQuit.gameObject.SetActive(false);
            buttonNew.gameObject.SetActive(false);

            //Read the file if fixed sequence
            if (VariablesHolderStroop.stroopGameMode == "Fixed")
            {
                // If custom "fixed colors file"
                string all_Info;
                if (VariablesHolderStroop.fixedFile.Contains("Niveau")) // changer pour mieux verif
                {
                    all_Info = VariablesHolderStroop.fixedFile;
                }
                else
                {
                    TextAsset txt = (TextAsset)Resources.Load("fixed_sequence", typeof(TextAsset));
                    all_Info = txt.text;
                }
                Debug.Log(all_Info);
                string[] info_Line = all_Info.Split('\n');
                //the starting line according to the level and read this line
                line = 2 * VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq] - 1;
                question = info_Line[line].Split(';');
            }

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
    }

    public void playInstruction() 
    // Called by the "Start" button or "Continue" button
    // Play the right level according to the sequence
    {
        if (currentIndexSeq < VariablesHolderStroop.stroopNumberTrials){
            if (VariablesHolderStroop.stroopSequence[currentIndexSeq] != "Single Task (Walk)")
            {
                canvasChercheurJeu.gameObject.SetActive(false);
                questionHolder.gameObject.SetActive(false);
                canvasParticipantInstructions.gameObject.SetActive(true);
                canvasChercheurInstructions.gameObject.SetActive(true);
                instructionButton.gameObject.SetActive(true);
                switch (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq])
                {

                    case 1:
                        instructionLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the color of the rectangle.\n  Are you ready ?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "LEVEL " + (currentIndexSeq + 1).ToString();
                        break;

                    case 2:
                        instructionLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the written color. \n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "LEVEL " + (currentIndexSeq + 1).ToString();
                        break;

                    case 3:
                        instructionLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the color of the word. that the letters are printed in and not the written color.\n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "LEVEL " + (currentIndexSeq + 1).ToString() ;
                        break;

                    case 4:
                        instructionLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "If the text is framed, select the written color. Otherwise, select the color of the word.\n \n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "LEVEL " + (currentIndexSeq + 1).ToString();
                        break;
                }
                playButton.gameObject.SetActive(true);
                instructionButton.gameObject.SetActive(false);
                textCalibraton.gameObject.SetActive(false);
            }
            else
            {
                // Do things for single task (for now, only write "single task")
                Response.CreateCheckpoint("Level: Single Task (Walk)");
                buttonContinue.gameObject.SetActive(false);
                buttonRestart.gameObject.SetActive(false);
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
                timeStartQuestion = DateTime.Now;
            }
        }
        
        else
        {
            // Do things for final screen after all levels (for now, only write "END")
            Response.CreateCheckpoint("Final screen");
            buttonContinue.gameObject.SetActive(false);
            buttonRestart.gameObject.SetActive(false);
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
            buttonQuit.gameObject.SetActive(true);
            buttonNew.gameObject.SetActive(true);
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

        if (VariablesHolderStroop.stroopGameMode == "Fixed")
        {
            if (question[n_question_fixed].Split(',')[0] == "END")
            {
                Debug.Log("end of trial");
                n_question_fixed = 0;
            }
            string ink_color = question[n_question_fixed].Split(',')[0];
            indexColor = file_convert(ink_color);
            n_question_fixed++;
        }
        else
        {
            // Sample random indices between 0 and 2
            indexColor = UnityEngine.Random.Range(0, 3);

        }
        // Change the color of the backgroundColor to the random color
        BackgroundImage.color = possibleColors[indexColor];
        Debug.Log(possibleColors[indexColor]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexColor]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexColor] + " ");
        timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexColor]);
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

        //Fixed sequence
        if (VariablesHolderStroop.stroopGameMode == "Fixed")
        {
            string ink_color = question[n_question_fixed].Split(',')[0];
            indexQuestion = file_convert(ink_color);
            n_question_fixed++;
        }
        //Random sequence
        else
        {
            // Sample random indices between 0 and 2
            indexQuestion = UnityEngine.Random.Range(0, 3);
        }

        // Change the color of questionHolder to the black
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexColor]; //POURQUOI?? 
        // Change the text of questionHolder to the question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexQuestion];
        //Debug.Log(possibleQuestions[indexQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexQuestion] + " ");
        timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexQuestion]);
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

        if (VariablesHolderStroop.stroopGameMode == "Fixed")
        {
            if (question[n_question_fixed].Split(',')[0] == "END")
            {
                Debug.Log("end of trial");
                n_question_fixed = 0;
            }
            string ink_color = question[n_question_fixed].Split(',')[0];
            indexColor = file_convert(ink_color);
            string word_color = question[n_question_fixed].Split(',')[1];
            indexQuestion = file_convert(word_color);
            n_question_fixed++;
        }
        //Ramdom sequence
        else
        {
            // Sample random indices between 0 and 2
            indexQuestion = UnityEngine.Random.Range(0, 3);
            indexColor = UnityEngine.Random.Range(0, 3);
        }

            // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexQuestion];
        Debug.Log(possibleQuestions[indexQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexColor]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexColor] + " ");
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexColor];
        timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexColor]);
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
        
        //fixed sequence
        if (VariablesHolderStroop.stroopGameMode == "Fixed")
        {
           
            if (question[n_question_fixed].Split(',')[0] == "END")
            {
                Debug.Log("end of trial");
                n_question_fixed = 0;
            }

            string ink_color = question[n_question_fixed].Split(',')[0];
            string word_color = question[n_question_fixed].Split(',')[1];
            string square = question[n_question_fixed].Split(',')[2];

            indexColor = file_convert(ink_color);
            indexQuestion = file_convert(word_color);
           
            switch (square)
            {
                case "0":
                    bool_Square = true;
                    break;
                case "1":
                    bool_Square = false;
                    break;
            }
            n_question_fixed++;
        }

        //Random sequence
        else
        {
            // Sample random indices either true or false 
            bool_Square = UnityEngine.Random.Range(0, 2) > 0;

            Debug.Log(bool_Square);
            // Sample random indices between 0 and 2
            indexQuestion = UnityEngine.Random.Range(0, 3);
            indexColor = UnityEngine.Random.Range(0, 3);
        }
        Rectangle.gameObject.SetActive(bool_Square);
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexQuestion];
        Debug.Log(possibleQuestions[indexQuestion]);
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = possibleColors[indexColor];
        if (bool_Square == true)
        {
            // Add the color as the correct answer to the list correctAnswers
            correctAnswers.Add(possibleQuestions[indexColor]);
            Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexColor]);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexColor] + " ");
        }
        else {
            // Add the text as the correct answer to the list correctAnswers
            correctAnswers.Add(possibleQuestions[indexQuestion]);
            Response.CreateCheckpoint("Question shown. True response: " + possibleQuestions[indexQuestion]);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexQuestion] + " ");

        }
        timeStartQuestion = DateTime.Now;
        Response.TriggerArduino("0");
    }


    int file_convert(string color)
    {
        int index=0;
        switch (color)
        {
            case "G":
                index = 0;
                break;
            case "R":
                index = 1;
                break;

            case "B":
                index = 2;
                break;

            default:
                n_question_fixed = 0;
                color = question[n_question_fixed].Split(',')[0];
                switch (color)
                {
                    case "G":
                        index = 0;
                        break;
                    case "R":
                        index = 1;
                        break;

                    case "B":
                        index = 2;
                        break;
                }
                break;
        }
        return index;
    }

    void Update()
    {
        if (flagBeginTimer == true)
        {
            //Debug.Log(timeValue);
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
                totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("Results: {0:00}/{1:00}", numCorrectAnswers, numTotalAnswers);
                Response.CreateCheckpoint("Result: " + numCorrectAnswers + "/" + numTotalAnswers);
                if (responseTimes.Count==0)
                {
                    timeEndQuestion = DateTime.Now;
                    responseTimes.Add((timeEndQuestion - timeStartQuestion).TotalSeconds);
                }
                Debug.Log(String.Join(",", responseTimes.Select(x => x.ToString()).ToArray()));
                averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Time: " + Math.Round(Queryable.Average(responseTimes.AsQueryable()),2).ToString() + " sec";
                Response.CreateCheckpoint("Average Response Time: " + Queryable.Average(responseTimes.AsQueryable()).ToString());
                // Show the button "Continue" (researcher's view)
                buttonContinue.gameObject.SetActive(true);
                buttonRestart.gameObject.SetActive(true);
                // Play a sound
                beep.Play();
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
                currentIndexSeq++;
                // Change the flag to compute the result only one time
                flagBeginTimer = false;
                timeValue = VariablesHolderStroop.stroopTrialTime;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        //Peut-etre ajouter un checkpoint pour signifier un nouveau test??
    }

    public void Restart()
    {
        currentIndexSeq--;
        n_question_fixed--;
        playInstruction();
    }

}