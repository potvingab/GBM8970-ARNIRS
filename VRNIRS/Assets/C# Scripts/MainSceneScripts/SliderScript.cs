using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private TextMeshProUGUI _sliderText;

    // Start is called before the first frame update
    void Start()
    {
        _sliderText.text = _slider.value.ToString("0.00");
        _slider.onValueChanged.AddListener((v) =>
        {
            if (_slider.wholeNumbers == false)
            {
                _sliderText.text = v.ToString("0.00");
            }
            if (_slider.wholeNumbers == true)
            {
                _sliderText.text = v.ToString("0");
            }
        });
    }
}
