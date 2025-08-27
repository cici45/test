using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    Button Btn_Quit, Btn_Enter, Btn_ScanCode;
    Image Logo, Tips;
    GameObject ScanCode;

    void Start()
    {
        MessageCenter.AddMsgListener("MainPanel", OpenMainPanel);
        ScanCode = transform.Find("ScanCodePage").gameObject;
        Btn_Quit = transform.Find("Btn_Quit").GetComponent<Button>();
        Btn_Enter = transform.Find("Btn_Enter").GetComponent<Button>();
        Btn_ScanCode = transform.Find("Btn_ScanCode").GetComponent<Button>();
        Logo = transform.Find("Logo").GetComponent<Image>();
        Tips = transform.Find("Tips").GetComponent<Image>();
        Btn_Quit.onClick.AddListener(OnQuitButtonClick);
        Btn_Enter.onClick.AddListener(OnEnterButtonClick);
        Btn_ScanCode.onClick.AddListener(OnScanCodeButtonClick);
        LoadAB(Logo, "Logo.png");
        LoadAB(Tips, "Tips.png");
      //  ScanCode.SetActive(false);
    }

    private void OnScanCodeButtonClick()
    {
        ScanCode.SetActive(true);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void OnEnterButtonClick()
    {
        this.gameObject.SetActive(false);
        ScanCodePage.IsScanCode = true;
        MessageCenter.SendMsg("AddPanel", true);
    }

    void OpenMainPanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        this.gameObject.SetActive(value);
        //Destroy(this.gameObject);
    }
    public void LoadAB(Image image, string objname)
    {
        string _path = Application.dataPath + "/StreamingAssets/" + objname;//获取地址

        FileStream _fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read);//使用流数据读取

        byte[] _buffur = new byte[_fileStream.Length];

        _fileStream.Read(_buffur, 0, _buffur.Length);//转换成字节流数据

        Texture2D _texture2d = new Texture2D(10, 10);//设置宽高

        _texture2d.LoadImage(_buffur);//将流数据转换成Texture2D

        Sprite _sprite = Sprite.Create(_texture2d, new Rect(0, 0, _texture2d.width, _texture2d.height), Vector3.zero);//创建一个Sprite

        image.sprite = _sprite;//赋值
    }
}
