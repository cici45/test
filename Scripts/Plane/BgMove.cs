using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgMove : MonoBehaviour
{
    
    bool isMove;
    List<RectTransform> Bgs = new List<RectTransform>();
    int currentStages = 1;
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Bgs.Add(transform.GetChild(i).GetComponent<RectTransform>()); 
        }
    }
    void OnEnable()
    {
        StartCoroutine("Moving");
        currentStages = 1;
    }
    void OnDisable()
    {
        StopCoroutine("Moving");
    }
    public void initBg()
    {
        for (int i = 0; i < Bgs.Count; i++)
        {
            Bgs[i].localPosition = i * Vector3.right * 1900;
        }
    }
    float bgspeed;
    public void setMove(bool value,float speed)
    { 
        isMove = value;
        bgspeed = speed;
    }
    IEnumerator Moving() 
    {
        while (true)
        {
            if (isMove)
            {
                for (int i = 0; i < Bgs.Count; i++)
                {
                    //if (Bgs[i].localPosition.x <= -2872) Bgs[i].localPosition += Vector3.right * 1900 * Bgs.Count;
                    Bgs[i].localPosition = Vector3.Lerp(Bgs[i].localPosition, Bgs[i].localPosition - Vector3.right * 300, bgspeed*Time.deltaTime);                  
                }
                if(Bgs[0].localPosition.x <= -2872)
                {
                    Bgs[0].localPosition = new Vector3(Bgs[2].localPosition.x+3840, 0,0);
                    BgChange(Bgs[0]);
                }
                if (Bgs[1].localPosition.x <= -2872)
                {
                    Bgs[1].localPosition = new Vector3(Bgs[0].localPosition.x + 3840, 0, 0);
                    BgChange(Bgs[1]);
                }
                if (Bgs[2].localPosition.x <= -2872)
                {
                    Bgs[2].localPosition = new Vector3(Bgs[1].localPosition.x + 3840, 0, 0);
                    BgChange(Bgs[2]);
                }
            }
            yield return null;
        }
    }
    int num = 0;
    void BgChange(Transform item)
    {
        if (currentStages != PlanMangenter.stages&& num<3)
        {
            switch (PlanMangenter.stages)
            {
                case 1:
                    break;
                case 2:
                    for (int i = 0; i < Bgs.Count; i++)
                    {
                        if (Bgs[i].localPosition.x > 2880)
                        {
                            Bgs[i].GetComponent<Image>().sprite = PlanMangenter.gameBg[4];
                            num++;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < Bgs.Count; i++)
                    {
                        if (Bgs[i].localPosition.x > 2880)
                        {
                            Bgs[i].GetComponent<Image>().sprite = PlanMangenter.gameBg[2];
                            num++;
                        }
                    }
                    
                    break;
                case 4:
                    for (int i = 0; i < Bgs.Count; i++)
                    {
                        if (Bgs[i].localPosition.x > 2880)
                        {
                            Bgs[i].GetComponent<Image>().sprite = PlanMangenter.gameBg[4];
                            num++;
                        }
                    }
                    break;
            }
        }
        else
        {
            num = 0;
            currentStages = PlanMangenter.stages;
        }
    }
}
