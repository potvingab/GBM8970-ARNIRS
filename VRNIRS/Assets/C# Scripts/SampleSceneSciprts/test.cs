using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    void Awake()
    {
        for (int i = 0; i < BoolArrayHolder.assetsChecks.Length; i++)
        {
            Debug.Log(BoolArrayHolder.assetsChecks[i]);
        }
        
    }
}
