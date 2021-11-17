using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using TMPro;
public class BLUE : MonoBehaviour {

    List<string> question = new List<string> { "BLUE", "RED", "GREEN" };
    public static Button blueButton;
    public GameObject text;
    public GameObject answer;

    public void changeText()
    {
        Debug.Log("You have clicked the blue button!");
        Questions.selectedAnswers.Add("BLUE");
        Questions.displayAnswers.Add("BLUE");
        answer.GetComponent<TMPro.TextMeshProUGUI>().text +=  "BLUE ";

        Questions.total += 1;
        Questions.randQuestion = Random.Range(0, 3);
        Questions.randColor = Random.Range(0, 3);
        text.GetComponent<TMPro.TextMeshProUGUI>().text = question[Questions.randQuestion];
        Questions.correctAnswers.Add(question[Questions.randQuestion]);
        text.GetComponent<TMPro.TextMeshProUGUI>().color = Questions.colors[Questions.randColor];
        Debug.Log(Questions.results);
    }
}
