using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 运动机能健康干预方案计算工具
/// </summary>
public class MotionFunctionCalculator : MonoBehaviour
{
    // 文档中给出的参数值
    [Header("固定参数（来自文档）")]
    public float flowCoefficientC = 0.69361f;   // 流量系数 C
    public float areaParameterS = 3.8147e-6f;   // 面积参数 S (m²)
    public float wallAreaSWall = 0.0010489f;    // 液体有效横截面积 S壁 (m²)
    public float cylinderAreaSCyl = 0.00048215f;// 活塞横截面积 S缸 (m²)
    public float fluidDensityRho = 794.2886f;   // 介质密度 ρ (kg/m³)
    public float correctionDelta = 0f;          // 修正量 ∆ (m²)

    // 实验数据列表（可从文档表格中提取）
   // [Header("实验数据")]
    [System.Serializable]
    public class ExperimentData
    {
        public int gear;               // 挡位 D挡
        public float loadG;            // 负载 G (N)
        public float timeT;            // 时间 t (s)
        public float initialLength;    // 初始长度 (mm)
        public float measuredVelocity; // 实测速度 v (m/s)
    }

    public List<ExperimentData> experimentList = new List<ExperimentData>
    {
        // 实验序号1
        new ExperimentData { gear = 1, loadG = 30f, timeT = 1f, initialLength = 35.25f, measuredVelocity = 0.030574429f },
        // 实验序号2
        new ExperimentData { gear = 2, loadG = 25f, timeT = 2f, initialLength = 39.8f, measuredVelocity = 0.015725594f },
        // 实验序号3
        new ExperimentData { gear = 5, loadG = 35f, timeT = 1f, initialLength = 52.34f, measuredVelocity = 0.006390653f },
        // 实验序号4
        new ExperimentData { gear = 6, loadG = 40f, timeT = 1f, initialLength = 30.34f, measuredVelocity = 0.006215459f },
        // 实验序号5
        new ExperimentData { gear = 7, loadG = 45f, timeT = 1f, initialLength = 29f, measuredVelocity = 0.005780347f },
        // 实验序号6
        new ExperimentData { gear = 7, loadG = 50f, timeT = 1f, initialLength = 22.75f, measuredVelocity = 0.006371288f },
        // 实验序号7
        new ExperimentData { gear = 8, loadG = 50f, timeT = 1f, initialLength = 24.4f, measuredVelocity = 0.005524544f }
    };

    // UI显示组件
    [Header("UI显示")]
    public Text resultText;
    public GameObject errorPanel;
    public Text errorText;

    public static MotionFunctionCalculator MotionFunction;
    private void Start()
    {
        MotionFunction = this;
        // 验证所有实验数据的误差
        ValidateExperimentData();
    }

    /// <summary>
    /// 验证实验数据并显示结果
    /// </summary>
    public void ValidateExperimentData()
    {
        try
        {
            string results = "实验数据验证结果:\n\n";
            results += "挡位\t负载(N)\t实测速度(mm/s)\t计算速度(mm/s)\t误差(%)\n";
            results += "----------------------------------------------------------------\n";

            foreach (var data in experimentList)
            {
                float calculatedVelocity = CalculateVelocity(data.gear, data.loadG);
                float errorPercentage = CalculateError(data.measuredVelocity, calculatedVelocity);

                results += $"{data.gear}\t{data.loadG:F1}\t\t{data.measuredVelocity * 1000:F2}\t\t\t{calculatedVelocity * 1000:F2}\t\t\t{errorPercentage:F2}\n";
            }

            if (resultText != null)
                resultText.text = results;
            else
                Debug.Log(results);
        }
        catch (Exception e)
        {
            ShowError($"验证过程出错: {e.Message}");
        }
    }

    /// <summary>
    /// 根据挡位和负载计算速度 v = f(D挡, G)
    /// </summary>
    /// <param name="gear">挡位 D挡</param>
    /// <param name="loadG">负载 G (N)</param>
    /// <returns>速度 v (m/s)</returns>
    public float CalculateVelocity(int gear, float loadG)
    {
        if (gear <= 0)
            throw new ArgumentException("挡位必须大于0", nameof(gear));
        
        if (loadG <= 0)
            throw new ArgumentException("负载必须大于0", nameof(loadG));

        // 代入化简后的公式 G = 30091.7334 * v² * D挡²
        // 推导得 v = sqrt(G / (30091.7334 * D挡²))
        float coefficient = 30091.7334f;
        float vSquared = loadG / (coefficient * Mathf.Pow(gear, 2));
        return Mathf.Sqrt(vSquared);
    }

    /// <summary>
    /// 计算误差百分比
    /// </summary>
    /// <param name="measuredValue">实测值</param>
    /// <param name="calculatedValue">计算值</param>
    /// <returns>误差百分比（%）</returns>
    public float CalculateError(float measuredValue, float calculatedValue)
    {
        if (measuredValue == 0)
            return 0f;
            
        return ((calculatedValue - measuredValue) / measuredValue) * 100f;
    }

    /// <summary>
    /// 完整公式计算（用于验证参数）
    /// </summary>
    /// <param name="gear">挡位 D挡</param>
    /// <param name="velocity">速度 v (m/s)</param>
    /// <returns>负载 G (N)</returns>
    public float CalculateLoad(int gear, float velocity)
    {
        if (gear <= 0)
            throw new ArgumentException("挡位必须大于0", nameof(gear));
            
        if (velocity < 0)
            throw new ArgumentException("速度不能为负数", nameof(velocity));

        // 原始公式：G = (S壁² * S缸 * ρ * v²) / (2 * C² * (S/D挡 + ∆)²)
        float numerator = Mathf.Pow(wallAreaSWall, 2) * cylinderAreaSCyl * fluidDensityRho * Mathf.Pow(velocity, 2);
        float denominator = 2f * Mathf.Pow(flowCoefficientC, 2) * Mathf.Pow((areaParameterS / gear + correctionDelta), 2);
        
        return numerator / denominator;
    }

    /// <summary>
    /// 显示错误信息
    /// </summary>
    private void ShowError(string message)
    {
        Debug.LogError(message);
        if (errorPanel != null && errorText != null)
        {
            errorText.text = message;
            errorPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 计算自定义挡位和负载的速度
    /// </summary>
    public void CalculateCustomValue(int gear, float load)
    {
        try
        {
            float velocity = CalculateVelocity(gear, load);
            string result = $"自定义计算结果:\n";
            result += $"挡位: {gear}\n";
            result += $"负载: {load} N\n";
            result += $"计算速度: {velocity * 1000:F2} mm/s";

            if (resultText != null)
                resultText.text = result;
            else
                Debug.Log(result);
        }
        catch (Exception e)
        {
            ShowError($"计算出错: {e.Message}");
        }
    }
}    