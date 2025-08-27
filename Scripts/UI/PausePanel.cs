using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public static string _GameName;
    Button Btn_Play, Btn_Close;
    GameTimer timer;
    void Awake()
    {
        Btn_Play = transform.Find("Image/Btn_Play").GetComponent<Button>();
        Btn_Close = transform.Find("Image/Btn_Close").GetComponent<Button>();
        Btn_Play.onClick.AddListener(OnButtonPlayClick);
        Btn_Close.onClick.AddListener(OnButtonCloseClick);
        MessageCenter.AddMsgListener("PausePanel", OpenPuasPanel);
        timer = GameObject.FindObjectOfType<GameTimer>();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        MessageCenter.SendMsg("ContinueOrStopGame", true);
        MessageCenter.SendMsg("StopGame", true);
    }

    private void OnButtonPlayClick()
    {
        if (timer.gameObject.activeSelf)
        {
            timer.isTimerStop = false;
        }
        MessageCenter.SendMsg("ContinueOrStopGame", false);
        MessageCenter.SendMsg("StopGame", false);
        //PlayerRherapistAnimations.playerRherapist.IsPlay();
        AudioPlayer.instantiate.ContinuePlayingAllAudio();
        this.gameObject.SetActive(false);
    }

    private void OnButtonCloseClick()
    {
        AudioPlayer.instantiate.PlayerTherapeutistAudio("感谢您的使用，祝您早日康复");
        MessageCenter.SendMsg("ResultPanel", true);
        Time.timeScale = 1;
        if (CountDownPanel.downPanel != null)
        {
            Destroy(CountDownPanel.downPanel?.gameObject);
        }
        Invoke("BackGamePanel", 4f);
        this.gameObject.SetActive(false);
    }

    void BackGamePanel()
    {
        if (timer.gameObject.activeSelf)
        {
            timer.isTimerStop = false;
        }
        if (!string.IsNullOrEmpty(_GameName))
        {
            MessageCenter.SendMsg("ContinueOrStopGame", false);
            MessageCenter.SendMsg("StopGame", false);
            MessageCenter.SendMsg("ReceiveData", false);
            //MessageCenter.SendMsg(_GameName, false);
            SetBtnPage.page.BackTimePage();
            AudioPlayer.instantiate.StopAllAudios();
            MessageCenter.SendMsg("ResultPanel", false);
            _GameName = "";
        }
    }

    void OpenPuasPanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        this.gameObject.SetActive(value);
        AudioPlayer.instantiate.PauseAllAudios();
        //PlayerRherapistAnimations.playerRherapist.IsPause();
    }
}
