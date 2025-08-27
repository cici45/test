using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Sprite[] spritesRight;
    Sprite[] spritesLeft;
    int leftindex = 0;
    int rightindex = 0;
    public static float balangrade=0;
    public static string currentpos;

    float left_1;
    float left_2;
    float right_1;
    float right_2;
    int currentx;
    int currenty;
    Vector3 currentps;

    private void Awake()
    {
        currentps = transform.localPosition;
    }
    private void OnEnable()
    {
        StartCoroutine("GameIsPlaying");
        left_1 = 0;
        left_2 = 0;
        right_1 = 0;
        right_2 = 0;
        currentx = 250;
        currenty = 150;
        transform.localPosition = currentps;
    }

    // Start is called before the first frame update
    void Start()
    {
        spritesRight = Resources.LoadAll<Sprite>("熊猫原地走/右");
        spritesLeft = Resources.LoadAll<Sprite>("熊猫原地走/左");
        BalancinManager.leftbutton.onClick.AddListener(() => {
            currentx = 200;
            currenty = 100;
        });
        BalancinManager.rightbutton.onClick.AddListener(() => {
            currentx = 300;
            currenty = 200;
        });
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y+2f,0);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{

        //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 2f, 0);
        //    Debug.Log(transform.localPosition.y);
        //}
        if (transform.localPosition.x != tageterx || transform.localPosition.y != tagetery&& BalancinManager.gameisplay && BalancinManager.gameisPause)
        {
            try
            {
                #region 
                //        if (tageterx > 0)
                //        {
                //            if (rightindex < 31&& transform.localPosition.x <= tageterx - 50)
                //            {
                //                rightindex += 1;
                //                transform.GetComponent<Image>().sprite = spritesLeft[rightindex];
                //                if (transform.localPosition.x <= tageterx - 50 && transform.localPosition.x < 600)
                //                {
                //                    transform.localPosition = new Vector3(transform.localPosition.x + 1f, transform.localPosition.y, transform.localPosition.z);
                //                }
                //            }
                //            else
                //            {
                //                rightindex = 0;
                //            }
                //        }
                //        if (tageterx < 0)
                //        {
                //            if (leftindex < 31 && transform.localPosition.x >= tageterx + 50)
                //            {
                //                leftindex += 1;
                //                transform.GetComponent<Image>().sprite = spritesRight[leftindex];
                //                if (transform.localPosition.x >= tageterx + 50 && transform.localPosition.x > -600)
                //                {
                //                    transform.localPosition = new Vector3(transform.localPosition.x - 1f, transform.localPosition.y, transform.localPosition.z);
                //                }
                //            }
                //            else
                //            {
                //                leftindex = 0;
                //            }
                //        }

                //--------------------
                //if (Count_Temp++ >20000)
                //{
                //    Change_Temp=!Change_Temp;
                //    Count_Temp = 0;
                //}


                //if (Change_Temp == false)
                //{
                //    tagetery = 50;
                //}
                //else { tagetery = -50; }

                //============================
                //Debug.Log(tagetery);
                //if (tagetery > 0)//向上
                //{
                //    if (rightindex < 31 && transform.localPosition.y < tagetery - 230)
                //    {
                //        rightindex += 1;
                //        transform.GetComponent<Image>().sprite = spritesLeft[rightindex];

                //        if (transform.localPosition.y < tagetery - 230)
                //        {
                //            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.2f * Time.deltaTime, transform.localPosition.z);
                //        }
                //    }
                //    else
                //    {
                //        rightindex = 0;
                //    }
                //}
                //else if (tagetery < 0)//向下
                //{
                //    Debug.Log("y轴的值："+transform.localPosition.y);
                //    if (rightindex < 31 && transform.localPosition.y > tagetery-230)
                //    {
                //        rightindex += 1;
                //        transform.GetComponent<Image>().sprite = spritesLeft[rightindex];

                //        if (transform.localPosition.y >= tagetery - 230)
                //        {

                //            transform.localPosition = new Vector3(transform.position.x, transform.localPosition.y - 0.2f, transform.localPosition.z);
                //        }
                //    }
                //    else
                //    {
                //        rightindex = 0;
                //    }
                //}
                #endregion

                if (tageterx > 0)
                {
                    if (rightindex < 31 && transform.localPosition.x <= tageterx - 50)
                    {
                        rightindex += 1;
                        transform.GetComponent<Image>().sprite = spritesLeft[rightindex];
                        if (transform.localPosition.x <= tageterx - 50 && transform.localPosition.x < 600)
                        {
                            int speed = Mathf.Abs(tageterx / 10);
                            transform.localPosition = new Vector3(transform.localPosition.x + 25f, transform.localPosition.y, transform.localPosition.z);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
                        }
                    }
                    else
                    {
                        rightindex = 0;
                    }
                }
                else if (tageterx < 0)
                {
                    if (rightindex < 31 && transform.localPosition.x >= tageterx + 50)
                    {
                        rightindex += 1;
                        transform.GetComponent<Image>().sprite = spritesRight[rightindex];
                        if (transform.localPosition.x >= tageterx + 50 && transform.localPosition.x > -600)
                        {
                            int speed = Mathf.Abs(tageterx / 10);
                            transform.localPosition = new Vector3(transform.localPosition.x - 25f, transform.localPosition.y, transform.localPosition.z);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
                        }
                    }
                    else
                    {
                        rightindex = 0;
                    }
                }

                Debug.Log(transform.localPosition.y + "   ====" + (tagetery));
                if (tagetery < 0 && transform.localPosition.y + 450 > tagetery)
                {
                    if (rightindex < 31 && transform.localPosition.y + 450 > tagetery)
                    {
                        rightindex += 1;
                        if (tageterx < 0)
                        {
                            transform.GetComponent<Image>().sprite = spritesLeft[rightindex];
                        }
                        else
                        {
                            transform.GetComponent<Image>().sprite = spritesLeft[rightindex];
                        }

                        if (transform.localPosition.y + 450 > tagetery)
                        {
                            int speed = Mathf.Abs(tagetery / 10);
                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 10f, transform.localPosition.z);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
                        }
                    }
                    else
                    {
                        rightindex = 0;
                    }
                }
                else if (tagetery > 0 && transform.localPosition.y + 221 < tagetery)
                {
                    if (rightindex < 31 && transform.localPosition.y + 221 < tagetery)
                    {
                        rightindex += 1;
                        if (tageterx < 0)
                        {
                            transform.GetComponent<Image>().sprite = spritesLeft[rightindex];
                        }
                        else
                        {
                            transform.GetComponent<Image>().sprite = spritesLeft[rightindex];
                        }
                        if (transform.localPosition.y + 221 < tagetery)
                        {
                            int speed = Mathf.Abs(tagetery / 10);
                            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 10f, transform.localPosition.z);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
                        }
                    }
                    else
                    {
                        rightindex = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }
    }

    //-----------------
    bool Change_Temp = false;
    int Count_Temp = 0;
    //===============
    /// <summary>
    /// 前走
    /// </summary>
    /// <returns></returns>
    //IEnumerator FrontMOve()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForFixedUpdate();
            
    //    }
    //}

    /// <summary>
    /// 后走
    /// </summary>
    /// <returns></returns>
    IEnumerator LaterMOve()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02F);
           
            if (rightindex < 32)
            {
                rightindex += 1;
                transform.GetComponent<Image>().sprite = spritesRight[rightindex];
                if (transform.localPosition.y > -350)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 4, transform.localPosition.z);
                }

            }
            else
            {
                rightindex = 0;
            }
        }
    }

    int tageterx;
    int tagetery;
    IEnumerator GameIsPlaying()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (BalancinManager.gameisplay && BalancinManager.gameisPause)
            {
                
                if (currentpos != null)
                {

                    string[] readData = currentpos.Split('_');

                    //if (currentx == 0)
                    //{
                    //    currentx = (int)double.Parse(readData[4]);
                    //    currenty = (int)double.Parse(readData[5]);
                    //}
                    //else
                    //{  左小右大，  前大后小

                    //if (readData.Length>=5)readData[5]) - currenty
                    //{
                    //if (Mathf.Abs(currentx - 230) > 10)
                    //{
                    tageterx = 12 * ((int)double.Parse(readData[4]) - 250);
                        //Debug.Log("tageterx"+ tageterx);
                        //tageterx = 0;
                    //}
                    //if (Mathf.Abs(currenty - 150) > 10)
                    //{
                    tagetery = ((int)double.Parse(readData[5]) - 150);
                        //tagetery = 0;
                    //}
                    //Debug.Log(tageterx + "   ____" + tagetery);
                    //StartCoroutine("FrontMOve");
                        //}
                    }
                //}
            }
        }
    }

    SetBtnPage setBtnPage = new SetBtnPage();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BalancinManager.receivenumber += 1;
        balangrade += 1;
        AudioPlayer.instantiate.PlayerTherapeutistAudio("完成动作鼓励");//往前
        BalancinManager.receivetext.text = "接到数量：" + BalancinManager.receivenumber.ToString();
    }
}
