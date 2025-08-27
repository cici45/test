using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRetransmission : MonoBehaviour
{
    enum UserInfo
    {
        phone,
        trainTime,
        state
    }
    List<string[]> data = new List<string[]>();
    int n = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (OperateUseSQL_H.IsTbleExist("DelayedUpload"))
        {
            OperateUseSQL_H.Read_Data("DelayedUpload",out data,out n);
            Debug.Log("data.count"+data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][3]=="false")
                {
                    Debug.Log(SettlePanel.settlePanel.Post("http://" + UserInfoData.Ip + "/comprehensive/inComprehensiveController", data[i][1]));
                    OperateUseSQL_H.UpdataData("DelayedUpload", UserInfo.phone.ToString(), data[i][0], UserInfo.trainTime.ToString(), data[i][2], UserInfo.state.ToString(), "true");
                    try
                    {
                  
                    }
                    catch (System.Exception)
                    {

                    }

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
