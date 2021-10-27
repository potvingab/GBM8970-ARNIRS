using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealistBool : MonoBehaviour
{

    public static bool realistCheck;

    public Toggle realToggleFixed;
    public Toggle realToggleRandom;

    public void Start()
    {
        realistCheck = false;

        realToggleFixed.onValueChanged.AddListener(
            (bool check) =>
            {
                CheckBox(check);
            }
        );

        realToggleRandom.onValueChanged.AddListener(
            (bool check) =>
            {
                CheckBox(check);
            }
        );
    }

    void CheckBox(bool check)
    {
        realistCheck = check;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
