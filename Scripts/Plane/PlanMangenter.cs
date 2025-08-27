using DG.Tweening;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XCharts;
using static TMPro.Examples.ObjectSpin;

public class PlanMangenter : MonoBehaviour
{
    public static RectTransform Plane;
    BgMove Bg;
    public static Image arrow;
    Text sum_Text;
    private Text gearsTip;
    Text remainder_Text;
    public static Text completionscount_Text;
    static Text tips_Text;
    Text currentangle_text;
    public static Transform grade_Text;
    LegAngle man;//����ģ��

    public static bool isPlay;

    //string TrainTableName = "Temporary_GeneralTab_TrainMode";
    List<string[]> TrainData = new List<string[]>();

    float intervalTime;//�ⲿ����
    public static float maxValue;
    float minValue;
    public static float currentAngle;
    string unit;//��λ
    bool gameisplay = false;

    bool stretch = true;//true����չ��false������
    public static Sprite[] golds;//��Ҿ���ͼ

    float currentpoint = 0;//ָ���ʼƫ��ֵ
    float currentplanepos = 0;//�ɻ���ʼƫ��ֵ
    public static int goldnumber = 151;//�������
    public static int completionindex = 0;//�������
    public static int goldsum;
    GameObject enemyPrefab;//���Ԥ����
    Transform goldparent;//��Ҹ�����
    Transform pointer;//ָ��
    float reality_coefficient;//ϵ��

    public static Sprite[] gameBg;
    public static int difficulty;

    Image progressBar;
    Image progressBar02;
    Transform targetPos;

    public static Action_Prompt action_Prompt_man;
    public static RectTransform target_plan;
    public static GameObject tarGetActionImage;
    void Awake()
    {
        #region �����ȡ���زļ���
        Plane = transform.Find("Plane").GetComponent<RectTransform>();
        target_plan = transform.Find("Target_Plane").GetComponent<RectTransform>();
        Bg = transform.Find("Bg").GetComponent<BgMove>();
        man = transform.Find("Rherapist").GetComponent<LegAngle>();
        action_Prompt_man = transform.Find("Man-ActionModel").GetComponent<Action_Prompt>();
        goldparent = transform.Find("EnemySpawn");
        enemyPrefab = Resources.Load<GameObject>("Plane/Enemy");
        golds = Resources.LoadAll<Sprite>("Sprite/���");
        pointer = transform.Find("Image/Pointer");
        arrow = transform.Find("Plane/Arrow").GetComponent<Image>();
        sum_Text = transform.Find("SumText").GetComponent<Text>();
        gearsTip = transform.Find("GearsTip").GetComponent<Text>();
        remainder_Text = transform.Find("RemainderText").GetComponent<Text>();
        completionscount_Text = transform.Find("CompletionscountText").GetComponent<Text>();
        tips_Text = transform.Find("TipsText").GetComponent<Text>();
        grade_Text = transform.Find("Grade_Text");
        currentangle_text = transform.Find("Image/currentangle").GetComponent<Text>();
        tarGetActionImage = transform.Find("TargetAction").gameObject;
        #endregion

        progressBar = transform.Find("Image/progressBar").GetComponent<Image>();
        progressBar02 = transform.Find("Image/progressBar02").GetComponent<Image>();
        targetPos = transform.Find("Image/targetPos");
        gameBg = Resources.LoadAll<Sprite>("20230731�ɻ���ս��������");

        Debug.Log("����������"+gameBg.Length);
        reality_coefficient = 1300;
        maxValue = 60;

        gearsTip.text = "��ǰ��λ:"+UserInfoData.gears;
        Debug.Log("�ɻ���"+Plane.position.y);
        Debug.Log("Ŀ�꣺"+target_plan.position.y);
    }
    void OnEnable()
    {
        MessageCenter.AddMsgListener("GameStartOrStop", GameStart);
        MessageCenter.AddMsgListener("ContinueOrStopGame", GameContinue);
        MessageCenter.AddMsgListener("StopGame", GameStop);
        if (stretch)
        {

            Plane.anchoredPosition = new Vector3(-500, 100, 0);
            Debug.Log("λ�ã�" + Plane.anchoredPosition.y);

        }
        else
        {
            Plane.anchoredPosition = new Vector3(-500, 850, 0);
        }
        Bg.setMove(true, 1);
        arrow.DOFade(0.6f, 1).SetEase(Ease.Linear).OnComplete(ArrowFlicker);
    }
    bool isj = false;
    /// <summary>
    /// �Ƕ�λ���޸�
    /// </summary>
    void GetAngle()
    {
        if (isPlay)
        {
            //Debug.Log(UserInfoData.CurrentAngle);
            try { currentAngle = Mathf.Abs(UserInfoData.CurrentAngle); }
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
            if (currentAngle <= 60 && currentAngle>0)
            {
                //Debug.Log("��ǰ��ȡ�Ƕ�ֵ��"+currentAngle);
                float angle = 60 * (currentAngle / 60);
                man.SetModelAngle(angle);
                pointer.localEulerAngles = new Vector3(0, 0, currentAngle);//max 140  min-40
                float plane_y = Plane.anchoredPosition.y + ((740 * (currentAngle / (60 * reality_coefficient))) - currentplanepos);
                //PlaneSpin(Plane.anchoredPosition.y, plane_y);
                if (plane_y < 735 && plane_y > 115)
                {
                    Plane.anchoredPosition = new Vector2(Plane.anchoredPosition.x, plane_y);
                }
                else if (plane_y < 115)
                {
                    Plane.anchoredPosition = new Vector2(Plane.anchoredPosition.x, 130);
                }
                else if (plane_y > 735)
                {
                    Plane.anchoredPosition = new Vector2(Plane.anchoredPosition.x, 740);
                }
                target_plan.position = new Vector3(target_plan.position.x, Plane.position.y, target_plan.position.z);
                float bar = (currentAngle / 60);
                progressBar02.fillAmount = 0.65f-(0.65f * bar);
                //Debug.Log("��������ֵ��"+ (0.65f * bar));
                currentpoint = currentAngle;
                currentangle_text.text = currentpoint.ToString("f2") + "��";
                currentplanepos = 740 * (currentAngle / (60 * reality_coefficient));
            }
        }
    }

