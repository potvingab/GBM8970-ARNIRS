﻿using System;
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

    public TextMeshProUGUI[] levelTextSelecters;

    public TextMeshProUGUI listOfResultsWanted;

    public GameObject pauseMenuUI;
    public GameObject pauseMenuUIHead;

    public GameObject endScreen;
    public GameObject endScreenLevel;
    public GameObject endScreenFinal;


    public static string[] clicks = new string[VariablesHolder.numberOfObjects];

    public static string[] reactionTime = new string[VariablesHolder.numberOfObjects];


    public static string[] allLevelResults ;


    //public static string[] allLevelResults = new string[19] {"", "", "", "", "", "", "", "",
     //   "", "", "", "", "", "", "", "", "", "", ""};

    public static int[] blockCondition ;

   // public static string[] allLevelResultsSaved = new string[19] {"", "", "", "", "", "", "", "",
     //   "", "", "", "", "", "", "", "", "", "", ""};

    //public static string[] clicks = new string[15] { "--", "--", "--", "--",
     //   "--", "--", "--", "--", "--", "--", "--", "--",
     //   "--", "--", "--"};

    //public static string[] reactionTime = new string[15] { "--", "--", "--", "--",
       // "--", "--", "--", "--", "--", "--", "--", "--",
      //  "--", "--", "--"};

    public static string currentLevelString = "";
    public static int[] currentLevelObjects;
    //public static int[] blockCondition = new int[19] { 2, 2, 2, 2, 2, 2, 2, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1 };


    public void Awake()
    {
        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        UnityEngine.Debug.Log(TimeSpawner.sizeOfArray);
        allLevelResults = new string[TimeSpawner.sizeOfArray];
        blockCondition = new int[TimeSpawner.sizeOfArray];
        for (int nLevelResults = 0; nLevelResults < TimeSpawner.sizeOfArray; nLevelResults++)
        {
            allLevelResults[nLevelResults] = "";
            blockCondition[nLevelResults] = 2;

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
                    TimeSpawner.TriggerArduino("1");
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
                    TimeSpawner.TriggerArduino("1");
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

    public void SaveLevelInfo()
    {
        // !!! A arranger : le bon fichier c'est VariablesHolder.fileName maintenant

        // if (!File.Exists(FileName.nameOfFile))
        // {
        //     string titleString = "id,timestamp,n-back,mode,type\n";
        //     //UnityEngine.Debug.Log(FileName.nameOfFileOriginal);
        //     //ATTENTION null??
        //     titleString += FileName.nameOfFileOriginal.Substring(0, FileName.nameOfFileOriginal.Length - 1) + "," + FileName.timeStamp + "," + VariablesHolder.nBackNumber + "," + FileName.mode + ",VISUAL\n";
        //     titleString += "Block,Condition,Total Accuracy,Mean Response Time,Weighted Response Time";
        //     for (int i = 1; i <= VariablesHolder.numberOfObjects; i++)
        //     {
        //         titleString += ",Item " + i + ",Expected Answer " + i + ",Subject Answer " + i + ",Accuracy " + i + ",Response Time " + i;
        //     }
        //     File.WriteAllText(FileName.nameOfFile, titleString + "\n");
        // }
        
        
        //ATTENTION!!!!!!
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
            levelString += "," + blockCondition[TimeSpawner.currentLevel];

            string postAccuracy = "";
            //Nombre de reponse supposee
            int accuracy = VariablesHolder.numberOfObjects - VariablesHolder.nBackNumber;
            int n = accuracy;
            int meanRT = 0;
            int weightedRT = 0;
            int correctAnswers = 0;

            for (int i = 0; i < VariablesHolder.nBackNumber; i++)
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

            for (int i = VariablesHolder.nBackNumber; i < VariablesHolder.numberOfObjects; i++)
            {
                postAccuracy += "," + (TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i] + 1);

                int rt = 2000;
                if (reactionTime[i] != "--")
                {
                    rt = int.Parse(reactionTime[i]);
                }
                if (TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i] == TimeSpawner.allArrayInt[TimeSpawner.currentLevel][i - VariablesHolder.nBackNumber])
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

        UnityEngine.Debug.Log(levelString);
        //File.AppendAllText(FileName.nameOfFile, levelString + "\n");

    }



    public void GetPercentageForLevel()
    {
        int percentage = VariablesHolder.numberOfObjects - VariablesHolder.nBackNumber;
        string textLevel = levelTextSelecters[TimeSpawner.currentLevel].text;

        int[] currentLeveObjectsPerc = TimeSpawner.allArrayInt[TimeSpawner.currentLevel];

        if (allLevelResults[TimeSpawner.currentLevel] != "")
        {
            for (int i = VariablesHolder.nBackNumber; i < VariablesHolder.numberOfObjects; i++)
            {
                if (currentLeveObjectsPerc[i] == 9)
                {

                }
                else if (currentLeveObjectsPerc[i] == currentLeveObjectsPerc[i - VariablesHolder.nBackNumber] && clicks[i] != "Same")
                {
                    percentage -= 1;
                }
                else if (currentLeveObjectsPerc[i] != currentLeveObjectsPerc[i - VariablesHolder.nBackNumber] && clicks[i] != "Diff")
                {
                    percentage -= 1;
                }
            }
            percentage = (int)(((float)percentage / (float)(VariablesHolder.numberOfObjects - VariablesHolder.nBackNumber)) * 100);
            levelTextSelecters[TimeSpawner.currentLevel].text = TimeSpawner.levelNames[TimeSpawner.currentLevel]
                + ": " + percentage + "%";

        }
    }

    public void ChangeText()
    {
        gameObjectList.text = "";

        int[] currentLeveObjectsPerc = TimeSpawner.allArrayInt[TimeSpawner.currentLevel];

        for (int i = 0; i < VariablesHolder.numberOfObjects; i++)
        {
            if (currentLeveObjectsPerc[i] == 9)
            {
                gameObjectList.text = "Just Moving trough the scene.";
                break;
            }
            else
            {
                //if (i < 2)
                //{
                //    gameObjectList.text = gameObjectList.text + (currentLevelObjects[i].name + ": No Input Needed; ");
                //}
                //else
                //{
                //    gameObjectList.text = gameObjectList.text + (currentLevelObjects[i].name + clicks[i]);
                //}
                //
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

        //gameObjectListTitle.text = TimeSpawner.levelNames[TimeSpawner.currentLevel];
        //ChangeText();
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
        //ATTENTION
       
        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }

        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void NextLevel()
    {
        TimeSpawner.currentLevel += 1;
        Destroyer.objectDestroyed = 0;
        TimeSpawner.order = -1;
        clickPosition = -2;
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
        //ATTENTION

        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void SkipTutorials()
    {
        //ATTENTION
        TimeSpawner.currentLevel +=2;
        Destroyer.objectDestroyed = 0;
        TimeSpawner.order = -1;
        clickPosition = -2;
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
        //ATTENTION

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
        /*if (TimeSpawner.currentLevel < 1)
        
        {
            endScreen.SetActive(true);
        }
        //ATTENTION!!number of level max!!
        else if (TimeSpawner.currentLevel == 6)
        {
            endScreenFinal.SetActive(true);
        }
        else
        {
            endScreenLevel.SetActive(true);
        }
        */
        UnityEngine.Debug.Log(TimeSpawner.currentLevel);
        UnityEngine.Debug.Log(TimeSpawner.sizeOfArray - 1);
        UnityEngine.Debug.Log("!!");
        if (TimeSpawner.currentLevel == TimeSpawner.sizeOfArray-1)
        {
            endScreenLevel.SetActive(true);
        }
        else
        {
            if (TimeSpawner.levelNames[TimeSpawner.currentLevel + 1].Contains("Tutorial"))
            {
                endScreen.SetActive(true);
            }
            else
            {
                endScreenLevel.SetActive(true);
            }
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        VariablesHolder.GameSpeed = 1;
        GameIsPaused = false;
        TimeSpawner.currentLevel = 0;
        TimeSpawner.order = -1;
        Destroyer.objectDestroyed = 0;
        clickPosition = -2;
        clicks = new string[VariablesHolder.numberOfObjects];
        reactionTime = new string[VariablesHolder.numberOfObjects];
        //ATTENTION

        for (int nObject = 0; nObject < VariablesHolder.numberOfObjects; nObject++)
        {
            clicks[nObject] = "--";
            reactionTime[nObject] = "--";
        }
        allLevelResults = new string[TimeSpawner.sizeOfArray];


        for (int nLevelResults = 0; nLevelResults < VariablesHolder.numberOfObjects; nLevelResults++)
        {
            allLevelResults[nLevelResults] = "";
            
        }
        
        SaveCondition = true;
    }
}
