using Org.BouncyCastle.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel : MonoBehaviour
{
    Button[] BtnArr;
    Toggle[] TogArr;
    Transform User, Page_1, GameParent;
    GameObject TipsPage, msgItem;
    Text Text_Name, Text_Age, Text_Sex, Text_ID;
    string TableName = "UserInfo";
    string[] colname = new string[] { "UserID" };
    int startIndex = 0;
    int endIndex = 3;
    List<string[]> _dataList = new List<string[]>();
    public static bool isCheckModel;

    private void Awake()
    {
        BtnArr = transform.GetComponentsInChildren<Button>();
        GameParent = GameObject.Find("GameParent").transform;
        Text_Name = transform.Find("Message/Page/Text_Name").GetComponent<Text>();
        Text_Age = transform.Find("Message/Page/Text_Age").GetComponent<Text>();
        Text_Sex = transform.Find("Message/Page/Text_Sex").GetComponent<Text>();
        Text_ID = transform.Find("Message/Page/Text_ID").GetComponent<Text>();
        msgItem = transform.Find("Message/Page/msgItem").gameObject;
        TipsPage = transform.Find("TipsPage").gameObject;
        Page_1 = transform.Find("Message/Page/Page_1");
        User = transform.Find("UserPage/Scroll View/Viewport/User");
    }

    private void OnEnable()
    {
        MessageCenter.AddMsgListener("UserPanel", OpenUserPanel);
        MessageCenter.AddMsgListener("UnDataToggleArrs", UnDataToggleArrs);
        if (!string.IsNullOrEmpty(UserInfoData.UserId))
            InitMsgPage(UserInfoData.UserId);
    }

    void Start()
    {
        for (int i = 0; i < BtnArr.Length; i++)
        {
            Button btn = BtnArr[i];
            btn.onClick.AddListener(delegate () { this.OnButtonClick(btn); });
        }
        msgItem.SetActive(false);
        TipsPage.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (Transform item in Page_1)
        {
            Destroy(item.gameObject);
        }
    }

    private void OnToggleValueChange(Toggle tog, bool isOn)
    {
        InitMsgPage(tog.transform.parent.name);
        if (!isOn) UserInfoData.UserId = null;
    }

    /// <summary>
    /// 更新Toggle事件
    /// </summary>
    /// <param name="pa"></param>
    void UnDataToggleArrs(ParameterData pa)
    {
        InitToggleArr();
    }


    private void OnButtonClick(Button btn)
    {
        switch (btn.name)
        {
            case "Btn_Add":
                MessageCenter.SendMsg("AddPanel", true);
                this.gameObject.SetActive(false);
                break;
            case "Btn_Delete":
                if (string.IsNullOrEmpty(UserInfoData.UserId))
                {
                    TipsPage_1.tipsPage.OpenThisPage("请先选择用户再进行操作！");
                }
                else
                {
                    TipsPage.SetActive(true);
                }
                break;
            case "Btn_Modify":
                if (string.IsNullOrEmpty(UserInfoData.UserId))
                {
                    TipsPage_1.tipsPage.OpenThisPage("请先选择用户再进行操作！");
                }
                else
                {
                    MessageCenter.SendMsg("ModifyUser", true);
                    this.gameObject.SetActive(false);
                }
                break;
            case "Btn_Up_1":
                if (startIndex >= 3)
                    startIndex -= 3;
                if (endIndex >= 6)
                    endIndex -= 3;
                InitUserItem();
                break;
            case "Btn_Down_1":
                if (_dataList == null) return;
                if (startIndex < _dataList.Count - 3)
                    startIndex += 3;
                if (endIndex < _dataList.Count)
                    endIndex += 3;
                InitUserItem();
                break;
            case "Btn_Start":
                OnStartTrain();
                if (UserInfoData.TrainPlanArr != null)
                {
                    
                }
                break;
            case "Btn_Quit":
                MessageCenter.SendMsg("MainPanel", true);
                this.gameObject.SetActive(false);
                break;
            case "Btn_Train":
                if (string.IsNullOrEmpty(UserInfoData.UserId))
                {
                    TipsPage_1.tipsPage.OpenThisPage("请先选择用户再进行操作！");
                }
                else
                {
                    isCheckModel = false;
                    MessageCenter.SendMsg("TrainPanel", true);
                    this.gameObject.SetActive(false);
                }
                break;
            case "Btn_Train_1":
                if (string.IsNullOrEmpty(UserInfoData.UserId))
                {
                    TipsPage_1.tipsPage.OpenThisPage("请先选择用户再进行操作！");
                }
                else
                {
                    isCheckModel = true;
                   // TestSettle.IsTest = true;
                    MessageCenter.SendMsg("TrainPanel", true);
                    this.gameObject.SetActive(false);
                }
                break;
            case "Btn_History":
                if (string.IsNullOrEmpty(UserInfoData.UserId))
                {
                    TipsPage_1.tipsPage.OpenThisPage("请先选择用户再进行操作！");
                }
                else
                {
                    MessageCenter.SendMsg("HistoryPanel", true);
                }
                break;
            case "Btn_OK":
                if (UserInfoData.UserId != null)
                {
                    string[] Values = new string[] { UserInfoData.GetUserInfo()[1] };
                    OperateUseSQL_H.Delete_DataValues(TableName, colname, Values);
                    TipsPage_1.tipsPage.OpenThisPage("用户删除成功！");
                    UserInfoData.UserId = null;
                    MessageCenter.SendMsg("InitUserPage", true);
                }
                TipsPage.SetActive(false);
                break;
            case "Btn_NO":
                TipsPage.SetActive(false);
                break;
            default:
                break;
        }
    }


    private void OnStartTrain()
    {
        if (UserInfoData.TrainPlanArr==null)
        {
            TipsPage_1.tipsPage.OpenThisPage("请选择历史训练方案！");
            return;
        }
        string Path = "";
        switch (UserInfoData.TrainPlanArr[0])
        {
            //case "数学运算":
            //    Path = "MathOperation";
            //    OpenGamePanel("HiedOrShowGameMathGmae", Path);
            //    break;
            //case "记忆训练":
            //    Path = "KanTuShiWuObj";
            //    OpenGamePanel("HiedOrShowGameKanTuShiWu", Path);
            //    break;
            //case "飞机大战3":
            //    Path = "Prefab/PlaneGame";
            //    OpenGamePanel("PlaneGameGame", Path);
            //    break;
            //case "钓鱼":
            //    Path = "Prefab/FishGame";
            //    OpenGamePanel("FishGameStart", Path);
            //    break;
            //case "夹气球":
            //    Path = "Prefab/CathBalloon";
            //    OpenGamePanel("CathBalloonStart", Path);
            //    break;
            case "城际穿梭":
                Path = "Prefab/RipIntoGame";
                OpenGamePanel("RipIntoGameStart", Path);
                break;
            case "宇宙航行":
                Path = "Prefab/Aircraftbattle3DGame";
                OpenGamePanel("GameStartOrStop", Path);
                break;
            default:
                break;
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 打开游戏界面
    /// </summary>
    /// <param name="GameName">打开游戏消息名称</param>
    /// <param name="GamePath">游戏预制体路径</param>
    private void OpenGamePanel(string GameName, string GamePath)
    {

        if (UserInfoData.UserId == null) return;
        SetBtnPage.page.OnStartTime(float.Parse(UserInfoData.TrainPlanArr[1]));
        DateTime dateTime = DateTime.Now;
        UserInfoData.StartTime = string.Format("{0:D}/{1:D}/{2:D} {3:D}:{4:D}", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute);
        UserInfoData.class_ag.Ini(int.Parse(UserInfoData.TrainPlanArr[3]), int.Parse(UserInfoData.TrainPlanArr[4]), 5);
        RestPanel.RestTime = int.Parse(UserInfoData.TrainPlanArr[5]);
        RestPanel.MotionTime = int.Parse(UserInfoData.TrainPlanArr[6]);
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
        List<string[]> _tempList = new List<string[]>();
        int tempRow = 0;
        OperateUseSQL_H.Read_Data("Temporary_TrainMode", out _tempList, out tempRow);
        if (_tempList != null && _tempList.Count > 0)
        {
            string[] DcdName = new string[] { "ProgrammeName" };
            string[] Values = new string[] { _tempList[0][0] };
            OperateUseSQL_H.Delete_DataValues("Temporary_TrainMode", DcdName, Values);
        }
        UserInfoData.MaxAugle = UserInfoData.TrainPlanArr[3];
        UserInfoData.TrainingWeight = UserInfoData.TrainPlanArr[8];
        List<string[]> tempList = new List<string[]>();
        string[] tempArr = UserInfoData.TrainPlanArr.Take(UserInfoData.TrainPlanArr.Count() - 1).ToArray();
        tempList.Add(UserInfoData.TrainPlanArr);
        string tableName = "A" + UserInfoData.UserId + "_TraingPlan";
        OperateUseSQL_H.Add_Data(tableName, UserInfoData.TrainPlanTable, tempList);
        List<string[]> modeList = new List<string[]>();
        modeList.Add(tempArr);
        OperateUseSQL_H.Add_Data("Temporary_TrainMode", UserInfoData.TrainModelTable, modeList);
        
        MessageCenter.SendMsg(GameName, true);
        MathRSA.Instance.GradeNum = 0;
        UserInfoData.TrainPlanArr = null;


    }
    
    /// <summary>
    /// 初始化用户列表
    /// </summary>
    void InitUserItem()
    {
        if (Page_1.childCount > 0)
        {
            foreach (Transform item in Page_1)
            {
                Destroy(item.gameObject);
            }
        }
        string tablename = "A" + UserInfoData.UserId + "_TraingPlan";
        int row = 0;
        OperateUseSQL_H.Read_Data(tablename, out _dataList, out row);
        if (_dataList == null || _dataList.Count == 0) return;
        _dataList.Reverse();
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i < _dataList.Count)
            {
                string[] data = _dataList[i];
                GameObject obj = Instantiate<GameObject>(msgItem);
                obj.transform.SetParent(Page_1);
                Toggle tog = obj.transform.GetChild(0).GetComponent<Toggle>();
                tog.group = Page_1.GetComponent<ToggleGroup>();
                tog.onValueChanged.AddListener((bool isOn) => { OnTrainValueChange(tog, isOn); });
                obj.transform.GetChild(1).GetComponent<Text>().text = data[0];
                obj.transform.GetChild(2).GetComponent<Text>().text = data[1] + "分钟";
                obj.transform.GetChild(3).GetComponent<Text>().text = UserInfoData.GetGameDifficulty(data[0], data[2]);
                obj.transform.GetChild(4).GetComponent<Text>().text = data[3];
                obj.transform.GetChild(5).GetComponent<Text>().text = data[4];
                obj.transform.GetChild(6).GetComponent<Text>().text = data[5] + "秒";
                obj.transform.GetChild(7).GetComponent<Text>().text = data[6] + "秒";
                obj.transform.GetChild(8).GetComponent<Text>().text = UserInfoData.GetTrainingType(data[0]);
                obj.name = i.ToString();
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
                if (!UserInfoData.HistoryPlanDic.ContainsKey(obj.name))
                    UserInfoData.HistoryPlanDic.Add(obj.name, data);
                else
                    UserInfoData.HistoryPlanDic[obj.name] = data;
                //if (i == startIndex) tog.isOn = true;
            }
        }
    }

    private void OnTrainValueChange(Toggle tog, bool isOn)
    {
        UserInfoData.GetHistoryPlan(tog.transform.parent.name);
    }

    /// <summary>
    /// 初始化Toggle数组
    /// </summary>
    void InitToggleArr()
    {
        TogArr = null;
        TogArr = User.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < TogArr.Length; i++)
        {
            Toggle tog = TogArr[i];
            tog.onValueChanged.RemoveAllListeners();
            tog.onValueChanged.AddListener((bool isOn) => { OnToggleValueChange(tog, isOn); });
            if (!string.IsNullOrEmpty(UserInfoData.UserId))
            {
                if (UserInfoData.UserId.Equals(tog.transform.parent.name))
                {
                    tog.isOn = true;
                }
                else
                {
                    if (ScanCodePage.IsScanCode)
                        tog.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 初始化用户信息显示
    /// </summary>
    /// <param name="name"></param>
    void InitMsgPage(string name)
    {
        UserInfoData.UserId = name;
        InitUserItem();
        string[] data = UserInfoData.GetUserInfo();
        if (data == null) return;
        Text_Name.text = data[0];
        Text_ID.text = data[1];
        Text_Age.text = data[2];
        Text_Sex.text = data[3];

    }


    void OpenUserPanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        this.gameObject.SetActive(value);
        if (value)
        {
            InitToggleArr();
        }
    }

}
