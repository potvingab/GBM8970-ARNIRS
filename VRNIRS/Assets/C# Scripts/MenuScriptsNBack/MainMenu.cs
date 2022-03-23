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
        int index = name.IndexOf(".txt");
        String masterFileName = name.Insert(index, "_Master");
        String arduinoFileName = name.Insert(index, "_Test_synchro_Arduino");
        String arFileName = name.Insert(index, "_Test_synchro_AR");
        using (StreamWriter sw = File.AppendText(masterFileName))
        {
            sw.Write("Parameter; " + "Filename; " + masterFileName + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolder.arduinoPort + "\n");
            sw.Write("Parameter; " + "Mode; " + VariablesHolder.gameMode + "\n");
            sw.Write("Parameter; " + "Number Trials; " + VariablesHolder.numberTrials.ToString() + "\n");
            sw.Write("Parameter; " + "Sequence; " + String.Join(",", VariablesHolder.sequence.ToArray()) + "\n");
            sw.Write("Parameter; " + "Sequence NBack; " + String.Join(", ", VariablesHolder.sequenceNBack.Select(x => x.ToString()).ToArray()) + "\n");
            sw.Write("Parameter; " + "Number Objects; " + VariablesHolder.numberOfObjects.ToString() + "\n");
            sw.Write("Parameter; " + "Speed; " + VariablesHolder.speed.ToString() + "\n");
        }
        using (StreamWriter sw = File.AppendText(arduinoFileName))
        {
            sw.Write("Parameter; " + "Filename; " + arduinoFileName + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolder.arduinoPort + "\n");
        }
        using (StreamWriter sw = File.AppendText(arFileName))
        {
            sw.Write("Parameter; " + "Filename; " + arFileName + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolder.arduinoPort + "\n");
        }
        TimeSpawner.CreateCheckpoint("End of Menu");
        SceneManager.LoadScene("N-back");
    }

    public void PlayStroop()
    {
        String name = VariablesHolderStroop.fileName;
        int index = name.IndexOf(".txt");
        String masterFileName = name.Insert(index, "_Master");
        String arduinoFileName = name.Insert(index, "_Test_synchro_Arduino");
        String arFileName = name.Insert(index, "_Test_synchro_AR");
        using (StreamWriter sw = File.AppendText(masterFileName))
        {
            sw.Write("Parameter; " + "Filename; " + VariablesHolderStroop.fileName + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolderStroop.arduinoPort + "\n");
            sw.Write("Parameter; " + "Trial Time; " + VariablesHolderStroop.trialTime.ToString() + "\n");
            sw.Write("Parameter; " + "Number Trials; " + VariablesHolderStroop.numberTrials.ToString() + "\n");
            sw.Write("Parameter; " + "Sequence; " + String.Join(",", VariablesHolderStroop.sequence.ToArray()) + "\n");
            sw.Write("Parameter; " + "Sequence Levels; " + String.Join(", ", VariablesHolderStroop.sequenceLevels.Select(x => x.ToString()).ToArray()) + "\n");
            sw.Write("Parameter; " + "Game Mode; " + VariablesHolderStroop.gameMode + "\n");
        }
        using (StreamWriter sw = File.AppendText(arduinoFileName))
        {
            sw.Write("Parameter; " + "Filename; " + arduinoFileName + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolderStroop.arduinoPort + "\n");
        }
        using (StreamWriter sw = File.AppendText(arFileName))
        {
            sw.Write("Parameter; " + "Filename; " + arFileName + "\n");
            sw.Write("Parameter; " + "Arduino Port; " + VariablesHolderStroop.arduinoPort + "\n");
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
