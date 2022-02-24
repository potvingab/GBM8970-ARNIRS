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
    // An instance is needed to use the method "Question" in other scripts
    public static Questions Instance;

    // Objects in participant's view
    public GameObject questionHolder;
    public GameObject redButton;
    public GameObject greenButton;
    public GameObject blueButton;
    public GameObject instructionDifficulty;
    public AudioSource beep;
    
    // Objectfs in searcher's view
    public GameObject timer;
    public GameObject WhitBgTL;
    public GameObject totalResults;
    public Button buttonContinue;
    public Button buttonRestart;
    public GameObject correctAnswersShown;
    public GameObject selectedAnswersShown;
    public GameObject averageResponseTime;
    public Button playButton;
    public GameObject playTutoButton;
    public Button instructionButton;
    public GameObject textLevel;
    public GameObject textCalibraton;
    public Button buttonQuit;
    public Button buttonNew;
    public GameObject whiteBackgrounds;
    public GameObject LevelDifficulty;


    // Parameters from the menu scene
    public static float timeValue = VariablesHolderStroop.stroopTrialTime;
    public static int currentIndexSeq = 0;

    // New variables used
    public static List<string> possibleQuestions = new List<string>{ "GREEN", "RED", "BLUE" };
    public static Color[] possibleColors = { Color.green, Color.red, Color.blue };
    public static int indexQuestion;
    public static int indexColor;
    public static bool bool_Square;


    public static string[] question;
    public static List<string> selectedAnswers = new List<string>(); // Answers selected by the participant
    public static List<string> correctAnswers = new List<string>(); // Correct answers (created by CreateNewRandomQuestion)
    public static int numCorrectAnswers = 0;
    public static int numTotalAnswers = 0;
    public static bool flagBeginTimer = false;
    public static List<double> responseTimes = new List<double>();
    public static DateTime timeStartQuestion;
    public static DateTime timeEndQuestion;
    public Image BackgroundImage; // New variable difficulty 1 
    public Image Rectangle; // New variable difficulty 4

    //fixed sequence variables
    public static int n_question_fixed = 0;
    public static int line = 0;
    public static bool flagTuto = false;
    public static bool end_of_trial = false;
    public static bool flagRestart = false;


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


    public void playLevel()
         // Called by the "Instruction" button or "Continue" button
    {   // Play the right difficulty according to the sequence
        flagTuto = false;
        WhitBgTL.gameObject.SetActive(true);
        // timeValue = VariablesHolderStroop.stroopTrialTime; //Restart timer(added to be able to replay the level before the timer stopped)
        //Baseline
        if (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq] == 0)
        {
            playTuto(); // the baseline works the same way as the tutorial
            return;
        }

        if (currentIndexSeq < (VariablesHolderStroop.stroopNumberTrials + 1))
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
            timer.gameObject.SetActive(true);
            whiteBackgrounds.gameObject.SetActive(true);
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
                //Debug.Log(all_Info);
                string[] info_Line = all_Info.Split('\n');
                //the starting line according to the level and read this line
                line = 2 * VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq] - 1;

                question = info_Line[line].Split(';');
                n_question_fixed = 0;
            }

            // Prepare the right difficulty
            Response.CreateCheckpoint("Difficulty: " + VariablesHolderStroop.stroopSequence[currentIndexSeq] + " " + VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq].ToString());
            Response.TriggerArduino("2");
            switch (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq])
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
            // Start the timer ("Update" function is executed)
           
            flagBeginTimer = true;
        }
    }

    public void playInstruction()
    // Called by the "Instruction" button or "Continue" button
    // Play the right intruction according to the sequence
    {
        
        if (currentIndexSeq < (VariablesHolderStroop.stroopNumberTrials + 1)){
            if (VariablesHolderStroop.stroopSequence[currentIndexSeq] != "Single Task (Walk)")
            {
                canvasChercheurJeu.gameObject.SetActive(false);
                questionHolder.gameObject.SetActive(false);
                canvasParticipantInstructions.gameObject.SetActive(true);
                canvasChercheurInstructions.gameObject.SetActive(true);

                //Afficher les boutons play et/ou tutorial
                if (flagTuto == true && flagRestart == false)
                {
                    playTutoButton.gameObject.SetActive(false);
                    playButton.gameObject.SetActive(true);
                }
                if (flagTuto == false && flagRestart == false)
                {
                    playTutoButton.gameObject.SetActive(true);
                    playButton.gameObject.SetActive(true);
                }

                if (flagRestart == true)
                {
                    if (flagTuto == false)
                    {
                        playTutoButton.gameObject.SetActive(false);
                        playButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        playTutoButton.gameObject.SetActive(true);
                        playButton.gameObject.SetActive(false);
                    }
                    flagRestart = false;
                }

                instructionButton.gameObject.SetActive(true);

                // Display the instruction to the participant's view and the level number
                switch (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq])
                {
                    case 0:
                        playTutoButton.gameObject.SetActive(false);
                        instructionDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the written color. \n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "CONTROL";
                        LevelDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text =  " CONTROL ";
                        break;
                    case 1:
                        instructionDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the color of the rectangle.\n  Are you ready ?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + "\n Difficulty: 1";
                        LevelDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + " Difficulty: 1 ";

                        break; 

                    case 2:
                        instructionDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the written color. \n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + "\n Difficulty: 2 ";
                        LevelDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + " Difficulty: 2 ";
                        break;

                    case 3:
                        instructionDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the color of the word. that the letters are printed in and not the written color.\n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + "\n Difficulty: 3";
                        LevelDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + " Difficulty: 3 ";

                        break;

                    case 4:
                        instructionDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "If the text is framed, select the written color. Otherwise, select the color of the word.\n \n Are you ready?";
                        textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + "\n Difficulty: 4 ";
                        LevelDifficulty.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + currentIndexSeq.ToString() + " Difficulty: 4 ";

                        break;
                }
                
                instructionButton.gameObject.SetActive(false);
                textCalibraton.gameObject.SetActive(false);
            }
            else
            {
                // Do things for single task (for now, only write "single task")
                Response.CreateCheckpoint("Single Task (Walk)");
                buttonContinue.gameObject.SetActive(false);
                buttonRestart.gameObject.SetActive(false);
                selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text = "SINGLE TASK (Walk)";
                BackgroundImage.gameObject.SetActive(false);
                Rectangle.gameObject.SetActive(false);
                questionHolder.gameObject.SetActive(true);
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "SINGLE TASK (Walk)";
                textLevel.GetComponent<TMPro.TextMeshProUGUI>().text = "SINGLE TASK (Walk)";
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Color.white;
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
            Response.TriggerArduino("3");
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
            questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
            questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Color.white;
            greenButton.gameObject.SetActive(false);
            redButton.gameObject.SetActive(false);
            blueButton.gameObject.SetActive(false);
            buttonQuit.gameObject.SetActive(true);
            buttonNew.gameObject.SetActive(true);
            timer.gameObject.SetActive(false);
            whiteBackgrounds.gameObject.SetActive(false);
        }
    }


    public void playTuto()
    {
        flagTuto = true;
        n_question_fixed = 0;

        if (currentIndexSeq < (VariablesHolderStroop.stroopNumberTrials + 1))
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
            averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Time";
            totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = "Results";
            buttonContinue.gameObject.SetActive(false);
            buttonQuit.gameObject.SetActive(false);
            buttonNew.gameObject.SetActive(false);
            timer.gameObject.SetActive(false);
            buttonRestart.gameObject.SetActive(false);
            WhitBgTL.gameObject.SetActive(false);
            TextAsset txt;

            if (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq] == 0)
            {
                txt = (TextAsset)Resources.Load("Baseline", typeof(TextAsset));
                line = 1;
                flagTuto = false;

            }
            else
            {
                txt = (TextAsset)Resources.Load("tutorial", typeof(TextAsset)); // change name
                line = 2 * VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq] - 1;
            }
  
            string all_Info = txt.text;
            string[] info_Line = all_Info.Split('\n');

            //the starting line according to the difficulty and read this line
            
            question = info_Line[line].Split(';');


            // Prepare the right difficulty
            Response.CreateCheckpoint("Difficulty: " + VariablesHolderStroop.stroopSequence[currentIndexSeq] + " " + VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq].ToString());
            Response.TriggerArduino("2");
            switch (VariablesHolderStroop.stroopSequenceLevels[currentIndexSeq])
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
        }
            // If there's not time left
        if (timeValue <=0 || end_of_trial == true )
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
                //Debug.Log(String.Join(",", responseTimes.Select(x => x.ToString()).ToArray()));
                averageResponseTime.GetComponent<TMPro.TextMeshProUGUI>().text = "Average Time: " + Math.Round(Queryable.Average(responseTimes.AsQueryable()),2).ToString() + " sec";
                Response.CreateCheckpoint("Average Response Time: " + Queryable.Average(responseTimes.AsQueryable()).ToString());
                // Show the button "Continue" (researcher's view)
                buttonContinue.gameObject.SetActive(true);                buttonRestart.gameObject.SetActive(true);
                // Play a sound
                beep.Play(); 

                // Change the text of the questionHolder (player's view)
                BackgroundImage.gameObject.SetActive(false);
                Rectangle.gameObject.SetActive(false);
                questionHolder.gameObject.SetActive(true);
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "WAIT FOR NEXT LEVEL";
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
                questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Color.white;
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
                if (flagTuto == false)
                {
                    currentIndexSeq += 1;
                }
                // Change the flag to compute the result only one time

                flagBeginTimer = false;
                end_of_trial = false;
                timeValue = VariablesHolderStroop.stroopTrialTime;
               
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
        if (flagTuto == false)
        {
            currentIndexSeq--;

        }
        
        flagRestart = true;
        numCorrectAnswers = 0;
        numTotalAnswers = 0;
        selectedAnswers = new List<string>(); // Answers selected by the participant
        correctAnswers = new List<string>(); // Correct answers 
        responseTimes = new List<double>();
        
        playInstruction();
    }

 

}