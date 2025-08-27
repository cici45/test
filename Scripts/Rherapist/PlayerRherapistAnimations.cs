using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRherapistAnimations : MonoBehaviour
{
    public static PlayerRherapistAnimations playerRherapist;
    AudioPlayer audio;
    Animator animator;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        //audio = GameObject.FindObjectOfType<AudioPlayer>();
        playerRherapist = this;
    }

    /// <summary>
    /// 鞠躬
    /// </summary>
    /// <param name="value"></param>
    public void Bow()
    {
        //animator.SetBool("Bow", true);
    }

    /// <summary>
    /// 讲话1
    /// </summary>
    /// <param name="value"></param>
    public void Speech_1()
    {
        //animator.SetBool("Speech_1", true);
    }

    /// <summary>
    /// 讲话2
    /// </summary>
    /// <param name="value"></param>
    public void Speech_2()
    {
       // animator.SetBool("Speech_2", true);
    }

    /// <summary>
    /// 挥手
    /// </summary>
    /// <param name="value"></param>
    public void Wave()
    {
        //animator.SetBool("Wave", true);
    }

    /// <summary>
    /// 待机
    /// </summary>
    public void Standby()
    {
        //animator.SetBool("Bow", false);
        //animator.SetBool("Speech_1", false);
        //animator.SetBool("Speech_2", false);
        //animator.SetBool("Wave", false);
    }

}
