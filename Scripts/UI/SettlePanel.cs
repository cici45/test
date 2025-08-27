using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class SettlePanel : MonoBehaviour
{
    Button[] BtnArr;
    Text ganeName, gameTime, gameDifficulty, gameSchedule, ganeScore;
    Text[] dataArr;
    Transform DataText, Data, MedicalEvaluation;
    GameObject Btn_Evaluation, Btn_Evaluation_1;
    string[] colname;
    string[] data;
    List<string[]> HistoryData = new List<string[]>();
    /// <summary>
    /// 获取选择方案内容
    /// </summary>
    List<string[]> Readdata = new List<string[]>();
    /// <summary>
    /// 获取测试结果数据
    /// </summary>
    List<string[]> TextList = new List<string[]>();
    int row = 0;
    string thisTime, startTime;
    /// <summary>
    /// 写入数据用
    /// </summary>
    List<string[]> AddList = new List<string[]>();

    List<string> testlist = new List<string>();/*患者基本信息*/
    List<string> datatestlist = new List<string>();/*评估具体数据*/
    List<string> ss = new List<string>();/*训练内容*/
    LineChart line_1, line_2;
    MedicalEvaluation medical;
    public static SettlePanel settlePanel;
    string[] Tempdata;
    private void Awake()
    {
        medical = transform.Find("HistoryPage/MedicalEvaluation").GetComponent<MedicalEvaluation>();
        gameDifficulty = transform.Find("HistoryPage/BG/Game/gameDifficulty").GetComponent<Text>();
        gameSchedule = transform.Find("HistoryPage/BG/Game/gameSchedule").GetComponent<Text>();
        ganeScore = transform.Find("HistoryPage/BG/Game/ganeScore").GetComponent<Text>();
        ganeName = transform.Find("HistoryPage/BG/Game/ganeName").GetComponent<Text>();
        gameTime = transform.Find("HistoryPage/BG/Game/gameTime").GetComponent<Text>();
        BtnArr = transform.GetComponentsInChildren<Button>();
        Data = transform.Find("HistoryPage/Data");
        MedicalEvaluation = transform.Find("HistoryPage/MedicalEvaluation");
        DataText = transform.Find("HistoryPage/Data/DataText");
        line_1 = transform.Find("HistoryPage/Data/Line/LineChart_1").GetComponent<LineChart>();
        line_2 = transform.Find("HistoryPage/Data/Line/LineChart_2").GetComponent<LineChart>();
        dataArr = DataText.GetComponentsInChildren<Text>();
        pdf = GameObject.Find("PdfPanel").GetComponent<PdfPanel>();
        settlePanel = this;
    }
    private void OnEnable()
    {
        if (UserPanel.isCheckModel)
        {
            Data.Find("BG/Text_9").gameObject.SetActive(false);
            Data.Find("DataText/Data_9").gameObject.SetActive(false);
        }
        else
        {
            Data.Find("BG/Text_9").gameObject.SetActive(true);
            Data.Find("DataText/Data_9").gameObject.SetActive(true);
        }
    }
    void Start()
    {
        for (int i = 0; i < BtnArr.Length; i++)
        {
            Button btn = BtnArr[i];
            btn.onClick.AddListener(delegate () { this.OnButtonClick(btn); });
            if (btn.name == "Btn_Evaluation")
            {
                Btn_Evaluation = btn.gameObject;
            }
            if (btn.name == "Btn_Evaluation_1")
            {
                Btn_Evaluation_1 = btn.gameObject;
            }
        }

        MessageCenter.AddMsgListener("SettlePanel", OpenSettlePanel);
        this.gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        if (!string.IsNullOrEmpty(UserInfoData.MaxAugle))
        {
            InitLineChart(line_1, "角速度稳定性");
            InitLineChart(line_2, "加速度稳定性");
            dataArr[0].text = "0.00";
            dataArr[1].text = "0.00";
            dataArr[2].text = "0.00";
            dataArr[3].text = "0.00";
            dataArr[4].text = "0.00";
            dataArr[5].text = "0.00";
            dataArr[6].text = "0.00";
            dataArr[7].text = "0.00";
            dataArr[8].text = "0.00";
        }
    }

    private void OnButtonClick(Button btn)
    {
        switch (btn.name)
        {
            case "Btn_Quit":
                MessageCenter.SendMsg("MainPanel", true);
                MessageCenter.SendMsg("TrainPanel", false);
                MessageCenter.SendMsg("UserPanel", false);
                this.gameObject.SetActive(false);
                break;

            case "Btn_DeBug":
                ScreenShot.instantiate.OnStartScreenShot();
                ScreenShot.instantiate.ScreenShot_ReadPixelsWithCamera();
                Invoke("WritePdf", 0.5f);
                break;
            case "Btn_History":
                MessageCenter.SendMsg("HistoryPanel", true);
                break;
            case "Btn_Add":
                MessageCenter.SendMsg("AddPanel", true);
                break;
            case "Btn_Evaluation":
                Btn_Evaluation.SetActive(false);
                Btn_Evaluation_1.SetActive(true);
                Data.gameObject.SetActive(false);
                MedicalEvaluation.gameObject.SetActive(true);
                break;
            case "Btn_Evaluation_1":
                Btn_Evaluation.SetActive(true);
                Btn_Evaluation_1.SetActive(false);
                Data.gameObject.SetActive(true);
                MedicalEvaluation.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    void WritePdf()
    {
        WritePDF.WritesPDF(testlist, ss, datatestlist);
    }

    float T;
    int Coefficient;
    /// <summary>
    /// 初始化结算数据界面
    /// </summary>
    private void InitDataText()
    {

        double[] TempVmax = new double[UserInfoData.settleData.V_Max.Length];
        double[] TempAccmax = new double[UserInfoData.settleData.ACC_Max.Length];
        if (UserInfoData.settleData.Total_Data_Max < 5)
        {
            UserInfoData.settleData.Total_Data_Max = 0;
            UserInfoData.settleData.Total_Data_Min = 0;
            UserInfoData.settleData.Total_V_Main = 0;
            UserInfoData.settleData.Total_V_Max = 0;
            UserInfoData.settleData.Total_ACC_Main = 0;
            UserInfoData.settleData.Total_Acc_Max = 0;
            UserInfoData.settleData.Muscle_State = 0;
            UserInfoData.settleData.Endurance_time = 0;
            UserInfoData.settleData.Endurance_Value = 0;

            for (int i = 0; i < TempVmax.Length; i++)
            {
                TempVmax[i] = 0;
            }
            for (int i = 0; i < TempAccmax.Length; i++)
            {
                TempAccmax[i] = 0;
            }
            UserInfoData.settleData.V_Max = TempVmax;
            UserInfoData.settleData.ACC_Max = TempAccmax;
        }
        else
        {
            for (int i = 0; i < TempVmax.Length; i++)
            {
                UserInfoData.settleData.V_Max[i] = Mathf.Abs((float)UserInfoData.settleData.V_Max[i]);
                TempVmax[i] = Math.Round(UserInfoData.settleData.V_Max[i] * 100, 2);
            }
            for (int i = 0; i < TempAccmax.Length; i++)
            {
                UserInfoData.settleData.ACC_Max[i] = Mathf.Abs((float)UserInfoData.settleData.ACC_Max[i]);
                TempAccmax[i] = Math.Round(UserInfoData.settleData.ACC_Max[i] * 1000, 2);
            }
        }
        UserInfoData.settleData.V_Max = TempVmax;
        UserInfoData.settleData.ACC_Max = TempAccmax;
            dataArr[0].text = Mathf.Abs((float)UserInfoData.settleData.Total_Data_Max).ToString("f2");
            dataArr[1].text = Mathf.Abs((float)UserInfoData.settleData.Total_Data_Min).ToString("f2");
            dataArr[2].text = Mathf.Abs((float)UserInfoData.settleData.Total_V_Main * 1000).ToString("f2");
            dataArr[3].text = Mathf.Abs((float)UserInfoData.settleData.Total_V_Max * 100).ToString("f2");
            dataArr[4].text = Mathf.Abs((float)UserInfoData.settleData.Total_ACC_Main * 1000).ToString("f2");
            dataArr[5].text = Mathf.Abs((float)UserInfoData.settleData.Total_Acc_Max * 1000).ToString("f2");
            dataArr[6].text = Mathf.Abs((float)UserInfoData.settleData.Muscle_State * 1000 * LoadProfileConfig.loadData.GetStrengthCoefficientData(Coefficient)).ToString("f2");
            //dataArr[7].text = Mathf.Abs((float)UserInfoData.settleData.Endurance_time / 1000).ToString("f2");
            //dataArr[8].text = Mathf.Abs((float)UserInfoData.settleData.Endurance_Value * 1000 ).ToString("f2");

        if (T <= LoadProfileConfig.loadData.TSet)
        {
            dataArr[7].text = "训练时间太短，无法计算";
            dataArr[8].text = "训练时间太短，无法计算";
        }
        else
        {
            dataArr[7].text = Mathf.Abs((float)UserInfoData.settleData.Endurance_time / 1000).ToString("f2");
            dataArr[8].text = Mathf.Abs((float)UserInfoData.settleData.Endurance_Value * 1000 * LoadProfileConfig.loadData.GetEnduranceCoefficientData(Coefficient)).ToString("f2");
        }

        int temp = int.Parse(UserInfoData.MaxAugle) / 4;
            int[] tempArr = new int[5];
            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i] = i * temp;
            }
            double[] Y_Vk = UserInfoData.settleData.V_Max;
            double[] Y_Rk = UserInfoData.settleData.ACC_Max;
            InitLineChart(line_1, "角速度稳定性", Y_Vk, tempArr);
            InitLineChart(line_2, "加速度稳定性", Y_Rk, tempArr);
            string[] _temp = UserInfoData.GetUserInfo();
            if (_temp != null)
            {
                testlist.Clear();
                testlist.Add(_temp[1]);
                testlist.Add(_temp[0]);
                testlist.Add(_temp[3]);
                testlist.Add("正常");
                testlist.Add("康复");
                DateTime dateTime = DateTime.Now;
            string time = dateTime.ToString("yyyy.MM.dd.HH.mm");
            testlist.Add(time);
            }
        
    }

    /// <summary>
    /// 游戏数据显示
    /// </summary>
    void InitMessagePage()
    {
        if (Readdata != null && Readdata.Count > 0)
        {
            ss.Clear();
            string[] _data = Readdata[Readdata.Count - 1];
            ganeName.text = "推胸拉背测试";
            gameDifficulty.text = "3";
            UserInfoData.CompletionDegree = ((PlanPlayer.gamegrade / ((PlanMangenter.goldsum) * 0.5f)) * 100);
            gameSchedule.text =( PlanPlayer.gamegrade*2).ToString();
            gameTime.text = _data[1] + "分钟";
            T = float.Parse(_data[1]);
            Coefficient = int.Parse(UserInfoData.TrainingWeight) - 1;
            ganeScore.text = PlanPlayer.gamegrade + "分";
            startTime = UserInfoData.StartTime;
            ss.Add(ganeName.text);
            ss.Add(gameTime.text);
            ss.Add(gameDifficulty.text);
            ss.Add(gameSchedule.text);
            ss.Add(ganeScore.text);
        }
    }
    //显示数据图表
    void InitLineChart(LineChart line, string tableName, double[] YtabelData = null, int[] XtabelData = null)
    {
        line.SetSize(360, 300);//代码动态添加图表需要设置尺寸
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
        if (tableName.Equals("角速度稳定性"))
        {
            line.yAxes[0].axisName.name = "v（°/s）";
        }
        else
        {
            line.yAxes[0].axisName.name = "a（°/s²）";
        }
        line.xAxes[0].axisName.name = "θ";
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
        float augle = float.Parse(UserInfoData.MaxAugle);
        float value = augle / 5f;
        //添加数据
        if (YtabelData != null && XtabelData != null)
        {
            int index = 0;
            if (YtabelData.Length > XtabelData.Length)
                index = XtabelData.Length;
            else
                index = YtabelData.Length;
            for (int i = 0; i < index; i++)
            {
                line.AddXAxisData(XtabelData[i] + "°");
                line.AddData(0, YtabelData[i]);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                float temp = value * (i + 1);
                line.AddXAxisData(temp + "°");
                line.AddData(0, 0);
            }
        }
    }


    void OpenSettlePanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        Tempdata = UserInfoData.GetUserInfo();
        if (Tempdata == null) return;
        string table = "A" + Tempdata[1] + "_TraingPlan";
        OperateUseSQL_H.Read_Data(table, out Readdata, out row);
        if (value)
        {
            UserInfoData.class_ag.Out_Data(out UserInfoData.settleData.Total_Data_Max, out UserInfoData.settleData.Total_Data_Min, out UserInfoData.settleData.Total_V_Main,
                out UserInfoData.settleData.Total_V_Max, out UserInfoData.settleData.V_Main, out UserInfoData.settleData.V_Max, out UserInfoData.settleData.V_Min,
                out UserInfoData.settleData.Total_ACC_Main, out UserInfoData.settleData.Total_Acc_Max, out UserInfoData.settleData.ACC_Main, out UserInfoData.settleData.ACC_Max,
                out UserInfoData.settleData.ACC_Min, out UserInfoData.settleData.n_Feedback, out UserInfoData.settleData.Muscle_State,
                out UserInfoData.settleData.Endurance_time, out UserInfoData.settleData.Endurance_Value);
            this.gameObject.SetActive(value);
            Invoke("InitSettle", 1f);
        }
    }

    PdfPanel pdf;
    void InitSettle()
    {
        InitMessagePage();
        InitDataText();
        AddTraingListData();
        pdf.InitThisPanel();
        Btn_Evaluation.SetActive(true);
        Btn_Evaluation_1.SetActive(false);
        Data.gameObject.SetActive(true);
        MedicalEvaluation.gameObject.SetActive(false);
        UserPanel.isCheckModel = false;
    }

    /// <summary>
    /// 上传数据至历史记录表
    /// </summary>
    private void AddTraingListData()
    {
        if (Tempdata == null) return;
        AddList.Clear();
        data = new string[22];
        colname = new string[] {"ID", "StartTime", "EndTime", "ProgrammeName","GameTime","GameDifficulty","Score","MaxAugle","TrainingWeight","最大运动角度", "最小运动角度",
            "平均运动角速度","最大运动角加速度","平均运动角加速度", "肌力","肌力稳定性", "持续时间", "耐力","XList","VKYList","RKYList","完成度" };
        DateTime dateTime = DateTime.Now;
        thisTime = dateTime.ToString("yyyy-MM-dd HH:mm");
        data[0] = Tempdata[1];
        data[1] = UserInfoData.StartTime;
        data[2] = thisTime;
        data[3] = ganeName.text;
        data[4] = gameTime.text;
        data[5] = gameDifficulty.text;
        data[6] = PlanPlayer.gamegrade.ToString();
        data[7] = UserInfoData.MaxAugle;
        data[8] = UserInfoData.TrainingWeight;
        data[21] = gameSchedule.text;

        //data[9] = Mathf.Abs((float)UserInfoData.settleData.Total_Data_Max).ToString("f2");
        //data[10] = Mathf.Abs((float)UserInfoData.settleData.Total_Data_Min).ToString("f2");
        //data[11] = Mathf.Abs((float)UserInfoData.settleData.Total_V_Main * 1000).ToString("f2");
        //data[12] = Mathf.Abs((float)UserInfoData.settleData.Total_V_Max * 100).ToString("f2");
        //data[13] = Mathf.Abs((float)UserInfoData.settleData.Total_ACC_Main * 1000).ToString("f2");
        //data[14] = Mathf.Abs((float)UserInfoData.settleData.Total_Acc_Max * 10000).ToString("f2");
        //data[15] = GetStrNum(UserInfoData.settleData.Muscle_State.ToString());
        //data[16] = Mathf.Abs((float)UserInfoData.settleData.Endurance_time / 1000).ToString("f2");
        //data[17] = GetStrNum(UserInfoData.settleData.Endurance_Value.ToString());

        for (int i = 0; i < dataArr.Length; i++)
        {
            data[9 + i] = dataArr[i].text;
        }

        float temp1 = float.Parse(data[9]) - float.Parse(data[10]);
        medical.InitMedicalEvaluationPage(temp1, data[14], data[17]);
        //float temp1 = float.Parse(data[9]) - float.Parse(data[10]);
        //float temp2 = float.Parse(data[14]);
        //float temp3 = float.Parse(data[17]);
        //medical.InitMedicalEvaluationPage(temp1, temp2, temp3);
        int temp = int.Parse(UserInfoData.MaxAugle) / 4;
        int[] tempArr = new int[5];
        for (int i = 0; i < tempArr.Length; i++)
        {
            tempArr[i] = i * temp;
        }
        datatestlist.Clear();
        for (int i = 0; i < dataArr.Length; i++)
        {
            datatestlist.Add(dataArr[i].text);
        }
        data[18] = string.Join("_", tempArr);
        data[19] = string.Join("_", UserInfoData.settleData.V_Max);
        data[20] = string.Join("_", UserInfoData.settleData.ACC_Max);
        data[21] = gameSchedule.text;
        AddList.Add(data);
        string tableName = "A" + Tempdata[1] + "_TraingList";
        OperateUseSQL_H.Add_Data(tableName, colname, AddList);
        if (UserInfoData.UserId == "007")
        {
            return;
        }
        ReadHistoryDataToPost();
    }

    string GetStrNum(string temp)
    {
        string[] tempArr = temp.Split('.');
        string str = "00";
        float num = 0;
        if (tempArr.Length > 1)
            str = tempArr[1].TrimStart('0');
        try
        {
            num = float.Parse(tempArr[0] + "." + str);
        }
        catch (Exception)
        {

            throw;
        }
        num = Mathf.Abs(num);
        if (num < 9)
        {
            num *= 10;
        }
        return num.ToString("f2");
    }

    /// <summary>
    /// 读取历史记录数据传到服务器
    /// </summary>
    void ReadHistoryDataToPost()
    {
        #region 第一版废弃
        //PostData post = new PostData();
        //post.phone = Tempdata[4];
        //post.uId = data[0];
        //post.date = data[1];
        //post.gameName = data[3];
        //post.equipment = UserInfoData.EquipmentName;
        //post.maxMontionAngle = data[9];
        //post.minMontionAngle = data[10];
        //post.aveMontionAngle = data[11];
        //post.arthrosis = (float.Parse(post.maxMontionAngle) - float.Parse(post.minMontionAngle)).ToString();
        //post.maxAccSpeed = data[12];
        //post.aveAccSpeed = data[13];
        //post.muscleStrength = data[14];
        //post.muscleStability = data[15];
        //post.endurance = data[17];
        //post.drillNum = "15";
        //post.difficultyLevel = data[5];
        //post.taskPercentage = data[21];

        //post.score = data[6];
        //post.equipmentDuration = data[4];
        //post.angularX = data[18];
        //post.angularY = data[19];
        //post.accSpeedX = data[18];
        //post.accSpeedY = data[20];
        //post.endurance_level = UserInfoData.endurance_level;
        //post.muscle_level = UserInfoData.muscle_level;
        //post.arthrosis_level = UserInfoData.arthrosis_level;
        //string dataJson = ObjectToJson(post);

        //PostCheckData data1 = new PostCheckData();
        //data1.checkTime = UserInfoData.checkTime;
        //data1.checkDuration = data[4];
        //data1.deviceNum = UserInfoData.EquipmentName;
        //data1.myodynamia = data[14];
        //data1.endurance = data[17];
        //data1.jointMotion = (float.Parse(post.maxMontionAngle) - float.Parse(post.minMontionAngle)).ToString();
        //data1.phone = Tempdata[4];
        //data1.checkGear = TestSettle.GameDifficulty;
        //Debug.Log(UserInfoData.EquipmentName + "UserInfoData.EquipmentName");

        //if (!UserPanel.isCheckModel)
        //{
        //    try
        //    {
        //        Debug.Log(DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/comprehensive/inComprehensiveController", post));
        //        // Debug.Log(Post("http://" + tempIp + "/comprehensive/inAssessmentTraining", dataJson));
        //        //TipsPage_1.tipsPage.OpenThisPage("数据传输成功！");
        //    }
        //    catch (Exception)
        //    {
        //        OperateUseSQL_H.Add_Data("DelayedUpload", new string[] { "phone", "data", "trainTime", "state", }, new List<string[]> { new string[] { post.phone, dataJson, post.date, "false" } });

        //        Debug.LogError("连接失败！！！！！！");
        //        //TipsPage_1.tipsPage.OpenThisPage("数据传输失败！");
        //    }
        //}
        //else
        //{

        //    try
        //    {
        //        // Debug.Log(Post("http://" + tempIp + "/comprehensive/inComprehensiveController", dataJson));
        //        Debug.Log(DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/comprehensive/inAssessmentTraining", data1));
        //        //TipsPage_1.tipsPage.OpenThisPage("数据传输成功！");
        //    }
        //    catch (Exception)
        //    {
        //        Debug.LogError("连接失败！！！！！！");
        //        //TipsPage_1.tipsPage.OpenThisPage("数据传输失败！");
        //    }
        //} 
        #endregion
        HistoryData.Reverse();
        PostTrainData trainData = new PostTrainData();
        trainData.userId =int.Parse(data[0]);
        trainData.phone = Tempdata[4];
        trainData.trainingDevice = UserInfoData.EquipmentName;
        trainData.muscleStrength =double.Parse(data[14]);
        double result=0;
        double.TryParse(data[17],out result);
        trainData.endurance = result;
        trainData.jointRangeOfMotion = (float.Parse(data[9]) - float.Parse(data[10]));
        trainData.comprehensiveScore = double.Parse(data[6]);
        trainData.grade = 0;
       // trainData.startDate = data[1];
        //trainData.endDate = data[2];
        trainData.load1 = "3";
        trainData.taskPercentage = data[21];
        if (UserPanel.isCheckModel)
        {
            trainData.type =0;
            trainData.movementTime ="1";
        }
        else
        {
            trainData.type = 1;
            trainData.movementTime = "20";
        }
        trainData.difficultyLevel = "无";
        trainData.level = "无";
        trainData.enduranceLevel = UserInfoData.endurance_level; 
        trainData.arthrosisLevel= UserInfoData.arthrosis_level;
        trainData.muscleLevel= UserInfoData.muscle_level;
        trainData.angularX= data[18];
        trainData.angularY= data[19];
        trainData.accSpeedX=data[18];
        trainData.accSpeedY=data[20];


        string dataJson = ObjectToJson(trainData);
        Debug.Log("dataJson:----------" + dataJson);
        Debug.Log(Post("http://" + UserInfoData.Ip + "/operate/addEquipmentTrainingData", dataJson));
        try
        {
           
           
        }
        catch (Exception)
        {
          //  OperateUseSQL_H.Add_Data("DelayedUpload", new string[] { "phone", "data", "trainTime", "state", }, new List<string[]> { new string[] { trainData.phone, dataJson, trainData.startDate, "false" } });

            Debug.LogError("连接失败！！！！！！");
           
        }
    }

    public string ObjectToJson(object obj)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        MemoryStream stream = new MemoryStream();
        serializer.WriteObject(stream, obj);
        byte[] dataBytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(dataBytes, 0, (int)stream.Length);
        return Encoding.UTF8.GetString(dataBytes);
    }

    //HTTP请求
    public string Post(string url, string content)
    {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.ContentType = "application/json";

        #region 添加Post 参数
        byte[] data = Encoding.UTF8.GetBytes(content);
        req.ContentLength = data.Length;
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }
        #endregion

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取响应内容
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }
}
