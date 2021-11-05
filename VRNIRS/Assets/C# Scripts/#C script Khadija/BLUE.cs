using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class BLUE : MonoBehaviour {

    public Button redButton;
    void Start()
    {
        Button btnState = redButton.GetComponent<Button>();
        btnState.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void TaskOnClick()
    {
        //Debug.Log("You have clicked the blue button!");
        Questions.randQuestion = -1;
        Questions.selectedAnswers = "BLUE";
    }
}
