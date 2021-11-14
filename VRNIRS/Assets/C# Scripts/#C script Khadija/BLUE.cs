using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using TMPro;
public class BLUE : MonoBehaviour {

    List<string> question = new List<string> { "BLUE", "RED", "GREEN" };
    List<string> correctAnswer = new List<string> { "BLUE", "RED", "GREEN" };
    public Button blueButton;
    public GameObject text;
    void Start()
    {
        //Button btnState = blueButton.GetComponent<Button>();
        //btnState.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void changeText()
    {
        Debug.Log("You have clicked the blue button!");
        Questions.selectedAnswers = "BLUE";
        if (correctAnswer[Questions.randQuestion] == Questions.selectedAnswers)
        {
            Questions.results += 1;

        }
        Questions.randQuestion = Random.Range(0, 3);
        Questions.randColor = Random.Range(0, 3);
        text.GetComponent<TMPro.TextMeshProUGUI>().text = question[Questions.randQuestion];
        text.GetComponent<TMPro.TextMeshProUGUI>().color = Questions.colors[Questions.randColor];
        Debug.Log(Questions.results);
    }
}
