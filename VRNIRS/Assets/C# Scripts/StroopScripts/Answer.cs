using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using TMPro;

public class Answer : MonoBehaviour {

    public GameObject selectedAnswersShown; // List of selected answers shown in the searcher's view
    public GameObject colorButton; // Button selected by the participant (RED, BLUE or GREEN)

    public static string color; // Color of the button (RED, BLUE or GREEN)

    public void changeText()
    {
		// Find the color of the selected button
		color = colorButton.GetComponent<Text>().text;
        // Add the answer selected by the participant (RED, BLUE or GREEN) to the list of selectedAnswers
        Questions.selectedAnswers.Add(color);
        // Add the selected answer to the list shown in the searcher's view
        selectedAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (color + " ");
		// Increase the total number of answers
        Questions.numTotalAnswers += 1;
		// Create and show a new random question
        Questions.Instance.CreateNewRandomQuestion();
    }
}
