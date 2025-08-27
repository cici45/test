using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class AddPanel : MonoBehaviour
{
    Button[] BtnArr;
    Toggle[] TogArr;
    Transform User;
    GameObject FindPage;
    InputField Input_Name, Input_Age, Input_ID, Input_Identifying;
    InputField Input_PatientId;
    Dropdown Dropdown_Sex;
    string TableName = "UserInfo";
    string[] colName = new string[] { "UserName", "UserID", "UserAge", "UserSex" ,"Identifying" };
    string[] data = new string[5];
    List<string[]> dataList = new List<string[]>();
    bool IsModify = false;
    string UserData;
    bool IsAdd = false;

    private void Awake()
    {
        BtnArr = transform.GetComponentsInChildren<Button>();
        User = transform.Find("UserPage/Scroll View/Viewport/User");
        Input_Name = transform.Find("AddPage/Page/Input_Name").GetComponent<InputField>();
        Input_Age = transform.Find("AddPage/Page/Input_Age").GetComponent<InputField>();
        Dropdown_Sex = transform.Find("AddPage/Page/Dropdown_Sex").GetComponent<Dropdown>();
        Input_ID = transform.Find("AddPage/Page/Input_ID").GetComponent<InputField>();
        Input_Identifying = transform.Find("AddPage/Page/Input_Identifying").GetComponent<InputField>();
        FindPage = transform.Find("FindPage").gameObject;
        Input_PatientId = transform.Find("FindPage/BG/Input_PatientId").GetComponent<InputField>();
    }

    void Start()
    {
        for (int i = 0; i < BtnArr.Length; i++)
        {
            Button btn = BtnArr[i];
            btn.onClick.AddListener(delegate () { this.OnButtonClick(btn); });
        }
        MessageCenter.AddMsgListener("AddPanel", OpenAddPanel);
        MessageCenter.AddMsgListener("ModifyUser", ModifyUserInfo);
        MessageCenter.AddMsgListener("UnDataToggleArrs", UnDataToggleArrs);
        FindPage.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void OnButtonClick(Button btn)
    {
        switch (btn.name)
        {
            case "Btn_Quit":
                if (ScanCodePage.IsScanCode)
                {
                    if (!IsAdd)
                    {
                        MessageCenter.SendMsg("MainPanel", true);
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        MessageCenter.SendMsg("UserPanel", true);
                        IsModify = false;
                        this.gameObject.SetActive(false);
                    }
                }
                else
                {
                    MessageCenter.SendMsg("UserPanel", true);
                    IsModify = false;
                    this.gameObject.SetActive(false);
                }
                break;
            case "Btn_Save":
                UpdateUesrData();
                break;
            case "Btn_Find":
                FindUserInfoData(Input_PatientId.text);
                break;
            case "Btn_Open":
                FindPage.SetActive(true);
                break;
            case "Btn_Off":
                FindPage.SetActive(false);
                break;
            default:
                break;
        }
    }


    private void OnToggleValueChange(Toggle tog, bool isOn)
    {
        UserInfoData.UserId = tog.transform.parent.name;
        InitUserInfo();
    }

    void FindUserInfoData(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            TipsPage_1.tipsPage.OpenThisPage("搜索内容不能为空！");
            return;
        }
        PostData_1 data_1 = new PostData_1();
        data_1.communityDoctorId = "12";
        data_1.patientId = str;
        //UserInfoData.Ip = "192.168.2.247:8080";
        try
        {
            UserData = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/comprehensive/basicPatientInformation", data_1);
            UserData d = JsonUtility.FromJson<UserData>(UserData);
            JsonUtility.FromJsonOverwrite(UserData, d);
            Debug.Log(UserData);
            if (OperateUseSQL_H.IsTbleExist("UserInfo"))
            {
                if (!OperateUseSQL_H.IsHavaData(d.data[0].phone, "UserInfo"))
                {
                    DateTime dateTime = DateTime.Now;
                    dataList.Clear();
                    data[0] = d.data[0].name;
                    data[1] = dateTime.ToString("yyyyMMddhhmmss");
                    data[2] = d.data[0].age;
                    data[3] = d.data[0].sex;
                    data[4] = d.data[0].phone;
                    dataList.Add(data);
                    OperateUseSQL_H.Add_Data(TableName, colName, dataList);
                    UserInfoData.UserId = data[1];
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    TipsPage_1.tipsPage.OpenThisPage("用户添加成功！");
                }
                else
                {
                    TipsPage_1.tipsPage.OpenThisPage("该用户已存在！");
                }
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                dataList.Clear();
                data[0] = d.data[0].name;
                data[1] = dateTime.ToString("yyyyMMddhhmmss");
                data[2] = d.data[0].age;
                data[3] = d.data[0].sex;
                data[4] = d.data[0].phone;
                dataList.Add(data);
                OperateUseSQL_H.Add_Data(TableName, colName, dataList);
                UserInfoData.UserId = data[1];
                gameObject.SetActive(false);
                gameObject.SetActive(true);
                TipsPage_1.tipsPage.OpenThisPage("用户添加成功！");
            }
            FindPage.SetActive(false);
        }
        catch (Exception)
        {
            Debug.Log("获取数据失败");
        }
    }


    /// <summary>
    /// 初始化用户信息显示
    /// </summary>
    void InitUserInfo()
    {
        if(IsModify)
        {
            string[] tempArr = UserInfoData.GetUserInfo();
            Input_Name.text = tempArr[0];
            Input_ID.text = tempArr[1];
            Input_Identifying.text = tempArr[4];
            Input_Identifying.interactable = false;
            Input_Age.text = tempArr[2];
            if (tempArr[3]=="男")
            {
                Dropdown_Sex.value = 0;
            }
            else
            {
                Dropdown_Sex.value = 1;
            }
        }
    }



    /// <summary>
    /// 更新Toggle点击事件
    /// </summary>
    void InitToggleArr()
    {
        TogArr = User.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < TogArr.Length; i++)
        {
            Toggle tog = TogArr[i];
            tog.onValueChanged.RemoveAllListeners();
            tog.onValueChanged.AddListener((bool isOn) => { OnToggleValueChange(tog, isOn); });
        }
    }

    /// <summary>
    /// 添加/修改用户并更新用户列表
    /// </summary>
    private void UpdateUesrData()
    {
        if (string.IsNullOrEmpty(Input_Name.text) || string.IsNullOrEmpty(Input_Age.text) || string.IsNullOrEmpty(Input_Identifying.text))
        {
            TipsPage_1.tipsPage.OpenThisPage("用户信息不完整，请完善用户信息！");
        }
        else
        {
            dataList.Clear();
            data[0] = Input_Name.text;
            data[1] = Input_ID.text;
            data[2] = Input_Age.text;
            data[3] = Dropdown_Sex.captionText.text;
            data[4] = Input_Identifying.text;
            dataList.Add(data);
            if(!IsModify)
            {
                if (OperateUseSQL_H.IsTbleExist("UserInfo"))
                {
                    if (!OperateUseSQL_H.IsHavaData(data[4], "UserInfo"))
                    {
                        OperateUseSQL_H.Add_Data(TableName, colName, dataList);
                        TipsPage_1.tipsPage.OpenThisPage("用户添加成功！");
                    }
                    else
                    {
                        TipsPage_1.tipsPage.OpenThisPage("用户已存在！");
                    }
                }
                else
                {
                    OperateUseSQL_H.Add_Data(TableName, colName, dataList);
                    TipsPage_1.tipsPage.OpenThisPage("用户添加成功！");
                }
                MessageCenter.SendMsg("InitUserPage", true);
                foreach (var item in UserInfoData.UserDataDic)
                {
                    string[] tempArr = item.Value;
                    if (tempArr[4].Equals(data[4]))
                    {
                        data[1] = tempArr[1];
                    }
                }
                Input_Name.text = "";
                Dropdown_Sex.value = 0;
                Input_Age.text = "";
                Input_Identifying.text = "";
                UserInfoData.UserId = data[1];
                IsAdd = true;
            }
            else
            {
                string[] tempData = new string[] { data[0], data[2], data[3] };
                string[] tempcol = new string[] { colName[0], colName[2], colName[3] };
                OperateUseSQL_H.Updata_Data(TableName, tempcol, tempData, data[1]);
                TipsPage_1.tipsPage.OpenThisPage("用户信息修改成功！");
                MessageCenter.SendMsg("InitUserPage", true);
                InitUserInfo();
            }
        }
    }


    private void ModifyUserInfo(ParameterData pa)
    {
        IsModify = (bool)pa.data;
        this.gameObject.SetActive(IsModify);
        InitUserInfo();
        Input_Identifying.interactable = false;
        if (!IsModify)
        {
            Input_Identifying.interactable = true;
            Input_Name.text = "";
            Input_Identifying.text = "";
            Dropdown_Sex.value = 0;
            Input_Age.text = "";
        }
    }


    void OpenAddPanel(ParameterData da)
    {
        bool value = (bool)da.data;
        this.gameObject.SetActive(value);
        DateTime dateTime = DateTime.Now;
        Input_ID.text = dateTime.ToString("yyyyMMddhhmmss");
        Input_ID.interactable = false;
    }

    void UnDataToggleArrs(ParameterData pa)
    {
        InitToggleArr();
    }

    private void OnDisable()
    {
        if (IsModify)
        {
            IsModify = false;
        }
    }

}
