using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{
    public GameObject errorTextInstruc;
	public GameObject errorTextGame;
	public GameObject errorButtonInstruc;
	public GameObject errorButtonGame;
	public GameObject errorBgInstruc;
	public GameObject errorBgGame;

    void OnEnable()
	{
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
	{
        Application.logMessageReceived -= HandleLog;
    }

	// If an error or exception occurs, write it in the bottom of the searcher's screen
    void HandleLog(string logString, string stackTrace, LogType type)
	{
		// Only write errors or exceptions that don't contain "index" or "nearest" (Meta minor errors that can be ignored)
        if ((type == LogType.Error) || (type == LogType.Exception) && (!logString.Contains("index") && (!logString.Contains("nearest"))))
		{
			errorTextGame.GetComponent<Text>().text = "Error: " + logString;
            errorTextInstruc.GetComponent<Text>().text = "Error: " + logString;
			// Custom message for Meta-related serious errors
			if (logString.Contains("meta"))
			{
				errorTextGame.GetComponent<Text>().text = "Error: Meta does not function properly. Please connect the headset or read the manual.";
            	errorTextInstruc.GetComponent<Text>().text = "Error: Meta does not function properly. Please connect the headset or read the manual.";
			}
            errorTextInstruc.gameObject.SetActive(true);
			errorTextGame.gameObject.SetActive(true);
			errorButtonInstruc.gameObject.SetActive(true);
			errorButtonGame.gameObject.SetActive(true);
			errorBgInstruc.gameObject.SetActive(true);
			errorBgGame.gameObject.SetActive(true);
        }   
    }

	// Hide the error message
    public void Dismiss()
	{
        errorTextInstruc.gameObject.SetActive(false);
		errorTextGame.gameObject.SetActive(false);
		errorButtonInstruc.gameObject.SetActive(false);
		errorButtonGame.gameObject.SetActive(false);
		errorBgInstruc.gameObject.SetActive(false);
		errorBgGame.gameObject.SetActive(false);
    }
}