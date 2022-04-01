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
    public static int objectDestroyed = 0;

	// Update is called once per frame
	void Update () {
		if(this.transform.position.z <= 9.37f-(15 * VariablesHolder.speed)) // Original = -10
        {
            Destruction();
        }
	}

    void Destruction()
    {
        Debug.Log("Destroy : " + this.gameObject);
        Destroy(this.gameObject);
        objectDestroyed++;
        if(objectDestroyed == VariablesHolder.numberOfObjects + 2)
        {
            levelListScreen.SetActive(false);
            if (TimeSpawner.currentLevel < VariablesHolder.numberOfTutorial)
            {
                endScreen.SetActive(true);
            }
            else if (TimeSpawner.currentLevel >= VariablesHolder.numberOfTutorial && TimeSpawner.currentLevel < (VariablesHolder.numberTrials + VariablesHolder.numberOfTutorial-1))
            {
                endScreenLevel.SetActive(true);
                UnityEngine.Debug.Log("Niveau :" + TimeSpawner.currentLevel);
                UnityEngine.Debug.Log("Niveaux totaux :" + (VariablesHolder.numberTrials + VariablesHolder.numberOfTutorial));
            }
            else
            {
                endScreenFinal.SetActive(true);
            }
            VolumeMenu.SetActive(true);
        }
    }
}
