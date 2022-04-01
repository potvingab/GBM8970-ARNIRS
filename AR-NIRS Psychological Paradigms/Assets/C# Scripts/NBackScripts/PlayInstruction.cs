using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInstruction : MonoBehaviour {

    public GameObject textInstruction;
    public GameObject levelInfo;
    public static float timeRest = 0;
    public static bool flagTimerRest = false;
    public GameObject timerRest;
    public static PlayInstruction Instance;

    

    public void DisplayInstruction()
    {
        Debug.Log(VariablesHolder.sequence[TimeSpawner.currentLevel]);
        if (VariablesHolder.sequence[TimeSpawner.currentLevel].Contains("Single Task (Walk)"))
            textInstruction.GetComponent<TMPro.TextMeshProUGUI>().text = "Just keep walking \n \n Are you ready?";
            
        else if (VariablesHolder.sequenceNBack[TimeSpawner.currentLevel] == 2)
            textInstruction.GetComponent<TMPro.TextMeshProUGUI>().text = "Press the button in your RIGHT hand if the objects is the SAME as the one you saw 2 objects before \n or press the button in your left hand if it is NOT. \n \n Are you ready?";
        
        else if (VariablesHolder.sequenceNBack[TimeSpawner.currentLevel] == 1)
        //else
            textInstruction.GetComponent<TMPro.TextMeshProUGUI>().text = "Press the button in your RIGHT hand if the objects is the SAME as the one you just saw \n or press the button in your LEFT hand if it is NOT. \n \n Are you ready?";

        
        timerRest.gameObject.SetActive(true);
        timeRest = 0;

        if (VariablesHolder.sequence[TimeSpawner.currentLevel].Contains("Single Task (Walk)"))
        {
            levelInfo.GetComponent<TMPro.TextMeshProUGUI>().text = VariablesHolder.sequence[TimeSpawner.currentLevel];
        }

        else
        {
            levelInfo.GetComponent<TMPro.TextMeshProUGUI>().text = VariablesHolder.sequence[TimeSpawner.currentLevel] + "\n N-back = " + VariablesHolder.sequenceNBack[TimeSpawner.currentLevel];
        }
        levelInfo.SetActive(true);
    }
    void Update()
    {
        //if (flagTimerRest == true)
        if (timerRest.gameObject.activeSelf == true)
        {
            // Increase and show the resting time
            //timerRest.gameObject.SetActive(true);
            timerRest.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format(" Resting Time: {0:00}", Mathf.FloorToInt(timeRest));
            timeRest += Time.deltaTime*0.25f;
        }
        else
        {
            timeRest = 0;
        }
        
    }
    public void setFlagTimer()
    {
        timerRest.gameObject.SetActive(false);
        timeRest = 0;
    }
}