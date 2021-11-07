using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class RED : MonoBehaviour {

    public Button redButton;
	void Start () {
        Button btnState = redButton.GetComponent<Button>();
        btnState.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void TaskOnClick()
    {
        Debug.Log("You have clicked the red button!");
        Questions.randQuestion = -1;
        Questions.selectedAnswers = "RED";
        //string createText = "RED" + Environment.NewLine;
        //File.AppendAllText(@"C:\Users\HP\Documents\4 année\Projet Intégrateur\answers.txt", createText);

   
    }
}
