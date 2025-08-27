using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void CompleteEvent();
public class GameTimer : MonoBehaviour
{
    static CompleteEvent oncompleted;
    Coroutine coroutine;
    public bool isTimerStop=false;
    private void Start()
    {
       
    }

    #region 计时器
    public void CreatTimer(float timer, CompleteEvent completeEvent_,Text timeText)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine= StartCoroutine(TimerGame(timer, completeEvent_, timeText));


    }

    static float inputtime;
    bool isStop = false;
    IEnumerator TimerGame(float gameTime, CompleteEvent completeEvent_,Text timeText )
    {
        if (oncompleted == null)
        {
            oncompleted = completeEvent_;
        }
        timeText.text = FormatTime(gameTime*60);
        inputtime = gameTime * 60;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (inputtime > 0 && !isTimerStop)
            {
                
                inputtime -= 1;
                timeText.text = FormatTime(inputtime);
            }
            else if (inputtime <= 0)
            {
                timeText.text = FormatTime(inputtime);
                Debug.Log("计时器结束");
                oncompleted();
            }
        }
        
    }
    #endregion
    /// <summary>
    /// 格式化时间
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns></returns>
    public static string FormatTime(float seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
        string str = "";
        if (ts.Hours > 0)
        {
            str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");

        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
            str = "00" + ":" + ts.Seconds.ToString("00");
        }
        return str;
    }
}
