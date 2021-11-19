using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedChanger : MonoBehaviour {

    public Slider speedChanger;
    [SerializeField] private TextMeshProUGUI _sliderText;

    // Use this for initialization
    void Start () {
        speedChanger.value = VariablesHolder.speed;
        _sliderText.text = speedChanger.value.ToString("0.00");
        speedChanger.onValueChanged.AddListener((v) =>
        {
            if (speedChanger.wholeNumbers == false)
            {
                _sliderText.text = v.ToString("0.00");
            }
            if (speedChanger.wholeNumbers == true)
            {
                _sliderText.text = v.ToString("0");
            }
        });
    }

    public void UpdateSpeed()
    {
        Debug.Log(VariablesHolder.speed);
        VariablesHolder.speed = speedChanger.value;
        Debug.Log(VariablesHolder.speed);
    }
}
