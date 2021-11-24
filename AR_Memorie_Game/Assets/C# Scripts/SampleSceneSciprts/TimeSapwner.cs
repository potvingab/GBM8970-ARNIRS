﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;

public class TimeSapwner : MonoBehaviour {

    public static SerialPort serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    public Transform spawnPos1;
    public Transform spawnPos2;
    public Transform spawnPos3;
    public GameObject[] spawneesReal;
    public GameObject[] spawneesNormal;
    public GameObject StartObject;
    public GameObject EndObject;
    public GameObject EmptyObject;

    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public static int order = -1;
    public static int currentLevel = 0;
    public static int startTime;
    public static Stopwatch reactionTime = new Stopwatch();

    public static int[] Tutorial1Int;
    public static int[] Tutorial2Int;
    public static int[] Tutorial3Int;
    public static int[] Tutorial4Int;
    public static int[] Tutorial5Int;
    public static int[] Tutorial6Int;
    public static int[] Tutorial7Int;
    public static int[] blanc1Int;
    public static int[] level1Int;
    public static int[] level2Int;
    public static int[] level3Int;
    public static int[] level4Int;
    public static int[] level5Int;
    public static int[] level6Int;
    public static int[] level7Int;
    public static int[] level8Int;

    public static GameObject[] Tutorial1;
    public static GameObject[] Tutorial2;
    public static GameObject[] Tutorial3;
    public static GameObject[] Tutorial4;
    public static GameObject[] Tutorial5;
    public static GameObject[] Tutorial6;
    public static GameObject[] Tutorial7;
    public static GameObject[] blanc1;
    public static GameObject[] level1;
    public static GameObject[] level2;
    public static GameObject[] level3;
    public static GameObject[] level4;
    public static GameObject[] level5;
    public static GameObject[] level6;
    public static GameObject[] level7;
    public static GameObject[] level8;

    public static int[][] allArrayInt;

    //public static GameObject[][] allArray;

    public static string[] levelNames = { "Tutorial 1", "Tutorial 2", "Tutorial 3", "Tutotial 4",
        "Tutotial 5", "Tutotial 6", "Tutotial 7", "Level 1", "Level 2", "Level 3", "Level 4",
    "Level 5", "Level 6", "Level 7", "Level 8", "Level 9", "Level 10", "Level 11", "Level 12"};

    public static int[] spawneeWanted;


    public int[] ArrayMaker()
    {
        spawneeWanted = new int[NumberOfObjects.numberOfObjects];
        for (int i = 0; i < NumberOfObjects.numberOfObjects; ++i)
        {
            int y = 0;

            if (RealistBool.realistCheck)
            {
               y = UnityEngine.Random.Range(0, spawneesReal.Length);
            }
            else
            {
                y = UnityEngine.Random.Range(0, spawneesNormal.Length);
            }

            if (BoolArrayHolder.assetsChecks[y])
            {
                spawneeWanted[i] = y;
            }
            else
            {
                i--;
            }
        }
        return spawneeWanted;
    }


    // Use this for initialization
    private void Awake()
    {

        Tutorial1Int = new int[15] {3, 8, 8, 0, 0, 7, 0, 1, 1, 8, 5, 5, 4, 7, 5};

        Tutorial2Int = new int[15] {1, 7, 4, 7, 0, 8, 0, 3, 1, 3, 8, 3, 4, 6, 3};

        Tutorial3Int = new int[15] {4, 2, 5, 6, 5, 3, 8, 5, 8, 7, 4, 0, 4, 6, 8};

        Tutorial4Int = new int[15] {7, 1, 1, 3, 1, 0, 6, 0, 7, 6, 7, 8, 5, 8, 2};

        Tutorial5Int = new int[15] {2, 7, 2, 8, 5, 8, 3, 2, 3, 4, 8, 4, 1, 4, 5};

        Tutorial6Int = new int[15] {5, 1, 7, 4, 7, 3, 8, 2, 8, 5, 7, 1, 3, 1, 8};

        Tutorial7Int = new int[15] {5, 1, 7, 4, 7, 3, 8, 2, 8, 5, 7, 1, 3, 1, 8};

        blanc1Int = new int[15] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };


        if (MainMenu.randomizer)
        {
            level1Int = ArrayMaker();
            level2Int = ArrayMaker();
            level3Int = ArrayMaker();
            level4Int = ArrayMaker();
            level5Int = ArrayMaker();
            level6Int = ArrayMaker();
            level7Int = ArrayMaker();
            level8Int = ArrayMaker();
        }
        else
        {

            level1Int = new int[15] {0, 2, 1, 2, 7, 5, 7, 3, 4, 7, 8, 2, 6, 2, 8};

            level2Int = new int[15] {2, 0, 4, 0, 7, 8, 4, 3, 3, 7, 3, 0, 2, 0, 4};

            level3Int = new int[15] {7, 6, 3, 6, 8, 6, 7, 3, 1, 3, 0, 6, 0, 8, 0};

            level4Int = new int[15] {2, 0, 4, 0, 3, 7, 3, 5, 8, 5, 2, 5, 0, 8, 0};

            level5Int = new int[15] {7, 4, 7, 0, 3, 0, 5, 1, 5, 3, 6, 2, 3, 1, 0};

            level6Int = new int[15] {4, 8, 0, 2, 1, 2, 4, 4, 6, 7, 6, 3, 8, 4, 7};

            level7Int = new int[15] {0, 5, 6, 5, 2, 7, 2, 8, 0, 6, 0, 2, 5, 2, 4};

            level8Int = new int[15] {7, 3, 2, 6, 2, 6, 8, 5, 8, 4, 5, 7, 2, 7, 0};

        }


