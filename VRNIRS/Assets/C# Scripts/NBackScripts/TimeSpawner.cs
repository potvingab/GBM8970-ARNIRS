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

    /*
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

    */
    public static int[] level;
    //public static GameObject[] level;


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
    //ATTENTION
    public static GameObject[][] allArray;


    //To move to menu!
    public static List<string> NBackSequence = new List<string>(); // from ["Dual Task", "Single Task (Stroop)", "Single Task (Walk)"]
    public static List<int> NBackSequenceN = new List<int>(); //N of each N-back

    public static string[] levelNames = new string[2*3]; 
    public int line;
    public static string[] SequenceFromFile;


    public static int[] spawneeWanted;


    public int[] ArrayMaker()
    {
        spawneeWanted = new int[NumberOfObjects.numberOfObjects];
        for (int i = 0; i < NumberOfObjects.numberOfObjects; ++i)
        {
            int y = 0;

            if (VariablesHolder.realistCheck)
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

    public int[] LevelGenerater(string name, int nLevel)
    {
        int[] sequence = new int[NumberOfObjects.numberOfObjects];

        switch (name)
        {
            case "Single Task (Walk)":
                for (int i = 0; i < NumberOfObjects.numberOfObjects; ++i)
                {
                    sequence[i] = 9;
                }
                break;


            default:
                if (VariablesHolder.gameMode == "Random")
                {
                    sequence = ArrayMaker();
                }
                else
                    sequence = ReadFile(nLevel, "FixedSequenceNBack", NumberOfObjects.numberOfObjects);
                break;
        }
        return sequence;
    }

    public int[] TutorialGenerator(int nLevel,int tutorialNumberofObject)
    {
        int[] sequence = new int[tutorialNumberofObject];
        //Name of file!!
        sequence = ReadFile(nLevel, "FixedSequenceNBack", 15);
         
        return sequence;
    }


    public int[] ReadFile(int level, string nameOfFile, int numberOfObjects)
    {
        int[] sequence;
        sequence = new int[numberOfObjects];
        string allInfo;
        TextAsset txt = (TextAsset)Resources.Load(nameOfFile, typeof(TextAsset));

        allInfo = txt.text;
        UnityEngine.Debug.Log(allInfo);

        string[] InfoLine = allInfo.Split('\n');
        UnityEngine.Debug.Log(InfoLine[1]);

        //the starting line according to the level and read this line
        line = 2 * (level+1) - 1;
        SequenceFromFile = InfoLine[line].Split(';');

        
        UnityEngine.Debug.Log(NumberOfObjects.numberOfObjects);
        for (int i = 0; i < NumberOfObjects.numberOfObjects; ++i)
        {
            sequence[i] = Convert.ToInt32(SequenceFromFile[i]);
        }

        return sequence;

    }


    // Use this for initialization
    private void Awake()
    {
        //if (VariablesHolder.useMeta == false){
         //   GameObject metaCamera = GameObject.Find("MetaCameraRig");
          //  GameObject metaHands = GameObject.Find("MetaHands");
            //Destroy(metaCamera);
            //Destroy(metaHands);
        //}


        //File pour tutorial
        /*
        Tutorial1Int = new int[15] { 3, 8, 8, 0, 0, 7, 0, 1, 1, 8, 5, 5, 4, 7, 5 };

        Tutorial2Int = new int[15] {1, 7, 4, 7, 0, 8, 0, 3, 1, 3, 8, 3, 4, 6, 3};

        Tutorial3Int = new int[15] {4, 2, 5, 6, 5, 3, 8, 5, 8, 7, 4, 0, 4, 6, 8};

        Tutorial4Int = new int[15] {7, 1, 1, 3, 1, 0, 6, 0, 7, 6, 7, 8, 5, 8, 2};

        Tutorial5Int = new int[15] {2, 7, 2, 8, 5, 8, 3, 2, 3, 4, 8, 4, 1, 4, 5};

        Tutorial6Int = new int[15] {5, 1, 7, 4, 7, 3, 8, 2, 8, 5, 7, 1, 3, 1, 8};

        Tutorial7Int = new int[15] {5, 1, 7, 4, 7, 3, 8, 2, 8, 5, 7, 1, 3, 1, 8};

        level = new int[15] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 
        


        if (VariablesHolder.gameMode == "Random")
        {
            level = ArrayMaker();

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
        */
        //formation of the

        //allArrayInt = new int[][] {Tutorial1Int, Tutorial2Int, Tutorial3Int, Tutorial4Int, Tutorial5Int, Tutorial6Int, Tutorial7Int,
        //  blanc1Int, level1Int, level2Int, level3Int, level4Int, blanc1Int, blanc1Int, level5Int, level6Int, level7Int, level8Int, blanc1Int};

        //ATTENTION
        //allArray = new GameObject[][]{Tutorial1, Tutorial2, Tutorial3, Tutorial4, Tutorial5, Tutorial6, Tutorial7,
            //blanc1, level1, level2, level3, level4, blanc1, blanc1, level5, level6, level7, level8, blanc1};
        //ATTENTION number of level

        //TEMP! 
        NBackSequence.Add("dual task");
        NBackSequence.Add("dual task");
        NBackSequence.Add("dual task");

        allArrayInt = new int[2*3][];
        for (int nLevel = 0; nLevel < 3; ++nLevel)
        {
            
            UnityEngine.Debug.Log(nLevel);
            int[] tutoTemp = TutorialGenerator(nLevel, 5);

            allArrayInt[2*nLevel] = (tutoTemp);
            UnityEngine.Debug.Log(NBackSequence[currentLevel]);
            levelNames[2 * nLevel] = "Tutorial " + (nLevel + 1);


            int[] LevelTemp = LevelGenerater(NBackSequence[nLevel], nLevel);
            //Number of questions
            allArrayInt[2 * nLevel +1] = LevelTemp;
            UnityEngine.Debug.Log(allArrayInt[0][0]);
            levelNames[2 * nLevel+1] = "Level " + (nLevel + 1);


        }
        UnityEngine.Debug.Log(levelNames[1]);
    }
            
        
        

    void Start ()
    {

        
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
	}

    public static void TriggerArduino(string line)
    {
        // 0: Question
        // 1: Response
        // Enlever commentaire si on utilise l'Arduino
        //if (!serialPort.IsOpen)
        //  serialPort.Open();
        //serialPort.WriteLine(line);
        //CreateCheckpoint("Test Délai");
    }

    public static void CreateCheckpoint(string nom)
    {
        using (StreamWriter sw = File.AppendText(VariablesHolder.fileName))
        {
            sw.Write("Checkpoint; " + nom + "; ");
            sw.Write(DateTime.Now.ToString("H:mm:ss.fff") + "\n");
        }
    }

    public void SpawnObject()
    {


        if (VariablesHolder.GameSpeed == 1)
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
                    int[] level = allArrayInt[currentLevel];


                    //spawneeWanted = allArrayInt[currentLevel];
                    if (NBackSequence[currentLevel] == "Single Task (Walk)")
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
                        UnityEngine.Debug.Log(level[order]);
                        bool tree = false;
                        bool house = false;
                        if (VariablesHolder.realistCheck)
                        {
                            if (spawneesReal[level[order]].name == "House")
                            {
                                house = true;
                            }
                            if (spawneesReal[level[order]].name == "Tree")
                            {
                                tree = true;
                            }

                            spawneeObject = spawneesReal[level[order]];
                        }
                        else
                        {
                            spawneeObject = spawneesNormal[level[order]];



                        }

                        int side = UnityEngine.Random.Range(0, 2);
                        if (side == 0)
                        {
                            if (house && VariablesHolder.realistCheck)
                            {
                                Vector3 temp = new Vector3(2.0f, 0, 0);
                                startTime = DateTime.Now.Millisecond; 
                                Instantiate(spawneeObject, spawnPos1.position - temp, spawnPos1.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                                house = false;
                            }
                            else if (tree && VariablesHolder.realistCheck)
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
                            if (house && VariablesHolder.realistCheck)
                            {
                                Vector3 temp = new Vector3(2.0f, 0, 0);
                                startTime = DateTime.Now.Millisecond;
                                Instantiate(spawneeObject, spawnPos2.position + temp, spawnPos2.rotation);
                                reactionTime.Reset();
                                reactionTime.Start();
                                house = false;
                            }
                            else if (tree && VariablesHolder.realistCheck)
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
                TriggerArduino("0");
                PauseMenu.clickPosition += 1;
                PauseMenu.SameObject = false;
            }
        }
       
       
    }

}
