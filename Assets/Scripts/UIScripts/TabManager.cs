using System;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabFrames;
    int lastTab = 0;

    void OnEnable()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int copy = i;
            tabButtons[i].onClick.AddListener(() => { ActivateTab(copy); });
        }
    }

    async void ActivateTab(int index)
    {
        Debug.Log(index + " _  last: " + lastTab);
        if (index > lastTab)
        {
            for (int i = 0; i < tabFrames.Length; i++)
            {
                if (i == index) break;
                LeanTween.moveLocalX(tabFrames[i], -1080, 0.1f);
                await System.Threading.Tasks.Task.Delay(100);
                tabFrames[i].SetActive(false);
            }
        } else if (index < lastTab)
        {
            for (int i = tabFrames.Length - 1; i >= 0; i--)
            {
                if (i < index) break;
                LeanTween.moveLocalX(tabFrames[i], 0, 0.2f);
                await System.Threading.Tasks.Task.Delay(100);
                tabFrames[i].SetActive(true);
            }
        }
        lastTab = index;
        //Debug.Log(index);
        //tabFrames[index].SetActive(true);
    }
}