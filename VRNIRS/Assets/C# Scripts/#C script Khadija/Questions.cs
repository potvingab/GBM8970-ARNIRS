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
    public GameObject text;
    public GameObject timer;
    public GameObject totalresults;
    public Button buttonContinue;
    public Button redButton;
    public Button greenButton;
    public Button blueButton;

    public static float timeValue = 10;
    List<string> question = new List<string> { "BLUE", "RED", "GREEN"};
    public static string selectedAnswers;
    public static int results = 0;
    public static int total = 0;
    public static int randQuestion;
    public static int randColor;
    public static Color[] colors = { Color.green, Color.red, Color.blue };
    public static List<string> displayAnswers = new List<string>(); //liste de réonse pour enregistrement

    void Start()
    {

        buttonContinue.gameObject.SetActive(false);
        totalresults.gameObject.SetActive(false);
        randQuestion = Random.Range(0, 3);
        randColor = Random.Range(0, 3);
        text.GetComponent<TMPro.TextMeshProUGUI>().text = question[randQuestion];
        text.GetComponent<TMPro.TextMeshProUGUI>().color = colors[randColor];

    }


    void Update()
    {
        if (timeValue >= 0)
        {
            timer.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Time left: {0:00}", Mathf.FloorToInt(timeValue));
            timeValue -= Time.deltaTime;
        }
        else
        {
            totalresults.gameObject.SetActive(true);
            totalresults.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Results: {0:00}/{1:00}", results,total);
            buttonContinue.gameObject.SetActive(true);
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "END";
            text.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
            greenButton.gameObject.SetActive(false);
            redButton.gameObject.SetActive(false);
            blueButton.gameObject.SetActive(false);


        }
    }
    public void EndTest()
    {
        SceneManager.LoadScene(0);
        buttonContinue.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(true);
        redButton.gameObject.SetActive(true);
        blueButton.gameObject.SetActive(true);

    }

}