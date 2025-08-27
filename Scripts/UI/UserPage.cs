using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class UserPage : MonoBehaviour
{
    Transform User;
    GameObject userItem;
    string TableName = "UserInfo";
    List<string[]> Readdata = new List<string[]>();
    List<string[]> ReadHistorydata = new List<string[]>();
    InputField Input_Search;
    Button Btn_Search;
    int row = 0;

    private void Awake()
    {
        OperateUseSQL_H.OpenSQL();
        UserInfoData.class_ag = new Ag_Class_Test.AG();
        User = transform.Find("Scroll View/Viewport/User");
        Input_Search = transform.Find("Input_Search").GetComponent<InputField>();
        Btn_Search = transform.Find("Btn_Search").GetComponent<Button>();
        userItem = transform.Find("userItem").gameObject;
        Btn_Search.onClick.AddListener(OnSearchButtonClick);
        Input_Search.onValueChanged.AddListener(OnSearchInputValueChanged);
    }


    void OnEnable()
    {
        userItem.SetActive(false);
        InitUserPage();
    }

    private void Start()
    {
        MessageCenter.AddMsgListener("InitUserPage", InitUserInfoPage);
    }

    private void InitUserInfoPage(ParameterData pa)
    {
        if ((bool)pa.data)
            InitUserPage();
    }

    private void OnSearchButtonClick()
    {
        if (!string.IsNullOrEmpty(Input_Search.text))
        {
            Search(User, Input_Search.text);
        }
    }
    private void OnSearchInputValueChanged(string arg0)
    {
        if (string.IsNullOrEmpty(arg0))
            Reset(User);
    }

    //private void Update()
    //{

    //}

    /// <summary>
    /// 初始化用户列表或历史记录列表
    /// </summary>
    void InitUserPage()
    {
        switch (transform.parent.name)
        {
            case "UserPanel":
            case "AddPanel":
                OperateUseSQL_H.Read_Data(TableName, out Readdata, out row);
                InitUserItem(Readdata);
                //MessageCenter.SendMsg("UnDataToggleArrs", true);
                break;
            case "HistoryPanel":
                string[] Historydata = UserInfoData.GetUserInfo();
                if (Historydata == null) return;
                string table = "A" + Historydata[1] + "_TraingList";
                OperateUseSQL_H.Read_Data(table, out ReadHistorydata, out row);
                InitUserItem(ReadHistorydata);
                //MessageCenter.SendMsg("UnDataHistoryToggleArrs", true);
                break;
            case "SettlePanel":
            case "TestSettle":
                string[] data = UserInfoData.GetUserInfo();
                if (data == null) return;
                if (User.childCount > 0)
                {
                    foreach (Transform item in User)
                    {
                        Destroy(item.gameObject);
                    }
                }
                GameObject obj = Instantiate<GameObject>(userItem);
                obj.transform.SetParent(User);
                obj.transform.Find("Toggle").GetComponent<Toggle>().group = User.GetComponent<ToggleGroup>();
                obj.transform.Find("Index").GetComponent<Text>().text = data[1];
                obj.transform.Find("Name").GetComponent<Text>().text = data[0];
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = Vector3.one;
                obj.name = "User_" + 0;
                obj.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 初始化用户或历史记录单条数据
    /// </summary>
    /// <param name="_dataList"></param>
    void InitUserItem(List<string[]> _dataList)
    {
        if (User.childCount > 0)
        {
            foreach (Transform item in User)
            {
                Destroy(item.gameObject);
            }
        }
        if (_dataList == null) return;
        _dataList.Reverse();
        for (int i = 0; i < _dataList.Count; i++)
        {
            string[] data = _dataList[i];
            GameObject obj = Instantiate<GameObject>(userItem);
            obj.transform.SetParent(User);
            obj.transform.Find("Toggle").GetComponent<Toggle>().group = User.GetComponent<ToggleGroup>();
            if (transform.parent.name == "HistoryPanel")
            {
                obj.transform.GetChild(2).GetComponent<Text>().text = i.ToString();
                obj.transform.GetChild(3).GetComponent<Text>().text = data[2];
                obj.transform.GetChild(4).GetComponent<Text>().text = data[3];
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = Vector3.one;
                obj.name = i.ToString();
            }
            else
            {
                obj.transform.Find("Index").GetComponent<Text>().text = data[1];
                obj.transform.Find("Name").GetComponent<Text>().text = data[0];
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = Vector3.one;
                obj.name = data[1];
            }
            obj.SetActive(true);
            if (transform.parent.name == "UserPanel")
            {
                if (!UserInfoData.UserDataDic.ContainsKey(obj.name))
                    UserInfoData.UserDataDic.Add(obj.name, data);
                else
                    UserInfoData.UserDataDic[obj.name] = data;
            }
            if (transform.parent.name == "HistoryPanel")
            {
                if (!UserInfoData.HistoryDataDic.ContainsKey(obj.name))
                    UserInfoData.HistoryDataDic.Add(obj.name, data);
                else
                    UserInfoData.HistoryDataDic[obj.name] = data;
            }
        }
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="parent">父物体</param>
    /// <param name="key">关键字</param>
    public void Search(Transform parent, string key)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (transform.parent.name == "HistoryPanel")
            {
                if (parent.GetChild(i).name.Contains(key) || parent.GetChild(i).GetChild(2).GetComponent<Text>().text.Contains(key) || parent.GetChild(i).GetChild(3).GetComponent<Text>().text.Contains(key))
                {
                    parent.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    parent.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                if (parent.GetChild(i).name.Contains(key) || parent.GetChild(i).GetChild(2).GetComponent<Text>().text.Contains(key))
                {
                    parent.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    parent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 还原
    /// </summary>
    /// <param name="parent">父物体</param>
    public void Reset(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(true);
        }
    }


    private void OnDisable()
    {
        InitUserPage();
        if (Readdata != null)
            Readdata.Clear();
    }
}
