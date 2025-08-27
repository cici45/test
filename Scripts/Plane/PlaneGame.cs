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
    bool isCollide;//碰撞标志
    bool isMove;//移动标志
    int timer;
    float durTime;//移动持续时间
    Vector3 startPos;
    Vector3 endPos;
    float interval = 0.05f;
    float startY = -430;
    float endY = 430;

    string TrainTableName = "Temporary_GeneralTab_TrainMode";
    List<string[]> TrainData = new List<string[]>();

    float intervalTime;//外部变量
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
        this.gameObject.SetActive(false);//读取后隐藏关闭脚本
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
    void SetArrow(bool isUp)//需要显示，与距离最近的敌人的位置关系，产生间隔，消失间隔
    {
        if (!Arrow.gameObject.activeInHierarchy) Arrow.gameObject.SetActive(true);
        if (isUp) Arrow.transform.eulerAngles = Vector3.forward * 90;
        else Arrow.transform.eulerAngles = Vector3.forward * (-90);
    }
    void showBar()//需要显示，间隔时间
    {
        if (!isCollide)
        {
            Bar.fillAmount = timer / (intervalTime - 1);
            if (timer <= Math.Floor(intervalTime / 2)) Text.text = "还差" + (maxValue - currentAngle).ToString("f1") + unit;
            else Text.text = "还差" + (maxValue - currentAngle).ToString("f1") + unit;
        }
        else
        {
            Text.text = "已发生碰撞";
        }
    }
    void PlanePos(float angle)
    {
        float Y = startY + ((angle - minValue) / (maxValue - minValue)) * (endY - startY);//角度=>y值
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
    void IntervalTimer()//重置和得分处理，游戏逻辑在碰撞中
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
                    Debug.Log("得分！");//++currentSportNum;                   
                    MathRSA.Instance.Scores(true);//if (currentSportNum <= sportNum)//运动次数内得分                   
                    EffectAndAnimation(2);//完成鼓励
                }
                else EffectAndAnimation(1);//差一点
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
                MathRSA.Instance.initial(currentAngle);//写入                                    
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
                EffectAndAnimation(0);//欢迎
                man.InitAngle();
            }
        }
        else
        {
            isPlay = false;
            EffectAndAnimation(-1);//感谢
            MathRSA.Instance.GameOverCallback();
            this.gameObject.SetActive(false);//停用          
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
        intervalTime = float.Parse(TrainData[0][1]);//0保持时间,1间隔时间,2正极限值,3负极限值
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
        if (condition <= -1)//感谢
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("结束感谢");//3D场景=>添加治疗师模型
            PlayerRherapistAnimations.playerRherapist.Bow();
            AudioPlayer.instantiate.StopAllAudios();
        }
        else if (condition == 0)//欢迎
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("开始欢迎");//音乐=>开始/结束/暂停/继续
            PlayerRherapistAnimations.playerRherapist.Wave();
        }
        else if (condition == 1)//差一点
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("就差一点了");
            PlayerRherapistAnimations.playerRherapist.Speech_1();
        }
        else//完成
        {
            AudioPlayer.instantiate.PlayerTherapeutistAudio("完成动作鼓励");//待机动画=>协程，动画控制
            PlayerRherapistAnimations.playerRherapist.Speech_2();
        }
    }
}
