﻿using System.Collections;
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
            //    gameobjectlist.text = gameobjectlist.text + (timesapwner.spawneewanted[i].name + "\n");
            //}
            levelListScreen.SetActive(false);
            if(TimeSapwner.currentLevel < 7)
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
    }
}