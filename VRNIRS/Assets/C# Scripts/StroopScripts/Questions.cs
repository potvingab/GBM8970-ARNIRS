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

    // Objectfs in searcher's view
    public GameObject timer;
    public GameObject totalResults;
    public Button buttonContinue;
    public GameObject correctAnswersShown;

    // Parameters from the menu scene
    public static float timeValue = VariablesHolderStroop.stroopTrialTime; 

    // New variables used
    public static List<string> possibleQuestions = new List<string>{ "BLUE", "RED", "GREEN"};
    public static Color[] possibleColors = { Color.green, Color.red, Color.blue };
    public static int indexRandQuestion;
    public static int indexRandColor;
    public static List<string> selectedAnswers = new List<string>(); // Answers selected by the participant
    public static List<string> correctAnswers = new List<string>(); // Correct answers (created by CreateNewRandomQuestion)
    public static int numCorrectAnswers = 0;
    public static int numTotalAnswers = 0;
    public static bool flagEndTimer = false;
    public static bool flagBeginTimer = false;

    // An instance is needed to use the method "CreateNewRandomQuestion" in other scripts
    void Awake()
    {
        Instance = this;
    }

    public void CreateNewRandomQuestion()
    {
        // Sample random indices between 0 and 2
        indexRandQuestion = Random.Range(0, 3);
        indexRandColor = Random.Range(0, 3);
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = possibleQuestions[indexRandQuestion];
        Debug.Log(possibleQuestions[indexRandQuestion]);
        // Add the correct answer to the list correctAnswers
        correctAnswers.Add(possibleQuestions[indexRandQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (possibleQuestions[indexRandQuestion] + " ");
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = possibleColors[indexRandColor];
    }

    void Start()
    {
        // The button "Continue" and the result are hidden in the beginning
        buttonContinue.gameObject.SetActive(false);
        totalResults.gameObject.SetActive(false);
        CreateNewRandomQuestion();
    }

    void Update()
    {
        if (flagBeginTimer == true)
        {
            // Each second, if there's still time on the timer, print the time and decrease it
            if (timeValue > 0)
            {
                timer.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Time left: {0:00}", Mathf.FloorToInt(timeValue));
                timeValue -= Time.deltaTime;
            }
            // If there's not time left
            else
            {
                if (flagEndTimer == false)
                {
                    // Compare the correct and selected answers, and compute the result (numCorrectAnswers/numTotalAnswers)
                    for (int i=0; i<selectedAnswers.Count; i++)
                    {
                        if (correctAnswers[i] == selectedAnswers[i])
                        {
                            numCorrectAnswers += 1;
                        }
                    }
                    // Show the result
                    totalResults.gameObject.SetActive(true);
                    totalResults.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Results: {0:00}/{1:00}", numCorrectAnswers, numTotalAnswers);
                    // Show the button "Continue"
                    buttonContinue.gameObject.SetActive(true);
                    // Change the text of the questionHolder to "END"
                    questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "END";
                    questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
                    // Hide the green, red and blue buttons
                    greenButton.gameObject.SetActive(false);
                    redButton.gameObject.SetActive(false);
                    blueButton.gameObject.SetActive(false);
                    // Change the flag to compute the result only one time
                    flagEndTimer = true;
                }
            }
        }
        
    }
    public void EndTest()
    {
        // Return to first scene
        SceneManager.LoadScene(0);
        // Default values to start another trial
        buttonContinue.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(true);
        redButton.gameObject.SetActive(true);
        blueButton.gameObject.SetActive(true);
        flagEndTimer = false;
        timeValue = VariablesHolderStroop.stroopTrialTime;
        numCorrectAnswers = 0;
        numTotalAnswers = 0;
    }
    public void StartTimer()
    {
        flagBeginTimer = true;
    }

}