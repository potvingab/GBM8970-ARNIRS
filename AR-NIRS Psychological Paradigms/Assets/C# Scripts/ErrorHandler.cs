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

    void HandleLog(string logString, string stackTrace, LogType type)
	{
        
        if ((type == LogType.Error) || (type == LogType.Exception))
		{
            errorTextInstruc.gameObject.SetActive(true);
			errorTextGame.gameObject.SetActive(true);
			errorButtonInstruc.gameObject.SetActive(true);
			errorButtonGame.gameObject.SetActive(true);
			errorBgInstruc.gameObject.SetActive(true);
			errorBgGame.gameObject.SetActive(true);
            errorTextGame.GetComponent<Text>().text = "Error: " + logString;
            errorTextInstruc.GetComponent<Text>().text = "Error: " + logString;
        }   
    }

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