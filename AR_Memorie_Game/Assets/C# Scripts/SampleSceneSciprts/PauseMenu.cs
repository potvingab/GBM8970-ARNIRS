using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

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

    public static string[] allLevelResults = new string[19] {"", "", "", "", "", "", "", "",
        "", "", "", "", "", "", "", "", "", "", ""};

    public static string[] allLevelResultsSaved = new string[19] {"", "", "", "", "", "", "", "",
        "", "", "", "", "", "", "", "", "", "", ""};

    public static string[] clicks = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};

    public static string[] reactionTime = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};

    public static string currentLevelString = "";
    public static int[] currentLevelObjects;
    public static int[] blockCondition = new int[19] {2,2,2,2,2,2,2,1,2,3,3,2,1,1,2,3,3,2,1};


    public void Awake()
    {
        GameIsPaused = true;
    }

    // Update is called once per frame
    void Update () {
        currentLevelString = "--";
        currentLevelObjects = TimeSapwner.allArrayInt[TimeSapwner.currentLevel];
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

        gameObjectListTitle.text = TimeSapwner.levelNames[TimeSapwner.currentLevel];

        if (!SameObject)
        {
            if(clickPosition >= 0 && clickPosition < NumberOfObjects.numberOfObjects)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    currentLevelString = "Same";
                    TimeSapwner.reactionTime.Stop();
                    endTime = DateTime.Now.Millisecond;
                    differenceTime = endTime - TimeSapwner.startTime;
                    reactionTime[clickPosition] = TimeSapwner.reactionTime.ElapsedMilliseconds.ToString();
                    UnityEngine.Debug.Log(differenceTime + " : " + TimeSapwner.reactionTime.ElapsedMilliseconds);
                    SameObject = true;
                    clicks[clickPosition] = currentLevelString;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    currentLevelString = "Diff";
                    TimeSapwner.reactionTime.Stop();
                    endTime = DateTime.Now.Millisecond;
                    differenceTime = Math.Abs(endTime - TimeSapwner.startTime);
                    reactionTime[clickPosition] = TimeSapwner.reactionTime.ElapsedMilliseconds.ToString();
                    UnityEngine.Debug.Log(differenceTime + " : " + TimeSapwner.reactionTime.ElapsedMilliseconds);
                    SameObject = true;
                    clicks[clickPosition] = currentLevelString;
                }
            }
            ChangeText();
        }

        if(TimeSapwner.order == NumberOfObjects.numberOfObjects + 1 && SaveCondition)
        {
            GetPercentageForLevel();
            SaveLevelInfo();
            SaveCondition = false;
        }

    }

    public void SaveLevelInfo()
    {
        if (!File.Exists(FileName.nameOfFile))
        {
            string titleString = "id,timestamp,n-back,mode,type\n";
            UnityEngine.Debug.Log(FileName.nameOfFileOriginal);
            titleString += FileName.nameOfFileOriginal.Substring(0, FileName.nameOfFileOriginal.Length - 1) + "," + FileName.timeStamp + "," + NBack.nBackNumber + "," + FileName.mode + ",VISUAL\n";
            titleString += "Block,Condition,Total Accuracy,Mean Response Time,Weighted Response Time";
            for (int i = 1; i <= NumberOfObjects.numberOfObjects; i++)
            {
                titleString += ",Item " + i + ",Expected Answer " + i + ",Subject Answer " + i + ",Accuracy " + i + ",Response Time " + i;
            }
            File.WriteAllText(FileName.nameOfFile, titleString + "\n");
        }
        string levelString = "";
        if (TimeSapwner.currentLevel < 7)
        {
            levelString += "T" + (TimeSapwner.currentLevel + 1);
        }
        else
        {
            levelString += (TimeSapwner.currentLevel - 6);
        }
        if (TimeSapwner.allArrayInt[TimeSapwner.currentLevel][0] == 9)
        {
            levelString += ",1,100,0,0";
            for (int i = 0; i < NumberOfObjects.numberOfObjects; i++)
            {
                levelString += ",0,0,0,1,0";
            }
        }
        else
        {
            levelString += "," + blockCondition[TimeSapwner.currentLevel];

            string postAccuracy = "";
            int accuracy = NumberOfObjects.numberOfObjects - NBack.nBackNumber;
            int n = accuracy;
            int meanRT = 0;
            int weightedRT = 0;
            int correctAnswers = 0;

            for (int i = 0; i < NBack.nBackNumber; i++)
            {
                postAccuracy += "," + (TimeSapwner.allArrayInt[TimeSapwner.currentLevel][i] + 1);
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

            for (int i = NBack.nBackNumber; i < NumberOfObjects.numberOfObjects; i++)
            {
                postAccuracy += "," + (TimeSapwner.allArrayInt[TimeSapwner.currentLevel][i] + 1);
               
                int rt = 2000;
                if (reactionTime[i] != "--")
                {
                    rt = int.Parse(reactionTime[i]);
                }
                if (TimeSapwner.allArrayInt[TimeSapwner.currentLevel][i] == TimeSapwner.allArrayInt[TimeSapwner.currentLevel][i - NBack.nBackNumber])
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
        File.AppendAllText(FileName.nameOfFile, levelString + "\n");

    }

    public void GetPercentageForLevel()
    {
        int percentage = NumberOfObjects.numberOfObjects - NBack.nBackNumber;
        string textLevel = levelTextSelecters[TimeSapwner.currentLevel].text;

        int[] currentLeveObjectsPerc = TimeSapwner.allArrayInt[TimeSapwner.currentLevel];

        if(allLevelResults[TimeSapwner.currentLevel] != "")
        {
            for(int i = NBack.nBackNumber; i < NumberOfObjects.numberOfObjects; i++)
            {
                if(currentLeveObjectsPerc[i] == 9)
                {

                }
                else if(currentLeveObjectsPerc[i] == currentLeveObjectsPerc[i - NBack.nBackNumber] && clicks[i] != "Same")
                {
                    percentage -= 1;
                }
                else if (currentLeveObjectsPerc[i] != currentLeveObjectsPerc[i - NBack.nBackNumber] && clicks[i] != "Diff")
                {
                    percentage -= 1;
                }
            }
            percentage = (int)(((float)percentage / (float)(NumberOfObjects.numberOfObjects - NBack.nBackNumber)) * 100);
            levelTextSelecters[TimeSapwner.currentLevel].text = TimeSapwner.levelNames[TimeSapwner.currentLevel]
                + ": " + percentage + "%";

        }
    }

    public void ChangeText()
    {
        gameObjectList.text = "";

        int[] currentLeveObjectsPerc = TimeSapwner.allArrayInt[TimeSapwner.currentLevel];

        for (int i = 0; i < NumberOfObjects.numberOfObjects; i++)
        {
            if(currentLeveObjectsPerc[i] == 9)
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
        allLevelResults[TimeSapwner.currentLevel] = gameObjectList.text;
    }

    public void Resume ()
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
        TimeSapwner.order = -1;
        clickPosition = -2;
        clicks = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        reactionTime = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void NextLevel()
    {
        TimeSapwner.currentLevel += 1;
        Destroyer.objectDestroyed = 0;
        TimeSapwner.order = -1;
        clickPosition = -2;
        clicks = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        reactionTime = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void SkipTutorials()
    {
        TimeSapwner.currentLevel = 7;
        Destroyer.objectDestroyed = 0;
        TimeSapwner.order = -1;
        clickPosition = -2;
        clicks = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        reactionTime = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        pauseMenuUIHead.SetActive(true);
        SaveCondition = true;
    }

    public void ResultsOfLevel(int levelChoice)
    {
        listOfResultsWanted.text = allLevelResults[levelChoice];
    }

    public void BackToEndScreen()
    {
        if (TimeSapwner.currentLevel < 7)
        {
            endScreen.SetActive(true);
        }
        else if(TimeSapwner.currentLevel == 18)
        {
            endScreenFinal.SetActive(true);
        }
        else
        {
            endScreenLevel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        VariablesHolder.GameSpeed = 1;
        GameIsPaused = false;
        TimeSapwner.currentLevel = 0;
        TimeSapwner.order = -1;
        Destroyer.objectDestroyed = 0;
        clickPosition = -2;
        clicks = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        allLevelResults = new string[19] {"", "", "", "", "", "", "", "",
        "","","","","","","","","","", ""};
        reactionTime = new string[15] { "--", "--", "--", "--",
        "--", "--", "--", "--", "--", "--", "--", "--",
        "--", "--", "--"};
        SaveCondition = true;
    }
}
