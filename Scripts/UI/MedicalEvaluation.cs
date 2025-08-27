using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using XCharts;

public class MedicalEvaluation : MonoBehaviour
{
    Text Data_1, Data_2, Data_3;
    BarChart barChart;
    float[] AngleData, strengthData, enduranceData;

    void Awake()
    {
        Data_1 = transform.Find("BG/Data_1").GetComponent<Text>();
        Data_2 = transform.Find("BG/Data_2").GetComponent<Text>();
        Data_3 = transform.Find("BG/Data_3").GetComponent<Text>();
        barChart = transform.Find("BarChart").GetComponent<BarChart>();
    }
    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void InitMedicalEvaluationPage(float jointRangeOfMotion, string strength, string endurance)
    {
        InitEvaluationData();
        GetMedicalEvaluation(jointRangeOfMotion, strength, endurance);
        if (strength.Equals("训练时间太短，无法计算"))
        {
            strength = "0";
        }
        if (endurance.Equals("训练时间太短，无法计算"))
        {
            endurance = "0";
        }
        barChart.SetSize(360, 300);//代码动态添加图表需要设置尺寸
        //设置标题：
        barChart.title.show = true;
        barChart.title.text = "康复数据表";

        //设置提示框和图例是否显示
        barChart.tooltip.show = true;
        barChart.legend.show = true;

        //设置是否使用双坐标轴和坐标轴类型
        barChart.xAxes[0].show = true;
        barChart.xAxes[1].show = false;
        barChart.yAxes[0].show = true;
        barChart.yAxes[1].show = false;
        barChart.xAxes[0].type = Axis.AxisType.Category;
        barChart.yAxes[0].type = Axis.AxisType.Value;
        //设置坐标轴分割线
        barChart.xAxes[0].splitNumber = 10;
        barChart.xAxes[0].boundaryGap = true;

        //清空数据，添加`Line`类型的`Serie`用于接收数据
        barChart.RemoveData();
        barChart.AddSerie(SerieType.Bar);
        barChart.series.AddData(0, jointRangeOfMotion, "关节活动度");
        barChart.AddXAxisData("关节活动度");
        barChart.series.AddData(0, float.Parse(strength), "肌力");
        barChart.AddXAxisData("肌力");
        barChart.series.AddData(0, float.Parse(endurance), "肌耐力");
        barChart.AddXAxisData("肌耐力");
        barChart.series.GetSerie(0).itemStyle.show = true;
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

    /// <summary>
    /// 画表格
    /// </summary>
    /// <param name="jointRangeOfMotion"></param>
    /// <param name="strength"></param>
    /// <param name="endurance"></param>
    void GetMedicalEvaluation(float jointRangeOfMotion, string strength, string endurance)
    {
        Data_1.text = GetAngleRate(jointRangeOfMotion);
        UserInfoData.arthrosis_level = Data_1.text;
        Data_2.text = strength;
        Data_3.text = endurance;
        if (!strength.Equals("训练时间太短，无法计算"))
        {
            Data_2.text = GetEnduranceRate(float.Parse(strength));
        }
        if (!endurance.Equals("训练时间太短，无法计算"))
        {
            Data_3.text = GetMuscleRate(float.Parse(endurance));
        }
        DrawTableTool.I.DeleteTable();
        DrawTableTool.I.DrawTable(3, 2, 240, 80, 3, Color.white);
        DrawTableTool.I.ShowText(new string[] { "关节活动度:", Data_1.text, "肌力:", Data_2.text, "肌耐力:", Data_3.text });
        UserInfoData.endurance_level = Data_3.text;
        UserInfoData.muscle_level = Data_2.text;
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
