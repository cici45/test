using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instantiate;
    AudioSource Audio_Tip, Audio_Therapeutist;

    private void Awake()
    {
        Audio_Tip = transform.Find("Audio_Tip").GetComponent<AudioSource>();
        Audio_Therapeutist = transform.Find("Audio_Therapeutist").GetComponent<AudioSource>();
        instantiate = this;
    }

    void Start()
    {
        //if (instantiate == null) {  }
        //PlayerTipsAudio("BG");
        //PlayerTherapeutistAudio("01");
    }


    /// <summary>
    /// 播放游戏背景音效
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayerTipsAudio(string audioName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioName);
        //audioSource(即是AudioSource组件)判断并播放加载好的文件
        if (Audio_Tip.isPlaying)
        {
            Audio_Tip.Stop();
            Audio_Tip.clip = null;
        }
        Audio_Tip.clip = clip;
        Audio_Tip.Play();
        //StartCoroutine("BackTipsAudio");
    }

    /// <summary>
    /// 播放治疗师语音
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayerTherapeutistAudio(string audioName)
    {

        if (Audio_Therapeutist.clip != null && Audio_Therapeutist.clip.name == "您真棒，请再接再厉！" && Audio_Therapeutist.isPlaying)
        {
            return;
        }
        else if (Audio_Therapeutist.clip != null && Audio_Therapeutist.clip.name == "感谢您的使用，祝您早日康复" && Audio_Therapeutist.isPlaying)
        {
            return;
        }
        else if (Audio_Therapeutist.clip != null && Audio_Therapeutist.clip.name == "请往前推" && Audio_Therapeutist.isPlaying)
        {
            return;
        }
        else if (Audio_Therapeutist.clip != null && Audio_Therapeutist.clip.name == "双手缓慢后缩" && Audio_Therapeutist.isPlaying)
        {
            return;
        }
        else if (Audio_Therapeutist.clip != null&&audioName == Audio_Therapeutist.clip.name)
        {
            return;
        }
        else
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioName);
            //Debug.Log(audioName);
            //audioSource(即是AudioSource组件)判断并播放加载好的文件
            if (Audio_Therapeutist.isPlaying)
            {
                Audio_Therapeutist.Stop();
            }
            Audio_Therapeutist.clip = clip;
            Audio_Therapeutist.Play();
        }
        //StartCoroutine("BackTherapeutistAudio");
    }

    /// <summary>
    /// 继续播放所有音效
    /// </summary>
    public void ContinuePlayingAllAudio()
    {
        if (Audio_Tip.clip != null)
            Audio_Tip.Play();
        if (Audio_Therapeutist.clip != null)
            Audio_Therapeutist.Play();
    }

    /// <summary>
    /// 暂停所有音效
    /// </summary>
    public void PauseAllAudios()
    {
        Audio_Tip.Pause();
        Audio_Therapeutist.Pause();
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllAudios()
    {
        Audio_Tip.Stop();
        Audio_Therapeutist.Stop();
        Audio_Tip.clip = null;
        Audio_Therapeutist.clip = null;
    }
    IEnumerator PlayerEncourageAudio()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + "掌声");
        if (Audio_Therapeutist.isPlaying)
        {
            Audio_Therapeutist.Stop();
        }
        Audio_Therapeutist.clip = clip;
        Audio_Therapeutist.Play();
        yield return new WaitForSeconds(1f);
        clip = Resources.Load<AudioClip>("Audio/" + "完成动作鼓励");
        Audio_Therapeutist.clip = clip;
        Audio_Therapeutist.Play();
        yield return new WaitForSeconds(Audio_Therapeutist.clip.length);
        Audio_Therapeutist.clip = null;
        //PlayerRherapistAnimations.playerRherapist.Standby();
    }

    ////IEnumerator BackTherapeutistAudio()
    ////{
    ////    float time = 0;
    ////    if (Audio_Therapeutist.clip!=null)
    ////    {
    ////        time = Audio_Therapeutist.clip.length;
    ////    }
    ////    yield return new WaitForSeconds(time); 
    ////    Audio_Therapeutist.clip = null;
    ////    //PlayerRherapistAnimations.playerRherapist.Standby();
    ////}

    //IEnumerator BackTipsAudio()
    //{
    //    float time = 0;
    //    if (Audio_Tip.clip != null)
    //    {
    //        time = Audio_Tip.clip.length;
    //    }
    //    yield return new WaitForSeconds(time);
    //    Audio_Tip.clip = null;
    //}

    private void OnDisable()
    {
        //DontDestroyOnLoad(this);
    }

}
