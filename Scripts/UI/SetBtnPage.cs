using UnityEngine;
using UnityEngine.UI;

public class SetBtnPage : MonoBehaviour
{
    public static SetBtnPage page;
    Button Btn_Puas, Btn_Play, Btn_Set;
    GameTimer timer;
    Text Text_Time, Text_Score;
    float sum = 0;
    void Start()
    {
        page = this;
        Text_Time = transform.Find("BG/Text_Time").GetComponent<Text>();
        Text_Score = transform.Find("BG/Text_Score").GetComponent<Text>();
        timer = transform.Find("BG").GetComponent<GameTimer>();
        Btn_Puas = transform.Find("Btn_Puas").GetComponent<Button>();
        Btn_Play = transform.Find("Btn_Play").GetComponent<Button>();
        Btn_Set = transform.Find("Btn_Set").GetComponent<Button>();
        Btn_Puas.onClick.AddListener(OnPuasButtonclick);
        Btn_Play.onClick.AddListener(OnPlayButtonclick);
        Btn_Set.onClick.AddListener(OnSetButtonclick);
        timer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Text_Score.text = PlanPlayer.gamegrade + "分";
    }

    private void OnSetButtonclick()
    {
        MessageCenter.SendMsg("SetPanel", true);
        timer.isTimerStop = true;
    }

    private void OnPuasButtonclick()
    {
        Debug.Log(1111111);
    }

    private void OnPlayButtonclick()
    {
        MessageCenter.SendMsg("PausePanel", true);
        timer.isTimerStop = true;
    }

    public void OnStartTime(float index)
    {
        sum = 0;
        timer.gameObject.SetActive(true);
        gameObject.SetActive(true);
        timer.CreatTimer(index, OnEndTime, Text_Time);
        MessageCenter.SendMsg("ReceiveData", true);
    }

    public void OnEndTime()
    {
        if (sum == 0)
        {
            sum++;
            AudioPlayer.instantiate.StopAllAudios();
            MessageCenter.SendMsg("ResultPanel", true);
            //MessageCenter.SendMsg("ContinueOrStopGame", true);
            AudioPlayer.instantiate.PlayerTherapeutistAudio("感谢您的使用，祝您早日康复");
            Invoke("BackGamePanel", 4f);
        }
    }

    void BackGamePanel()
    {
        if (TestSettle.IsTest)
        {
            MessageCenter.SendMsg("TestSettle", true);
        }
        else
        {
            MessageCenter.SendMsg("SettlePanel", true);
        }
        MessageCenter.SendMsg("ReceiveData", false);
        MessageCenter.SendMsg("ResultPanel", false); 
        MessageCenter.SendMsg("RestPanel", false);
        MessageCenter.SendMsg(PausePanel._GameName, false);
        timer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void BackTimePage()
    {
        MessageCenter.SendMsg(PausePanel._GameName, false);
        if (TestSettle.IsTest)
        {
            MessageCenter.SendMsg("TestSettle", true);
        }
        else
        {
            MessageCenter.SendMsg("SettlePanel", true);
        }
        MessageCenter.SendMsg("RestPanel", false);
        MessageCenter.SendMsg("ReceiveData", false);
        timer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
