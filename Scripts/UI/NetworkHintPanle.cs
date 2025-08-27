using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkHintPanle : MonoBehaviour
{
    public static NetworkHintPanle tipsPage;

    Text tips;
    Button Btn_OK;
    Button notBtn;
    public UnityAction notAction;
    public  UnityAction okAction;

    private void Awake()
    {
        tipsPage = this;
        this.gameObject.SetActive(false);
        tips =transform.Find("BG/tips").GetComponent<Text>();
        Btn_OK = transform.Find("BG/Btn_OK").GetComponent<Button>();
        notBtn = transform.Find("BG/Btn_Not").GetComponent<Button>();
        notBtn.onClick.AddListener(OnNotButtonClick);
        Btn_OK.onClick.AddListener(OnOkButtonClick);
    }
    private void Start()
    {
       
    }
   
    public void OpenHintPanel(string _tip) 
    {
        tips.text = _tip;
        gameObject.SetActive(true);
        
    }
    private void OnNotButtonClick()
    {
        this.gameObject.SetActive(false);
        if (notAction!=null)
        {
            notAction();
        }
    }
    private void OnOkButtonClick()
    {
        this.gameObject.SetActive(false);
        if (okAction != null)
        {
            okAction();
        }
    }
}
