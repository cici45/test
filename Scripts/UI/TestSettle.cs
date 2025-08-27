using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class TestSettle : MonoBehaviour
{
    public static string AngleTableName = "A202307311613";
    public static string GameTime;
    public static string GameDifficulty;
    public static bool IsTest;

    Text Text_Name, Text_Age, Text_Sex, Text_ID, StartTime, gameTime, gameDifficulty;
    Button Btn_Quit, Btn_Save, Btn_Data, Btn_Back;
    LineChart LineChart_1, LineChart_2, LineChart_3;
    InputField Input_1, Input_2, Input_3, Input_Name;
    GameObject DataPage;
    List<string[]> Readdata;
    int row;
    string TableName;
    List<string[]> TableData = new List<string[]>();
    string[] Table = new string[] { "ID", "StartTime", "gameTime", "gameDifficulty", "关节活动度", "肌力", "耐力", "医生签名", "AngleTableName" };
    CheckRecord check;
    void Awake()
    {
        MessageCenter.AddMsgListener("TestSettle", OpenTestSettle);

        DataPage = transform.Find("DataPage").gameObject;

        Text_Name = transform.Find("BG/Text_Name").GetComponent<Text>();
        Text_Age = transform.Find("BG/Text_Age").GetComponent<Text>();
        Text_Sex = transform.Find("BG/Text_Sex").GetComponent<Text>();
        Text_ID = transform.Find("BG/Text_ID").GetComponent<Text>();
        StartTime = transform.Find("BG/StartTime").GetComponent<Text>();
        gameTime = transform.Find("BG/gameTime").GetComponent<Text>();
        gameDifficulty = transform.Find("BG/gameDifficulty").GetComponent<Text>();

        Btn_Quit = transform.Find("BG/Btn_Quit").GetComponent<Button>();
        Btn_Save = transform.Find("BG/Btn_Save").GetComponent<Button>();
        Btn_Data = transform.Find("BG/Btn_Data").GetComponent<Button>();
        Btn_Back = transform.Find("DataPage/Btn_Back").GetComponent<Button>();

        Input_1 = transform.Find("BG/Input_1").GetComponent<InputField>();
        Input_2 = transform.Find("BG/Input_2").GetComponent<InputField>();
        Input_3 = transform.Find("BG/Input_3").GetComponent<InputField>();
        Input_Name = transform.Find("BG/Input_Name").GetComponent<InputField>();

        LineChart_1 = transform.Find("DataPage/LineChart_1").GetComponent<LineChart>();
        LineChart_2 = transform.Find("DataPage/LineChart_2").GetComponent<LineChart>();
        LineChart_3 = transform.Find("DataPage/LineChart_3").GetComponent<LineChart>();
    }

    private void Start()
    {
        check = new CheckRecord();
        Btn_Quit.onClick.AddListener(OnQuitButtonClick);
        Btn_Save.onClick.AddListener(OnSaveButtonClick);
        Btn_Back.onClick.AddListener(OnBackButtonClick);
        Btn_Data.onClick.AddListener(OnDataButtonClick);
        //Input_1.onValueChanged.AddListener(OnInput1Changed);
        //Input_2.onValueChanged.AddListener(OnInput2Changed);
        //Input_3.onValueChanged.AddListener(OnInput3Changed);
        //Input_Name.onValueChanged.AddListener(OnInputNameChanged);
        
        DataPage.SetActive(false);
        this.gameObject.SetActive(false);
    }

    //private void OnInput1Changed(string arg0)
    //{
    //    throw new NotImplementedException();
    //}

    //private void OnInput2Changed(string arg0)
    //{
    //    throw new NotImplementedException();
    //}

    //private void OnInput3Changed(string arg0)
    //{
    //    throw new NotImplementedException();
    //}

    //private void OnInputNameChanged(string arg0)
    //{
    //    throw new NotImplementedException();
    //}

    private void OnBackButtonClick()
    {
        DataPage.SetActive(false);
    }

    private void OnDataButtonClick()
    {
        DataPage.SetActive(true);
    }

    private void OnSaveButtonClick()
    {
        SavePanelData();
    }

    private void OnQuitButtonClick()
    {
        MessageCenter.SendMsg("UserPanel", true);
        this.gameObject.SetActive(false);
    }

    void SavePanelData()
    {
        TableData.Clear();
        if (string.IsNullOrEmpty(Input_Name.text))
        {
            TipsPage_1.tipsPage.OpenThisPage("请填写医生名称或医生ID完成签名！");
        }
        else
        {
            TableName = "A" + Text_ID.text + "_TestSettle";
            string[] tempData = new string[9];
            tempData[0] = Text_ID.text;
            tempData[1] = StartTime.text;
            tempData[2] = gameTime.text;
            tempData[3] = gameDifficulty.text;
            tempData[4] = Input_1.text;
            tempData[5] = Input_2.text;
            tempData[6] = Input_3.text;
            tempData[7] = Input_Name.text;
            tempData[8] = AngleTableName;
            check.checkTime = StartTime.text;
            check.checkDuration = gameTime.text;
            check.checkGear= gameDifficulty.text;
            check.deviceNum = UserInfoData.EquipmentName;
            check.myodynamia = Input_2.text;
            check.endurance = Input_3.text;
            check.jointMotion = Input_1.text;
            TableData.Add(tempData);
            OperateUseSQL_H.Add_Data(TableName, Table, TableData);
            try
            {
                Debug.Log(DataUpload.loadData.Post("http://"+UserInfoData.Ip+"/comprehensive/inAssessmentTraining", check));//测试ip 192.168.2.55:8081
            }
            catch (Exception)
            {
                Debug.LogError("连接失败！！！！！！");
            }
            MessageCenter.SendMsg("UserPanel", true);
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        
        IsTest = false;
    }

    private void OpenTestSettle(ParameterData pa)
    {
        bool value = (bool)pa.data;
        if (value)
        {
            UserInfoData.class_ag.Out_Data(out UserInfoData.settleData.Total_Data_Max, out UserInfoData.settleData.Total_Data_Min, out UserInfoData.settleData.Total_V_Main,
               out UserInfoData.settleData.Total_V_Max, out UserInfoData.settleData.V_Main, out UserInfoData.settleData.V_Max, out UserInfoData.settleData.V_Min,
               out UserInfoData.settleData.Total_ACC_Main, out UserInfoData.settleData.Total_Acc_Max, out UserInfoData.settleData.ACC_Main, out UserInfoData.settleData.ACC_Max,
               out UserInfoData.settleData.ACC_Min, out UserInfoData.settleData.n_Feedback, out UserInfoData.settleData.Muscle_State,
               out UserInfoData.settleData.Endurance_time, out UserInfoData.settleData.Endurance_Value);
            Input_1.text = (UserInfoData.settleData.Total_Data_Max - UserInfoData.settleData.Total_Data_Min).ToString("f2");
            Input_2.text= Mathf.Abs((float)UserInfoData.settleData.Total_Acc_Max * 1000).ToString("f2");
            Input_3.text= Mathf.Abs((float)UserInfoData.settleData.Endurance_Value * 1000).ToString("f2");
            InitTestSettlePanel();
        }
        this.gameObject.SetActive(value);
    }

    void InitTestSettlePanel()
    {
        OperateUseSQL_H.Read_Data(AngleTableName, out Readdata, out row);
        if (Readdata != null)
        {
            double[] tempArr1 = new double[200];
            double[] tempArr2 = new double[200];
            double[] tempArr3 = new double[200];
            double[] tempArrX = new double[200];
            float index = 0;
            if (Readdata.Count < 200)
            {
                index = Readdata.Count;
            }
            else
            {
                index = 200;
            }
            for (int i = 0; i < index; i++)
            {
                double temp1 = Mathf.Abs(float.Parse(Readdata[i][0]));
                double temp2 = Mathf.Abs(float.Parse(Readdata[i][1]) * 100);
                double temp3 = Mathf.Abs(float.Parse(Readdata[i][2]) * 10000);

                tempArr1[i] = Math.Round(temp1, 2);
                tempArr2[i] = Math.Round(temp2, 2);
                tempArr3[i] = Math.Round(temp3, 2);
                tempArrX[i] = Math.Round(i * 0.15, 2);
            }
            Debug.Log(UserInfoData.UserId);
            if (UserInfoData.UserId != null)
            {
                string[] tempArr = UserInfoData.GetUserInfo();
                if (tempArr != null)
                {
                    Text_Name.text = tempArr[0];
                    Text_ID.text = tempArr[1];
                    Text_Age.text = tempArr[2];
                    Text_Sex.text = tempArr[3];
                    StartTime.text = UserInfoData.StartTime.Split(' ')[0];
                    gameTime.text = GameTime;
                    gameDifficulty.text = GameDifficulty;
                }
            }
            InitLineChart(LineChart_1, "角度变化图", tempArr1, tempArrX);
            InitLineChart(LineChart_2, "角速度变化图", tempArr2, tempArrX);
            InitLineChart(LineChart_3, "角加速度变化图", tempArr3, tempArrX);
            check.xAxis = string.Join("_", tempArrX);
            check.angleCurve = string.Join("_", tempArr1);
            check.velocityCurve = string.Join("_", tempArr2);
            check.accelerationCurve = string.Join("_", tempArr3);
        }
    }


    void InitLineChart(LineChart line, string tableName, double[] YtabelData = null,double[] XtabelData = null)
    {
        line.SetSize(860, 300);//代码动态添加图表需要设置尺寸
        //设置标题：
        line.title.show = true;
        line.title.text = tableName;

        //设置提示框和图例是否显示
        line.tooltip.show = true;
        line.legend.show = false;

        //设置是否使用双坐标轴和坐标轴类型
        line.xAxes[0].show = true;
        line.xAxes[1].show = false;
        line.yAxes[0].show = true;
        line.yAxes[1].show = false;
        line.xAxes[0].type = Axis.AxisType.Category;
        line.yAxes[0].type = Axis.AxisType.Value;
        switch (tableName)
        {
            case "角度变化图":
                line.yAxes[0].axisName.name = "θ";
                break;
            case "角速度变化图":
                line.yAxes[0].axisName.name = "v(°/s)";
                break;
            case "角加速度变化图":
                line.yAxes[0].axisName.name = "a(°/s²)";
                break;
            default:
                break;
        }
        line.xAxes[0].axisName.name = "t";
        line.xAxes[0].axisName.show = true;
        line.yAxes[0].axisName.show = true;
        line.xAxes[0].axisName.textStyle.offset = new Vector2(10, 0);
        line.yAxes[0].axisName.textStyle.offset = new Vector2(0, 20);

        //设置坐标轴分割线
        line.xAxes[0].splitNumber = 10;
        line.xAxes[0].boundaryGap = true;

        //清空数据，添加`Line`类型的`Serie`用于接收数据
        line.RemoveData();
        line.AddSerie(SerieType.Line);

        //添加数据
        if (YtabelData != null&& XtabelData != null)
        {
            for (int i = 0; i < YtabelData.Length; i++)
            {
                line.AddData(0, YtabelData[i]);
                line.AddXAxisData(XtabelData[i] + "s");
            }
        }
    }

}

public class CheckRecord
{
    /// <summary>
    /// 检查时间
    /// </summary>
    public string checkTime;
    /// <summary>
    /// 检查时长
    /// </summary>
    public string checkDuration;
    /// <summary>
    /// 检查挡位
    /// </summary>
    public string checkGear;
    /// <summary>
    /// 设备编号
    /// </summary>
    public string deviceNum;
    /// <summary>
    /// 肌力
    /// </summary>
    public string myodynamia;
    /// <summary>
    /// 耐力
    /// </summary>
    public string endurance;
    /// <summary>
    /// 关节活动度
    /// </summary>
    public string jointMotion;
    /// <summary>
    /// X轴
    /// </summary>
    public string xAxis;
    /// <summary>
    /// 角度变化y轴
    /// </summary>
    public string angleCurve;
    /// <summary>
    /// 速度变化y轴
    /// </summary>
    public string velocityCurve;
    /// <summary>
    /// 加速度变化y轴
    /// </summary>
    public string accelerationCurve;
}

