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
    /// ����ϵ��
    /// </summary>
    string strengthCoefficientData;
    /// <summary>
    /// ����ϵ��
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

    //��ȡXML
    void LoadXml(string Path)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(Path);
        XmlNodeList xmlNodeList = xml.SelectSingleNode("ProfileData").ChildNodes;
        //���������ӽڵ�
        foreach (XmlElement xl1 in xmlNodeList)
        {
            switch (xl1.GetAttribute("Name"))
            {
                case "�豸����":
                    UserInfoData.EquipmentName = xl1.InnerText;
                    break;
                case "����ϵ��":
                    UserInfoData.Scale = float.Parse(xl1.InnerText);
                    break;
                case "��λ":
                    UserInfoData.Unit = xl1.InnerText;
                    break;
                case "model":
                    break;
                case "�ӿ�IP":
                    UserInfoData.Ip= xl1.InnerText;
                    break;
                case "Sample_Time":
                    break;
                case "Delay_Accept_Time":
                    break;
                case "����":
                    HistoryMedical.enduranceDataText = xl1.InnerText;
                    break;
                case "����":
                    HistoryMedical.strengthDataText = xl1.InnerText;
                    break;
                case "�ؽڻ��":
                    HistoryMedical.AngleDataText = xl1.InnerText;
                    break;
                case "TSet":
                    TSet = float.Parse(xl1.InnerText);
                    break;
                case "����ϵ��":
                    strengthCoefficientData = xl1.InnerText;
                    strengthCoefficient = strengthCoefficientData.Split('_');
                    break;
                case "����ϵ��":
                    enduranceCoefficientData = xl1.InnerText;
                    enduranceCoefficient = enduranceCoefficientData.Split('_');
                    break;
                case "ҽԺ����":
                    UserInfoData.HospitalName = xl1.InnerText;
                    break;
                case "Ԥ׼������ʱ":
                    UserInfoData.countDown = float.Parse(xl1.InnerText);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// ��ȡ����ϵ��
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetStrengthCoefficientData(int index)
    {
        return float.Parse(strengthCoefficient[index]);
    }

    /// <summary>
    /// ��ȡ����ϵ��
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetEnduranceCoefficientData(int index)
    {
        return float.Parse(enduranceCoefficient[index]);
    }

}