        allArrayInt = new int[][] {Tutorial1Int, Tutorial2Int, Tutorial3Int, Tutorial4Int, Tutorial5Int, Tutorial6Int, Tutorial7Int,
            blanc1Int, level1Int, level2Int, level3Int, level4Int, blanc1Int, blanc1Int, level5Int, level6Int, level7Int, level8Int, blanc1Int};

        /*
        allArray = new GameObject[][]{Tutorial1, Tutorial2, Tutorial3, Tutorial4, Tutorial5, Tutorial6, Tutorial7,
            blanc1, level1, level2, level3, level4, blanc1, blanc1, level5, level6, level7, level8, blanc1};*/
    }

    void Start ()
    {
        
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
	}

    public static void CreateCheckpoint(string nom)
    {

        string fileName = @"C:\Users\achil\TempsVRNIRS.txt";
        using (StreamWriter sw = File.AppendText(fileName))
        {
            // Add some text to file    
            //Byte[] title = new System.Text.UTF8Encoding(true).GetBytes("\n Checkpoint ");
            //sw.Write(title, 0, title.Length);
            sw.Write("\n Checkpoint; " + nom + " ;");

            //Byte[] temps_init = new System.Text.UTF8Encoding(true).GetBytes(DateTime.Now.ToString("H:mm:ss zzz"));
            //sw.Write(temps_init, 0, temps_init.Length);
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff"));

            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            //stopWatch.Start();

            //sw.Write("\n verif " + elapsedTime);
        }
    }

    public void SpawnObject()
    {
        if(VariablesHolder.GameSpeed == 1)
        {
            if (order < NumberOfObjects.numberOfObjects + 1)
            {
                GameObject spawneeObject;
                if(order == -1)
                {
                    spawneeObject = StartObject;
                    Instantiate(spawneeObject, spawnPos3.position, spawnPos3.rotation);
                }
                else if (order == NumberOfObjects.numberOfObjects)
                {
                    spawneeObject = EndObject;
                    Instantiate(spawneeObject, spawnPos3.position, spawnPos3.rotation);
                }
                else
                {
                    spawneeWanted = allArrayInt[currentLevel];
                    if(spawneeWanted == blanc1Int)
                    {
                        spawneeObject = EmptyObject;
                        int side = UnityEngine.Random.Range(0, 2);
                        if (side == 0)
                        {
                            startTime = DateTime.Now.Millisecond;
                            Instantiate(spawneeObject, spawnPos1.position, spawnPos1.rotation);
                            reactionTime.Reset();
                            reactionTime.Start();
                        }
                        else
                        {
                            startTime = DateTime.Now.Millisecond;
                            Instantiate(spawneeObject, spawnPos2.position, spawnPos2.rotation);
                            reactionTime.Reset();
                            reactionTime.Start();
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log(spawneeWanted[order]);
                        bool tree = false;
                        bool house = false;
                        if (RealistBool.realistCheck)
                        {
                            if (spawneesReal[spawneeWanted[order]].name == "House")
                            {
                                house = true;
                            }
                            if (spawneesReal[spawneeWanted[order]].name == "Tree")
                            {
                                tree = true;
                            }

                            spawneeObject = spawneesReal[spawneeWanted[order]];
                        }
                        else
                        {
                            spawneeObject = spawneesNormal[spawneeWanted[order]];
                        }

                        int side = UnityEngine.Random.Range(0, 2);
                        if (side == 0)
                        {
                            if (house && RealistBool.realistCheck)
                            {
                                Vector3 temp = new Vector3(2.0f, 0, 0);
                                startTime = DateTime.Now.Millisecond; 
                                Instantiate(spawneeObject, spawnPos1.position - temp, spawnPos1.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                                house = false;
                            }
                            else if (tree && RealistBool.realistCheck)
                            {
                                Vector3 temp = new Vector3(0.5f, 0, 0);
                                startTime = DateTime.Now.Millisecond;
                                Instantiate(spawneeObject, spawnPos1.position - temp, spawnPos1.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                                tree = false;
                            }
                            else
                            {
                                startTime = DateTime.Now.Millisecond;
                                Instantiate(spawneeObject, spawnPos1.position, spawnPos1.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                            }
                            
                        }
                        else
                        {
                            if (house && RealistBool.realistCheck)
                            {
                                Vector3 temp = new Vector3(2.0f, 0, 0);
                                startTime = DateTime.Now.Millisecond;
                                Instantiate(spawneeObject, spawnPos2.position + temp, spawnPos2.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                                house = false;
                            }
                            else if (tree && RealistBool.realistCheck)
                            {
                                Vector3 temp = new Vector3(0.5f, 0, 0);
                                startTime = DateTime.Now.Millisecond;
                                Instantiate(spawneeObject, spawnPos2.position + temp, spawnPos2.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                                tree = false;
                            }
                            else
                            {
                                startTime = DateTime.Now.Millisecond;
                                Instantiate(spawneeObject, spawnPos2.position, spawnPos2.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                            }
                            
                        }
                    }
                    
                    
                }
                if (stopSpawning)
                {
                    CancelInvoke("SpawnObject");
                }
                order++;
                CreateCheckpoint("Spawn");
                serialPort.Open();
                serialPort.WriteLine("1");
                serialPort.Close();
                PauseMenu.clickPosition += 1;
                PauseMenu.SameObject = false;
            }
        }
       
       
    }
}
