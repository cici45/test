using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;

public class RestPanel : MonoBehaviour
{
    /// <summary>
    /// 休息时间
    /// </summary>
    public static int RestTime = 15;
    /// <summary>
    /// 运动时间
    /// </summary>
    public static int MotionTime = 45;

    VideoPlayer video;
    Text tips;
    GameObject Image;
    bool value;
    private void Awake()
    {
        video = transform.GetComponent<VideoPlayer>();
        Image = transform.Find("Image").gameObject;
        tips = transform.Find("Image/tips").GetComponent<Text>();
        MessageCenter.AddMsgListener("RestPanel", OpenRestPanel);

    }
    void Start()
    {
        video.source = VideoSource.Url;
        video.url = Application.streamingAssetsPath + "/Video/Open.mp4";
        video.Play();
        transform.DOScale(Vector3.zero, 1).OnComplete(() => gameObject.SetActive(false));

    }

    private void OpenRestPanel(ParameterData pa)
    {
        value = (bool)pa.data;
        gameObject.SetActive(value);
        if (value)
        {
            Image.SetActive(true);
            transform.DOScale(Vector3.one, 1);
            num = 0;
            timer = 0;
            video.Play();
        }
    }


    float timer = 0;
    int num = 0;
    private void FixedUpdate()
    {
        if (value)
        {
            timer += 0.02f;
            if (RestTime - 1 - (int)timer >= 0)
            {
                tips.text = RestTime - 1 - (int)timer + "";
                if (RestTime - 1 - (int)timer <= 2 && num == 0)
                {
                    num++;
                    AudioPlayer.instantiate.PlayerTherapeutistAudio("请继续训练");
                }
            }
            else
            {
                transform.DOScale(Vector3.zero, 0.5f);
                Invoke("OnBack", 0.5f);
            }
        }
    }

    private void OnBack()
    {
        if (video.isPlaying)
        {
            video.Stop();
            MessageCenter.SendMsg("StopGame", false);
            gameObject.SetActive(false);
        }
    }
}