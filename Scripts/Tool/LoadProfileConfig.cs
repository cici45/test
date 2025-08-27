using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LoadProfileConfig : MonoBehaviour
{
    public static LoadProfileConfig loadData;

    public float TSet;
    public float GearPosition;
    /// <summary>
    /// 肌力系数
    /// </summary>
    string strengthCoefficientData;
    /// <summary>
    /// 耐力系数
    /// </summary>
    string enduranceCoefficientData;

    string[] strengthCoefficient;
    string[] enduranceCoefficient;
    void Awake()
    {
        LoadXml(Application.streamingAssetsPath + "/ProfileConfig.xml");
    }

    private void Start()
    {
        loadData = this;
    }

    //读取XML
    void LoadXml(string Path)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(Path);
        XmlNodeList xmlNodeList = xml.SelectSingleNode("ProfileData").ChildNodes;
        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            switch (xl1.GetAttribute("Name"))
            {
                case "设备名称":
                    UserInfoData.EquipmentName = xl1.InnerText;
                    break;
                case "比例系数":
                    UserInfoData.Scale = float.Parse(xl1.InnerText);
                    break;
                case "单位":
                    UserInfoData.Unit = xl1.InnerText;
                    break;
                case "model":
                    break;
                case "接口IP":
                    UserInfoData.Ip= xl1.InnerText;
                    break;
                case "Sample_Time":
                    break;
                case "Delay_Accept_Time":
                    break;
                case "耐力":
                    HistoryMedical.enduranceDataText = xl1.InnerText;
                    break;
                case "肌力":
                    HistoryMedical.strengthDataText = xl1.InnerText;
                    break;
                case "关节活动度":
                    HistoryMedical.AngleDataText = xl1.InnerText;
                    break;
                case "TSet":
                    TSet = float.Parse(xl1.InnerText);
                    break;
                case "肌力系数":
                    strengthCoefficientData = xl1.InnerText;
                    strengthCoefficient = strengthCoefficientData.Split('_');
                    break;
                case "耐力系数":
                    enduranceCoefficientData = xl1.InnerText;
                    enduranceCoefficient = enduranceCoefficientData.Split('_');
                    break;
                case "医院名称":
                    UserInfoData.HospitalName = xl1.InnerText;
                    break;
                case "预准备倒计时":
                    UserInfoData.countDown = float.Parse(xl1.InnerText);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 获取肌力系数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetStrengthCoefficientData(int index)
    {
        return float.Parse(strengthCoefficient[index]);
    }

    /// <summary>
    /// 获取耐力系数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetEnduranceCoefficientData(int index)
    {
        return float.Parse(enduranceCoefficient[index]);
    }

}

