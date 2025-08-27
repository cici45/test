using Ag_Class_Test;
using Org.BouncyCastle.Bcpg;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class UserInfoData : MonoBehaviour
{
    // ѵ��ģ�����ݱ��ֶΣ��������ơ���Ϸʱ�䡢�Ѷȡ��Ƕȷ�Χ��
    public static string[] TrainModelTable = new string[] { "ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" };
    public static string[] TrainPlanTable = new string[] { "ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType", "TrainingWeight"  };

    public static SettleData settleData = new SettleData();
    public static AG class_ag;
    //Ӳ�����˶�����
    public static float Scale;
    public static float CurrentAngle;
    public static float GameTime;
    public static float countDown;//����ʱ
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
    public static string gears;//��λ

    public static string[] TrainPlanArr;
    public static Dictionary<string, string[]> UserDataDic = new Dictionary<string, string[]>();
    public static Dictionary<string, string[]> HistoryDataDic = new Dictionary<string, string[]>();
    public static Dictionary<string, string[]> HistoryPlanDic = new Dictionary<string, string[]>();
    public static float CompletionDegree;
    public static string HospitalName;
    /// <summary>
    /// ��ȡ�û���Ϣ
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
    /// ��ȡ��ʷ��¼��Ϣ
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
    /// ѵ����Ϣ��ֵ
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
    /// ��ȡѵ������
    /// </summary>
    /// <param name="GameName">��Ϸ����</param>
    /// <returns></returns>
    public static string GetTrainingType(string GameName)
    {
        string TrainingType = "";
        switch (GameName)
        {
            case "���溽��":
                TrainingType = "����ѵ��";
                break;
            case "�Ǽʴ���":
                TrainingType = "����ѵ��";
                break;
            default:
                break;
        }
        return TrainingType;
    }

    /// <summary>
    /// ��ȡѵ���Ѷ�
    /// </summary>
    /// <param name="GameName">��Ϸ����</param>
    /// <param name="difficulty">�Ѷ���ֵ</param>
    /// <returns></returns>
    public static string GetGameDifficulty(string GameName, string difficulty)
    {
        string _name = "";
        switch (GameName)
        {
            case "���溽��":
            case "�Ǽʴ���":
                switch (difficulty)
                {
                    case "10":
                        _name = "��";
                        break;
                    case "7":
                        _name = "һ��";
                        break;
                    case "5":
                        _name = "����";
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
    public string checkTime;//���ʱ��
    public string checkDuration;//���ʱ��
    public string deviceNum;//�豸���
    public string myodynamia;//����
    public string endurance;//����
    public string jointMotion;//�ؽڻ��
    public string phone;
    public string checkGear;//��鵵λ
}
public class PostData
{
    /// <summary>
    /// ��ʶλ
    /// </summary>
    public string phone;
    /// <summary>
    /// ����id
    /// </summary>
    public string uId;
    /// <summary>
    /// ѵ������
    /// </summary>
    public string date;
    /// <summary>
    /// �豸���
    /// </summary>
    public string equipment;
    /// <summary>
    /// �ؽڻ��
    /// </summary>
    public string arthrosis;
    /// <summary>
    /// ����˶��Ƕ�
    /// </summary>
    public string maxMontionAngle;
    /// <summary>
    /// ��С�˶��Ƕ�
    /// </summary>
    public string minMontionAngle;
    /// <summary>
    /// ƽ���˶��Ƕ�
    /// </summary>
    public string aveMontionAngle;
    /// <summary>
    /// ����˶����ٶ�
    /// </summary>
    public string maxAccSpeed;
    /// <summary>
    /// ƽ���˶����ٶ�
    /// </summary>
    public string aveAccSpeed;
    /// <summary>
    /// ����
    /// </summary>
    public string muscleStrength;
    /// <summary>
    /// �����ȶ���
    /// </summary>
    public string muscleStability;
    /// <summary>
    /// ������
    /// </summary>
    public string endurance;
    /// <summary>
    /// ��Ϸ����
    /// </summary>
    public string gameName;
    /// <summary>
    /// ��Ϸ����
    /// </summary>
    public string drillNum;
    /// <summary>
    /// ѵ���Ѷ�
    /// </summary>
    public string difficultyLevel;
    /// <summary>
    /// ѵ���ѶȰٷֱ�
    /// </summary>
    public string taskPercentage;
    /// <summary>
    /// ����
    /// </summary>
    public string score;
    /// <summary>
    /// ѵ��ʱ��
    /// </summary>
    public string equipmentDuration;
    /// <summary>
    /// ��������
    /// </summary>
    public string endurance_level;
    /// <summary>
    /// ��������
    /// </summary>
    public string muscle_level;

    /// <summary>
    /// �ؽڶ�����
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
    ///   �û�id
    /// </summary>
    public int userId;
    /// <summary>
    /// //�ֻ��ţ��ж��û�Ψһ��ʶ��
    /// </summary>

    public string phone;
    /// <summary>
    ///   ѵ���豸
    /// </summary>
    public string trainingDevice;
    /// <summary>
    /// ������ʹ�� DECIMAL �洢���ܴ���С������������
    /// </summary>
    public double muscleStrength;
    /// <summary>
    ///  �������洢��������
    /// </summary>
    public double endurance;
    /// <summary>
    /// �ؽڻ��
    /// </summary>
    public double jointRangeOfMotion;
    /// <summary>
    ///  �˶������ۺ����ֵ���
    /// </summary>
    public double comprehensiveScore;
    /// <summary>
    /// �ȼ�����
    /// </summary>
    public double grade;
    /// <summary>
    /// ѵ����ʼ����
    /// </summary>
   // public string startDate;
    /// <summary>
    /// ѵ����������
    /// </summary>
   // public string endDate;
    /// <summary>
    /// //����
    /// </summary>
    public string load1;
    /// <summary>
    ///  //������� ������ɶȰٷֱ�  ��ɴ���
    /// </summary>
    public string taskPercentage;
    /// <summary>
    ///  ѵ��ʱ��
    /// </summary>
    public string movementTime;
    /// <summary>
    /// ѵ���Ѷ�
    /// </summary>
    public string difficultyLevel;
    /// <summary>
    /// ҽѧ�����ֵ�
    /// </summary>
    public string level;
    /// <summary>
    /// ��������
    /// </summary>
    public string muscleLevel;
    /// <summary>
    /// ��������
    /// </summary>
    public string enduranceLevel;
    /// <summary>
    ///  �ؽڻ������
    /// </summary>
    public string arthrosisLevel;
    /// <summary>
    ///  ���ٶ�X���ַ���
    /// </summary>
    public string angularX;
    /// <summary>
    /// /���ٶ�Y���ַ���
    /// </summary>
    public string angularY;
    /// <summary>
    /// ���ٶ�X���ַ���
    /// </summary>
    public string accSpeedX;
    /// <summary>
    /// ���ٶ�Y���ַ���
    /// </summary>
    public string accSpeedY;
    /// <summary>
    /// ���� �����߸�Ԥ
    /// </summary>
    public int type;

}
public class SettleData
{
    public double Total_Data_Max;//���Ƕ�
    public double Total_Data_Min;//��С�Ƕ�
    public double Total_V_Main;//ƽ���ٶ�
    public double Total_V_Max;//����ٶ�
    public double[] V_Main = new double[5];//ƽ���ٶ�����
    public double[] V_Max = new double[5];//����ٶ�����
    public double[] V_Min = new double[5];//��С�ٶ�����
    public double Total_ACC_Main;//ƽ�����ٶ�
    public double Total_Acc_Max;//����
    public double[] ACC_Main = new double[5];//ƽ�����ٶ�����
    public double[] ACC_Max = new double[5];//�����ٶ�����
    public double[] ACC_Min = new double[5];//��С���ٶ�����
    public int n_Feedback;//���鳤��
    public double Muscle_State;//�����ȶ���
    public double Endurance_time;//����ʱ��
    public double Endurance_Value;//����
}

