using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoolArrayHolder : MonoBehaviour {

    public static bool[] assetsChecks;

    public Toggle[] toggles;

    public void Start()
    {
        assetsChecks = new bool[toggles.Length];
        for (int i = 0; i < toggles.Length; i++)
        {
            assetsChecks[i] = true;

            int index = i;

            Toggle t = toggles[i];
            t.onValueChanged.AddListener(
                (bool check) =>
                {
                    CheckBox(index, check);
                }
                );
        }
    }

    void CheckBox(int index, bool check)
    {
        assetsChecks[index] = check;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void UpdateSpeed(bool[] newAssets)
    {
        assetsChecks = newAssets;
    }

}
