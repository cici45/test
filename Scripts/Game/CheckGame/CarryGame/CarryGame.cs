using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarryGame : MonoBehaviour
{
    public static Transform[] bgImages;
    public RectTransform playerCar;
    public static Transform arrowImage, pointer, targetPos;
    public Text sumText, remainderText,
        completionscountText, currentAngleText, tipsText;
    public static Image progressBar02;
    private Image dynamicImage;
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

    bool gameisplay = false;
    int index;

    public static int goldNumber = 151;//�������
    public static int completionIndex = 0;//�������
    public static int goldSum;


    public static Sprite[] gameBg;
    public static int difficulty;//�Ѷ�

    bool isplaying = true;
    public static int goldsum;//����
    public static bool upOrDown;
    public static GameObject tarGetActionImage;
    public static Text currentGearPositionTip;//��λ
    public float testAngle;
    private bool hasReached60;

    public Transform pointerOut;
    public Text angle;

    RectTransform pole;

    Sprite[] sprites;

    Vector3 dynamicStartPos;
    Vector3 dynamicEndPos;
    private void Awake()
    {
        #region �����ȡ
        bgImages = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Transform>();

        pointer = transform.Find("Bg/Image/Pointer").GetComponent<RectTransform>();
    
        completionscountText = transform.Find("Bg/CompletionscountText").GetComponent<Text>();
        currentAngleText = transform.Find("Bg/Image/currentangle").GetComponent<Text>();
        // tipsText = transform.Find("GameControl/TipsText").GetComponent<Text>();
        progressBar02 = transform.Find("Bg/Image/progressBar02").GetComponent<Image>();
       // pointerOut= transform.Find("Bg/Image/progressBar02").GetComponent<Image>();
        tipsText = transform.Find("Bg/TipsBg/TipsText").GetComponent<Text>();
        pole = transform.Find("Bg/Climp/Pole").GetComponent<RectTransform>();
        sprites= Resources.LoadAll<Sprite>("CarryGame");
        dynamicImage = transform.Find("Bg/DynamicImage").GetComponent<Image>();
        dynamicStartPos= dynamicImage.transform.localPosition;
        PlanPlayer.gamegrade = 0;
        //action_Prompt_man = transform.Find("GameControl/Man-ActionModel").GetComponent<Action_Prompt>();
        #endregion


    }

    private void OnEnable()
    {
        MessageCenter.AddMsgListener("CarryGameCheckStart", GameStart);
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

            MessageCenter.RemoveMsgListener("CarryGameCheckStart", GameStart);
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
            pointerOut.localEulerAngles = new Vector3(0, 0, currentAngle);
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
           // currentAngle = testAngle;
            Debug.Log("��ǰ�Ƕ�" + currentAngle);
            if (currentAngle < 60 && currentAngle >= 0)
            {
               
                pole.sizeDelta = new Vector2(17, currentAngle*8.23f);
                CodedDiscShow();
            }
            // �Ƕȴﵽ�򳬹�60��
            if (currentAngle >= 60 * 0.9)
            {
                hasReached60 = true;
                pole.Find("Hand/Obj").GetComponent<Image>().sprite=sprites[index];
                pole.Find("Hand/Obj").gameObject.SetActive(true);
                dynamicImage.gameObject.SetActive(false);
                leftArrows.localEulerAngles = new Vector3(0,0,0);
                AudioPlayer.instantiate.PlayerTipsAudio("�Խ���");
                // playerCar.Find("Arrow").localEulerAngles = new Vector3(0, 0, -90);
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
      //  playerCar.Find("Arrow").localEulerAngles = new Vector3(0, 0, 90);

        hasReached60 = false;
        PlanPlayer.gamegrade += 0.5f;
        completionIndex++;
        completionscountText.text = "��ɴ�����" + completionIndex.ToString();
        leftArrows.localEulerAngles = new Vector3(0, 0, 180);
        Image image=  Instantiate(pole.Find("Hand/Obj").GetComponent<Image>(), transform.Find("Bg/StartPos").localPosition, Quaternion.identity);
        image.transform.SetParent(transform.Find("Bg"));
        image.transform.localScale = Vector3.one;
        image.transform.localPosition = transform.Find("Bg/StartPos").localPosition;
        image.transform.DOLocalMove(transform.Find("Bg/EndPos").localPosition,0.5f).OnComplete(()=> 
        {
            image.gameObject.SetActive(false);
            Destroy(image.gameObject);
        });
        index++;
        if (index > 7)
        {
            index = 0;
        }
        dynamicImage.sprite = sprites[index];
        dynamicImage.gameObject.SetActive(true);
        pole.Find("Hand/Obj").gameObject.SetActive(false);
    }
}