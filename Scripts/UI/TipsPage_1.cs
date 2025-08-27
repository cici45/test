using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPage_1 : MonoBehaviour
{
    public static TipsPage_1 tipsPage;

    Text tips;
    Button Btn_OK;

    private void Awake()
    {
        tips =transform.Find("BG/tips").GetComponent<Text>();
        Btn_OK = transform.Find("BG/Btn_OK").GetComponent<Button>();
        Btn_OK.onClick.AddListener(OnOKButtonClick);
    }
    private void Start()
    {
        tipsPage = this;
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Invoke("OnOKButtonClick", 2);
    }
    public void OpenThisPage(string _tip)
    {
        tips.text= _tip;
        gameObject.SetActive(true);
    }

    private void OnOKButtonClick()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
