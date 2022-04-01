using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool SameObject = false;
    public static bool SaveCondition = true;
    public static int clickPosition = -2;
    public static int endTime;
    public static int differenceTime;
    public string saveCurrentText = "";

    public GameObject[] orderObject;
    public TextMeshProUGUI gameObjectList;
    public TextMeshProUGUI gameObjectListTitle;

    public GameObject[] levelSelecters;
    public TextMeshProUGUI[] levelTextSelecters;

    public TextMeshProUGUI listOfResultsWanted;

    public GameObject pauseMenuUI;
    public GameObject pauseMenuUIHead;

    public GameObject endScreen;
    public GameObject endScreenLevel;
    public GameObject endScreenFinal;
    
    public static string[] clicks;

    public static string[] reactionTime;
    public static string[] allLevelResults;

    public static string currentLevelString = "";
    public static int[] currentLevelObjects;
   
    public void Awake()
    {
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
        currentLevelObjects = new int[VariablesHolder.numberOfObjects];
        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        { 
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        allLevelResults = new string[VariablesHolder.sizeOfArray];
        for (int nLevelResults = 0; nLevelResults < VariablesHolder.sizeOfArray; nLevelResults++)
        {
            allLevelResults[nLevelResults] = "";
        }
        GameIsPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentLevelString = "--";
        currentLevelObjects = TimeSpawner.allArrayInt[TimeSpawner.currentLevel];
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                pauseMenuUIHead.SetActive(true);
                Resume();
            }
            else
            {
                pauseMenuUIHead.SetActive(false);
                Pause();
            }
        }
        gameObjectListTitle.text = TimeSpawner.levelNames[TimeSpawner.currentLevel];

        if (!SameObject)
        {
            if (clickPosition >= 0 && clickPosition < VariablesHolder.numberOfObjects)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    currentLevelString = "Same";
                    TimeSpawner.reactionTime.Stop();
                    endTime = DateTime.Now.Millisecond;
                    differenceTime = endTime - TimeSpawner.startTime;
                    reactionTime[clickPosition] = TimeSpawner.reactionTime.ElapsedMilliseconds.ToString();
                    UnityEngine.Debug.Log(differenceTime + " : " + TimeSpawner.reactionTime.ElapsedMilliseconds);
                    SameObject = true;
                    clicks[clickPosition] = currentLevelString;

                    TimeSpawner.CreateCheckpoint("Same");
                    TimeSpawner.TriggerArduino("0");
                    TimeSpawner.ArduinoCheckpoint("Answer");
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    currentLevelString = "Diff";
                    TimeSpawner.reactionTime.Stop();
                    endTime = DateTime.Now.Millisecond;
                    differenceTime = Math.Abs(endTime - TimeSpawner.startTime);
                    reactionTime[clickPosition] = TimeSpawner.reactionTime.ElapsedMilliseconds.ToString();
                    UnityEngine.Debug.Log(differenceTime + " : " + TimeSpawner.reactionTime.ElapsedMilliseconds);
                    SameObject = true;
                    clicks[clickPosition] = currentLevelString;

                    TimeSpawner.CreateCheckpoint("Diff");
                    TimeSpawner.TriggerArduino("0");
                    TimeSpawner.ArduinoCheckpoint("Answer");
                }
            }
            ChangeText();
        }

        if (TimeSpawner.order == VariablesHolder.numberOfObjects + 1 && SaveCondition)
        {
            GetPercentageForLevel();
            SaveLevelInfo();
            SaveCondition = false;
        }

    }

    public void TriggerInstructions()
    {
        TimeSpawner.TriggerArduino("0");
        TimeSpawner.CreateCheckpoint("Trigger Resting Time");
        TimeSpawner.ArduinoCheckpoint("Trigger Resting Time");
    }

    public void SaveLevelInfo()
    {
        string levelString = "";
        if (TimeSpawner.currentLevel < 7)
        {
            levelString += "T" + (TimeSpawner.currentLevel + 1);
        }
        else
        {
            levelString += (TimeSpawner.currentLevel - 6);
        }
        //if single walk
        if (TimeSpawner.allArrayInt[TimeSpawner.currentLevel][0] == 9)
        {
            levelString += ",1,100,0,0";
            for (int i = 0; i < VariablesHolder.numberOfObjects; i++)
            {
                levelString += ",0,0,0,1,0";
            }
        }
        else
        {
            string postAccuracy = "";
            int accuracy = VariablesHolder.numberOfObjects - VariablesHolder.sequenceNBack[TimeSpawner.currentLevel] ;
            int n = accuracy;
            int meanRT = 0;
            int weightedRT = 0;
            int correctAnswers = 0;

            for (int i = 0; i < VariablesHolder.sequenceNBack[TimeSpawner.currentLevel]; i++)
            {
                postAccuracy += "," + (TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i] + 1);
                postAccuracy += ",0";
                if (clicks[i] == "Same")
                {
                    postAccuracy += "," + 1 + ",0";
                }
                else if (clicks[i] == "Diff")
                {
                    postAccuracy += "," + 2 + ",1";
                }
                else
                {
                    postAccuracy += "," + 0 + ",1";
                }
                postAccuracy += "," + reactionTime[i];
            }

            for (int i = VariablesHolder.sequenceNBack[TimeSpawner.currentLevel]; i < VariablesHolder.numberOfObjects; i++)
            {
                postAccuracy += "," + (TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i] + 1);

                int rt = 2000;
                if (reactionTime[i] != "--")
                {
                    rt = int.Parse(reactionTime[i]);
                }
                if (TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i] == TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i - VariablesHolder.sequenceNBack[TimeSpawner.currentLevel]])
                {
                    postAccuracy += ",1";
                    if (clicks[i] == "Same")
                    {
                        postAccuracy += "," + 1 + ",1";
                        meanRT += rt;
                        weightedRT += rt;
                        correctAnswers++;
                    }
                    else if (clicks[i] == "Diff")
                    {
                        postAccuracy += "," + 2 + ",0";
                        accuracy -= 1;
                        weightedRT += 2000;
                    }
                    else
                    {
                        postAccuracy += "," + 0 + ",0";
                        accuracy -= 1;
                        weightedRT += 2000;
                    }
                }
                else
                {
                    postAccuracy += ",2";
                    if (clicks[i] == "Same")
                    {
                        postAccuracy += "," + 1 + ",0";
                        accuracy -= 1;
                        weightedRT += 2000;
                    }
                    else if (clicks[i] == "Diff")
                    {
                        postAccuracy += "," + 2 + ",1";
                        meanRT += rt;
                        weightedRT += rt;
                        correctAnswers++;
                    }
                    else
                    {
                        postAccuracy += "," + 0 + ",0";
                        accuracy -= 1;
                        weightedRT += 2000;
                    }
                }
                postAccuracy += "," + reactionTime[i];
            }
            if (correctAnswers == 0)
            {
                correctAnswers = 1;
            }
            levelString += "," + (int)((accuracy * 100) / n) + "," + (int)(meanRT / correctAnswers) + "," + (int)(weightedRT / n) + postAccuracy;
        }
    }


    public void GetPercentageForLevel()
    {
        int percentage = VariablesHolder.numberOfObjects - VariablesHolder.sequenceNBack[TimeSpawner.currentLevel];
        string textLevel = levelTextSelecters[TimeSpawner.currentLevel].text;

        int[] currentLevelObjectsPerc = TimeSpawner.allArrayInt[TimeSpawner.currentLevel];

        if (allLevelResults[TimeSpawner.currentLevel] != "")
        {
            for (int i = VariablesHolder.sequenceNBack[TimeSpawner.currentLevel]; i < VariablesHolder.numberOfObjects; i++)
            {
                //Attention
                if (currentLevelObjectsPerc[i] == 9)
                {

                }
                else if (currentLevelObjectsPerc[i] == currentLevelObjectsPerc[i - VariablesHolder.sequenceNBack[TimeSpawner.currentLevel]] && clicks[i] != "Same")
                {
                    percentage -= 1;
                }
                else if (currentLevelObjectsPerc[i] != currentLevelObjectsPerc[i - VariablesHolder.sequenceNBack[TimeSpawner.currentLevel]] && clicks[i] != "Diff")
                {
                    percentage -= 1;
                }
            }
            percentage = (int)(((float)percentage / (float)(VariablesHolder.numberOfObjects - VariablesHolder.sequenceNBack[TimeSpawner.currentLevel])) * 100);
            levelSelecters[TimeSpawner.currentLevel].gameObject.SetActive(true);
            levelTextSelecters[TimeSpawner.currentLevel].text = TimeSpawner.levelNames[TimeSpawner.currentLevel]
                + ": " + percentage + "%";

        }
    }

    public void ChangeText()
    {
        gameObjectList.text = "";
        for (int i = 0; i < VariablesHolder.numberOfObjects; i++)
        {
            if(TimeSpawner.allArrayInt[TimeSpawner.currentLevel][0] == 9)
            {
                gameObjectList.text = "Just Moving trough the scene.";
                break;
            }
            else
            {
                gameObjectList.text = gameObjectList.text + (orderObject[currentLevelObjects[i]].name + ": " + clicks[i] + "; ");    
            }
        }
        allLevelResults[TimeSpawner.currentLevel] = gameObjectList.text;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseMenuUIHead.SetActive(true);
        VariablesHolder.GameSpeed = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        VariablesHolder.GameSpeed = 0;
        GameIsPaused = true;
    }

    public void Restart()
    {
        Destroyer.objectDestroyed = 0;
        TimeSpawner.order = -1;
        clickPosition = -2;
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
       
        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void NextLevelIncrement()
    {
        TimeSpawner.currentLevel += 1;
    }

    public void NextLevel()
    {
        
        Destroyer.objectDestroyed = 0;
        TimeSpawner.order = -1;
        clickPosition = -2;
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
       
        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }


    public void SkipTutorialIncrement()
    {
        TimeSpawner.currentLevel = VariablesHolder.numberOfTutorial;
        UnityEngine.Debug.Log(TimeSpawner.currentLevel);
    }


    public void SkipTutorials()
    {
        Destroyer.objectDestroyed = 0;
        TimeSpawner.order = -1;
       
        clickPosition = -2;
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
       
        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void ResultsOfLevel(int levelChoice)
    {
        listOfResultsWanted.text = allLevelResults[levelChoice];
    }

    public void BackToEndScreen()
    {
        if (TimeSpawner.currentLevel < VariablesHolder.numberOfTutorial)
        {
            endScreen.SetActive(true);
        }
        else if (TimeSpawner.currentLevel >= VariablesHolder.numberOfTutorial && TimeSpawner.currentLevel < (VariablesHolder.numberTrials + VariablesHolder.numberOfTutorial-1))
        {
            endScreenLevel.SetActive(true);
        }
        else
        {
            endScreenFinal.SetActive(true);
        }
    }

    public void QuitGame()
    {
        TimeSpawner.TriggerArduino("U");
        TimeSpawner.serialPort.Close();
        Application.Quit();
    }
}
