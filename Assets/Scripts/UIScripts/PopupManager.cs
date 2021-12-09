using System.Collections;
using System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using UnityEngine;
using System;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public GameObject loadingPanel;

    public float animSpeed = 0.3f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ShowLoadingPopup()
    {
        loadingPanel.SetActive(true);
        LeanTween.scale(loadingPanel, new Vector3(1, 1, 1), animSpeed).setEase(LeanTweenType.easeInQuad);
    }

    public async void HideLoadingPopup()
    {
        LeanTween.scale(loadingPanel, new Vector3(0, 0, 0), animSpeed).setEase(LeanTweenType.easeInQuad);
        await Tasks.Task.Delay((int) Math.Floor(animSpeed * 100f));
        loadingPanel.SetActive(false);
    }
}
