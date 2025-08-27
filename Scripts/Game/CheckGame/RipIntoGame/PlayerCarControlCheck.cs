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
    public static Transform itemParent;//��Ҹ�����
    public static Transform grade_Text;
    public static Action_Prompt action_Prompt_man;
    public static LegAngle man;//����ģ��
    //public static Transform itemParent;//�ϰ��︸����s
    public static GameObject itemBarrier;//�ϰ���Ԥ����

    public Transform leftArrows;
    public Transform RightArrows;
    public static bool isPlay;

    public static float maxValue;
    public static float currentAngle;
    private float coefficient;//ϵ��
    string unit;//��λ
    bool gameisplay = false;

    bool stretch = true;//true����չ��false������

    float currentPoint = 0;//ָ���ʼƫ��ֵ
    float currentCarPos = 0;//�ɻ���ʼƫ��ֵ
    public static int goldNumber = 151;//�������
    public static int completionIndex = 0;//�������
    public static int goldSum;

    float reality_coefficient;//ϵ��

    public static Sprite[] gameBg;
    public static int difficulty;//�Ѷ�

    List<string[]> Traindata = new List<string[]>();//ѵ����������
    int row;//����
    int TrainTimes;//ѵ��ʱ�䣨�֣�
    int difficultyscale = 0;//�Ѷȵȼ�
    Model_Airplane model_Airplane = new Model_Airplane();//ѵ��ģʽ
    List<Vector3> arr = new List<Vector3>();//λ�ü���
    int num;//�������������
    int goldnumber=50;//������
    int completionindex;//ʣ������
    float currentpoint;//��ǰ�Ƕ�
    double time_interval;//����ʱ����
    float allTime = 0;//��ʱ�䣨�룩
    float motionTime = 0;//��Ϣʱ��

    float intervaltime = 50;//���ɼ��
    int currentgoldnum = 0;
    int arrindex = 0;
    bool isplaying = true;
    public static int goldsum;//����
    public static bool upOrDown;
    public static GameObject tarGetActionImage;
    public static Text currentGearPositionTip;//��λ
    public float testAngle;
    private bool hasReached60;

    public Transform pointerOut;
    public Text angle;


    private void Awake()
    {
        #region �����ȡ
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
        Debug.Log("��ͣ��Ϸ��2");
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
        Debug.Log("��ͣ��Ϸ��1");

    }


    private void GameStart(ParameterData pa)
    {
        gameisplay = (bool)pa.data;
        isPlay = (bool)pa.data;
        tipsText.text = "�ֲ�����ǰ�������󡣾��Լ����Ŭ����ɣ�";
        if (gameisplay)
        {
            currentAngleText.text = 0.ToString();
           
            completionscountText.text = "���������" + completionIndex.ToString();
           
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
    /// ������ʾ
    /// </summary>

    private void CodedDiscShow()
    {
        if (UserInfoData.CurrentAngle <= 60)
        {
            float bar = (currentAngle / 60);
            progressBar02.fillAmount = 0.65f - (0.65f * bar);
            pointerOut.localEulerAngles = new Vector3(0,0,currentAngle);
            angle.text = currentAngle.ToString("f2") + "��";
        }
    }

    void GetAngle()
    {
       // isPlay = true;
       // Debug.Log(isPlay+"��Ϸ״̬");
        if (isPlay)
        {
           
            currentAngle = UserInfoData.CurrentAngle;
            //currentAngle = testAngle;
            Debug.Log("��ǰ�Ƕ�"+currentAngle);
            if (currentAngle<60&& currentAngle >= 0)
            {
                playerCar.anchoredPosition = new Vector3(-340, 240 + currentAngle * 10, 0);
                CodedDiscShow();
            }
            // �Ƕȴﵽ�򳬹�60��
            if (currentAngle >= 60*0.9)
            {
                hasReached60 = true;
                playerCar.Find("Arrow").localEulerAngles = new Vector3(0, 0, -90);
            }

            // �ǶȻص�0�ȣ���֮ǰ�ﵽ��60�� -> ���һ�ζ���
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
        completionscountText.text = "��ɴ�����" + completionIndex.ToString();
        
    }
}