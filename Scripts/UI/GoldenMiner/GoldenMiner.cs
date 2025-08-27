using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldenMiner : MonoBehaviour
{
    public static float timer;//间隔时间
    public static float sportTime;//保持时间
    public static int goldTotal;
    public static bool isPause;

    private float CurrentAngle;//传感器数值
    private GameObject Up;
    private GameObject Down;
    GameObject gold;
    Transform goldImage;
    Transform Handle;
    float schedule;
    Slider Grapple;
    Slider slider;

    private void Awake()
    {
        MessageCenter.AddMsgListener("GoldenMinerGame", openGoldenMiner);
        MessageCenter.AddMsgListener("ContinueOrStopGame", ContinueOrStopGame);
    }

    private void OnEnable()
    {
        isPause= false;
    }

    void Start()
    {
        goldImage = transform.Find("goldImage");
        Handle = transform.Find("worker/Grapple/Handle Slide Area/Handle");
        gold = transform.Find("gold").gameObject; 
        Grapple = transform.Find("worker/Grapple").GetComponent<Slider>();
        slider = transform.Find("worker/Slider").GetComponent<Slider>();
        Up = transform.Find("worker/up").gameObject;
        Down = transform.Find("worker/down").gameObject;
        gold.SetActive(false);
    }
    /// <summary>
    /// 游戏暂停方法
    /// </summary>
    /// <param name="pa">true暂停，false继续</param>
    private void ContinueOrStopGame(ParameterData pa)
    {
        bool value = (bool)pa.data;
        if (value == false)
        {
            isPause = value;
        }
        else
        {
            isPause = value;
        }
    }
    /// <summary>
    /// 打开地图方法
    /// </summary>
    /// <param name="pa">true打开，false关闭</param>
    private void openGoldenMiner(ParameterData pa)
    {
        bool value = (bool)pa.data;
        if (!value)
        {
            MathRSA.Instance.GameOverCallback();
            CancelInvoke("getData");
            CancelInvoke("LoadGameGold");
        }
        else
        {
            MathRSA.Instance.IsotonicOrEquilongFunc();
            GetTableGoldenMiner();
        }
        gameObject.SetActive(value);
    }


    /// <summary>
    /// 获取游戏模型
    /// </summary>
    private void GetTableGoldenMiner()
    {
        string TableName = "Temporary_GeneralTab_TrainMode";
        List<string[]> data = new List<string[]>();//接收读取到的数据
        int n = 0;
        OperateUseSQL_H.Read_Data(TableName, out data, out n);
        if (data != null && data.Count > 0)
        {
            Debug.Log("data[0][2] ; " + data[0][2]);
            timer = float.Parse(data[0][0]) + float.Parse(data[0][1]);
            sportTime = float.Parse(data[0][0]);
            schedule = float.Parse(data[0][2]) - float.Parse(data[0][3]);
            goldTotal = int.Parse(data[0][4]);
            InvokeRepeating("getData", 0, 0.05f);
            InvokeRepeating("LoadGameGold", 0, timer);
        }
    }

    /// <summary>
    /// 获取传感器角度方法
    /// </summary>
    private void getData()
    {
        CurrentAngle = UserInfoData.CurrentAngle;
        if (Handle.childCount > 0)
        {
            if (Grapple.value < 0.1f)
            {
                foreach (Transform item in Handle)
                {
                    Destroy(item.gameObject);
                }
                slider.value = 0;
                MathRSA.Instance.Scores(true);
            }
            Up.SetActive(true);
            Down.SetActive(false);
        }
        else
        {
            Up.SetActive(false);
            Down.SetActive(true);
        }
    }

    void Update()
    {
        if (!isPause && CurrentAngle >= 0)
        {
            Grapple.value = Mathf.Lerp(Grapple.value, CurrentAngle / schedule, 0.1f);
        }
        else
        {
            Grapple.value = 0;
        }
    }

   void LoadGameGold()
    {
        GameObject obj = Instantiate<GameObject>(gold);
        obj.transform.SetParent(goldImage);
        obj.transform.localPosition= Vector3.zero;
        obj.transform.localScale= Vector3.one;
        obj.SetActive(true);
        slider.value = 0;
    }

    private void OnDisable()
    {
        Up.SetActive(false);
        Down.SetActive(true);
        if (goldImage.childCount>0)
        {
            foreach (Transform item in goldImage)
            {
                Destroy(item.gameObject);
            }
        }
    }

}
