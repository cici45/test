using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlaneGame : MonoBehaviour
{
    RectTransform Plane;
    BgMove Bg;
    Image Arrow;
    Image Bar;
    Text Text;
    LegAngle man;

    bool isPlay;
    bool isCollide;//��ײ��־
    bool isMove;//�ƶ���־
    int timer;
    float durTime;//�ƶ�����ʱ��
    Vector3 startPos;
    Vector3 endPos;
    float interval = 0.05f;
    float startY = -430;
    float endY = 430;

    string TrainTableName = "Temporary_GeneralTab_TrainMode";
    List<string[]> TrainData = new List<string[]>();

    float intervalTime;//�ⲿ����
    float maxValue;
    float minValue;
    float currentAngle;
    string unit;
    void Awake()
    {
        Plane = transform.Find("Plane").GetComponent<RectTransform>();
        Bg = transform.Find("Bg").GetComponent<BgMove>();
        //man = transform.Find("Man").GetComponent<LegAngle>();

        Arrow = transform.Find("Arrow").GetComponent<Image>();
        Bar = transform.Find("BarBg/Bar").GetComponent<Image>();
        Text = transform.Find("BarBg/Text").GetComponent<Text>();
        MessageCenter.AddMsgListener("PlaneGameStart", GameStart);
        MessageCenter.AddMsgListener("ContinueOrStopGame", GameContinue);
        this.gameObject.SetActive(false);//��ȡ�����عرսű�
    }
    void OnEnable()
    {
        InvokeRepeating("IntervalTimer", interval, 1f);
        InvokeRepeating("GetAngle", interval, interval);
        StartCoroutine("PlaneMove");
    }
    void OnDisable()
    {
        CancelInvoke("IntervalTimer");
        CancelInvoke("GetAngle");
        StopCoroutine("PlaneMove");
    }
    void SetArrow(bool isUp)//��Ҫ��ʾ�����������ĵ��˵�λ�ù�ϵ�������������ʧ���
    {
        if (!Arrow.gameObject.activeInHierarchy) Arrow.gameObject.SetActive(true);
        if (isUp) Arrow.transform.eulerAngles = Vector3.forward * 90;
        else Arrow.transform.eulerAngles = Vector3.forward * (-90);
    }
    void showBar()//��Ҫ��ʾ�����ʱ��
    {
        if (!isCollide)
        {
            Bar.fillAmount = timer / (intervalTime - 1);
            if (timer <= Math.Floor(intervalTime / 2)) Text.text = "����" + (maxValue - currentAngle).ToString("f1") + unit;
            else Text.text = "����" + (maxValue - currentAngle).ToString("f1") + unit;
        }
        else
        {
            Text.text = "�ѷ�����ײ";
        }
    }
    void PlanePos(float angle)
    {
        float Y = startY + ((angle - minValue) / (maxValue - minValue)) * (endY - startY);//�Ƕ�=>yֵ
        Y = Mathf.Round(Y * 10f) / 10f;
        if (Y >= startY && Y <= endY) endPos = new Vector3(Plane.localPosition.x, Y, 0);
        startPos = Plane.localPosition;       
        durTime = 0;
        isMove = true;
    }
    IEnumerator PlaneMove()
    {
        while (true)
        {
            yield return null;
            if (isPlay && isMove)
            {
                durTime += Time.deltaTime;
                if (durTime < interval)
                {
                    Plane.localPosition = Vector3.Lerp(startPos, endPos, durTime / interval);                   
                }
                else
                {
                    isMove = false;
                    durTime = 0;
                    Plane.localPosition = endPos;                  
                }
            }
        }
    }
    void IntervalTimer()//���ú͵÷ִ�����Ϸ�߼�����ײ��
    {
        if (isPlay)
        {         
            timer += 1;
            if (timer == Math.Floor(intervalTime / 2))
            {
                GameObject obj = EnemyPool.instance.Get(intervalTime / 2f);
                obj.transform.localPosition = Vector3.right * (-100);
                obj.SetActive(true);
                obj = EnemyPool.instance.Get(intervalTime / 2f);
                obj.transform.localPosition = Vector3.up * 330;
                obj.SetActive(true);
                SetArrow(false);
            }
            if (timer >= intervalTime)
            {             
                if (!isCollide)
                {
                    Debug.Log("�÷֣�");//++currentSportNum;                   
                    MathRSA.Instance.Scores(true);//if (currentSportNum <= sportNum)//�˶������ڵ÷�                   
                    EffectAndAnimation(2);//��ɹ���
                }
                else EffectAndAnimation(1);//��һ��
                timer = 0;
                isCollide = false;

                //EnemyPool.instance.Clear();
                GameObject obj = EnemyPool.instance.Get(intervalTime / 2f);
                obj.transform.localPosition = Vector3.right * (-100);
                obj.SetActive(true);
                obj = EnemyPool.instance.Get(intervalTime / 2f);
                obj.transform.localPosition = Vector3.up * (-330);
                obj.SetActive(true);
                SetArrow(true);
            }                    
        }
    }
    void GetAngle()
    {
        if (isPlay)
        {
            try { currentAngle = UserInfoData.CurrentAngle; }
            catch (Exception e) { Debug.Log(e); }
            if (currentAngle < 8000)
            {
                MathRSA.Instance.initial(currentAngle);//д��                                    
                if (currentAngle < minValue) currentAngle = minValue;
                if (currentAngle > maxValue) currentAngle = maxValue;               
                PlanePos(Mathf.Round(currentAngle * 10f) / 10f);               
                showBar();
                man.SetModelAngle(currentAngle);
            }
        }
    }
    void GameStart(ParameterData pa)
    {
        bool isStart = (bool)pa.data;
        if (isStart)
        {
            TrainData.Clear();
            MathRSA.Instance.IsotonicOrEquilongFunc();
            int n;
            OperateUseSQL_H.Read_Data(TrainTableName, out TrainData, out n);
            if (TrainData != null && TrainData.Count != 0)
            {
                isPlay = true;
                isCollide = false;               
                isMove = false;
                timer = 0;
                durTime = 0;
                //currentSportNum = 0;
                ReadMode();
                startPos = new Vector3(Plane.localPosition.x, startY, 0);
                endPos = startPos;
                Plane.localPosition = startPos;
                Bg.initBg();
                //Bg.setMove(true);

                this.gameObject.SetActive(true);
                EnemyPool.instance.Clear();
                GameObject obj = EnemyPool.instance.Get(intervalTime/2f);
                obj.transform.localPosition = Vector3.right * (-100);
                obj.SetActive(true);
                obj = EnemyPool.instance.Get(intervalTime/2f);
                obj.transform.localPosition = Vector3.up * (-380);
                obj.SetActive(true);
                EnemyPool.instance.setMove(isPlay);

                showBar();
                SetArrow(true);
                EffectAndAnimation(0);//��ӭ
                man.InitAngle();
            }
        }
        else
        {
            isPlay = false;
            EffectAndAnimation(-1);//��л
            MathRSA.Instance.GameOverCallback();
            this.gameObject.SetActive(false);//ͣ��          
        }
    }
    void GameContinue(ParameterData pa)
    {
        bool isContinue = (bool)pa.data;
        if (!isContinue)
        {
            isPlay = true;
            //Bg.setMove(isPlay);
            EnemyPool.instance.setMove(isPlay);
            AudioPlayer.instantiate.ContinuePlayingAllAudio();
        }
        else
        {
            isPlay = false;
            //Bg.setMove(isPlay);
            EnemyPool.instance.setMove(isPlay);
            AudioPlayer.instantiate.PauseAllAudios();
        }
    }
    void ReadMode()
    {
        intervalTime = float.Parse(TrainData[0][1]);//0����ʱ��,1���ʱ��,2������ֵ,3������ֵ
        maxValue = float.Parse(TrainData[0][2]);
        minValue = int.Parse(TrainData[0][3]);
        //sportNum = 5;//int.Parse(TrainData[0][4]);
        unit = UserInfoData.Unit;
    }
    void setCollide(bool value)
    { 
        isCollide = value;
    }
    void EffectAndAnimation(int condition)
    {
        if (condition <= -1)//��л
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("������л");//3D����=>�������ʦģ��
            PlayerRherapistAnimations.playerRherapist.Bow();
            AudioPlayer.instantiate.StopAllAudios();
        }
        else if (condition == 0)//��ӭ
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("��ʼ��ӭ");//����=>��ʼ/����/��ͣ/����
            PlayerRherapistAnimations.playerRherapist.Wave();
        }
        else if (condition == 1)//��һ��
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("�Ͳ�һ����");
            PlayerRherapistAnimations.playerRherapist.Speech_1();
        }
        else//���
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("��ɶ�������");//��������=>Э�̣���������
            PlayerRherapistAnimations.playerRherapist.Speech_2();
        }
    }
}
