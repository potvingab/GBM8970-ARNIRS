using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object3DSelection : MonoBehaviour {

    [SerializeField] private Button leftSwicth;
    [SerializeField] private Button rightSwicth;
    private int currentModel;

    private void Awake()
    {
        SelectModel(0);
    }

    public void SelectModel(int _index)
    {
        leftSwicth.interactable = (_index != 0);
        rightSwicth.interactable = (_index != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeModel(int _change)
    {
        currentModel += _change;
        SelectModel(currentModel);
    }
}
