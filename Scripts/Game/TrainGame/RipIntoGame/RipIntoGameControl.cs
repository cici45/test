using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RipIntoGameControl : MonoBehaviour
{
    public static Transform[] bgImages;
    public static RectTransform playerCar;
    public static Transform arrowImage, pointer, targetPos;
    public static Text sumText, remainderText, 
        completionscountText, currentAngleText, tipsText;
    public static Image progressBar02;
    public static Transform itemParent;//��Ҹ�����
    public static Transform grade_Text;
    public static Action_Prompt action_Prompt_man;
    public static LegAngle man;//����ģ��
    //public static Transform itemParent;//�ϰ��︸����
    public static GameObject itemBarrier;//�ϰ���Ԥ����
    
    public static bool isPlay;

    public static float maxValue;
    public static float currentAngle;
    private  float coefficient;//ϵ��
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
    int goldnumber;//������
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
        tipsText = transform.Find("GameControl/TipsText").GetComponent<Text>();
        progressBar02 = transform.Find("GameControl/Image/progressBar02").GetComponent<Image>();
        grade_Text = transform.Find("GameControl/Grade_Text");
        tarGetActionImage = transform.Find("GameControl/TargetAction").gameObject;

        man = transform.Find("GameControl/Rherapist").GetComponent<LegAngle>();
        //action_Prompt_man = transform.Find("GameControl/Man-ActionModel").GetComponent<Action_Prompt>();
        #endregion
        gameBg = Resources.LoadAll<Sprite>("������Ϸ/����");
        itemBarrier= Resources.Load<GameObject>("Prefab/Barrier");//-300   20   340

       
    }

    private void OnEnable()
    {
        MessageCenter.AddMsgListener("RipIntoGameStart", GameStart);
        MessageCenter.AddMsgListener("ContinueOrStopGame", GameContinue);
        MessageCenter.AddMsgListener("StopGame", GameStop);
        gameObject.SetActive(true);
        playerCar.anchoredPosition = new Vector2(-340, 240);
        arrowImage.GetComponent<Image>().DOFade(0.6f, 1).SetEase(Ease.Linear).OnComplete(ArrowFlicker);
        upOrDown = true;
        IsCheakModel();
        //isPlay = true;
    }
    private void IsCheakModel()
    {
        if (UserPanel.isCheckModel)
        {
           man.gameObject.SetActive(false);
           tipsText.gameObject.SetActive(false);
           arrowImage.GetComponent<Image>().enabled=false;
        }
        else
        {

        }



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
            man.gameObject.SetActive(true);
            tarGetActionImage.SetActive(true);
            //action_Prompt_man.gameObject.SetActive(true);
            DOTween.TogglePauseAll();
            //StartCoroutine("GoldCreate");
            //StartCoroutine("Rest_Time");
        }
        else
        {
            man.gameObject.SetActive(false);
            DOTween.TogglePauseAll();
            //StopAllCoroutines();
        }
    }

    private void GameContinue(ParameterData pa)
    {
        isPlay = !(bool)pa.data;
        isplaying = !(bool)pa.data;
        Debug.Log("��ͣ��Ϸ��1");
        if (isPlay)
        {
            //man.gameObject.SetActive(true);
            //DOTween.TogglePauseAll();
            //StartCoroutine("GoldCreate");
            //StartCoroutine("Rest_Time");
        }
        else
        {
            //DOTween.TogglePauseAll();
            //StopAllCoroutines();
        }
    }

    
    private void GameStart(ParameterData pa)
    {
        gameisplay = (bool)pa.data;
        isPlay = (bool)pa.data;
        
        if (gameisplay)
        {
            if (Traindata != null && Traindata.Count > 0)
            {
                Traindata.Clear();
            }
            OperateUseSQL_H.Read_Data("Temporary_TrainMode", out Traindata, out row);
            if (Traindata != null && Traindata.Count > 0)
            {
                maxValue = int.Parse(Traindata[0][3]);
                TrainTimes = int.Parse(Traindata[0][1]);
                reality_coefficient = maxValue / 60;
                difficulty = int.Parse(Traindata[0][2]);
                targetPos.localEulerAngles = new Vector3(0, 0, maxValue);
                arr.Clear();
                num = 0;
                time_interval = 0;
                allTime = (TrainTimes * 55);
                model_Airplane.Input_Ini(allTime - 40, 1, RestPanel.MotionTime, RestPanel.RestTime, difficulty, 0, 0, -400, 240, 0, out arr, out num, out time_interval);

            }
            if (itemParent.childCount > 0)
            {
                for (int index = 0; index < itemParent.childCount; index++)
                {
                    Destroy(itemParent.GetChild(index).gameObject);
                }
            }
            motionTime = RestPanel.MotionTime;
            stages = 1;
            man.InitAngle();

            TargetActionControl.Initialize();
            //action_Prompt_man.InitAngle();

            //action_Prompt_man.gameObject.SetActive(false);

            //gameObject.SetActive(true);

            BarrierControl.goldmovespeed = 0;
            
            AudioPlayer.instantiate.PlayerTherapeutistAudio("��ӭ");
            
            progressBar02.fillAmount = 0.65f;

            arrowImage.gameObject.SetActive(false);
            intervaltime = 0;

            PlanPlayer.gamegrade = 0;
            completionIndex = 0;
            currentgoldnum = 0;
            currentAngleText.text = 0.ToString();
            arrindex = 0;
            isplaying = true;
            if (difficulty == 10)
            {
                goldnumber = num;
                time_interval = (float)time_interval +5f;
            }
            else if (difficulty == 7)
            {
                goldnumber = num;
                time_interval = (float)time_interval + 4f;
            }
            else if (difficulty == 5)
            {
                goldnumber = num + (TrainTimes * 7);
                time_interval = (float)time_interval + 3f;
            }
            isplaying = true;
            //sumText, remainderText, completionscountText, currentAngleText;
            goldsum = goldnumber;
            currentpoint = 0;
            pointer.localEulerAngles = new Vector3(0, 0, 0);//max 140  min-40
            sumText.text = "��������" + (goldnumber).ToString();
            remainderText.text = "ʣ��������" + (goldnumber).ToString();
            completionscountText.text = "���������" + completionIndex.ToString();
            PlanMangenter.goldsum = goldsum;
            PlanPlayer.gamegrade = 0;
            StartCoroutine("GoldCreate");
            StartCoroutine("Rest_Time");
            StartCoroutine("Moving");

        }
        else
        {
            DOTween.TogglePauseAll();

            //if (gameObject!=null)
            //{
            //    
            //}

            //for (int index = 0; index < itemParent.childCount; index++)
            //{
            //    Destroy(itemParent.GetChild(index).gameObject);
            //}
            MessageCenter.RemoveMsgListener("RipIntoGameStart", GameStart);
            MessageCenter.RemoveMsgListener("ContinueOrStopGame", GameContinue);
            MessageCenter.RemoveMsgListener("StopGame", GameStop);
            Destroy(gameObject);
        }
    }


    void StartIEnumerator()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame  870  220
    void Update()
    {
        if (isPlay && gameisplay)
        {
            GetAngle();
        }

    }


    #region �������
    
    /// <summary>
    /// �������
    /// </summary>
    /// <returns></returns>
    IEnumerator GoldCreate()
    {
        while (gameisplay)
        {
            yield return new WaitForFixedUpdate();
            #region �ɵ�
            if (intervaltime > 0 && isplaying)
            {
                intervaltime -= 0.02f;
            }
            if (goldnumber > 0 && intervaltime <= 0)
            {
                intervaltime = (float)time_interval;
                if (currentgoldnum == 0)
                {

                    GameObject item = Instantiate(itemBarrier, itemParent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
                    item.name = goldnumber.ToString();
                    item.SetActive(true);
                    GameObject item01 = Instantiate(itemBarrier, itemParent);
                    item01.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
                    item01.SetActive(true);
                }
                else if (currentgoldnum == 1)
                {
                    GameObject item = Instantiate(itemBarrier, itemParent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 340);
                    item.SetActive(true);
                    GameObject item01 = Instantiate(itemBarrier, itemParent);
                    item01.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
                    item01.SetActive(true);
                    currentgoldnum = -1;
                }
                
                currentgoldnum++;
                goldnumber -= 1;
                remainderText.text = "ʣ��������" + goldnumber.ToString();
            }
            #endregion

            #region �㷨��
            //if (intervaltime > 0)
            //    {
            //        intervaltime -= 0.02f;
            //    }
            //    else
            //    {
            //        intervaltime = (float)time_interval;
            //        if (arr.Count > 0 && arr != null&& arrindex< arr.Count)
            //        {
            //            if (arr[arrindex].z != 1)
            //            {
            //                if (arr[arrindex].y == 1 && isPlay)
            //                {
            //                    isplaying = true;
            //                    GameObject item = Instantiate(enemyPrefab, goldparent);
            //                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, arr[arrindex].x);
            //                    item.name = (goldnumber- currentgoldnum).ToString();
            //                    item.SetActive(true);
            //                    currentgoldnum++;
            //                    remainder_Text.text = "ʣ��������" + (goldnumber - currentgoldnum).ToString();
            //                 }
            //            }
            //            else if(arr[arrindex].z == 1&& isplaying)
            //            {
            //                Debug.Log("������Ϣʱ��");
            //                isplaying = false;
            //                MessageCenter.SendMsg("ContinueOrStopGame",false);
            //                MessageCenter.SendMsg("RestPanel",true);
            //                man.gameObject.SetActive(false);
            //            }
            //            arrindex++;
            //        }
            //    }
            #endregion
        }
    }
    #endregion


    #region ��ͷ��ɻ���ת
    /// <summary>
    /// ��ʾ��ͷ�л�
    /// </summary>
    /// <param name="isswitchover"></param>
    public static void ArrowSwitchover(bool isswitchover)
    {
        if (isswitchover)
        {
            arrowImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/��ͷ/��");
            tipsText.text = "�ֲ�����ǰ";
            arrowImage.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-175, 0);
            TargetActionControl.RefreshState(1, 0);
        }
        else
        {
            //Debug.Log("�ص�ԭλ");
            arrowImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/��ͷ/��");
           // tipsText.text = "�����" + "\n" + "˫�ֻص�ԭλ";
            tipsText.text = "�ֲ������";
            arrowImage.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(175, 0);
            TargetActionControl.RefreshState(0, 1);
        }
        arrowshowtime = 0;
    }

    void ArrowFlicker()
    {
        arrowImage.GetComponent<Image>().DOFade(1f, 1).SetEase(Ease.Linear).OnComplete(ArrowFlicker01);
    }

    void ArrowFlicker01()
    {
        arrowImage.GetComponent<Image>().DOFade(0.6f, 1).SetEase(Ease.Linear).OnComplete(ArrowFlicker);
    }

    public static int stages = 1;
    void GameBgChange()
    {
        float sumGrade = goldsum * 0.5f;
        //Debug.Log("��ǰ�÷֣�"+PlanPlayer.gamegrade+"                     "+ sumGrade / 4);
        if (PlanPlayer.gamegrade > sumGrade / 4 && stages == 1)
        {
            stages++;
            //Debug.Log("�ڶ���");
        }
        else if (PlanPlayer.gamegrade > (sumGrade / 4) + (sumGrade / 4) && stages == 2)
        {
            stages++;
        }
        else if (PlanPlayer.gamegrade > (sumGrade / 4) + (sumGrade / 4) + (sumGrade / 4) && stages == 3)
        {
            stages++;
        }
    }
    #endregion

    /// <summary>
    /// ��Ϣʱ��
    /// </summary>
    /// <returns></returns>
    IEnumerator Rest_Time()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (motionTime > 0 && isplaying)
            {
                motionTime -= 0.02f;
            }
            else if (isplaying && motionTime <= 0 && allTime > RestPanel.RestTime + 5)
            {
                Debug.Log("������Ϣʱ��");
                motionTime = RestPanel.MotionTime;
                MessageCenter.SendMsg("StopGame", true);
                man.gameObject.SetActive(false);
                tarGetActionImage.SetActive(false);
                //action_Prompt_man.gameObject.SetActive(false);
                MessageCenter.SendMsg("RestPanel", true);
            }
        }
    }
    static float arrowshowtime;
    private void FixedUpdate()
    {
        allTime -= 0.02f;
        if (arrowshowtime < ScoketServer.arrowshow_Time && isplaying)
        {
            arrowshowtime += 0.02f;
        }
        else
        {
            arrowImage.gameObject.SetActive(false);
        }
    }

    IEnumerator Moving()
    {
        while (true)
        {
            if (isPlay&&gameisplay&&isplaying)
            {
                if (bgImages[1].localPosition.x < -1920)
                {
                    bgImages[1].localPosition = new Vector3(bgImages[3].localPosition.x + 1920, 0, 0);
                    
                }
                if (bgImages[2].localPosition.x < -1920)
                {
                    bgImages[2].localPosition = new Vector3(bgImages[1].localPosition.x + 1920, 0, 0);
                    

                }
                if (bgImages[3].localPosition.x < -1920)
                {
                    bgImages[3].localPosition = new Vector3(bgImages[2].localPosition.x + 1920, 0, 0);
                    

                }
                for (int i = 1; i < bgImages.Length; i++)
                {
                    bgImages[i].localPosition = Vector3.Lerp(bgImages[i].localPosition, bgImages[i].localPosition - Vector3.right * 300, 1 * Time.deltaTime);
                }
                //Debug.Log("����λ�ã�"+bgImages[0].localPosition.x+"\n"+ bgImages[1].localPosition.x + "\n" + bgImages[2].localPosition.x);
            }
            yield return null;
        }
    }


    void GetAngle()
    {
        if (isPlay)
        {
            //Debug.Log(UserInfoData.CurrentAngle);
            try 
            {
             currentAngle = Mathf.Abs(UserInfoData.CurrentAngle);
            // currentAngle = testAngle; //���Դ���
             coefficient= currentAngle / maxValue;
             coefficient = coefficient <= 0 ? 0 : coefficient;
             coefficient = coefficient >= 1 ? 1 : coefficient;
             man.ModeAngleUpdate(coefficient);
             Debug.Log("��ǰ�Ƕ�ֵ��" + currentAngle);
            }
            catch (Exception e) { Debug.Log(e); }
            #region ����
            //if (currentAngle < 60 && !isj)
            //{
            //    currentAngle += 0.5f;
            //    //Debug.Log("ֵ��" + currentAngle);
            //}
            //else if (currentAngle > 0)
            //{
            //    currentAngle -= 0.5f;
            //    isj = true;
            //}
            //else if (currentAngle <= 0 && isj)
            //{
            //    isj = false;
            //}
            #endregion
            if (currentAngle <= 60 && currentAngle > 0)
            {
               
                float angle = 60 * (currentAngle / 60);
               // man.SetModelAngle(angle);
               
                pointer.localEulerAngles = new Vector3(0, 0, currentAngle);//max 140  min-40
                float plane_y = playerCar.anchoredPosition.y + ((870 * (currentAngle / (60 * reality_coefficient))) - currentCarPos);
                //PlaneSpin(Plane.anchoredPosition.y, plane_y);
                if (plane_y < 870 && plane_y > 220)
                {
                    playerCar.anchoredPosition = new Vector2(playerCar.anchoredPosition.x, plane_y);
                }
                else if (plane_y < 220)
                {
                    playerCar.anchoredPosition = new Vector2(playerCar.anchoredPosition.x, 220);
                }
                else if (plane_y > 860)
                {
                    playerCar.anchoredPosition = new Vector2(playerCar.anchoredPosition.x, 860);
                }
                //target_plan.position = new Vector3(target_plan.position.x, playerCar.position.y, target_plan.position.z);
                float bar = (currentAngle / 60);
                progressBar02.fillAmount = 0.65f - (0.65f * bar);
                //Debug.Log("��������ֵ��"+ (0.65f * bar));
                currentpoint = currentAngle;
                currentAngleText.text = currentpoint.ToString("f2") + "��";
                currentCarPos = 870 * (currentAngle / (60 * reality_coefficient));
            }
        }
    }
}
