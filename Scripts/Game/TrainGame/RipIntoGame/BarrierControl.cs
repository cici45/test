using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BarrierControl : MonoBehaviour
{
    public static float goldmovespeed;
    public static bool isCollide;
    public static bool isF;
    private void OnEnable()
    {
        ischange = true;
        goldIndex = 0;
        goldmovespeed = RipIntoGameControl.difficulty * 1.4f;
        transform.DOLocalMoveX(-1350, goldmovespeed).SetEase(Ease.Linear).OnComplete(MoveEnd);
        image_A = 1;
        ischangcolor = true;
        isCollide = false;
        isF = true;
    }

    void MoveEnd()
    {
        Debug.Log("吃金声"+isCollide);
        if (!isCollide)
        {
            AudioPlayer.instantiate.PlayerTipsAudio("吃金声");
            //Destroy(collision.gameObject);
            RipIntoGameControl.completionIndex += 1;
            if (RipIntoGameControl.completionIndex % 15 == 10)
            {

                AudioPlayer.instantiate.PlayerTherapeutistAudio("您真棒，请再接再厉！");
            }

            PlanPlayer.gamegrade += 0.5f;
            RipIntoGameControl.grade_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-220, 0);
            RipIntoGameControl.grade_Text.transform.localScale = new Vector3(0, 0, 0);
            RipIntoGameControl.grade_Text.gameObject.SetActive(true);
            RipIntoGameControl.grade_Text.transform.DOLocalMoveY(80, 0.8f).SetEase(Ease.Linear).OnComplete(Grade_End);
            RipIntoGameControl.grade_Text.transform.DOScale(new Vector3(1, 1, 1), 0.8f).SetEase(Ease.Linear);
            RipIntoGameControl.completionscountText.text = "完成数量：" + RipIntoGameControl.completionIndex.ToString();
        }
        Destroy(gameObject);
        
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
        
    }
    
    bool ischange = true;
    float image_A = 1;
    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<RectTransform>().anchoredPosition.x < -650 && transform.GetComponent<RectTransform>().anchoredPosition.x > -700 && ischange&& transform.GetComponent<RectTransform>().anchoredPosition.y!=20)
        {
            //RipIntoGameControl.action_Prompt_man.gameObject.SetActive(true);
            int playerPos = (int)(RipIntoGameControl.playerCar.anchoredPosition.y - 240);
            int barrierPos = (int)(transform.GetComponent<RectTransform>().anchoredPosition.y + 300);
            if (transform.GetComponent<RectTransform>().anchoredPosition.y>=300)
            {
                RipIntoGameControl.upOrDown = true;
            }
            else if(transform.GetComponent<RectTransform>().anchoredPosition.y <= -300)
            {
                RipIntoGameControl.upOrDown = false;
            }
            if (barrierPos==0)
            {
                TargetActionControl.SetTargetAngle(1);
            }
            else
            {
                TargetActionControl.SetTargetAngle(0);
            }
            //if(RipIntoGameControl.upOrDown&& isF)
            //{
            //    isF = false;
            //    RipIntoGameControl.action_Prompt_man.SetModelAngle_Target((barrierPos + 320f), 640f, RipIntoGameControl.maxValue);
            //}
            //else if(!RipIntoGameControl.upOrDown && isF)
            //{
            //    isF = false;
            //    RipIntoGameControl.action_Prompt_man.SetModelAngle_Target((barrierPos - 320f), 640f, RipIntoGameControl.maxValue);
            //}
            
            //int targetAngle = (int)((Mathf.Abs(transform.GetComponent<RectTransform>().anchoredPosition.y + 300f) / 640f) * RipIntoGameControl.maxValue);
            //int tarfetPos = (int)((Mathf.Abs(transform.GetComponent<RectTransform>().anchoredPosition.y + 300f) / 640f) * 870);
            
            float player = (RipIntoGameControl.playerCar.anchoredPosition.y - 220) / 650f;
            float barrier = (transform.GetComponent<RectTransform>().anchoredPosition.y + 300) / 640f;
            if (RipIntoGameControl.upOrDown && ischange)//playerPos < 850 && transform.GetComponent<RectTransform>().anchoredPosition.y < 300&& Mathf.Abs(playerPos - barrierPos) <= 250
            {
                ischange = false;
                RipIntoGameControl.ArrowSwitchover(false);
                RipIntoGameControl.arrowImage.gameObject.SetActive(true);
                AudioPlayer.instantiate.PlayerTherapeutistAudio("腿部请向后");
                
            }
            else if (!RipIntoGameControl.upOrDown && ischange)//playerPos >= 600 && transform.GetComponent<RectTransform>().anchoredPosition.y >= 250
            {
                ischange = false;
                RipIntoGameControl.arrowImage.gameObject.SetActive(true);
                RipIntoGameControl.ArrowSwitchover(true);
                AudioPlayer.instantiate.PlayerTherapeutistAudio("腿部请向前");
            }
            else if(Mathf.Abs(player - barrier) >= 0.25 && ischange)
            {
                ischange = false;
                RipIntoGameControl.arrowImage.gameObject.SetActive(false);
            }
            
            #region 废弃
            //int targetAngle = (int)((Mathf.Abs(transform.GetComponent<RectTransform>().anchoredPosition.y + 300f) / 640.0f) * RipIntoGameControl.maxValue);
            //if (Mathf.Abs(UserInfoData.CurrentAngle) - targetAngle > 5  && ischange)
            //{
            //    ischange = false;

            //    RipIntoGameControl.ArrowSwitchover(false);
            //    RipIntoGameControl.arrowImage.gameObject.SetActive(true);
            //    AudioPlayer.instantiate.PlayerTherapeutistAudio("双手缓慢后缩");

            //}
            //else if (Mathf.Abs(UserInfoData.CurrentAngle) - targetAngle <= 0  && ischange)
            //{
            //    ischange = false;
            //    RipIntoGameControl.arrowImage.gameObject.SetActive(true);
            //    RipIntoGameControl.ArrowSwitchover(true);
            //    AudioPlayer.instantiate.PlayerTherapeutistAudio("请往前推");

            //}
            //else
            //{
            //    ischange = false;
            //    RipIntoGameControl.arrowImage.gameObject.SetActive(false);
            //}
            #endregion
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    void Grade_End()
    {
        RipIntoGameControl.grade_Text.gameObject.SetActive(false);
        RipIntoGameControl.grade_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        RipIntoGameControl.grade_Text.transform.localScale = new Vector3(0, 0, 0);
    }
    int showTime = 0;
    //IEnumerator ImageShoeEnd()
    //{
    //    //yield return new WaitForFixedUpdate();
    //    //while (showTime < 20)
    //    //{
    //    //    showTime += 1;
    //    //}
        

    //}
}
