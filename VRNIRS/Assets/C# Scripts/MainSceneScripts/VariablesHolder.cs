using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesHolder : MonoBehaviour {
    public static float speed;
    public static float GameSpeed;

    void Start()
    {
        speed = 1;
        GameSpeed = 0;
    }

    void Awake()
    {
        
        DontDestroyOnLoad(transform.gameObject);
    }
    
    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
