using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using TMPro;

public class RED : MonoBehaviour {

    public GameObject answer;
    List<string> question = new List<string> { "BLUE", "RED", "GREEN" };
    public static Button redButton;
    public GameObject text;

    public void changeText()
    {
        Debug.Log("You clicked the red button!");
        Questions.selectedAnswers.Add("RED");
        Questions.displayAnswers.Add("RED");
        answer.GetComponent<TMPro.TextMeshProUGUI>().text += "RED ";

        Questions.total += 1;
        Questions.randQuestion = Random.Range(0, 3);
        Questions.randColor = Random.Range(0, 3);
        text.GetComponent<TMPro.TextMeshProUGUI>().text = question[Questions.randQuestion];
        Questions.correctAnswers.Add(question[Questions.randQuestion]);
        text.GetComponent<TMPro.TextMeshProUGUI>().color = Questions.colors[Questions.randColor];
        Debug.Log(Questions.results);


    }
}
