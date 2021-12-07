using System;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabFrames;

    void OnEnable()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int copy = i;
            tabButtons[i].onClick.AddListener(() => { ActivateTab(copy); });
        }
    }

    void ActivateTab(int index)
    {
        for (int i = 0; i < tabFrames.Length; i++)
        {
            tabFrames[i].SetActive(false);
        }
        Debug.Log(index);
        tabFrames[index].SetActive(true);
    }
}