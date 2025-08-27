using Senser_Driver_Space;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScanCodePage : MonoBehaviour
{
    public static bool IsScanCode;
    public static string Phone;
    public static string TrainTime;
    public static string DifficultyLevel;
    Button Btn_Back, Btn_Find;
  
    string UserData;
    string UserDataState;
    string TrainData;
    string TableName = "UserInfo";
    string[] colName = new string[] { "UserName", "UserID", "UserAge", "UserSex", "Identifying" };
    string[] data = new string[5];

    bool isNum = false;//是否为数字
    bool isTrue = false;
    int userCount;
    List<string[]> dataList = new List<string[]>();
    InputField Input_PatientId;
    Dropdown userListDropdown;
    Dictionary<string, string[]> tempDataDic = new Dictionary<string, string[]>();
    Senser_Driver senser_1;
  
    void Awake()
    {
        senser_1 = new Senser_Driver(Application.streamingAssetsPath + "/CodeScanningGun/Com_XML.xml");
        Input_PatientId = transform.Find("BG/Input_PatientId").GetComponent<InputField>();
        Btn_Back = transform.Find("BG/Btn_Back").GetComponent<Button>();
        Btn_Find = transform.Find("BG/Btn_Find").GetComponent<Button>();
        userListDropdown = transform.Find("BG/UserListDropdown").GetComponent<Dropdown>();
        Btn_Back.onClick.AddListener(OnBackButtonClick);
        Btn_Find.onClick.AddListener(MultiuserPprocessing);
        Input_PatientId.onEndEdit.AddListener(FindUserInfoData);
    }
    private void Start()
    {
        NetworkHintPanle.tipsPage.okAction = SpecificUserLogin;
        NetworkHintPanle.tipsPage.notAction = () => { Application.Quit(); };
        IsConnectedInternet();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        IsScanCode = false;
        StartCoroutine("ReadScanCodeData");
    }

    private void OnDisable()
    {
        StopCoroutine("ReadScanCodeData");
    }



    void FindUsers()
    {
        AnalysisUserString(UserData);
        //AnalysisTrainString(TrainData);
    }

    IEnumerator ReadScanCodeData()
    {
        while (true)
        {
            byte[] Scannner_Resder;//= new char[1];

            yield return new WaitForSeconds(0.5f);
            bool Succ_Sign = senser_1.Read_Buff(out Scannner_Resder);
            if (Succ_Sign)
            {
                TipsPage_1.tipsPage.OpenThisPage("正在获取扫码信息，请稍后！");
                string Temp = Encoding.UTF8.GetString(Scannner_Resder, 0, Scannner_Resder.Length); ;
                Debug.Log("Scanner_  " + Temp);
                string[] tempArr = Temp.Split('#');
                PostData_5 data = new PostData_5();
                
                data.userId = tempArr[0];
                string TemoJson = DataUpload.loadData.ObjectToJson(data); 
                
                
                PostData_6 data1 = new PostData_6();
                data1.userId =data.userId;
                data1.deviceNumber = UserInfoData.EquipmentName;
                string TemoJson1 = DataUpload.loadData.ObjectToJson(data1);

                Debug.Log(TemoJson1);

                Debug.Log(TemoJson);
                try
                {
                    UserData = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/operate/getuserinformation", TemoJson);
                    UserDataState = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/operate/ifprescriptioncontent", TemoJson1);
                }
                catch (Exception)
                {
                    Debug.Log("获取数据失败");
                    UserData = null;
                }
                
                Debug.Log(UserData);
                IsScanCode = true;
                FindUsers();
            }
        }
    }

    private void AnalysisUserString(string userData)
    {
        if (string.IsNullOrEmpty(userData))
        {
            TipsPage_1.tipsPage.OpenThisPage("获取扫码信息失败，请重新扫码！");
            IsScanCode = false;
        }
        else
        {
            UserData3 d = JsonUtility.FromJson<UserData3>(UserDataState);
            JsonUtility.FromJsonOverwrite(UserDataState, d);
            Debug.Log(UserDataState);
           

            if (d.data == null)
            {
                TipsPage_1.tipsPage.OpenThisPage("获取数据失败！");
                return;
            }
            if (!d.data.hasPrescription )
            {
                TipsPage_1.tipsPage.OpenThisPage("您没有生成检查或干预方案！");
                return;
            }
            if (!d.data.paid)
            {
                TipsPage_1.tipsPage.OpenThisPage("您没有没有缴费！");
                return;
            }
            if (!d.data.deviceCorrect)
            {
                TipsPage_1.tipsPage.OpenThisPage("设备未正确！");
                return;
            }

            UserData5 d1 = JsonUtility.FromJson<UserData5>(UserData);
            JsonUtility.FromJsonOverwrite(UserData, d1);
            UserInfoData.gears= d.data.gear;
            DateTime dateTime = DateTime.Now;
            dataList.Clear();
            data[0] = d1.data.name;
            data[1] = d1.data.userId;
            data[2] = d1.data.age;
            data[3] = d1.data.sex;
            data[4] = d1.data.phone;
            if (OperateUseSQL_H.IsTbleExist("UserInfo"))
            {
                if (!OperateUseSQL_H.IsHavaData(d1.data.phone, "UserInfo"))
                {

                    dataList.Add(data);
                    OperateUseSQL_H.Add_Data(TableName, colName, dataList);
                }
            }
            else
            {

                dataList.Add(data);
                OperateUseSQL_H.Add_Data(TableName, colName, dataList);
            }
            foreach (var item in UserInfoData.UserDataDic)
            {
                string[] tempArr = item.Value;
                if (tempArr[4].Equals(data[4]))
                {
                    data[1] = tempArr[1];
                }
            }
            if (d.data.trainingType ==0 )
            {
                UserPanel.isCheckModel = true;
            }
            else
            {
                UserPanel.isCheckModel = false;
            }
            TipsPage_1.tipsPage.OpenThisPage(d.data.value+"!");
            UserInfoData.UserId = data[1];
            IsScanCode = true;
            MessageCenter.SendMsg("OpenGame", true);
            Input_PatientId.text = "";
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 查找用户提示
    /// </summary>
    /// <param name="str"></param>
    void FindUserInfoData(string str)
    {
        PostData_3 data1 = new PostData_3();//名字
        PostData_3_1 data = new PostData_3_1();//手机号
        if (string.IsNullOrEmpty(str))
        {
            TipsPage_1.tipsPage.OpenThisPage("输入不能为空！");
            return;
        }
        try
        {
            for (int i = 0; i < str.Length; i++)
            {

                if (!char.IsNumber(str, i))
                {
                    isNum = false;
                    break;
                }
                isNum = true;
            }

            if (isNum)
            {

               
                data.phone = str;
            }
            else
            {
                data1.name = str;
               
            }

        }
        catch (Exception)
        {


        }
        
        try
        {
            //UserData = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/comprehensive/basicPatientInformation", data_1);
            // UserData = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/comprehensive/basicPatientInformation1", data_1);
            string TemoJson1="";
            if (!isNum)
            {
                TemoJson1 = DataUpload.loadData.ObjectToJson(data1);
                UserData = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/operate/duplicate-drop-down-box", TemoJson1);
                Debug.Log(TemoJson1+"[]");
                UserData d = JsonUtility.FromJson<UserData>(UserData);
                JsonUtility.FromJsonOverwrite(UserData, d);
                userCount = d.data.Length;
                if (userCount==0)
                {
                    TipsPage_1.tipsPage.OpenThisPage("无此用户信息！");
                    isTrue = true;
                    return;
                }
                tempDataDic.Clear();
                Debug.Log(userListDropdown.options + "   " + userListDropdown);
                userListDropdown.options = new List<Dropdown.OptionData>();
                List<Dropdown.OptionData> options1 = userListDropdown.options;
                for (int i = 0; i < d.data.Length; i++)
                {
                    tempDataDic.Add(d.data[i].name + "  " + d.data[i].age + "   " + d.data[i].userId, new string[]
                    {
                        d.data[i].name,
                        d.data[i].userId,
                        d.data[i].age,
                        d.data[i].sex,
                        d.data[i].phone
                    });
                
                    options1.Add(new Dropdown.OptionData(d.data[i].name + "  " + d.data[i].age + "   " + d.data[i].userId));
                }
                userListDropdown.onValueChanged.AddListener((int a) =>
                {
                    Input_PatientId.text = userListDropdown.captionText.text;
                });
                
                Input_PatientId.text = d.data[0].name + "  " + d.data[0].age + "   " + d.data[0].userId;

            }
         
        }
        catch (Exception)
        {
            TipsPage_1.tipsPage.OpenThisPage("获取数据失败！");
            Debug.Log("获取数据失败");
            UserData = null;
            return;
        }



    }
    /// <summary>
    /// 多用户登入处理
    /// </summary>
    public void MultiuserPprocessing()
    {
        KeyboardManager.key.HideKeyboard();

        if (isTrue)
        {
            TipsPage_1.tipsPage.OpenThisPage("无此用户信息！");
            return;
        }
        //PostData_3 data1 = new PostData_3();//名字
        //PostData_3_1 data = new PostData_3_1();//手机号
        if (isNum)
        {
            PostData_3_1 data_2 = new PostData_3_1();//手机号
            data_2.phone = Input_PatientId.text;
            string TemoJson = DataUpload.loadData.ObjectToJson(data_2);
           
            PostData_6_1 data1 = new PostData_6_1();
            data1.phone = data_2.phone;
            data1.deviceNumber = UserInfoData.EquipmentName;
            string TemoJson1 = DataUpload.loadData.ObjectToJson(data1);
            UserData = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/operate/getuserinformation", TemoJson);
            UserDataState = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/operate/ifprescriptioncontent", TemoJson1);
           
            UserData5 d1 = JsonUtility.FromJson<UserData5>(UserData);
            if (d1.code=="201")
            {
                TipsPage_1.tipsPage.OpenThisPage("无此用户信息！");
                return;
            }
            JsonUtility.FromJsonOverwrite(UserData, d1);
            data[0] = d1.data.name;
            data[1] = d1.data.userId;
            data[2] = d1.data.age;
            data[3] = d1.data.sex;
            data[4] = d1.data.phone;
        }
        else
        {
            foreach (var item in tempDataDic)
            {
                if (item.Key == Input_PatientId.text)
                {
                    data[0] = item.Value[0];
                    data[1] = item.Value[1];
                    data[2] = item.Value[2];
                    data[3] = item.Value[3];
                    data[4] = item.Value[4];

                }
            }

            PostData_6 data1 = new PostData_6();
            data1.userId = data[1];
            data1.deviceNumber = UserInfoData.EquipmentName;
            string TemoJson1 = DataUpload.loadData.ObjectToJson(data1);
            UserDataState = DataUpload.loadData.Post("http://" + UserInfoData.Ip + "/operate/ifprescriptioncontent", TemoJson1);
            
        }


        UserData3 d = JsonUtility.FromJson<UserData3>(UserDataState);
        JsonUtility.FromJsonOverwrite(UserDataState, d);
        Debug.Log(UserDataState);


        if (d.data == null)
        {
            TipsPage_1.tipsPage.OpenThisPage("获取数据失败！");
            return;
        }
        if (!d.data.hasPrescription)
        {
            TipsPage_1.tipsPage.OpenThisPage("您没有生成检查或干预方案！");
            return;
        }
        if (!d.data.paid)
        {
            TipsPage_1.tipsPage.OpenThisPage("您没有没有缴费！");
            return;
        }
        if (!d.data.deviceCorrect)
        {
            TipsPage_1.tipsPage.OpenThisPage("设备未正确！");
            return;
        }




        DateTime dateTime = DateTime.Now;
        UserInfoData.gears = d.data.gear;
        dataList.Clear();

        if (OperateUseSQL_H.IsTbleExist("UserInfo"))
        {
            if (!OperateUseSQL_H.IsHavaData(data[4], "UserInfo"))
            {

                dataList.Add(data);
                OperateUseSQL_H.Add_Data(TableName, colName, dataList);
            }
        }
        else
        {

            dataList.Add(data);
            OperateUseSQL_H.Add_Data(TableName, colName, dataList);
        }
        foreach (var item in UserInfoData.UserDataDic)
        {
            string[] tempArr = item.Value;
            if (tempArr[4].Equals(data[4]))
            {
                data[1] = tempArr[1];
            }
        }
        if (d.data.trainingType == 0)
        {
            UserPanel.isCheckModel = true;
        }
        else
        {
            UserPanel.isCheckModel = false;
        }
        TipsPage_1.tipsPage.OpenThisPage(d.data.value+"!");
        UserInfoData.UserId = data[1];
        IsScanCode = true;
        isTrue = false;
        MessageCenter.SendMsg("OpenGame", true);
        Input_PatientId.text = "";
        this.gameObject.SetActive(false);

    }

    public void AnalysisTrainString(string json)
    {
        TrainData d = JsonUtility.FromJson<TrainData>(json);
        JsonUtility.FromJsonOverwrite(json, d);
        if (d.data != null)
        {
            TrainTime = d.data[0].movementTime;
            DifficultyLevel = d.data[0].difficultyLevel;
        }
    }

    private void OnBackButtonClick()
    {
        this.gameObject.SetActive(false);
    }

    private void SpecificUserLogin()
    {
        DateTime dateTime = DateTime.Now;
        dataList.Clear();
        data[0] = "#001";
        data[1] = "007";
        data[2] = "99";
        data[3] = "男";
        data[4] = "123456789";
        if (OperateUseSQL_H.IsTbleExist("UserInfo"))
        {
            if (!OperateUseSQL_H.IsHavaData(data[4], "UserInfo"))
            {

                dataList.Add(data);
                OperateUseSQL_H.Add_Data(TableName, colName, dataList);
            }
        }
        else
        {

            dataList.Add(data);
            OperateUseSQL_H.Add_Data(TableName, colName, dataList);
        }
        foreach (var item in UserInfoData.UserDataDic)
        {
            string[] tempArr = item.Value;
            if (tempArr[4].Equals(data[4]))
            {
                data[1] = tempArr[1];
            }
        }
        TipsPage_1.tipsPage.OpenThisPage("登录成功！");
        UserInfoData.UserId = data[1];
        //IsScanCode = true;

        MessageCenter.SendMsg("UserPanel", true);
        Input_PatientId.text = "";
        this.gameObject.SetActive(false);

    }
    private void OnFindButtonClick()
    {
        FindUserInfoData(Input_PatientId.text);
    }
    [DllImport("wininet")]
    private extern static bool InternetGetConnectedState(ref int connectionDescription, int reservedValue);

    public bool IsConnectedInternet()
    {
        int i = 0;
        if (InternetGetConnectedState(ref i, 0))
        {
            Debug.Log("已联网");
            return true;
        }
        else
        {
            Debug.Log("未联网");
            NetworkHintPanle.tipsPage.OpenHintPanel("请检查网络,是否使用特定用户！");
            return false;
        }
    }

}

