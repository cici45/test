using DG.Tweening;
using Org.BouncyCastle.Ocsp;
using Senser_Driver_Space;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class TrainPanel : MonoBehaviour
{
    Text time, time_Hold, time_Exercise;
    Slider slider, Slider_Exercise, Slider_Hold;
    Button[] BtnArr;
    Toggle Tog_Simple, Tog_General, Tog_Difficulty;
    string ProgrammeName, Difficulty;
    Transform GameParent;
    //InputField Input_Max, Input_Min, Input_TrainingWeight;
    Dropdown Dropdown_Max, Dropdown_Min, Dropdown_TrainingWeight;
    List<string[]> Readdata = new List<string[]>();
    int row = 0;
    Senser_Driver senser_1;
    string[] Tempdata;
    private void Awake()
    {
        GameParent = GameObject.Find("GameParent").transform;
        Tog_Simple = transform.Find("UP/Tog_Simple").GetComponent<Toggle>();
        Tog_General = transform.Find("UP/Tog_General").GetComponent<Toggle>();
        Tog_Difficulty = transform.Find("UP/Tog_Difficulty").GetComponent<Toggle>();
        time = transform.Find("UP/time").GetComponent<Text>();
        slider = transform.Find("UP/Slider").GetComponent<Slider>();
        time_Hold = transform.Find("Center/time_Hold").GetComponent<Text>();
        Slider_Hold = transform.Find("Center/Slider_Hold").GetComponent<Slider>();
        time_Exercise = transform.Find("Center/time_Exercise").GetComponent<Text>();
        Slider_Exercise = transform.Find("Center/Slider_Exercise").GetComponent<Slider>();
        BtnArr = transform.GetComponentsInChildren<Button>();
        //Input_Max=transform.Find("Center/Input_Max").GetComponent<InputField>();
        //Input_Min = transform.Find("Center/Input_Min").GetComponent<InputField>();
        //Input_TrainingWeight = transform.Find("Center/Input_TrainingWeight").GetComponent<InputField>();
        Dropdown_Max = transform.Find("Center/Dropdown_Max").GetComponent<Dropdown>();
        Dropdown_Min = transform.Find("Center/Dropdown_Min").GetComponent<Dropdown>();
        Dropdown_TrainingWeight = transform.Find("Center/Dropdown_TrainingWeight").GetComponent<Dropdown>();
        senser_1 = new Senser_Driver(Application.streamingAssetsPath + "/AutomaticGearAdjustment/Com_XML.xml");
        Dropdown_TrainingWeight.onValueChanged.AddListener(AutomaticGearAdjustment);

    }


    void Start()
    {
        for (int i = 0; i < BtnArr.Length; i++)
        {
            Button btn = BtnArr[i];
            btn.onClick.AddListener(delegate () { this.OnButtonClick(btn); });
        }
        MessageCenter.AddMsgListener("TrainPanel", OpenTrainPanel);
        MessageCenter.AddMsgListener("OpenGame", OpenGame);
        Tog_Simple.onValueChanged.AddListener(OnSimpToggleValueChange);
        Tog_General.onValueChanged.AddListener(OnGeneralToggleValueChange);
        Tog_Difficulty.onValueChanged.AddListener(OnDifficultyToggleValueChange);
        Tog_Simple.isOn = true;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int num = (int)(slider.value);
        time.text = num.ToString();
        int num2 = (int)(Slider_Hold.value);
      //  RestPanel.RestTime = num2;
        time_Hold.text = num2.ToString();
        int num3 = (int)(Slider_Exercise.value);
        time_Exercise.text = num3.ToString();
        RestPanel.MotionTime = num3;
    }
    void OpenGame(ParameterData pa)
    {
       // UserPanel.isCheckModel = true;
        string path;
        string path1;
        path = "Prefab/Aircraftbattle3DGame";
        path1 = "Prefab/Cheak/CarryCheckGame";
        MessageCenter.SendMsg("UserPanel", true);
        if (UserPanel.isCheckModel)
        {
            time.text = "1";
            ProgrammeName = "搬运宝箱";
        }
        else
        {
            time.text = "20";
            ProgrammeName = "宇宙航行";
        }
        OpenTrainPanel(new ParameterData(true));
        if (UserPanel.isCheckModel)
        {
            LoadGamePanel("CarryGameCheckStart", path1);
        }
        else
        {
            LoadGamePanel("GameStartOrStop", path);
        }
       
    }

    void OpenTrainPanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        this.gameObject.SetActive(value);
        //Input_Max.text = "";
        //Input_Min.text = "";
        Tempdata = UserInfoData.GetUserInfo();
        OperateUseSQL_H.Read_Data("Temporary_TrainMode", out Readdata, out row);
        senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 02 ab 00 10 ff ff"), 0, 10);
        Debug.Log("1122");
        //Destroy(this.gameObject);
    }

    private void OnSimpToggleValueChange(bool arg0)
    {
        Difficulty = "简单";
    }

    private void OnGeneralToggleValueChange(bool arg0)
    {
        Difficulty = "一般";
    }

    private void OnDifficultyToggleValueChange(bool arg0)
    {
        Difficulty = "困难";
    }

    private void OnButtonClick(Button btn)
    {
        string Path = "";
        switch (btn.name)
        {
            case "Btn_Back":
                MessageCenter.SendMsg("UserPanel", true);
                this.gameObject.SetActive(false);
                break;
            //case "Btn_MathOperation":
            //    ProgrammeName = "数学运算";
            //    Path = "MathOperation";
            //    LoadGamePanel("HiedOrShowGameMathGmae", Path);
            //    break;
            //case "Btn_MemoryTraining":
            //    ProgrammeName = "记忆训练";
            //    Path = "KanTuShiWuObj";
            //    LoadGamePanel("HiedOrShowGameKanTuShiWu", Path);
            //    break;
            //case "Btn_Miner":
            //    ProgrammeName = "飞机大战3";
            //    Path = "Prefab/PlaneGame";
            //    LoadGamePanel("PlaneGameStart", Path);
            //    break;
            //case "Btn_Fishing":
            //    ProgrammeName = "钓鱼";
            //    Path = "Prefab/FishGame";
            //    LoadGamePanel("FishGameStart", Path);
            //    break;
            //case "Btn_CycleRacing":
            //    ProgrammeName = "夹气球";
            //    Path = "Prefab/CathBalloon";
            //    LoadGamePanel("CathBalloonStart", Path);
            //    break;
            case "Btn_RipIntoGame":
                ProgrammeName = "城际穿梭";
                Path = "Prefab/RipIntoGame";
                LoadGamePanel("RipIntoGameStart", Path);
                break;
            case "Btn_AircraftBattle":
                ProgrammeName = "宇宙航行";
                Path = "Prefab/Aircraftbattle3DGame";
                LoadGamePanel("GameStartOrStop", Path);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 打开游戏界面
    /// </summary>
    /// <param name="GameName">消息名</param>
    /// <param name="GamePath">游戏预制体路径</param>
    void LoadGamePanel(string GameName, string GamePath)
    {
        //if (string.IsNullOrEmpty(Input_Max.text)||string.IsNullOrEmpty(Input_Min.text)||string.IsNullOrEmpty(Input_TrainingWeight.text))
        //{
        //    TipsPage_1.tipsPage.OpenThisPage("数据填写不完全，请完所有数据值填写！");
        //}
        //else
        //{
        MathRSA.Instance.GradeNum = 0;
        InitTemporaryTrainModeTable();
        SetBtnPage.page.OnStartTime(float.Parse(time.text));
        PausePanel._GameName = GameName;
        if (GameParent.Find(GameName))
        {
            GameParent.Find(GameName).gameObject.SetActive(true);
        }
        else
        {
            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>(GamePath));
            obj.name = GameName;
            obj.transform.SetParent(GameParent);
            obj.transform.localPosition = Vector2.zero;
            obj.transform.localScale = Vector3.one;
        }
        GameObject obj1 =  Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/CountDownPanel"));
        obj1.name = "CountDownPanel";
        obj1.transform.SetParent(GameParent);
        obj1.transform.localPosition = Vector2.zero;
        obj1.transform.localScale = Vector3.one;
        MessageCenter.SendMsg("ReceiveData", true);
        this.gameObject.SetActive(false);
        Time.timeScale = 0;
        DOVirtual.Float(UserInfoData.countDown, 0f, UserInfoData.countDown, (value) =>
        {
            int seconds = Mathf.CeilToInt(value);
            CountDownPanel.downPanel.NumTip.text = seconds.ToString();  // 更新倒计时文本
        }).SetUpdate(true).OnComplete(() =>
        {
            // CountDownPanel.downPanel.NumTip.text = "开始！";  // 倒计时结束，显示"开始！"
            Time.timeScale = 1;
            // MessageCenter.SendMsg(GameName, true);
            MessageCenter.SendMsg(GameName, true);
            MessageCenter.SendMsg("UserPanel", false);
            Destroy(obj1);
            // 执行开始游戏的方法
        }).SetEase(Ease.Linear);

    }


   
    /// <summary>
    /// 上传所选参数
    /// </summary>
    void InitTemporaryTrainModeTable()
    {
        if (Readdata != null && Readdata.Count > 0)
        {
            string[] DcdName = new string[] { "ProgrammeName" };
            string[] Values = new string[] { Readdata[0][0] };
            OperateUseSQL_H.Delete_DataValues("Temporary_TrainMode", DcdName, Values);
        }
        string[] tempdata = new string[8];
        tempdata[0] = ProgrammeName;
        tempdata[1] = time.text;
        tempdata[6] = time_Exercise.text;
        tempdata[5] = time_Hold.text;
        switch (ProgrammeName)
        {
            case "宇宙航行":
            case "城际穿梭":
                switch (Difficulty)
                {
                    case "简单":
                        tempdata[2] = "10";
                        break;
                    case "一般":
                        tempdata[2] = "7";
                        break;
                    case "困难":
                        tempdata[2] = "5";
                        break;
                    default:
                        break;
                }
                tempdata[7] = "1";
                break;
            default:
                break;
        }
        tempdata[3] = Dropdown_Max.captionText.text;
        UserInfoData.MaxAugle = Dropdown_Max.captionText.text;
        tempdata[4] = Dropdown_Min.captionText.text;
        List<string[]> tempList = new List<string[]>();
        tempList.Add(tempdata);
        OperateUseSQL_H.Add_Data("Temporary_TrainMode", UserInfoData.TrainModelTable, tempList);
        if (Tempdata == null) return;
        List<System.String> list_2 = new List<System.String>(tempdata);
        list_2.Add(Dropdown_TrainingWeight.captionText.text);
        UserInfoData.TrainingWeight = Dropdown_TrainingWeight.captionText.text;
        List<string[]> data_2 = new List<string[]> { list_2.ToArray() };
        string tableName = "A" + Tempdata[1] + "_TraingPlan";
        OperateUseSQL_H.Add_Data(tableName, UserInfoData.TrainPlanTable, data_2);
        DateTime dateTime = DateTime.Now;
        UserInfoData.StartTime = dateTime.ToString("yyyy/MM/dd HH:mm");
        UserInfoData.class_ag.Ini(int.Parse(Dropdown_Max.captionText.text), int.Parse(Dropdown_Min.captionText.text), 5);
        TestSettle.GameDifficulty = Dropdown_TrainingWeight.captionText.text;
        TestSettle.GameTime = time.text;
    }
    /// <summary>
    /// 自动档位调整
    /// </summary>
    public void AutomaticGearAdjustment(int n)
    {
        switch (n)
        {
            case 0:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 00 00 00 10 ff ff"), 0, 10);
                break;
            case 1:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 01 56 00 10 ff ff"), 0, 10);
                break;
            case 2:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 02 ab 00 10 ff ff"), 0, 10);
                break;
            case 3:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 04 00 00 10 ff ff"), 0, 10);
                break;
            case 4:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 05 55 00 10 ff ff"), 0, 10);
                break;
            case 5:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 06 aa 00 10 ff ff"), 0, 10);
                break;
            case 6:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 07 ff 00 10 ff ff"), 0, 10);
                break;
            case 7:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 09 54 00 10 ff ff"), 0, 10);
                break;
            case 8:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 0a a9 00 10 ff ff"), 0, 10);
                break;
            case 9:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 0b fe 00 10 ff ff"), 0, 10);
                break;
            case 10:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 0d 53 00 10 ff ff"), 0, 10);
                break;
            case 11:
                senser_1.Com_232.Write_Byte_Data(senser_1.Com_232.ConvertStringToByteArray("ff ff 00 01 0e a8 00 10 ff ff"), 0, 10);
                break;
            default:
                break;

        }

    }
}

