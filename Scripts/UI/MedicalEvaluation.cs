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
        if (strength.Equals("ѵ��ʱ��̫�̣��޷�����"))
        {
            strength = "0";
        }
        if (endurance.Equals("ѵ��ʱ��̫�̣��޷�����"))
        {
            endurance = "0";
        }
        barChart.SetSize(360, 300);//���붯̬���ͼ����Ҫ���óߴ�
        //���ñ��⣺
        barChart.title.show = true;
        barChart.title.text = "�������ݱ�";

        //������ʾ���ͼ���Ƿ���ʾ
        barChart.tooltip.show = true;
        barChart.legend.show = true;

        //�����Ƿ�ʹ��˫�����������������
        barChart.xAxes[0].show = true;
        barChart.xAxes[1].show = false;
        barChart.yAxes[0].show = true;
        barChart.yAxes[1].show = false;
        barChart.xAxes[0].type = Axis.AxisType.Category;
        barChart.yAxes[0].type = Axis.AxisType.Value;
        //����������ָ���
        barChart.xAxes[0].splitNumber = 10;
        barChart.xAxes[0].boundaryGap = true;

        //������ݣ����`Line`���͵�`Serie`���ڽ�������
        barChart.RemoveData();
        barChart.AddSerie(SerieType.Bar);
        barChart.series.AddData(0, jointRangeOfMotion, "�ؽڻ��");
        barChart.AddXAxisData("�ؽڻ��");
        barChart.series.AddData(0, float.Parse(strength), "����");
        barChart.AddXAxisData("����");
        barChart.series.AddData(0, float.Parse(endurance), "������");
        barChart.AddXAxisData("������");
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
    /// �����
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
        if (!strength.Equals("ѵ��ʱ��̫�̣��޷�����"))
        {
            Data_2.text = GetEnduranceRate(float.Parse(strength));
        }
        if (!endurance.Equals("ѵ��ʱ��̫�̣��޷�����"))
        {
            Data_3.text = GetMuscleRate(float.Parse(endurance));
        }
        DrawTableTool.I.DeleteTable();
        DrawTableTool.I.DrawTable(3, 2, 240, 80, 3, Color.white);
        DrawTableTool.I.ShowText(new string[] { "�ؽڻ��:", Data_1.text, "����:", Data_2.text, "������:", Data_3.text });
        UserInfoData.endurance_level = Data_3.text;
        UserInfoData.muscle_level = Data_2.text;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dataArr"></param>
    string GetEnduranceRate(float data)
    {
        string temp = "";
        if (data <= enduranceData[1])
        {
            temp = data + "(����)";
        }
        else if (data <= enduranceData[2])
        {
            temp = data + "(��)";
        }
        else if (data <= enduranceData[3])
        {
            temp = data + "(����)";
        }
        else if (data <= enduranceData[4])
        {
            temp = data + "(����)";
        }
        else
        {
            temp = data + "(����)";
        }
        return temp;
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dataArr"></param>
    string GetMuscleRate(float data)
    {
        string temp = "";
        if (data <= strengthData[1])
        {
            temp = data + "(����)";
        }
        else if (data <= strengthData[2])
        {
            temp = data + "(��)";
        }
        else if (data <= strengthData[3])
        {
            temp = data + "(����)";
        }
        else if (data <= strengthData[4])
        {
            temp = data + "(����)";
        }
        else
        {
            temp = data + "(����)";
        }
        return temp;
    }


    /// <summary>
    /// �Ƕ�����
    /// </summary>
    /// <param name="dataArr"></param>
    string GetAngleRate(float data)
    {
        string temp = "";
        if (data <= AngleData[1])
        {
            temp = data + "(����)";
        }
        else if (data <= AngleData[2])
        {
            temp = data + "(��)";
        }
        else if (data <= AngleData[3])
        {
            temp = data + "(����)";
        }
        else if (data <= AngleData[4])
        {
            temp = data + "(����)";
        }
        else
        {
            temp = data + "(����)";
        }
        return temp;
    }

}
