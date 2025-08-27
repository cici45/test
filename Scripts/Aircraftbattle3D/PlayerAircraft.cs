using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAircraft : MonoBehaviour
{
    //public Button button;
    Transform player;
    List<Transform> fallingstones=new List<Transform>();
    Text residuenumber;
    Text hinttext;
    public static Text accomplishnumber;
    int residuenumber_=100;
    int accomplishnumber_=0;
    int timeinterval=2;
    bool gameisplay=false;
    bool gameisPause=false;
    float currentAngle=0;
    int angle;
    public static float grade;
    FallingstonesMove fallingstonesMove = new FallingstonesMove();

    Transform Scapula_L;
    Transform Shoulder_L;
    Transform Elbow_L;
    Transform Scapula_R;
    Transform Shoulder_R;
    Transform Elbow_R;

    Vector3 Scapula_L_Angel;
    Vector3 Shoulder_L_Angel;
    Vector3 Elbow_L_Angel;
    Vector3 Scapula_R_Angel;
    Vector3 Shoulder_R_Angel;
    Vector3 Elbow_R_Angel;

    Transform front;
    Transform later;
    private void Awake()
    {
        
        player = transform;
        for(int i = 0; i < transform.parent.GetChild(2).childCount; i++)
        {
            //Debug.Log(transform.GetChild(2).GetChild(i).name);
            fallingstones.Add(transform.parent.GetChild(2).GetChild(i).transform);
        }
        residuenumber = transform.parent.Find("residuenumberText").GetComponent<Text>();
        accomplishnumber= transform.parent.Find("accomplishnumberText").GetComponent<Text>();
        hinttext = transform.parent.Find("hintText").GetComponent<Text>();
        MessageCenter.AddMsgListener("GameStartOrStop", GamePlay);//开始or暂停s
        MessageCenter.AddMsgListener("GamePauseOrContinue", GameEnd);//开始or暂停

        Scapula_L = transform.parent.Find("Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L");
        Shoulder_L = transform.parent.Find("Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L");
        Elbow_L = transform.parent.Find("Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L");
        Scapula_L_Angel = Scapula_L.localEulerAngles;
        Shoulder_L_Angel = Shoulder_L.localEulerAngles;
        Elbow_L_Angel = Elbow_L.localEulerAngles;

        Scapula_R = transform.parent.Find("Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R");
        Shoulder_R = transform.parent.Find("Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R");
        Elbow_R = transform.parent.Find("Rherapist/root/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R");
        Scapula_R_Angel = Scapula_R.localEulerAngles;
        Shoulder_R_Angel = Shoulder_R.localEulerAngles;
        Elbow_R_Angel = Elbow_R.localEulerAngles;

        front = transform.parent.Find("Image (1)");
        later = transform.parent.Find("Image (3)");
    }

    void GamePlay(ParameterData pa)
    {
        gameisplay = (bool)pa.data;
        if (gameisplay)
        {
            transform.parent.gameObject.SetActive(true);
            grade = 0;
            currentAngle = 0;
            Scapula_L.localEulerAngles = Scapula_L_Angel;
            Shoulder_L.localEulerAngles = Shoulder_L_Angel;
            Elbow_L.localEulerAngles = Elbow_L_Angel;

            Scapula_R.localEulerAngles = Scapula_R_Angel;
            Shoulder_R.localEulerAngles = Shoulder_R_Angel;
            Elbow_R.localEulerAngles = Elbow_R_Angel;

            Scapula_LCurrent_Angel = 0;
            Elbow_LCurrent_Angel = 0;
            Scapula_RCurrent_Angel = 0;
            Elbow_RCurrent_Angel = 0;

            gameisPause = true;
            residuenumber_ = 120;
            accomplishnumber_ = 0;
            player.localPosition = new Vector3(-45.6837f, -34.7f, -820.9f);
            residuenumber.text = "剩余陨石数量：" + residuenumber_;
            accomplishnumber.text = "躲过陨石数量：" + accomplishnumber_;
            PlayerRherapistAnimations.playerRherapist.Bow();
            front.gameObject.SetActive(false);
            later.gameObject.SetActive(false);
            StartCoroutine("CreateFallingstones");
            StartCoroutine("PlayerMove");
        }
        else
        {
            for (int index = 0; index < fallingstones.Count; index++)
            {
                fallingstones[index].gameObject.SetActive(false);
            }
            gameisPause = false;
            PlayerRherapistAnimations.playerRherapist.Wave();
            DOTween.TogglePauseAll();
            StopAllCoroutines();
            transform.parent.gameObject.SetActive(false);
        }
    }

    void GameEnd(ParameterData pa)
    {
        gameisPause = (bool)pa.data;
        if (gameisPause)
        {
            DOTween.TogglePauseAll();
            StartCoroutine("CreateFallingstones");
            StartCoroutine("PlayerMove");
        }
        else
        {
            DOTween.TogglePauseAll();
            StopAllCoroutines();
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        for(int index=0;index< fallingstones.Count; index++)
        {
            fallingstones[index].gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int realtimeangle;
    float Scapula_LCurrent_Angel=0;
    float Elbow_LCurrent_Angel = 0;
    float Scapula_RCurrent_Angel = 0;
    float Elbow_RCurrent_Angel = 0;
    IEnumerator PlayerMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (gameisplay&& gameisPause)
            {
               
                if (UserInfoData.CurrentAngle!= currentAngle&& UserInfoData.CurrentAngle<60)
                {
                    Debug.Log(UserInfoData.CurrentAngle);
                    //player.localPosition = new Vector3(player.localPosition.x, player.localPosition.y, player.localPosition.z + (UserInfoData.CurrentAngle - currentAngle));

                    Scapula_L.localEulerAngles = new Vector3(Scapula_L.localEulerAngles.x, Scapula_L.localEulerAngles.y, Scapula_L.localEulerAngles.z - (UserInfoData.CurrentAngle - Scapula_LCurrent_Angel));

                    Scapula_R.localEulerAngles = new Vector3(Scapula_R.localEulerAngles.x, Scapula_R.localEulerAngles.y, Scapula_R.localEulerAngles.z - (UserInfoData.CurrentAngle - Scapula_RCurrent_Angel));
                   
                    int elbow = (int)(90 * (UserInfoData.CurrentAngle / 60));

                    Elbow_R.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y+(elbow- Elbow_LCurrent_Angel), Elbow_R.localEulerAngles.z);
                    Elbow_L.localEulerAngles = new Vector3(Elbow_R.localEulerAngles.x, Elbow_R.localEulerAngles.y + (elbow - Elbow_RCurrent_Angel), Elbow_R.localEulerAngles.z);


                    currentAngle = UserInfoData.CurrentAngle;
                    Elbow_RCurrent_Angel = elbow;
                    Elbow_LCurrent_Angel = elbow;
                    Scapula_LCurrent_Angel = UserInfoData.CurrentAngle;
                    Scapula_RCurrent_Angel = UserInfoData.CurrentAngle;
                }
                else if ((int)UserInfoData.CurrentAngle<=5)
                {
                    Scapula_L.localEulerAngles = Scapula_L_Angel;
                    Scapula_R.localEulerAngles = Scapula_R_Angel;
                }
            }
        }
    }

   int fallingindex=-1;
    int pos = 0;
   IEnumerator CreateFallingstones()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (gameisplay&& gameisPause)
            {
                if (timeinterval > 0)
                {
                    timeinterval -= 1;
                }
                else if (timeinterval <= 0&& residuenumber_>0)
                {
                    timeinterval = 7;
                    residuenumber_ -= 1;
                    residuenumber.text = "剩余陨石数量：" + residuenumber_;
                    //Debug.Log("11");
                    if (fallingindex < 4)
                    {
                        fallingindex += 2;
                    }
                    else
                    {
                        fallingindex = 1;
                    }
                    if (pos == 0)
                    {
                        fallingstones[fallingindex].localPosition=new Vector3(220.6f, -47, -820);
                        fallingstones[fallingindex-1].localPosition = new Vector3(309.1f, -47, -660);
                        pos = 1;
                        hinttext.text = "请向前推动";
                        PlayerRherapistAnimations.playerRherapist.Speech_1();
                        AudioPlayer.instantiate.PlayerTherapeutistAudio("请往前推");//往前
                        front.gameObject.SetActive(true);
                        later.gameObject.SetActive(false);
                    }
                    else
                    {
                        fallingstones[fallingindex-1].localPosition = new Vector3(221.6f, -36, -730);
                        fallingstones[fallingindex].localPosition = new Vector3(309.1f, -47, -660);
                        pos = 0;
                        hinttext.text = "请放松，将手收回原位";
                        PlayerRherapistAnimations.playerRherapist.Speech_2();
                        AudioPlayer.instantiate.PlayerTherapeutistAudio("请往后拉");//往后
                        front.gameObject.SetActive(false);
                        later.gameObject.SetActive(true);

                    }
                    fallingstones[fallingindex].gameObject.SetActive(true);
                    fallingstones[fallingindex - 1].gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("Fallingstone"))
        {
            fallingstones[fallingindex].gameObject.SetActive(false);
            fallingstones[fallingindex - 1].gameObject.SetActive(false);
        }
    }

    void DOTweenPlay()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
