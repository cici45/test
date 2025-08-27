using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoldMangenter : MonoBehaviour
{
    public static float goldmovespeed;

    private void OnEnable()
    {
        ischange = true;
        goldIndex = 0;
        goldmovespeed = PlanMangenter.difficulty * 1.4f;
        transform.DOLocalMoveX(-2050, goldmovespeed).SetEase(Ease.Linear).OnComplete(MoveEnd);
        image_A = 1;
        ischangcolor = true;
    }

    void MoveEnd()
    {
        Destroy(gameObject);
        //AudioPlayer.instantiate.PlayerTherapeutistAudio("就差一点了");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    int index = 3;
    int goldIndex = 0;
    int time_gold = 8;
    bool ischangcolor = false;
    private void FixedUpdate()
    {
        if (time_gold > 0)
        {
            time_gold -= 1;
        }
        else
        {
            time_gold = 8;
            if (index > 0)
            {
                index -= 1;
            }
            else
            {
                if (goldIndex < PlanMangenter.golds.Length - 1)
                {

                    transform.GetComponent<Image>().sprite = PlanMangenter.golds[goldIndex];
                    goldIndex += 1;
                }
                else
                {
                    transform.GetComponent<Image>().sprite = PlanMangenter.golds[0];
                    goldIndex = 0;
                }
            }
        }
        //if (transform.GetComponent<RectTransform>().anchoredPosition.x < -1220 && transform.GetComponent<RectTransform>().anchoredPosition.x > -1380)
        //{
        //    if (image_A>0.6f&& ischangcolor)
        //    {
        //        image_A -= 0.1f;
        //        transform.GetComponent<Image>().color = new Color(1,1,1, image_A);
        //    }
        //    else if (image_A <= 0.6f)
        //    {
        //        ischangcolor = false;
        //    }
        //    else if (image_A < 1f && !ischangcolor)
        //    {
        //        image_A += 0.1f;
        //        transform.GetComponent<Image>().color = new Color(1, 1, 1, image_A);
        //    }
        //    else if (image_A >= 1f)
        //    {
        //        ischangcolor = true;
        //    }
        //}
    }

    bool ischange = true;
    float image_A = 1;
    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<RectTransform>().anchoredPosition.x < -1200)
        {
            Destroy(gameObject);
        }
        if (transform.GetComponent<RectTransform>().anchoredPosition.x < -960 && transform.GetComponent<RectTransform>().anchoredPosition.x > -1000 && ischange)
        {
            //PlanMangenter.action_Prompt_man.gameObject.SetActive(true);
            //PlanMangenter.arrow.gameObject.SetActive(true);
            //Debug.Log("位置比："+Mathf.Abs(PlanMangenter.target_plan.anchoredPosition.y)+">>>>>>>>>>>>>>>>>>>>>"+ Mathf.Abs(transform.GetComponent<RectTransform>().anchoredPosition.y));
            //PlanMangenter.action_Prompt_man.SetModelAngle_Target((transform.GetComponent<RectTransform>().anchoredPosition.y+400f),570f, PlanMangenter.maxValue);
            int targetAngle = (int)((Mathf.Abs(transform.GetComponent<RectTransform>().anchoredPosition.y + 370f) / 570f) * PlanMangenter.maxValue);
            TargetActionControl.SetTargetAngle(((float)targetAngle / PlanMangenter.maxValue));
            if (Mathf.Abs(UserInfoData.CurrentAngle) - targetAngle > 3 && ischange)
            {
                ischange = false;
                PlanMangenter.ArrowSwitchover(false);
                PlanMangenter.arrow.gameObject.SetActive(true);
                AudioPlayer.instantiate.PlayerTherapeutistAudio("双手缓慢后缩");
               

            }
            else if (Mathf.Abs(UserInfoData.CurrentAngle) - targetAngle < 3 && ischange)
            {
                ischange = false;
                PlanMangenter.arrow.gameObject.SetActive(true);
                PlanMangenter.ArrowSwitchover(true);
                AudioPlayer.instantiate.PlayerTherapeutistAudio("请往前推");
            }
            else
            {
                ischange = false;
                PlanMangenter.arrow.gameObject.SetActive(false);
            }

        }
    }

}