    private void Update()
    {
        if (gameisplay && isPlay)
        {
            GetAngle();
            GameBgChange();
        }
    }

    void GameStop(ParameterData pa)
    {
        isplaying = !(bool)pa.data;
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
            DOTween.TogglePauseAll();
            //StopAllCoroutines();
        }
    }

    #region ��Ϸ��ʼ��
    List<string[]> Traindata = new List<string[]>();
    int row;
    int TrainTimes;
    int difficultyscale = 0;
    Model_Airplane model_Airplane = new Model_Airplane();
    List<Vector3> arr = new List<Vector3>();
    int num;
    double time_interval;
    float allTime = 0;
    float motionTime = 0;
    /// <summary>
    /// ��Ϸ������ʼ��
    /// </summary>
    /// <param name="pa"></param>
    void GameStart(ParameterData pa)
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
                allTime = (TrainTimes * 60);
                model_Airplane.Input_Ini((TrainTimes * 60) - 10, 1, RestPanel.MotionTime, RestPanel.RestTime, difficulty, 0, 0, -400, 240, 0, out arr, out num, out time_interval);

            }
            if (goldparent.childCount > 0)
            {
                for (int index = 0; index < goldparent.childCount; index++)
                {
                    Destroy(goldparent.GetChild(index).gameObject);
                }
            }
            motionTime = RestPanel.MotionTime;
            stages = 1;
            man.InitAngle();
            TargetActionControl.Initialize();
            
            tarGetActionImage.SetActive(true);
            gameObject.SetActive(true);
            Plane.gameObject.SetActive(true);
            Plane.anchoredPosition = new Vector2(-320, 120);
            target_plan.position = new Vector3(target_plan.position.x, Plane.position.y, target_plan.position.z);
            AudioPlayer.instantiate.PlayerTherapeutistAudio("��ӭ");
            progressBar.fillAmount = 0.65f;
            progressBar02.fillAmount = 0.65f;

            arrow.gameObject.SetActive(false);

            
            PlanPlayer.gamegrade = 0;
            completionindex = 0;
            currentgoldnum = 1;
            currentangle_text.text = 0.ToString();
            arrindex = 0;
            isplaying = true;
            if (difficulty == 10)
            {
                goldnumber = num;

            }
            else if (difficulty == 7)
            {
                goldnumber = num;
                time_interval = (float)time_interval-0.03f;
            }
            else if (difficulty == 5)
            {
                goldnumber = num + (TrainTimes * 7);
                time_interval = (float)time_interval - 0.04f;
            }
            isplaying = true;
            intervaltime = (float)time_interval;
            goldsum = goldnumber;
            currentpoint = 0;
            pointer.localEulerAngles = new Vector3(0, 0, 0);//max 140  min-40
            sum_Text.text = "��������" + (goldnumber).ToString();
            remainder_Text.text = "ʣ��������" + (goldnumber).ToString();
            completionscount_Text.text = "���������" + completionindex.ToString();
            StartCoroutine("GoldCreate");
            StartCoroutine("Rest_Time");
        }
        else
        {
            DOTween.TogglePauseAll();
            StopAllCoroutines();
            gameObject.SetActive(false);
            Bg.setMove(false, 1);
            for (int index = 0; index < goldparent.childCount; index++)
            {
                Destroy(goldparent.GetChild(index).gameObject);
            }
            MessageCenter.RemoveMsgListener("GameStartOrStop", GameStart);
            MessageCenter.RemoveMsgListener("ContinueOrStopGame", GameContinue);
            MessageCenter.RemoveMsgListener("StopGame", GameStop);
            Destroy(transform.parent.gameObject);
        }
    }

    /// <summary>
    /// ��Ϸ��ͣ
    /// </summary>
    /// <param name="pa"></param>
    void GameContinue(ParameterData pa)
    {
        isPlay = !(bool)pa.data;
        isplaying= !(bool)pa.data;
        if (isPlay)
        {
            //man.gameObject.SetActive(true);
            DOTween.TogglePauseAll();
            //StartCoroutine("GoldCreate");
            //StartCoroutine("Rest_Time");
        }
        else
        {
            DOTween.TogglePauseAll();
            //StopAllCoroutines();
        }
    }
    #endregion
    #region �������
    float intervaltime = 50;
    int currentgoldnum = 0;
    int arrindex = 0;
    bool isplaying = true;
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
                if (currentgoldnum % 5 == 0)
                {

                    GameObject item = Instantiate(enemyPrefab, goldparent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -160);
                    item.name = goldnumber.ToString();
                    item.SetActive(true);
                }
                else if (currentgoldnum % 5 == 4)
                {
                    GameObject item = Instantiate(enemyPrefab, goldparent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    item.SetActive(true);

                }
                else if (currentgoldnum % 5 == 3)
                {
                    GameObject item = Instantiate(enemyPrefab, goldparent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200);
                    item.SetActive(true);

                }
                else if (currentgoldnum % 5 == 2)
                {
                    GameObject item = Instantiate(enemyPrefab, goldparent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -80);
                    item.SetActive(true);

                }
                else if (currentgoldnum % 5 == 1)
                {
                    GameObject item = Instantiate(enemyPrefab, goldparent);
                    item.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -370);
                    item.SetActive(true);
                }
                currentgoldnum++;
                goldnumber -= 1;
                remainder_Text.text = "ʣ��������" + goldnumber.ToString();
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
            arrow.sprite = Resources.Load<Sprite>("Sprite/��ͷ/��");
            tips_Text.text = "����ǰ�ƾ�";
            arrow.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -160);
        }
        else
        {
            //Debug.Log("�ص�ԭλ");
            arrow.sprite = Resources.Load<Sprite>("Sprite/��ͷ/��");
            tips_Text.text = "�����"+"\n"+ "˫�ֻص�ԭλ";
            arrow.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 160);
        }
        arrowshowtime = 0;
    }

    void ArrowFlicker()
    {
        arrow.DOFade(1f, 1).SetEase(Ease.Linear).OnComplete(ArrowFlicker01);
    }

    void ArrowFlicker01()
    {
        arrow.DOFade(0.6f, 1).SetEase(Ease.Linear).OnComplete(ArrowFlicker);
    }

    void PlaneSpin(float oldAngle,float newAngle)
    {
        if((newAngle - oldAngle) > 0)
        {
            if (Plane.localEulerAngles.z < -170)
            {
                Plane.localEulerAngles = new Vector3(0, 0, Plane.localEulerAngles.z + 2);
            }
        }
        else if((newAngle - oldAngle) < 0)
        {
            if(Plane.localEulerAngles.z > -190)
            {
                Plane.localEulerAngles = new Vector3(0, 0, Plane.localEulerAngles.z - 2);
            }
        }
        else if((newAngle - oldAngle) == 0)
        {
            Plane.localEulerAngles = new Vector3(0, 0, -180);
        }
    }

    public static int stages=1;
    void GameBgChange()
    {
        float sumGrade = goldsum * 0.5f;
        //Debug.Log("��ǰ�÷֣�"+PlanPlayer.gamegrade+"                     "+ sumGrade / 4);
        if (PlanPlayer.gamegrade > sumGrade/4&& stages==1)
        {
            stages++;
            //Debug.Log("�ڶ���");
        }
        else if(PlanPlayer.gamegrade > (sumGrade / 4)+ (sumGrade / 4) && stages == 2)
        {
            stages++;
        }
        else if (PlanPlayer.gamegrade > (sumGrade / 4) + (sumGrade / 4) + (sumGrade / 4) && stages == 3)
        {
            stages++;
        }
    }
    #endregion

    IEnumerator Rest_Time()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (motionTime > 0 && isplaying)
            {
                motionTime -= 0.02f;
            }
            else if (isplaying && motionTime <= 0 && allTime > RestPanel.RestTime+5)
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
            arrow.gameObject.SetActive(false);
        }
    }
}
