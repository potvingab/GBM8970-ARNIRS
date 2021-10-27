using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NBack : MonoBehaviour {

    public static int nBackNumber;
    public Slider NBackSlider;
    public Slider NBackLevelSlider;

    void Awake()
    {
        nBackNumber = 2;
        DontDestroyOnLoad(transform.gameObject);
    }

    public void UpdateNBack()
    {
        nBackNumber = (int)NBackSlider.value;
    }
    public void UpdateNBackLevel()
    {
        nBackNumber = (int)NBackLevelSlider.value;
    }
}
