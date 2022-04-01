using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.IO.Ports;
using System.Diagnostics;

public class TimeSpawner : MonoBehaviour {

    public static string fileName = VariablesHolder.fileName;

    public static SerialPort serialPort = new SerialPort(VariablesHolder.arduinoPort, 9600, Parity.None, 8, StopBits.One);
    public Transform spawnPos1;
    public Transform spawnPos2;
    public Transform spawnPos3;
    public GameObject[] spawnees;
    public GameObject StartObject;
    public GameObject StartObject_txt;
    public GameObject EndObject;
    public GameObject EndObject_txt;
    public GameObject EmptyObject;
    public GameObject plane;
    public GameObject clone_txt;

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

    public static GameObject[][] allArray;

    public static string[] levelNames;
    public int lineToRead;
    public static string[] SequenceFromFile;




    public static int[] spawneeWanted;

    public int[] ArrayMaker()
    {
        spawneeWanted = new int[VariablesHolder.numberOfObjects];
        for (int i = 0; i < VariablesHolder.numberOfObjects; ++i)
        {
            int y = 0;
            y = UnityEngine.Random.Range(0, spawnees.Length);
            if (BoolArrayHolder.assetsChecks[y])
            {
                spawneeWanted[i] = y;
            }
            else
            {
                i--;
            }
        }
        UnityEngine.Debug.Log(spawneeWanted);
        return spawneeWanted;
    }

    public int[] LevelGenerator(string name)
    {
        int[] sequence = new int[VariablesHolder.numberOfObjects];
        switch (name)
        {
            case "Single Task (Walk)":
                for (int i = 0; i < VariablesHolder.numberOfObjects; ++i)
                {
                    sequence[i] = 9;
                }
                break;
            default:
                UnityEngine.Debug.Log(VariablesHolder.gameMode);
                lineToRead++;
                if (VariablesHolder.gameMode == "Random")
                {
                    sequence = ArrayMaker();
                }
                else
                {
                    if (VariablesHolder.fixedFile.Contains("Empty"))
                    {
                        sequence = ReadFile(lineToRead, "FixedSequenceNBack");
                    }
                    else
                    {
                        sequence = ReadFile(lineToRead, VariablesHolder.fixedFile);
                    }
                }
                break;
        }
        return sequence;
    }


    public void TutorialsGenerator()
    {
        for (int ntuto = 0; ntuto < VariablesHolder.numberOfTutorial; ntuto++)
        {
            UnityEngine.Debug.Log("n tuto tot: " + VariablesHolder.numberOfTutorial);
            string nameOfFile;
            if (VariablesHolder.fixedFile.Contains("Empty"))
            {
                nameOfFile = "FixedSequenceNBack";
            }
            else
            {
                nameOfFile = VariablesHolder.fixedFile;
            }
            UnityEngine.Debug.Log("sizeofarray: " + allArrayInt.Length);
            allArrayInt[ntuto] = ReadFile(ntuto + 1, nameOfFile);
            levelNames[ntuto] = "Tutorial " + (ntuto + 1);
            UnityEngine.Debug.Log("n tuto: " + ntuto);
        }
        return;
    }


    public int[] ReadFile(int line, string nameOfFile)
    {
        try
        {
            string allInfo;
            if (VariablesHolder.fixedFile.Contains("Empty") == false)
            {
                allInfo = VariablesHolder.fixedFile;
            }
            else
            {
                TextAsset txt = (TextAsset)Resources.Load(nameOfFile, typeof(TextAsset));
                allInfo = txt.text;
            }
            string[] InfoLine = allInfo.Split('\n');
            SequenceFromFile = InfoLine[line].Split(';');

            int[] sequence = new int[VariablesHolder.numberOfObjects];
            int objectIndex;
            for (int i = 0; i < VariablesHolder.numberOfObjects; ++i)
            {
                //Single Walk
                if (SequenceFromFile[1].Contains("walk"))
                {
                    objectIndex = 9;
                }
                else
                {
                    objectIndex = Convert.ToInt32(SequenceFromFile[i + 1]);
                }

                sequence[i] = objectIndex;
            }

            return sequence;
        }
        catch
        {
            int[] sequence = new int[0];
            return sequence;

        }
    }


