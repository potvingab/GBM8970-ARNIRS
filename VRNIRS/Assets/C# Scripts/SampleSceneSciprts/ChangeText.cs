using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {
	public GameObject Text;

	public void Update () {
		Text.GetComponent<Text>().text = VariablesHolderStroop.stroopTrialTime.ToString() + " secs, " + VariablesHolderStroop.stroopNumberTrials.ToString() + " trials \nSequence: " + String.Join(", ", VariablesHolderStroop.stroopSequence.ToArray()) + "\nLevels: " + String.Join(", ", VariablesHolderStroop.stroopSequenceLevels.Select(x => x.ToString()).ToArray()) + "\nMode: " + VariablesHolderStroop.stroopGameMode;
	}
}
