using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class HistoryPanel : MonoBehaviour
{
    Button [] BtnArr;
    Toggle[] TogArr;
    Text[] dataArr;
    LineChart line_1, line_2;
    BarChart line_3;
    Text ganeName, gameTime, gameDifficulty, gameSchedule, ganeScore;
    Transform User;
    GameObject Btn_Evaluation, Btn_Evaluation_1;
    List<string[]> Readdata = new List<string[]>();
    List<string[]> TextList = new List<string[]>();
    GameObject Data, MedicalEvaluation, _Page, _Line;
    HistoryMedical medical;
    int row = 0;
    int index_Name;

    private void Awake()
    {
        BtnArr = transform.GetComponentsInChildren<Button>();
        medical = transform.Find("HistoryPage/MedicalEvaluation").GetComponent<HistoryMedical>();
        gameDifficulty = transform.Find("HistoryPage/BG/Game/gameDifficulty").GetComponent<Text>();
        gameSchedule = transform.Find("HistoryPage/BG/Game/gameSchedule").GetComponent<Text>();
        ganeScore = transform.Find("HistoryPage/BG/Game/ganeScore").GetComponent<Text>();
        ganeName = transform.Find("HistoryPage/BG/Game/ganeName").GetComponent<Text>();
        gameTime = transform.Find("HistoryPage/BG/Game/gameTime").GetComponent<Text>();
        line_1 = transform.Find("HistoryPage/Data/Line/LineChart_1").GetComponent<LineChart>();
        line_2 = transform.Find("HistoryPage/Data/Line/LineChart_2").GetComponent<LineChart>();
        line_3 = transform.Find("HistoryPage/Data/Line/BarChart").GetComponent<BarChart>();
        MedicalEvaluation = transform.Find("HistoryPage/MedicalEvaluation").gameObject;
        Data = transform.Find("HistoryPage/Data").gameObject;
        _Line = transform.Find("HistoryPage/Data/Line").gameObject;
        _Page= transform.Find("HistoryPage/Data/Page").gameObject;
        dataArr = _Page.transform.Find("DataTexts").GetComponentsInChildren<Text>();
        User = transform.Find("UserPage/Scroll View/Viewport/User");
    }

    void Start()
    {
        for (int i = 0; i < BtnArr.Length; i++)
        {
            Button btn = BtnArr[i];
            btn.onClick.AddListener(delegate () { this.OnButtonclick(btn); });
            if (btn.name == "Btn_Evaluation")
            {
                Btn_Evaluation = btn.gameObject;
            }
            if (btn.name == "Btn_Evaluation_1")
            {
                Btn_Evaluation_1 = btn.gameObject;
            }
        }
        MessageCenter.AddMsgListener("HistoryPanel", OpenHistoryPanel);
        //MessageCenter.AddMsgListener("UnDataHistoryToggleArrs", UnDataToggleArrs);
        this.gameObject.SetActive(false);
    }


    private void OnButtonclick(Button btn)
    {
        switch (btn.name)
        {
            case "Btn_Up_1":
                _Line.SetActive(true);
                _Page.SetActive(false);
                break;
            case "Btn_Down_1":
                _Line.SetActive(false);
                _Page.SetActive(true);
                break;
            case "Btn_Back":
                //MessageCenter.SendMsg("UserPanel", true);
                this.gameObject.SetActive(false);
                break;
            case "Btn_Evaluation":
                Btn_Evaluation.SetActive(false);
                Btn_Evaluation_1.SetActive(true);
                Data.SetActive(false);
                MedicalEvaluation.SetActive(true);
                break;
            case "Btn_Evaluation_1":
                Btn_Evaluation.SetActive(true);
                Btn_Evaluation_1.SetActive(false);
                Data.SetActive(true);
                MedicalEvaluation.SetActive(false);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Toggle数组更新后重新添加事件
    /// </summary>
    void InitToggleArr()
    {
        TogArr = User.GetComponentsInChildren<Toggle>();
        if (TogArr.Length == 0) return;
        for (int i = 0; i < TogArr.Length; i++)
        {
            Toggle tog = TogArr[i];
            tog.onValueChanged.AddListener((bool isOn) => { OnToggleValueChange(tog, isOn); });
        }
    }

    List<string[]> tempList = new List<string[]>();

    /// <summary>
    /// 初始化游戏信息
    /// </summary>
    /// <param name="togName"></param>
    void InitMessagePage(string togName)
    {
        tempList.Clear();
        if (Readdata != null && Readdata.Count > 0)
        {
            if (Readdata.Count <= int.Parse(togName)) return;
            string[] _data = Readdata[int.Parse(togName)];
            ganeName.text = _data[3];
            gameTime.text = _data[4];
            gameDifficulty.text = _data[5];
            gameSchedule.text = _data[21];
            ganeScore.text = _data[6] + "分";
            string[] Y_Vk = _data[19].Split('_');
            string[] X_k = _data[18].Split('_');
            string[] Y_Rk = _data[20].Split('_');
            InitLineChart(line_1, _data[7], "角速度稳定性", Y_Vk, X_k);
            InitLineChart(line_2, _data[7], "加速度稳定性", Y_Rk, X_k);
            for (int i = index_Name; i < Readdata.Count; i++)
            {
                tempList.Add(Readdata[i]);
            }
            string[] tempArr = new string[9];
            string[] tempStr1 = tempList[tempList.Count - 1];
            string[] tempStr2 = tempList[tempList.Count / 2];
            string[] tempStr3 = tempList[0];
            if (tempList.Count == 1)
            {
                tempArr[0] = tempStr1[14];
                if (tempArr[0].Equals("训练时间太短，无法计算"))
                    tempArr[0] = "0";
                tempArr[1] = tempStr1[17];
                if (tempArr[1].Equals("训练时间太短，无法计算"))
                    tempArr[1] = "0";
                tempArr[2] = (float.Parse(tempStr1[9]) - float.Parse(tempStr1[10])).ToString();
                tempArr[3] = "0";
                tempArr[4] = "0";
                tempArr[5] = "0";
                tempArr[6] = tempStr3[14];
                if (tempArr[6].Equals("训练时间太短，无法计算"))
                    tempArr[6] = "0";
                tempArr[7] = tempStr3[17];
                if (tempArr[7].Equals("训练时间太短，无法计算"))
                    tempArr[7] = "0";
                tempArr[8] = (float.Parse(tempStr3[9]) - float.Parse(tempStr3[10])).ToString();
            }
            else if (tempList.Count == 2)
            {
                tempArr[0] = tempStr1[14];
                if (tempArr[0].Equals("训练时间太短，无法计算"))
                    tempArr[0] = "0";
                tempArr[1] = tempStr1[17];
                if (tempArr[1].Equals("训练时间太短，无法计算"))
                    tempArr[1] = "0";
                tempArr[2] = (float.Parse(tempStr1[9]) - float.Parse(tempStr1[10])).ToString();
                tempArr[3] = "0";
                tempArr[4] = "0";
                tempArr[5] = "0";
                tempArr[6] = tempStr3[14];
                if (tempArr[6].Equals("训练时间太短，无法计算"))
                    tempArr[6] = "0";
                tempArr[7] = tempStr3[17];
                if (tempArr[7].Equals("训练时间太短，无法计算"))
                    tempArr[7] = "0";
                tempArr[8] = (float.Parse(tempStr3[9]) - float.Parse(tempStr3[10])).ToString();
            }
            else
            {
                tempArr[0] = tempStr1[14];
                if (tempArr[0].Equals("训练时间太短，无法计算"))
                    tempArr[0] = "0";
                tempArr[1] = tempStr1[17];
                if (tempArr[1].Equals("训练时间太短，无法计算"))
                    tempArr[1] = "0";
                tempArr[2] = (float.Parse(tempStr1[9]) - float.Parse(tempStr1[10])).ToString();
                tempArr[3] = tempStr2[14];
                if (tempArr[3].Equals("训练时间太短，无法计算"))
                    tempArr[3] = "0";
                tempArr[4] = tempStr2[17];
                if (tempArr[4].Equals("训练时间太短，无法计算"))
                    tempArr[4] = "0";
                tempArr[5] = (float.Parse(tempStr2[9]) - float.Parse(tempStr2[10])).ToString();
                tempArr[6] = tempStr3[14];
                if (tempArr[6].Equals("训练时间太短，无法计算"))
                    tempArr[6] = "0";
                tempArr[7] = tempStr3[17];
                if (tempArr[7].Equals("训练时间太短，无法计算"))
                    tempArr[7] = "0";
                tempArr[8] = (float.Parse(tempStr3[9]) - float.Parse(tempStr3[10])).ToString();
            }
            InitBarChart(line_3, "训练记录对比", tempArr);
            medical.InitMedicalEvaluationPage(tempArr);
            _Line.SetActive(true);
            InitDataText(_data);
        }
    }

    private void InitDataText(string[] tempData)
    {
        for (int i = 0; i < dataArr.Length; i++)
        {
            dataArr[i].text = tempData[i+9].ToString();
        }
        _Page.SetActive(false);
    }

    /// <summary>
    /// 初始化折线图
    /// </summary>
    void InitLineChart(LineChart line, string MaxAugle, string tableName, string[] YtabelData = null, string[] XtabelData = null)
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
        line.xAxes[0].axisName.name = "θ（°）";
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
        float augle = float.Parse(MaxAugle);
        float value = augle / 5f;

        //添加数据
        if (YtabelData != null && XtabelData != null)
        {
            for (int i = 0; i < YtabelData.Length; i++)
            {
                if (!string.IsNullOrEmpty(XtabelData[i]))
                    line.AddXAxisData(XtabelData[i] + "°");
                if (!string.IsNullOrEmpty(YtabelData[i]))
                    line.AddData(0, float.Parse(YtabelData[i]));
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                float temp = value * (i + 1);
                line.AddXAxisData(temp + "°");
                line.AddData(0, Random.Range(0, 6));
            }
        }
    }

    void InitBarChart(BarChart bar, string tableName, string[] tabelData1 = null)
    {
        bar.SetSize(360, 300);//代码动态添加图表需要设置尺寸
        //设置标题：
        bar.title.show = true;
        bar.title.text = tableName;

        //设置提示框和图例是否显示
        bar.tooltip.show = true;
        bar.legend.show = false;

        //设置是否使用双坐标轴和坐标轴类型
        bar.xAxes[0].show = true;
        bar.xAxes[1].show = false;
        bar.yAxes[0].show = true;
        bar.yAxes[1].show = true;
        bar.xAxes[0].type = Axis.AxisType.Category;
        bar.yAxes[0].type = Axis.AxisType.Value;
        bar.yAxes[1].type = Axis.AxisType.Value;
        bar.yAxes[0].axisName.name = "°";
        bar.yAxes[1].axisName.name = "N";
        bar.xAxes[0].axisName.name = "次数";
        bar.yAxes[1].axisLine.lineStyle.color = Color.white;
        bar.yAxes[1].axisLabel.textStyle.color = Color.white;
        //设置坐标轴分割线
        bar.xAxes[0].splitNumber = 10;
        bar.xAxes[0].boundaryGap = true;

        //清空数据，添加`Line`类型的`Serie`用于接收数据
        bar.RemoveData();
        bar.AddSerie(SerieType.Bar);
        bar.AddSerie(SerieType.Bar);
        bar.AddSerie(SerieType.Bar);
        if (tabelData1 != null)
        {
            bar.series.AddData(0, float.Parse(tabelData1[0]));
            bar.series.AddData(1, float.Parse(tabelData1[1]));
            bar.series.AddData(2, float.Parse(tabelData1[2]));
            bar.AddXAxisData("第一次");
            bar.series.AddData(0, float.Parse(tabelData1[3]));
            bar.series.AddData(1, float.Parse(tabelData1[4]));
            bar.series.AddData(2, float.Parse(tabelData1[5]));
            bar.AddXAxisData("中间次");
            bar.series.AddData(0, float.Parse(tabelData1[6]));
            bar.series.AddData(1, float.Parse(tabelData1[7]));
            bar.series.AddData(2, float.Parse(tabelData1[8]));
            bar.AddXAxisData("当前次");
            bar.series.GetSerie(0).barWidth = 0.2f;
            bar.series.GetSerie(0).name = "肌力";
            bar.series.GetSerie(0).itemStyle.show = true;
            bar.series.GetSerie(1).barWidth = 0.2f;
            bar.series.GetSerie(1).name = "肌耐力";
            bar.series.GetSerie(1).itemStyle.show = true;
            bar.series.GetSerie(2).barWidth = 0.2f;
            bar.series.GetSerie(2).name = "关节活动度";
            bar.series.GetSerie(2).itemStyle.show = true;
            bar.series.GetSerie(1).yAxisIndex = 1;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                float temp = (i + 1);
                bar.AddXAxisData("第" + temp + "次");
                bar.series.AddData(0, Random.Range(1, 6));
                bar.series.AddData(1, Random.Range(1, 6));
                bar.series.GetSerie(0).barWidth = 0.3f;
                bar.series.GetSerie(1).barWidth = 0.3f;
            }
        }
    }


    private void OnToggleValueChange(Toggle tog, bool isOn)
    {
        index_Name = int.Parse(tog.transform.parent.name);
        InitMessagePage(tog.transform.parent.name);
    }

    /// <summary>
    /// 打开本界面
    /// </summary>
    /// <param name="pa"></param>
    void OpenHistoryPanel(ParameterData pa)
    {
        bool value = (bool)pa.data;
        this.gameObject.SetActive(value);
        string[] data = UserInfoData.GetUserInfo();
        _Page.SetActive(false);
        if (value)
        {
            string table = "A" + data[1] + "_TraingList";
            OperateUseSQL_H.Read_Data(table, out Readdata, out row);
            if (Readdata != null && Readdata.Count > 0)
                Readdata.Reverse();
            InitMessagePage("0");
            InitToggleArr();
            Btn_Evaluation.SetActive(true);
            Btn_Evaluation_1.SetActive(false);
            Data.SetActive(true);
            MedicalEvaluation.SetActive(false);
        }
    }

    void UnDataToggleArrs(ParameterData pa)
    {
        InitToggleArr();
    }

    private void OnDisable()
    {
        if (TogArr!=null)
        {
            for (int i = 0; i < TogArr.Length; i++)
            {
                Toggle tog = TogArr[i];
                tog.onValueChanged.RemoveAllListeners();
            }
        }
    }

}
