using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta;
using Meta.HandInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;





public class Response : Interaction
{

    
    public GameObject selectedAnswersShown; // List of selected answers shown in the searcher's view
    public GameObject cube; // Button selected by the participant (RED, BLUE or GREEN)
    public string color;


    private HandFeature _handFeature; //follow the hand during a grab
    private GameObject _selectedGameObject;
    //private string color;


    //public static string color; // Color of the button (RED, BLUE or GREEN)       

    protected override void Engage() //when it's in the zone, hand close
    {
        _handFeature = GrabbingHands[0];

        if (_handFeature == null)

        {
            _selectedGameObject = Instantiate(cube); //create a clone to move  object

        }
        Debug.Log("grabbed");

        changeText();



       


    }
    protected override void Disengage()
    {
        if (_handFeature == null || _selectedGameObject == null)

        {

            return;
        }

        _selectedGameObject.SendMessage("detach");
        _selectedGameObject = null;

    }

    protected override void Manipulate()
    {
        return;
    }


   
    

    public void changeText()
    {
        Debug.Log("changetext");
        // Find the color of the selected button
        //color = "blue";
        //color = cube.GetComponent<TMPro.TextMeshProUGUI>().text;
        //colors = color.text;
        Debug.Log(color);
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

