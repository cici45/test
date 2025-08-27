using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanPlayer : MonoBehaviour
{
    public static float gamegrade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioPlayer.instantiate.PlayerTipsAudio("吃金声");
        Destroy(collision.gameObject);
        PlanMangenter.completionindex += 1;
        if (PlanMangenter.completionindex% 15 == 10)
        {
            //AudioPlayer.instantiate.PlayerTipsAudio("掌声");
            AudioPlayer.instantiate.PlayerTherapeutistAudio("您真棒，请再接再厉！");
        }
        //if (gamegrade < 100)
        //{
            //gamegrade = float.Parse(((float)PlanMangenter.completionindex / (float)PlanMangenter.goldnumber).ToString("#0.0")) * 100;
            gamegrade += 0.5f;
        //}
        //else if(gamegrade > 100)
        //{
        //    gamegrade = 100;
        //}
        PlanMangenter.grade_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-220, 0);
        PlanMangenter.grade_Text.transform.localScale = new Vector3(0, 0, 0);
        PlanMangenter.grade_Text.gameObject.SetActive(true);
        PlanMangenter.grade_Text.transform.DOLocalMoveY(80, 0.8f).SetEase(Ease.Linear).OnComplete(Grade_End);
        PlanMangenter.grade_Text.transform.DOScale(new Vector3(1, 1, 1), 0.8f).SetEase(Ease.Linear);
        //Debug.Log("得分："+ (PlanMangenter.completionindex / PlanMangenter.goldsum-1) +"         "+ gamegrade);
        PlanMangenter.completionscount_Text.text = "完成数量：" + PlanMangenter.completionindex.ToString();
    }

    void Grade_End()
    {
        StartCoroutine("ImageShoeEnd");
        showTime = 0;
    }
    int showTime = 0;
    IEnumerator ImageShoeEnd()
    {
        yield return new WaitForFixedUpdate();
        while(showTime < 50)
        {
            showTime += 1;
        }
        PlanMangenter.grade_Text.gameObject.SetActive(false);
        PlanMangenter.grade_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        PlanMangenter.grade_Text.transform.localScale = new Vector3(0, 0, 0);
        
    }
}
