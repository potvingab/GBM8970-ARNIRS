using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class Questions : MonoBehaviour
{
    public GameObject text;
    public GameObject timer;
    public static float timeValue = 10;
    List<string> question = new List<string> { "BLUE", "RED", "GREEN"};
    public static string selectedAnswers;
    public static int results = 0;
    public static int randQuestion;
    public static int randColor;
    public static Color[] colors = { Color.green, Color.red, Color.blue };
   
    void Start()
    {
        

        randQuestion = Random.Range(0, 3);
        randColor = Random.Range(0, 3);
        text.GetComponent<TMPro.TextMeshProUGUI>().text = question[randQuestion];
        text.GetComponent<TMPro.TextMeshProUGUI>().color = colors[randColor];

    }


    void Update()
    {
        if (timeValue >= 0)
        {
            timer.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("{0:00}", Mathf.FloorToInt(timeValue));
            timeValue -= Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

}