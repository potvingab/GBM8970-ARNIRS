using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInstruction : MonoBehaviour {

    public GameObject textInstruction;

    public void DisplayIntruction()
    {
      
        if(VariablesHolder.sequenceNBack[TimeSpawner.currentLevel] == 1)
            textInstruction.GetComponent<TMPro.TextMeshProUGUI>().text = "Press the button in your RIGHT hand if the objects is the SAME as the one you just saw \n or press the button in your LEFT hand if it is NOT. \n \n Are you ready?";
        if (VariablesHolder.sequenceNBack[TimeSpawner.currentLevel] == 2)
            textInstruction.GetComponent<TMPro.TextMeshProUGUI>().text = "Press the button in your RIGHT hand if the objects is the SAME as the one you saw 2 objects before \n or press the button in your left hand if it is NOT. \n \n Are you ready?";

        textInstruction.SetActive(true);

    }
}
