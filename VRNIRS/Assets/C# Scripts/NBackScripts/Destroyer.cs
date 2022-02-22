using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Destroyer : MonoBehaviour {

    public GameObject endScreen;
    public GameObject endScreenLevel;
    public GameObject endScreenFinal;
    public GameObject levelListScreen;
    //public TextMeshProUGUI gameObjectList;

    public static int objectDestroyed = 0;

	// Update is called once per frame
	void Update () {
		if(this.transform.position.z <= -10)
        {
            Destruction();
        }
	}

    void Destruction()
    {
        Destroy(this.gameObject);
        objectDestroyed++;
        if(objectDestroyed == NumberOfObjects.numberOfObjects + 2)
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
            if (TimeSpawner.currentLevel == TimeSpawner.sizeOfArray)
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
    }
}
