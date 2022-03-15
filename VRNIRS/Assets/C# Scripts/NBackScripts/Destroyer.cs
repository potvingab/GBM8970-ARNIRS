using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Destroyer : MonoBehaviour {

    public GameObject endScreen;
    public GameObject endScreenLevel;
    public GameObject endScreenFinal;
    public GameObject levelListScreen;
    public GameObject VolumeMenu;
    //public TextMeshProUGUI gameObjectList;

    public static int objectDestroyed = 0;

	// Update is called once per frame
	void Update () {
		if(this.transform.position.z <= -10) // Original = -10, attention 9.37!
        {
            Destruction();
            TimeSpawner.CreateCheckpoint("Destroy");
        }
	}

    void Destruction()
    {
        Debug.Log("Destroy : " + this.gameObject);
        Destroy(this.gameObject);
        objectDestroyed++;
        if(objectDestroyed == VariablesHolder.numberOfObjects + 2)
        {
            //for (int i = 0; i < objectdestroyed + 2; i++)
            //{
            //    gameobjectlist.text = gameobjectlist.text + (TimeSpawner.spawneewanted[i].name + "\n");
            //}
            levelListScreen.SetActive(false);
            //if(TimeSpawner.currentLevel < 7)
            //{
            //    endScreen.SetActive(true);
            //}
            //else if(TimeSpawner.currentLevel == 18)
            //{
            //   endScreenFinal.SetActive(true);
            //}
            //else
            //{
            //    endScreenLevel.SetActive(true);
            //}

            UnityEngine.Debug.Log(TimeSpawner.currentLevel);
            UnityEngine.Debug.Log("!!");
            if (TimeSpawner.currentLevel == VariablesHolder.sizeOfArray - 1)
            {
                endScreenLevel.SetActive(true);
            }
            else
            {
                Debug.Log(TimeSpawner.levelNames[TimeSpawner.currentLevel]);
                Debug.Log(TimeSpawner.levelNames.Length);
                if (TimeSpawner.levelNames[TimeSpawner.currentLevel + 1].Contains("Tutorial"))
                {
                    endScreen.SetActive(true);
                }
                else
                {
                    endScreenLevel.SetActive(true);
                }
            }
            VolumeMenu.SetActive(true);



        }
    }
}
