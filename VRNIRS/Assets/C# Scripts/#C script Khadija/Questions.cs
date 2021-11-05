using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.IO;


public class Questions : MonoBehaviour
{
    List<string> question = new List<string> { "BLUE", "RED", "GREEN"};
    List<string> correctAnswer = new List<string> { "BLUE", "RED", "GREEN" };
    public static int randQuestion = -1; //Empêche le changement de l'affichage constant des questions
    public static int index = 0;
    public static string selectedAnswers;
    public static int results = 0;
    public static int iteration = 5;

    void Start()
    {
        //int i = 0;
        //while (i <= iteration & randQuestion == -1)
        //{
        //    randQuestion = Random.Range(0, 3);
        //    GetComponent<TMPro.TextMeshProUGUI>().text = question[randQuestion];


        //    if (correctAnswer[index] == selectedAnswers)
        //    {
        //       results += 1;

        //     }
        //    i++;
        //    Debug.Log(selectedAnswers);
        //}
    }

    void Update()
    {
        if (randQuestion == -1)
        {
            randQuestion = Random.Range(0, 3);
        }

        if (randQuestion > -1)
        {
            GetComponent<TMPro.TextMeshProUGUI>().text = question[randQuestion];
            index = randQuestion;
        }

        if (correctAnswer[index] == selectedAnswers)
        {
            results += 1;
            selectedAnswers = "x";
         
        }
        //foreach (var i in selectedAnswers)
        //{
        //    Debug.Log(i);
        //}
        Debug.Log(results);
    }

}