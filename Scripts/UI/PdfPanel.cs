using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class PdfPanel : MonoBehaviour
{
    Text[] DataArr;
    BarChart barChart;
    float[] AngleData, strengthData, enduranceData;
    List<string[]> Readdata = new List<string[]>();
    int row;

    void Awake()
    {
        DataArr = transform.Find("Data").GetComponentsInChildren<Text>();
        barChart = transform.Find("BarChart").GetComponent<BarChart>();
    }

    public void InitThisPanel()
    {
        string table = "A" + UserInfoData.UserId + "_TraingList";
        OperateUseSQL_H.Read_Data(table, out Readdata, out row);
        Readdata.Reverse();
        if (Readdata != null && Readdata.Count > 0)
        {
            string[] tempArr = new string[9];
            string[] tempStr1 = Readdata[Readdata.Count - 1];
            string[] tempStr2 = Readdata[Readdata.Count / 2];
            string[] tempStr3 = Readdata[0];
            if (Readdata.Count == 1)
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
            else if (Readdata.Count == 2)
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
            InitMedicalEvaluationPage(tempArr);
        }
    }

    public void InitMedicalEvaluationPage(string[] tabelData1)
    {
        InitEvaluationData();
        GetMedicalEvaluation(tabelData1);
        barChart.SetSize(360, 300);//代码动态添加图表需要设置尺寸
        //设置标题：
        barChart.title.show = true;
        barChart.title.text = "康复数据表";

        //设置提示框和图例是否显示
        barChart.tooltip.show = true;
        barChart.legend.show = false;

        //设置是否使用双坐标轴和坐标轴类型
        barChart.xAxes[0].show = true;
        barChart.xAxes[1].show = false;
        barChart.yAxes[0].show = true;
        barChart.yAxes[1].show = true;
        barChart.xAxes[0].type = Axis.AxisType.Category;
        barChart.yAxes[0].type = Axis.AxisType.Value;
        barChart.yAxes[1].type = Axis.AxisType.Value;
        barChart.yAxes[0].axisName.name = "°";
        barChart.yAxes[1].axisName.name = "N";
        barChart.xAxes[0].axisName.name = "次数";
        barChart.yAxes[1].axisLine.lineStyle.color = Color.white;
        barChart.yAxes[1].axisLabel.textStyle.color = Color.white;
        //设置坐标轴分割线
        barChart.xAxes[0].splitNumber = 10;
        barChart.xAxes[0].boundaryGap = true;

        //清空数据，添加`Line`类型的`Serie`用于接收数据
        barChart.RemoveData();
        barChart.AddSerie(SerieType.Bar);
        barChart.AddSerie(SerieType.Bar);
        barChart.AddSerie(SerieType.Bar);
        if (tabelData1 != null)
        {
            barChart.series.AddData(0, float.Parse(tabelData1[0]));
            barChart.series.AddData(1, float.Parse(tabelData1[1]));
            barChart.series.AddData(2, float.Parse(tabelData1[2]));
            barChart.AddXAxisData("第一次");
            barChart.series.AddData(0, float.Parse(tabelData1[3]));
            barChart.series.AddData(1, float.Parse(tabelData1[4]));
            barChart.series.AddData(2, float.Parse(tabelData1[5]));
            barChart.AddXAxisData("中间次");
            barChart.series.AddData(0, float.Parse(tabelData1[6]));
            barChart.series.AddData(1, float.Parse(tabelData1[7]));
            barChart.series.AddData(2, float.Parse(tabelData1[8]));
            barChart.AddXAxisData("当前次");
            barChart.series.GetSerie(0).barWidth = 0.2f;
            barChart.series.GetSerie(0).name = "肌力";
            barChart.series.GetSerie(0).itemStyle.show = true;
            barChart.series.GetSerie(1).barWidth = 0.2f;
            barChart.series.GetSerie(1).name = "肌耐力";
            barChart.series.GetSerie(1).itemStyle.show = true;
            barChart.series.GetSerie(2).barWidth = 0.2f;
            barChart.series.GetSerie(2).name = "关节活动度";
            barChart.series.GetSerie(2).itemStyle.show = true;
            barChart.series.GetSerie(1).yAxisIndex = 1;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                float temp = (i + 1);
                barChart.AddXAxisData("第" + temp + "次");
                barChart.series.AddData(0, Random.Range(1, 6));
                barChart.series.AddData(1, Random.Range(1, 6));
                barChart.series.GetSerie(0).barWidth = 0.3f;
                barChart.series.GetSerie(1).barWidth = 0.3f;
            }
        }
    }

    void InitEvaluationData()
    {
        string[] tempArr1 = HistoryMedical.AngleDataText.Split('_');
        string[] tempArr2 = HistoryMedical.strengthDataText.Split('_');
        string[] tempArr3 = HistoryMedical.enduranceDataText.Split('_');
        AngleData = new float[tempArr1.Length];
        strengthData = new float[tempArr2.Length];
        enduranceData = new float[tempArr3.Length];
        for (int i = 0; i < AngleData.Length; i++)
        {
            AngleData[i] = float.Parse(tempArr1[i]);
        }
        for (int i = 0; i < strengthData.Length; i++)
        {
            strengthData[i] = float.Parse(tempArr2[i]);
        }
        for (int i = 0; i < enduranceData.Length; i++)
        {
            enduranceData[i] = float.Parse(tempArr3[i]);
        }
    }

    void GetMedicalEvaluation(string[] dataArr)
    {
        for (int i = 0; i < dataArr.Length; i++)
        {
            GetObtainingRatings(i, dataArr[i]);
        }
        UserInfoData.endurance_level = dataArr[0];
        UserInfoData.muscle_level = dataArr[1];
    }

    ///
    void GetObtainingRatings(int index, string data)
    {
        string temp = "";
        switch (index % 3)
        {
            case 0:
                temp = GetEnduranceRate(float.Parse(data));
                break;
            case 1:
                temp = GetMuscleRate(float.Parse(data));
                break;
            case 2:
                temp = GetAngleRate(float.Parse(data));
                break;
            default:
                break;
        }
        DataArr[index].text = temp;
    }


    /// <summary>
    /// 肌力评级
    /// </summary>
    /// <param name="dataArr"></param>
    string GetEnduranceRate(float data)
    {
        string temp = "";
        if (data <= enduranceData[1])
        {
            temp = data + "(极差)";
        }
        else if (data <= enduranceData[2])
        {
            temp = data + "(差)";
        }
        else if (data <= enduranceData[3])
        {
            temp = data + "(及格)";
        }
        else if (data <= enduranceData[4])
        {
            temp = data + "(良好)";
        }
        else
        {
            temp = data + "(优秀)";
        }
        return temp;
    }


    /// <summary>
    /// 耐力评级
    /// </summary>
    /// <param name="dataArr"></param>
    string GetMuscleRate(float data)
    {
        string temp = "";
        if (data <= strengthData[1])
        {
            temp = data + "(极差)";
        }
        else if (data <= strengthData[2])
        {
            temp = data + "(差)";
        }
        else if (data <= strengthData[3])
        {
            temp = data + "(及格)";
        }
        else if (data <= strengthData[4])
        {
            temp = data + "(良好)";
        }
        else
        {
            temp = data + "(优秀)";
        }
        return temp;
    }


    /// <summary>
    /// 角度评级
    /// </summary>
    /// <param name="dataArr"></param>
    string GetAngleRate(float data)
    {
        string temp = "";
        if (data <= AngleData[1])
        {
            temp = data + "(极差)";
        }
        else if (data <= AngleData[2])
        {
            temp = data + "(差)";
        }
        else if (data <= AngleData[3])
        {
            temp = data + "(及格)";
        }
        else if (data <= AngleData[4])
        {
            temp = data + "(良好)";
        }
        else
        {
            temp = data + "(优秀)";
        }
        return temp;
    }
}
