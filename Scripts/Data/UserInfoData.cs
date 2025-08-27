using Ag_Class_Test;
using Org.BouncyCastle.Bcpg;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class UserInfoData : MonoBehaviour
{
    // 训练模型数据表字段（方案名称、游戏时间、难度、角度范围）
    public static string[] TrainModelTable = new string[] { "ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" };
    public static string[] TrainPlanTable = new string[] { "ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType", "TrainingWeight"  };

    public static SettleData settleData = new SettleData();
    public static AG class_ag;
    //硬件与运动参数
    public static float Scale;
    public static float CurrentAngle;
    public static float GameTime;
    public static float countDown;//倒计时
    public static string Ip;
    public static string endurance_level;
    public static string muscle_level;
    public static string arthrosis_level;
    public static string Unit;
    public static string EquipmentName;
    public static string MaxAugle;
    public static string TrainingWeight;
    public static string UserId;
    public static string StartTime;
    public static string checkTime;
    public static string HistoreTogName;
    public static string gameCount;
    public static string gears;//档位

    public static string[] TrainPlanArr;
    public static Dictionary<string, string[]> UserDataDic = new Dictionary<string, string[]>();
    public static Dictionary<string, string[]> HistoryDataDic = new Dictionary<string, string[]>();
    public static Dictionary<string, string[]> HistoryPlanDic = new Dictionary<string, string[]>();
    public static float CompletionDegree;
    public static string HospitalName;
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    public static string[] GetUserInfo()
    {
        if (string.IsNullOrEmpty(UserId)) return null;
        if (UserDataDic.ContainsKey(UserId))
            return UserDataDic[UserId];
        else
            return null;
    }

    /// <summary>
    /// 获取历史记录信息
    /// </summary>
    /// <returns></returns>
    public static string[] GetHistoryData()
    {
        if (string.IsNullOrEmpty(HistoreTogName)) return null;
        if (HistoryDataDic.ContainsKey(HistoreTogName))
            return HistoryDataDic[HistoreTogName];
        else
            return null;
    }

    /// <summary>
    /// 训练信息赋值
    /// </summary>
    /// <param name="name"></param>
    public static void GetHistoryPlan(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;
        if (HistoryPlanDic.ContainsKey(name))
        {
            TrainPlanArr = HistoryPlanDic[name];
            MaxAugle = TrainPlanArr[3];
        }
        else
            TrainPlanArr = null;
    }

    /// <summary>
    /// 获取训练类型
    /// </summary>
    /// <param name="GameName">游戏名称</param>
    /// <returns></returns>
    public static string GetTrainingType(string GameName)
    {
        string TrainingType = "";
        switch (GameName)
        {
            case "宇宙航行":
                TrainingType = "等张训练";
                break;
            case "城际穿梭":
                TrainingType = "等张训练";
                break;
            default:
                break;
        }
        return TrainingType;
    }

    /// <summary>
    /// 获取训练难度
    /// </summary>
    /// <param name="GameName">游戏名称</param>
    /// <param name="difficulty">难度数值</param>
    /// <returns></returns>
    public static string GetGameDifficulty(string GameName, string difficulty)
    {
        string _name = "";
        switch (GameName)
        {
            case "宇宙航行":
            case "城际穿梭":
                switch (difficulty)
                {
                    case "10":
                        _name = "简单";
                        break;
                    case "7":
                        _name = "一般";
                        break;
                    case "5":
                        _name = "困难";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        return _name;
    }
}
public class PostCheckData
{
    public string checkTime;//检查时间
    public string checkDuration;//检查时长
    public string deviceNum;//设备编号
    public string myodynamia;//肌力
    public string endurance;//耐力
    public string jointMotion;//关节活动度
    public string phone;
    public string checkGear;//检查档位
}
public class PostData
{
    /// <summary>
    /// 标识位
    /// </summary>
    public string phone;
    /// <summary>
    /// 患者id
    /// </summary>
    public string uId;
    /// <summary>
    /// 训练日期
    /// </summary>
    public string date;
    /// <summary>
    /// 设备编号
    /// </summary>
    public string equipment;
    /// <summary>
    /// 关节活动度
    /// </summary>
    public string arthrosis;
    /// <summary>
    /// 最大运动角度
    /// </summary>
    public string maxMontionAngle;
    /// <summary>
    /// 最小运动角度
    /// </summary>
    public string minMontionAngle;
    /// <summary>
    /// 平均运动角度
    /// </summary>
    public string aveMontionAngle;
    /// <summary>
    /// 最大运动加速度
    /// </summary>
    public string maxAccSpeed;
    /// <summary>
    /// 平均运动加速度
    /// </summary>
    public string aveAccSpeed;
    /// <summary>
    /// 肌力
    /// </summary>
    public string muscleStrength;
    /// <summary>
    /// 肌力稳定性
    /// </summary>
    public string muscleStability;
    /// <summary>
    /// 肌耐力
    /// </summary>
    public string endurance;
    /// <summary>
    /// 游戏名称
    /// </summary>
    public string gameName;
    /// <summary>
    /// 游戏次数
    /// </summary>
    public string drillNum;
    /// <summary>
    /// 训练难度
    /// </summary>
    public string difficultyLevel;
    /// <summary>
    /// 训练难度百分比
    /// </summary>
    public string taskPercentage;
    /// <summary>
    /// 分数
    /// </summary>
    public string score;
    /// <summary>
    /// 训练时间
    /// </summary>
    public string equipmentDuration;
    /// <summary>
    /// 耐力评级
    /// </summary>
    public string endurance_level;
    /// <summary>
    /// 肌力评级
    /// </summary>
    public string muscle_level;

    /// <summary>
    /// 关节度评级
    /// </summary>
    public string arthrosis_level;

    public string angularX;

    public string angularY;

    public string accSpeedX;

    public string accSpeedY;

}
/// <summary>
/// 3.0
/// </summary>
public class PostTrainData
{
    /// <summary>
    ///   用户id
    /// </summary>
    public int userId;
    /// <summary>
    /// //手机号（判断用户唯一标识）
    /// </summary>

    public string phone;
    /// <summary>
    ///   训练设备
    /// </summary>
    public string trainingDevice;
    /// <summary>
    /// 肌力，使用 DECIMAL 存储可能带有小数的力量数据
    /// </summary>
    public double muscleStrength;
    /// <summary>
    ///  耐力，存储耐力数据
    /// </summary>
    public double endurance;
    /// <summary>
    /// 关节活动度
    /// </summary>
    public double jointRangeOfMotion;
    /// <summary>
    ///  运动机能综合评分单次
    /// </summary>
    public double comprehensiveScore;
    /// <summary>
    /// 等级单次
    /// </summary>
    public double grade;
    /// <summary>
    /// 训练开始日期
    /// </summary>
   // public string startDate;
    /// <summary>
    /// 训练结束日期
    /// </summary>
   // public string endDate;
    /// <summary>
    /// //负荷
    /// </summary>
    public string load1;
    /// <summary>
    ///  //完成数量 任务完成度百分比  完成次数
    /// </summary>
    public string taskPercentage;
    /// <summary>
    ///  训练时长
    /// </summary>
    public string movementTime;
    /// <summary>
    /// 训练难度
    /// </summary>
    public string difficultyLevel;
    /// <summary>
    /// 医学评定分档
    /// </summary>
    public string level;
    /// <summary>
    /// 肌力评分
    /// </summary>
    public string muscleLevel;
    /// <summary>
    /// 耐力评分
    /// </summary>
    public string enduranceLevel;
    /// <summary>
    ///  关节活动度评分
    /// </summary>
    public string arthrosisLevel;
    /// <summary>
    ///  角速度X轴字符串
    /// </summary>
    public string angularX;
    /// <summary>
    /// /角速度Y轴字符串
    /// </summary>
    public string angularY;
    /// <summary>
    /// 加速度X轴字符串
    /// </summary>
    public string accSpeedX;
    /// <summary>
    /// 加速度Y轴字符串
    /// </summary>
    public string accSpeedY;
    /// <summary>
    /// 类型 检查或者干预
    /// </summary>
    public int type;

}
public class SettleData
{
    public double Total_Data_Max;//最大角度
    public double Total_Data_Min;//最小角度
    public double Total_V_Main;//平均速度
    public double Total_V_Max;//最大速度
    public double[] V_Main = new double[5];//平均速度数组
    public double[] V_Max = new double[5];//最大速度数组
    public double[] V_Min = new double[5];//最小速度数组
    public double Total_ACC_Main;//平均加速度
    public double Total_Acc_Max;//肌力
    public double[] ACC_Main = new double[5];//平均加速度数组
    public double[] ACC_Max = new double[5];//最大加速度数组
    public double[] ACC_Min = new double[5];//最小加速度数组
    public int n_Feedback;//数组长度
    public double Muscle_State;//肌力稳定性
    public double Endurance_time;//持续时间
    public double Endurance_Value;//耐力
}

