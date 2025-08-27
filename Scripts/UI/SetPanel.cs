using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : MonoBehaviour
{
    Button Btn_OK, Btn_Back;
    Slider Slider_Therapeutist, Slider_Tip;
    Text Text_Therapeutist, Text_Tip;
    AudioSource Audio_Therapeutist, Audio_Tip;
    float value_Therapeutist, value_Tip, change_Therapeutist, change_Tip;
    GameTimer timer;
    void Awake()
    {
        Audio_Therapeutist = GameObject.Find("Audio_Therapeutist").GetComponent<AudioSource>();
        Audio_Tip = GameObject.Find("Audio_Tip").GetComponent<AudioSource>();
        Text_Therapeutist = transform.Find("Image/Text_Therapeutist").GetComponent<Text>();
        Text_Tip = transform.Find("Image/Text_Tip").GetComponent<Text>();
        Slider_Therapeutist = transform.Find("Image/Slider_Therapeutist").GetComponent<Slider>();
        Slider_Tip = transform.Find("Image/Slider_Tip").GetComponent<Slider>();
        Btn_OK = transform.Find("Image/Btn_OK").GetComponent<Button>();
        Btn_Back = transform.Find("Image/Btn_Back").GetComponent<Button>();
        Btn_OK.onClick.AddListener(OnOKButtonClick);
        Btn_Back.onClick.AddListener(OnBackButtonClick);
        MessageCenter.AddMsgListener("SetPanel", OpenSetPanel);
        timer = GameObject.FindObjectOfType<GameTimer>();
    }

    private void OnEnable()
    {
        MessageCenter.SendMsg("ContinueOrStopGame", true);
        MessageCenter.SendMsg("StopGame", true);
        //MessageCenter.SendMsg("GamePauseOrContinue", true);
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnOKButtonClick()
    {
        Audio_Therapeutist.volume = change_Therapeutist;
        Audio_Tip.volume = change_Tip;
        timer.isTimerStop = false;
        MessageCenter.SendMsg("ContinueOrStopGame", false);
        MessageCenter.SendMsg("StopGame", false);
        //MessageCenter.SendMsg("GamePauseOrContinue", false);
        this.gameObject.SetActive(false); 
    }

    private void OnBackButtonClick()
    {
        Slider_Therapeutist.value = value_Therapeutist;
        Slider_Tip.value = value_Tip;
        timer.isTimerStop = false;
        MessageCenter.SendMsg("ContinueOrStopGame", false);
        MessageCenter.SendMsg("StopGame", false);
        //MessageCenter.SendMsg("ContinueOrStopGame", false);
        //MessageCenter.SendMsg("GamePauseOrContinue", false);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int num1 = (int)(Slider_Tip.value * 100); 
        int num2 = (int)(Slider_Therapeutist.value * 100);
        change_Therapeutist = Slider_Therapeutist.value;
        change_Tip = Slider_Tip.value;
        Text_Tip.text = num1.ToString();
        Text_Therapeutist.text = num2.ToString();
    }

    void OpenSetPanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        this.gameObject.SetActive(value);
        AudioPlayer.instantiate.PauseAllAudios();
        value_Therapeutist = Slider_Therapeutist.value;
        value_Tip = Slider_Tip.value;
    }

    private void OnDisable()
    {
        AudioPlayer.instantiate.ContinuePlayingAllAudio();
    }

}
