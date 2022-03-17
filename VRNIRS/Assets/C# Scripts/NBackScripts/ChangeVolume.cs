using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SFB;
using TMPro;
using System.Text.RegularExpressions;

public class ChangeVolume : MonoBehaviour
{

    public GameObject AudioVolume;
    public GameObject AudioVolumeField;
    public static float audioVolume;

    // Use this for initialization
    void Start()
    {
        AudioVolumeField.GetComponent<TMP_InputField>().text = VariablesHolder.audioVolume.ToString("f1");
        AudioVolume.GetComponent<Slider>().value = VariablesHolder.audioVolume;
        AudioVolume.GetComponent<Slider>().onValueChanged.AddListener(delegate
        {
            SliderValueChanged(AudioVolume.GetComponent<Slider>());
        });
        AudioVolumeField.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate
        {
            SliderFieldValueChanged(AudioVolumeField.GetComponent<TMP_InputField>());
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SliderValueChanged(Slider slid)
    {
        Debug.Log(slid.value.ToString());
        AudioVolumeField.GetComponent<TMP_InputField>().text = slid.value.ToString("f1");
    }
    void SliderFieldValueChanged(TMP_InputField inp)
    {
        Debug.Log(inp.text);
        var val = Math.Min(1, float.Parse(inp.text));
        val = Math.Max(0, float.Parse(inp.text));
        inp.text = val.ToString();
        AudioVolume.GetComponent<Slider>().value = val;
    }

    public void ChangeParameters()
    {
        audioVolume = AudioVolume.GetComponent<Slider>().value;
        AudioListener.volume = audioVolume/100;
        Debug.Log("Audio Volume: " + audioVolume);
    }
}
