using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour {

    public static bool bool_Square;
    // Objectfs in participant's view
    public GameObject questionHolder;
    public Image BackgroundImage; // New variable difficulty 1 
    public Image Rectangle; // New variable difficulty 4

    // Pages of the scene
    public GameObject correctAnswersShown;
    public GameObject canvasChercheurInstructions;
    public GameObject canvasParticipantInstructions;
    public GameObject canvasChercheurJeu;
    public GameObject canvasParticipantJeu;

    // An instance is needed to use the method "Difficulty" in other scripts
    public static Difficulty Instance;
    void Awake()
    {
        Instance = this;
        canvasChercheurInstructions.SetActive(true);
        canvasParticipantInstructions.SetActive(true);
        canvasChercheurJeu.SetActive(false);
        canvasParticipantJeu.SetActive(false);
    }

    int file_convert(string color)
    {
        int index = 0;
        switch (color)
        {
            case "G":
                index = 0;
                break;
            case "R":
                index = 1;
                Debug.Log(index);
                break;

            case "B":
                index = 2;
                break;
        }
        return index;
    }
    // Difficulty 0: Negative Control 
    public void BaseLine()
    {
        Debug.Log("BaseLine");
        // setActive the right components
        BackgroundImage.gameObject.SetActive(false);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);
       
        if (Questions.question[Questions.n_question_fixed].Split(',')[0] != "R" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "B" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "G")
        {
            Debug.Log("end of trial");
            Questions.timeValue = 0;
            return;
        }
        Questions.indexQuestion = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[0]);
        Questions.n_question_fixed++;
     


        // Change the text of questionHolder to the  question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = Questions.possibleQuestions[Questions.indexQuestion];
        
        // Add the correct answer to the list correctAnswers
        Questions.correctAnswers.Add(Questions.possibleQuestions[Questions.indexQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (Questions.possibleQuestions[Questions.indexQuestion] + " ");
        
        // Change the color of questionHolder to the color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Questions.possibleColors[Questions.indexQuestion];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Questions.possibleColors[Questions.indexQuestion];
        Questions.timeStartQuestion = DateTime.Now;
    }

    // Difficulty 1: Background Color
    public void backgroundColor()
    {
        // TODO: Instructions = "Select the color of the rectangle.\n Are you ready?"
        Debug.Log("backgroundColor");

        // setActive the right components
        BackgroundImage.gameObject.SetActive(true);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(false);

        //Fixed sequence or tutorial
        if (VariablesHolderStroop.gameMode == "Fixed" || Questions.flagTuto == true)
        {
            Debug.Log(Questions.question[Questions.n_question_fixed].Split(',')[0]);

            if (Questions.question[Questions.n_question_fixed].Split(',')[0] != "R" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "B" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "G")
            {
                Debug.Log("end of trial");

                Questions.timeValue = 0;
                return;

            }
            
            Questions.indexColor = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[0]);
            Questions.n_question_fixed++;
        }
        //Random sequence
        else
        {
            // Sample random indices between 0 and 2
            Questions.indexColor = UnityEngine.Random.Range(0, 3);

        }
        // Change the color of the backgroundColor to the random color
        BackgroundImage.color = Questions.possibleColors[Questions.indexColor];
        //Debug.Log(Questions.possibleColors[Questions.indexColor]);
        // Add the correct answer to the list correctAnswers
        Questions.correctAnswers.Add(Questions.possibleQuestions[Questions.indexColor]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (Questions.possibleQuestions[Questions.indexColor] + " ");
        Questions.timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown; True response: " + Questions.possibleQuestions[Questions.indexColor]);
        Response.TriggerArduino("0");
        return;
    }

    // Difficulty 2: Black Text
    public void blackText()
    {
        // TODO: Instructions = "Select the written color.\n Are you ready?"
        Debug.Log("blackText");

        // setActive the right components
        BackgroundImage.gameObject.SetActive(false);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        //Fixed sequence or tutorial
        if (VariablesHolderStroop.gameMode == "Fixed" || Questions.flagTuto == true)
        {
            if (Questions.question[Questions.n_question_fixed].Split(',')[0] != "R" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "B" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "G")
            {
                Debug.Log("end of trial");
                //Questions.end_of_trial = true;
                Questions.timeValue = 0;
                return;
            }
            Debug.Log(Questions.question[Questions.n_question_fixed].Split(',')[0]);
            Questions.indexQuestion = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[0]);
            Questions.n_question_fixed++;
        }
        //Random sequence
        else
        {
            // Sample random indices between 0 and 2
            Questions.indexQuestion = UnityEngine.Random.Range(0, 3);
        }

        // Change the color of questionHolder to the black
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Questions.possibleColors[Questions.indexColor]; 
        // Change the text of questionHolder to the question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = Questions.possibleQuestions[Questions.indexQuestion];
        //Debug.Log(possibleQuestions[indexQuestion]);
        // Add the correct answer to the list correctAnswers
        Questions.correctAnswers.Add(Questions.possibleQuestions[Questions.indexQuestion]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (Questions.possibleQuestions[Questions.indexQuestion] + " ");
        Questions.timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + Questions.possibleQuestions[Questions.indexQuestion]);
        Response.TriggerArduino("0");
        return;
    }


    // Difficulty 3: Ink Color (not the written color)
    public void inkColor()
    {
        // TODO: Instructions = "Select the color of the ink that the letters are printed in and not the written color.\n Are you ready?"
        Debug.Log("CreateQuestion");

        // Set Active the right components
        BackgroundImage.gameObject.SetActive(false);
        Rectangle.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);

        //Fixed sequence or tutorial
        if (VariablesHolderStroop.gameMode == "Fixed" || Questions.flagTuto == true)
        {
            Debug.Log(Questions.question[Questions.n_question_fixed].Split(',')[0]);

            if (Questions.question[Questions.n_question_fixed].Split(',')[0] != "R" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "B" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "G")
            {
                Debug.Log("end of trial");
                //Questions.end_of_trial = true;
                Questions.timeValue = 0;
                return;

            }
            Debug.Log(Questions.question[Questions.n_question_fixed].Split(',')[0]);
            Questions.indexColor = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[0]);
            Questions.indexQuestion = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[1]);
            Questions.n_question_fixed++;
        }

        //Ramdom sequence
        else
        {
            // Sample random indices between 0 and 2
            Questions.indexQuestion = UnityEngine.Random.Range(0, 3);
            Questions.indexColor = UnityEngine.Random.Range(0, 3);
            //Making sure the written text and text color are not the same
            while (Questions.indexQuestion == Questions.indexColor)
            {
                Debug.Log("same");
                Questions.indexQuestion = UnityEngine.Random.Range(0, 3);
                Questions.indexColor = UnityEngine.Random.Range(0, 3);
            }
        }

        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = Questions.possibleQuestions[Questions.indexQuestion];
        //Debug.Log(Questions.possibleQuestions[Questions.indexQuestion]);
        // Add the correct answer to the list correctAnswers
        Questions.correctAnswers.Add(Questions.possibleQuestions[Questions.indexColor]);
        correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (Questions.possibleQuestions[Questions.indexColor] + " ");
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Questions.possibleColors[Questions.indexColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Questions.possibleColors[Questions.indexColor];
        Questions.timeStartQuestion = DateTime.Now;

        Response.CreateCheckpoint("Question shown. True response: " + Questions.possibleQuestions[Questions.indexColor]);
        Response.TriggerArduino("0");
        return;
    }


    // Difficulty 4: Ink Color by default, Written Color if rectangle
    public void randomRectangle()
    {
        
        // TODO: Instructions = "By default, select the color of the ink that the letters are printed in and not the written color.\n If the text is framed, select the written color.\n Are you ready?"
        Debug.Log("randomRectangle");

        // Set Active the right components
        BackgroundImage.gameObject.SetActive(false);
        questionHolder.gameObject.SetActive(true);
        //Fixed sequence or tutorial
        if (VariablesHolderStroop.gameMode == "Fixed" || Questions.flagTuto == true)
        {
            Debug.Log(Questions.question[Questions.n_question_fixed].Split(',')[0]);

            if (Questions.question[Questions.n_question_fixed].Split(',')[0] != "R" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "B" && Questions.question[Questions.n_question_fixed].Split(',')[0] != "G")
            {
                Debug.Log("end of trial");
                Questions.timeValue = 0;
                Debug.Log(Questions.timeValue);
                //Questions.end_of_trial = true;
                return;

            }
            Questions.indexColor = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[0]);
            Questions.indexQuestion = file_convert(Questions.question[Questions.n_question_fixed].Split(',')[1]);

            switch (Questions.question[Questions.n_question_fixed].Split(',')[2])
            {
                case "0":
                    bool_Square = false;
                    break;
                case "1":
                    bool_Square = true;
                    break;
            }
            Questions.n_question_fixed++;

        }

        //Random sequence
        else
        {
            // Sample random indices either true or false 
            bool_Square = UnityEngine.Random.Range(0, 2) > 0;

            Debug.Log(bool_Square);
            // Sample random indices between 0 and 2
            Questions.indexQuestion = UnityEngine.Random.Range(0, 3);
            Questions.indexColor = UnityEngine.Random.Range(0, 3);
            //Making sure the written text and text color are not the same
            while (Questions.indexQuestion == Questions.indexColor)
            {
                Debug.Log("same");
                Questions.indexQuestion = UnityEngine.Random.Range(0, 3);
                Questions.indexColor = UnityEngine.Random.Range(0, 3);
            }

        }
        Rectangle.gameObject.SetActive(bool_Square);
        // Change the text of questionHolder to the random question
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().text = Questions.possibleQuestions[Questions.indexQuestion];
        //Debug.Log(Questions.possibleQuestions[Questions.indexQuestion]);
        // Change the color of questionHolder to the random color
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().color = Questions.possibleColors[Questions.indexColor];
        questionHolder.GetComponent<TMPro.TextMeshProUGUI>().faceColor = Questions.possibleColors[Questions.indexColor];
                
        if (bool_Square == false)
        {
            // Add the color as the correct answer to the list correctAnswers
            Questions.correctAnswers.Add(Questions.possibleQuestions[Questions.indexColor]);
            Response.CreateCheckpoint("Question shown. True response: " + Questions.possibleQuestions[Questions.indexColor]);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (Questions.possibleQuestions[Questions.indexColor] + " ");
        }
        else
        {
            // Add the text as the correct answer to the list correctAnswers
            Questions.correctAnswers.Add(Questions.possibleQuestions[Questions.indexQuestion]);
            Response.CreateCheckpoint("Question shown. True response: " + Questions.possibleQuestions[Questions.indexQuestion]);
            correctAnswersShown.GetComponent<TMPro.TextMeshProUGUI>().text += (Questions.possibleQuestions[Questions.indexQuestion] + " ");

        }
        Questions.timeStartQuestion = DateTime.Now;
        Response.TriggerArduino("0");
        return;
    }
}
