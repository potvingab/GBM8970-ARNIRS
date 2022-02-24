using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayNBack()
    {
        String name = VariablesHolder.fileName;
        String baseName = name.Replace(".txt", "");
        using (StreamWriter sw = File.AppendText(baseName + "_Master" + ".txt"))
        {
            sw.Write("Parameter; " + "Filename; " + baseName + "_Master" + ".txt" + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolder.arduinoPort + "\n");
            //sw.Write("Parameter; " + "Trial Time; " + VariablesHolderStroop.stroopTrialTime.ToString() + "\n");
            //sw.Write("Parameter; " + "Number Trials; " + VariablesHolderStroop.stroopNumberTrials.ToString() + "\n");
            //sw.Write("Parameter; " + "Sequence; " + String.Join(",", VariablesHolderStroop.stroopSequence.ToArray()) + "\n");
            //sw.Write("Parameter; " + "Sequence Levels; " + String.Join(", ", VariablesHolderStroop.stroopSequenceLevels.Select(x => x.ToString()).ToArray()) + "\n");
            //sw.Write("Parameter; " + "Game Mode; " + VariablesHolderStroop.stroopGameMode + "\n");
        }
        using (StreamWriter sw = File.AppendText(baseName + "_Test_synchro_Arduino" + ".txt"))
        {
            sw.Write("Parameter; " + "Filename; " + baseName + "_Test_synchro_Arduino" + ".txt" + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolder.arduinoPort + "\n");
        }
        TimeSpawner.CreateCheckpoint("End of Menu");
        SceneManager.LoadScene("N-back");
    }

    public void PlayStroop()
    {
        using (StreamWriter sw = File.AppendText(VariablesHolderStroop.fileName))
        {
            sw.Write("Parameter; " + "Filename: " + VariablesHolderStroop.fileName + "\n");
            sw.Write("Parameter; " + "Arduino Port: " + VariablesHolderStroop.arduinoPort + "\n");
            sw.Write("Parameter; " + "Trial Time: " + VariablesHolderStroop.stroopTrialTime.ToString() + "\n");
            sw.Write("Parameter; " + "Number Trials: " + VariablesHolderStroop.stroopNumberTrials.ToString() + "\n");
            sw.Write("Parameter; " + "Sequence: " + String.Join(",", VariablesHolderStroop.stroopSequence.ToArray()) + "\n");
            sw.Write("Parameter; " + "Sequence Levels: " + String.Join(", ", VariablesHolderStroop.stroopSequenceLevels.Select(x => x.ToString()).ToArray()) + "\n");
            sw.Write("Parameter; " + "Game Mode: " + VariablesHolderStroop.stroopGameMode + "\n");
        }
        Response.CreateCheckpoint("End of Menu");
        SceneManager.LoadScene("Stroop");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
