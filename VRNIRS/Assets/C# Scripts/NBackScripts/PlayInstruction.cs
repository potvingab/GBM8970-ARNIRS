using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInstruction : MonoBehaviour {

    public GameObject textInstruction;

    public void DisplayIntruction()
    {
        textInstruction.SetActive(true);

        textInstruction.GetComponent<TMPro.TextMeshProUGUI>().text = "Select the written color. \n Are you ready?";

    }
}
