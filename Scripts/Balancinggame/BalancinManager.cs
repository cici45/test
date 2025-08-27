using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BalancinManager : MonoBehaviour
{
    Transform player;
    Transform monsterground;
    List<Transform> monster = new List<Transform>();
    public static bool gameisplay = true;
    public static bool gameisPause = true;
    int currentindex = 0;
    int intervaltime = 100;

    public static Text residuetext;
    public static Text receivetext;
    int residuenumber;
    public static int receivenumber;

    Transform right;
    Transform left;

    public static Button rightbutton;
    public static Button leftbutton;
    private void Awake()
    {
        player = transform.Find("Player");
        right  = transform.Find("Right");
        left = transform.Find("Left");
        rightbutton = transform.Find("RightButton").GetComponent<Button>();
        leftbutton = transform.Find("LeftButton").GetComponent<Button>();
        player.localPosition = new Vector3(0,player.localPosition.y,0);
        monsterground = transform.Find("MonsterGround");
        for (int monsterindex=0;monsterindex< monsterground.childCount; monsterindex++)
        {
            monster.Add(monsterground.GetChild(monsterindex));
            monsterground.GetChild(monsterindex).gameObject.SetActive(false);
        }
        residuenumber = 100;
        receivenumber = 0;
        residuetext = transform.Find("Text").GetComponent<Text>();
        receivetext = transform.Find("Text (1)").GetComponent<Text>();
        residuetext.text = "剩余数量：" + residuenumber.ToString();
        receivetext.text = "接到数量：" + receivenumber.ToString();
        StartCoroutine("MonsterIns");
        MessageCenter.AddMsgListener("GameStartOrStop_1", GamePlay);//开始or暂停s
        MessageCenter.AddMsgListener("GamePauseOrContinue", GameEnd);//开始or暂停
    }
    void GamePlay(ParameterData pa)
    {
        gameisplay = (bool)pa.data;
        if (gameisplay)
        {
            player.localPosition = new Vector3(0, player.localPosition.y, 0);
            player.gameObject.SetActive(true);
            for (int monsterindex = 0; monsterindex < monsterground.childCount; monsterindex++)
            {
                monster.Add(monsterground.GetChild(monsterindex));
                monsterground.GetChild(monsterindex).localPosition=new Vector3(monsterground.GetChild(monsterindex).localPosition.x, 27, 0);
                monsterground.GetChild(monsterindex).gameObject.SetActive(false);
            }
            residuenumber = 100;
            receivenumber = 0;
            PlayerManager.balangrade = 0;
            residuetext = transform.Find("Text").GetComponent<Text>();
            receivetext = transform.Find("Text (1)").GetComponent<Text>();
            residuetext.text = "剩余数量：" + residuenumber.ToString();
            receivetext.text = "接到数量：" + receivenumber.ToString();
            StartCoroutine("MonsterIns");
            
            //StartCoroutine("PlayerManager.GameIsPlaying");
        }
        else
        {
            DOTween.TogglePauseAll();
            gameisPause = false;
            //Destroy(gameObject);
            player.gameObject.SetActive(false);
            gameObject.SetActive(false);


        }
    }

    void GameEnd(ParameterData pa)
    {
        gameisPause = (bool)pa.data;
        if (gameisPause)
        {
            DOTween.TogglePauseAll();
           
        }
        else
        {
            DOTween.TogglePauseAll();
            
        }
    }

    IEnumerator MonsterIns()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (gameisplay && gameisPause)
            {
                if (intervaltime > 0)
                {
                    intervaltime -= 1;
                }
                else
                {

                    intervaltime = 200;
                    int i = Random.Range(0, 8);
                    if (i != currentindex&& residuenumber>0)
                    {
                        residuenumber -= 1;
                        residuetext.text = "剩余数量：" + residuenumber.ToString();
                        currentindex = i;
                        monster[i].localPosition = new Vector3(monster[i].localPosition.x, 27, 0);
                        monster[i].gameObject.SetActive(true);
                        monster[i].GetComponent<Image>().color = new Color(1,1,1,1);
                        if (monster[i].localPosition.x - player.localPosition.x > 0)
                        {
                            AudioPlayer.instantiate.PlayerTherapeutistAudio("请往右一点");//往右
                            right.gameObject.SetActive(true);
                            left.gameObject.SetActive(false);
                        }
                        else
                        {
                            AudioPlayer.instantiate.PlayerTherapeutistAudio("请往左一点");//往左
                            right.gameObject.SetActive(false);
                            left.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