    // Use this for initialization
    private void Awake()
    {
        allArrayInt = new int[VariablesHolder.sizeOfArray][];
        levelNames = new string[VariablesHolder.sizeOfArray];

        TutorialsGenerator();
        lineToRead = VariablesHolder.numberOfTutorial;
        for (int nLevel = VariablesHolder.numberOfTutorial; nLevel < VariablesHolder.sizeOfArray; ++nLevel)
        {
            allArrayInt[nLevel] = LevelGenerator(VariablesHolder.sequence[nLevel]);
            levelNames[nLevel] = "Level " + (nLevel - VariablesHolder.numberOfTutorial + 1);
        }
    }


    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
        if (!VariablesHolder.useVisual)
        {
            plane.SetActive(false);
        }
    }

    public static void TriggerArduino(string line)
    {
        // 0: Question
        // 1: Response
        // Enlever commentaire si on utilise l'Arduino
        if (!serialPort.IsOpen)
            serialPort.Open();
        serialPort.WriteLine(line);
        ARCheckpoint("Trigger sent");
    }

    public static void CreateCheckpoint(string nom)
    {
        String name = VariablesHolder.fileName;
        int index = name.IndexOf(".txt");
        String masterFileName = name.Insert(index, "_Master");
        using (StreamWriter sw = File.AppendText(masterFileName))
        {
            sw.Write("Checkpoint; " + nom + "; ");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff") + "\n");
        }
        ARCheckpoint("Event received");
    }

    public static void ArduinoCheckpoint(string nom)
    {
        String name = VariablesHolder.fileName;
        int index = name.IndexOf(".txt");
        String arduinoFileName = name.Insert(index, "_Test_synchro_Arduino");
        if (!serialPort.IsOpen)
            serialPort.Open();
        string delay = serialPort.ReadLine();
        using (StreamWriter sw = File.AppendText(arduinoFileName))
        {
            sw.Write("Arduino Delay; " + nom + "; " + delay + " μs" + "\n");
        }
    }

    static void ARCheckpoint(string nom) 
    {
        String name = VariablesHolder.fileName;
        int index = name.IndexOf(".txt");
        String arFileName = name.Insert(index, "_Test_synchro_AR");
        using (StreamWriter sw = File.AppendText(arFileName))
        {
            sw.Write("Checkpoint; " + nom + "; ");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff") + "\n");
        }
    }

    void DisableText()
    {
        clone_txt.gameObject.SetActive(false);
    }

    public void SpawnObject()
    {
        if (VariablesHolder.GameSpeed == 1)
        {
            if (order < VariablesHolder.numberOfObjects + 1)
            {
                GameObject spawneeObject;
                GameObject spawneeObject_txt;
                if (order == -1)
                {
                    if (VariablesHolder.useVisual)
                    {
                        spawneeObject = StartObject;
                        spawneeObject_txt = StartObject_txt;
                    }
                    else
                    {
                        spawneeObject = EmptyObject;
                        spawneeObject_txt = EmptyObject;
                    }
                    Instantiate(spawneeObject, spawnPos3.position, spawnPos3.rotation);
                    clone_txt = Instantiate(spawneeObject_txt, spawnPos3.position, spawnPos3.rotation);
                   
                    Invoke("DisableText", 5f);//invoke after 5 seconds
                    CreateCheckpoint("Start");
                    TriggerArduino("0");
                    ArduinoCheckpoint("Start");
                }
                else if (order == VariablesHolder.numberOfObjects)
                {
                    if (VariablesHolder.useVisual)
                    {
                        spawneeObject = EndObject;
                        spawneeObject_txt = EndObject_txt;
                    }
                    else
                    {
                        spawneeObject = EmptyObject;
                        spawneeObject_txt = EmptyObject;
                    }
                    Instantiate(spawneeObject, spawnPos3.position, spawnPos3.rotation);
                    clone_txt = Instantiate(spawneeObject_txt, spawnPos3.position, spawnPos3.rotation);
                    Invoke("DisableText", 5f);//invoke after 5 seconds
                    CreateCheckpoint("End");
                    TriggerArduino("0");
                    ArduinoCheckpoint("End");
                }
                else
                { 
                    spawneeWanted = allArrayInt[currentLevel];
                    if (VariablesHolder.sequence[currentLevel].Contains("Single Task (Walk)"))
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
                        spawneeObject = spawnees[spawneeWanted[order]];
                        
                        int side = UnityEngine.Random.Range(0, 2);
                        if (side == 0)
                        {
                            startTime = DateTime.Now.Millisecond;
                            GameObject clone = Instantiate(spawneeObject, spawnPos1.position, spawnPos1.rotation);
                            UnityEngine.Debug.Log("Audio:" + VariablesHolder.useAudio);
                            UnityEngine.Debug.Log("Visuel:" + VariablesHolder.useVisual);
                            if (VariablesHolder.useAudio)
                            {
                                AudioSource sound = clone.GetComponent<AudioSource>();
                                //sound.volume = VariablesHolder.audioVolume;
                                UnityEngine.Debug.Log(sound);
                                sound.Play();
                            }
                            if (!VariablesHolder.useVisual)
                            {
                                clone.gameObject.transform.localScale = new Vector3(0, 0, 0);
                            }
                            CreateCheckpoint("Spawn " + clone.ToString());
                            TriggerArduino("0");
                            ArduinoCheckpoint("Spawn " + clone.ToString());
                            reactionTime.Reset();
                            reactionTime.Start();
                        }
                        else
                        {
                            startTime = DateTime.Now.Millisecond;
                            GameObject clone = Instantiate(spawneeObject, spawnPos2.position, spawnPos2.rotation);
                            UnityEngine.Debug.Log("Audio:" + VariablesHolder.useAudio);
                            UnityEngine.Debug.Log("Visuel:" + VariablesHolder.useVisual);
                            if (VariablesHolder.useAudio)
                            {
                                AudioSource sound = clone.GetComponent<AudioSource>();
                                //sound.volume = VariablesHolder.audioVolume;
                                UnityEngine.Debug.Log(sound);
                                sound.Play();
                            }
                            if (!VariablesHolder.useVisual)
                            {
                                clone.gameObject.transform.localScale = new Vector3(0, 0, 0);
                            }
                            CreateCheckpoint("Spawn " + clone.ToString());
                            TriggerArduino("0");
                            ArduinoCheckpoint("Spawn " + clone.ToString());
                            reactionTime.Reset();
                            reactionTime.Start();
                        }
                    }
                }
                if (stopSpawning)
                {
                    CancelInvoke("SpawnObject");
                }
                order++;
                PauseMenu.clickPosition += 1;
                PauseMenu.SameObject = false;
            }
        }
    }
}
