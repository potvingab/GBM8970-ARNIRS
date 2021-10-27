using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfObjects : MonoBehaviour {

    public static int numberOfObjects;
    public Slider sizeSlider;
    public Slider sizeSliderLevel;

    void Awake()
    {
        numberOfObjects = 15;
        DontDestroyOnLoad(transform.gameObject);
    }

    public void UpdateSize()
    {
        numberOfObjects = (int)sizeSlider.value;
    }
    public void UpdateSizeLevel()
    {
        numberOfObjects = (int)sizeSliderLevel.value;
    }
}
