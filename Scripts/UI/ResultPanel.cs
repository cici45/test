using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    Text Score, CompletionDegree;
    private void Awake()
    {
        Score = transform.Find("BG/Score").GetComponent<Text>();
        CompletionDegree = transform.Find("BG/CompletionDegree").GetComponent<Text>();
        MessageCenter.AddMsgListener("ResultPanel", OpenResultPanel);
        gameObject.SetActive(false);
    }


    void OnEnable()
    {
        InitThisPanel(PlanPlayer.gamegrade);
        MessageCenter.SendMsg("ContinueOrStopGame", true);
    }
    private void OpenResultPanel(ParameterData pa)
    {
        gameObject.SetActive((bool)pa.data);
    }

    void InitThisPanel(float score)
    {
        Score.text = score + "·Ö";
        CompletionDegree.text = ((PlanPlayer.gamegrade / ((PlanMangenter.goldsum) * 0.5f)) * 100).ToString("f2") + "%";
    }
}
