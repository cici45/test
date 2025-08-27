using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCarControlCheck : MonoBehaviour
{
    public static Transform[] bgImages;
    public  RectTransform playerCar;
    public static Transform arrowImage, pointer, targetPos;
    public  Text sumText, remainderText,
        completionscountText, currentAngleText, tipsText;
    public static Image progressBar02;
    public static Transform itemParent;//金币父物体
    public static Transform grade_Text;
    public static Action_Prompt action_Prompt_man;
    public static LegAngle man;//人物模型
    //public static Transform itemParent;//障碍物父物体s
    public static GameObject itemBarrier;//障碍物预制体

    public Transform leftArrows;
    public Transform RightArrows;
    public static bool isPlay;

    public static float maxValue;
    public static float currentAngle;
    private float coefficient;//系数
    string unit;//单位
    bool gameisplay = false;

    bool stretch = true;//true：伸展、false：屈收

    float currentPoint = 0;//指针初始偏移值
    float currentCarPos = 0;//飞机初始偏移值
    public static int goldNumber = 151;//金币总量
    public static int completionIndex = 0;//完成数量
    public static int goldSum;

    float reality_coefficient;//系数

    public static Sprite[] gameBg;
    public static int difficulty;//难度

    List<string[]> Traindata = new List<string[]>();//训练方案数据
    int row;//行数
    int TrainTimes;//训练时间（分）
    int difficultyscale = 0;//难度等级
    Model_Airplane model_Airplane = new Model_Airplane();//训练模式
    List<Vector3> arr = new List<Vector3>();//位置集合
    int num;//计算出来的数量
    int goldnumber=50;//总数量
    int completionindex;//剩余数量
    float currentpoint;//当前角度
    double time_interval;//生成时间间隔
    float allTime = 0;//总时间（秒）
    float motionTime = 0;//休息时间

    float intervaltime = 50;//生成间隔
    int currentgoldnum = 0;
    int arrindex = 0;
    bool isplaying = true;
    public static int goldsum;//分数
    public static bool upOrDown;
    public static GameObject tarGetActionImage;
    public static Text currentGearPositionTip;//档位
    public float testAngle;
    private bool hasReached60;

    public Transform pointerOut;
    public Text angle;


    private void Awake()
    {
        #region 组件获取
        bgImages = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();

        playerCar = transform.Find("GameControl/PlayerCar").GetComponent<RectTransform>();
        arrowImage = transform.Find("GameControl/PlayerCar/Arrow").GetComponent<RectTransform>();
        pointer = transform.Find("GameControl/Image/Pointer").GetComponent<RectTransform>();
        targetPos = transform.Find("GameControl/Image/targetPos").GetComponent<RectTransform>();
        itemParent = transform.Find("GameControl/EnemySpawn");
        sumText = transform.Find("GameControl/SumText").GetComponent<Text>();
        remainderText = transform.Find("GameControl/RemainderText").GetComponent<Text>();
        completionscountText = transform.Find("GameControl/CompletionscountText").GetComponent<Text>();
        currentAngleText = transform.Find("GameControl/Image/currentangle").GetComponent<Text>();
       // tipsText = transform.Find("GameControl/TipsText").GetComponent<Text>();
        progressBar02 = transform.Find("GameControl/Image/progressBar02").GetComponent<Image>();
        grade_Text = transform.Find("GameControl/Grade_Text");
        tarGetActionImage = transform.Find("GameControl/TargetAction").gameObject;

        man = transform.Find("GameControl/Rherapist").GetComponent<LegAngle>();
        //action_Prompt_man = transform.Find("GameControl/Man-ActionModel").GetComponent<Action_Prompt>();
        #endregion


    }

    private void OnEnable()
    {
        MessageCenter.AddMsgListener("RipIntoGameCheckStart", GameStart);
        MessageCenter.AddMsgListener("ContinueOrStopGame", GameContinue);
        MessageCenter.AddMsgListener("StopGame", GameStop);

    }
  
    private void OnDisable()
    {
        StopAllCoroutines();


    }

    private void GameStop(ParameterData pa)
    {
        isplaying = !(bool)pa.data;
        Debug.Log("暂停游戏：2");
        if (isplaying)
        {

            DOTween.TogglePauseAll();

        }
        else
        {
            man.gameObject.SetActive(false);
            DOTween.TogglePauseAll();

        }
    }

    private void GameContinue(ParameterData pa)
    {
        isPlay = !(bool)pa.data;
        isplaying = !(bool)pa.data;
        Debug.Log("暂停游戏：1");

    }


    private void GameStart(ParameterData pa)
    {
        gameisplay = (bool)pa.data;
        isPlay = (bool)pa.data;
        tipsText.text = "手部请往前，再往后。尽自己最大努力完成！";
        if (gameisplay)
        {
            currentAngleText.text = 0.ToString();
           
            completionscountText.text = "完成数量：" + completionIndex.ToString();
           
        }
        else
        {
            DOTween.TogglePauseAll();

            MessageCenter.RemoveMsgListener("RipIntoGameCheckStart", GameStart);
            MessageCenter.RemoveMsgListener("ContinueOrStopGame", GameContinue);
            MessageCenter.RemoveMsgListener("StopGame", GameStop);
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame  870  220
    void Update()
    {
       GetAngle();
    }

    /// <summary>
    /// 码盘显示
    /// </summary>

    private void CodedDiscShow()
    {
        if (UserInfoData.CurrentAngle <= 60)
        {
            float bar = (currentAngle / 60);
            progressBar02.fillAmount = 0.65f - (0.65f * bar);
            pointerOut.localEulerAngles = new Vector3(0,0,currentAngle);
            angle.text = currentAngle.ToString("f2") + "°";
        }
    }

    void GetAngle()
    {
       // isPlay = true;
       // Debug.Log(isPlay+"游戏状态");
        if (isPlay)
        {
           
            currentAngle = UserInfoData.CurrentAngle;
            //currentAngle = testAngle;
            Debug.Log("当前角度"+currentAngle);
            if (currentAngle<60&& currentAngle >= 0)
            {
                playerCar.anchoredPosition = new Vector3(-340, 240 + currentAngle * 10, 0);
                CodedDiscShow();
            }
            // 角度达到或超过60度
            if (currentAngle >= 60*0.9)
            {
                hasReached60 = true;
                playerCar.Find("Arrow").localEulerAngles = new Vector3(0, 0, -90);
            }

            // 角度回到0度，且之前达到过60度 -> 完成一次动作
            if (currentAngle <= 5 && hasReached60)
            {
                CompleteAction();

            }
        }
    }
    void CompleteAction()
    {
        playerCar.Find("Arrow").localEulerAngles = new Vector3(0, 0, 90);
       
        hasReached60 = false;
        PlanPlayer.gamegrade += 0.5f;
        completionIndex++;
        completionscountText.text = "完成次数：" + completionIndex.ToString();
        
    }
}