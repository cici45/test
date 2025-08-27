using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections;
using XCharts;
using System.IO;
using System;
using UnityEngineInternal;
using Senser_Driver_Space;

public class ScoketServer : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern float SetCursorPos(float x, float y);
    Coroutine coroutine;
    string Msg = "ReceiveData";
    Senser_Driver senser;
    Thread thread;
    bool IsPalyer;
    public static float arrowshow_Time;
    //Senser_Driver Sensors;
    private void Awake()
    {
        senser = new Senser_Driver(Application.streamingAssetsPath + "/Com_XML.xml");
        string textTxt = File.ReadAllText(Application.streamingAssetsPath + "/IP/IPConfig.txt");
        string[] ip = textTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        UserInfoData.EquipmentName = ip[2].Split('：')[1];
        UserInfoData.Scale = float.Parse(ip[3].Split('：')[1]);
        UserInfoData.Unit = ip[4].Split('：')[1];
        model = int.Parse(ip[5].Split('：')[1]);
        arrowshow_Time= float.Parse(ip[7]);
        //Debug.Log("ooooooooooo"+(20.0 / 60.0));
    }

    // Start is called before the first frame update
    void Start()
    {
        //openServer();
        OpenRead();
        StartCoroutine(SaveTime());
        MessageCenter.AddMsgListener(Msg, StartRetData);
    }
    int Position_Offset = 0;
    void StartRetData(ParameterData parameter)
    {
        IsPalyer = (bool)parameter.data;
        if (IsPalyer)
        {
            Debug.Log(IsPalyer);
            //OpenCV.OpenExe();
            temp = 0;
            Position_Offset = Mathf.Abs((int)senser.Read_Data(temp) * (int)UserInfoData.Scale);
        }
        else
        {
            Position_Offset = 0;
            UserInfoData.CurrentAngle = 0;
            //senser.BackDriver();
        }
        issave = (bool)parameter.data;
        timer = 0;
    }

    void OpenRead()
    {
        thread = new Thread(ReadData);
        thread.IsBackground = true;
        thread.Start();
    }

    float temp = 0;
    int currentTemp;
    void ReadData()
    {
        try
        {
            while (true)
            {
                //temp++;
                //UserInfoData.CurrentAngle = (Mathf.Sin((float)temp / 50)) * 60;

               // if ((int)Mathf.Abs((int)senser.Read_Data(temp) / UserInfoData.Scale)- Position_Offset > 0)
                //{

                //    UserInfoData.CurrentAngle = (float)senser.Read_Data(temp) / UserInfoData.Scale;
                //    Debug.Log("编码器实际值：" + UserInfoData.CurrentAngle);
                //    UserInfoData.CurrentAngle = Mathf.Abs(UserInfoData.CurrentAngle);
                //}
                //  currentTemp = Mathf.Abs((int)senser.Read_Data(temp) * (int)UserInfoData.Scale);
                // currentTemp = (int)UserInfoData.CurrentAngle;

                currentTemp = Mathf.Abs((int)senser.Read_Data(temp) * (int)UserInfoData.Scale);
                //currentTemp = (int)UserInfoData.CurrentAngle;
                if (currentTemp == 0 || currentTemp == Position_Offset)
                {
                    //UserInfoData.CurrentAngle = 0;
                }
                else
                {
                    Debug.Log(currentTemp + "当前角度源数据");
                    UserInfoData.CurrentAngle = currentTemp;
                    if (IsPalyer)
                    {
                        timer += 30;
                        UserInfoData.class_ag.Data_Input_Instead(UserInfoData.CurrentAngle, timer, model);
                    }

                }
                Delay(30);
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void Delay(int milliSecond)
    {
        int start = Environment.TickCount;
        while (Math.Abs(Environment.TickCount - start) < milliSecond)
        {
            //Application.DoEvents();
            //Application.Rtf();
        }
    }

    /// <summary>
    /// 打开链接
    /// </summary>
    void openServer()
    {
        try
        {
            string textTxt = File.ReadAllText(Application.streamingAssetsPath + "/IP/IPConfig.txt");
            string[] ip = textTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            UserInfoData.EquipmentName = ip[2].Split('：')[1];
            UserInfoData.Scale = float.Parse(ip[3].Split('：')[1]);
            UserInfoData.Unit = ip[4].Split('：')[1];
            model = int.Parse(ip[5].Split('：')[1]);
            IPAddress pAddress = IPAddress.Parse(ip[0]);
            IPEndPoint pEndPoint = new IPEndPoint(pAddress, int.Parse(ip[1]));
            Socket socket_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket_server.Bind(pEndPoint);
            socket_server.Listen(5);//设置最大连接数
            Debug.Log("监听成功");
            //创建新线程执行监听，否则会阻塞UI，导致unity无响应
            Thread thread = new Thread(listen);
            thread.IsBackground = true;
            thread.Start(socket_server);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// 监听
    /// </summary>
    static Socket socketSend;
    string retData = null;
    bool wriet = true;
    bool issave = false;
    float time = 0;
    void listen(object o)
    {

        Socket socketWatch = o as Socket;
        try
        {
            while (true)
            {
                socketSend = socketWatch.Accept();
                Debug.Log(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");

                Thread r_thread = new Thread(Received);
                r_thread.IsBackground = true;
                r_thread.Start(socketSend);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    /// <summary>
    /// 获取消息
    /// </summary>
    /// <param name="o"></param>
    void Received(object o)
    {
        try
        {
            Socket socketSend = o as Socket;
            while (true)
            {

                byte[] buffer = new byte[1024];
                int len = socketSend.Receive(buffer);
                if (len == 0) break;
                if (issave)
                {
                    retData = Encoding.UTF8.GetString(buffer, 0, len);
                    //Debug.Log("服务器打印客户端返回消息：" + socketSend.RemoteEndPoint + ":" + retData);
                    ReadData(retData);
                }

            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }


    public static void close()
    {
        try
        {
            socketSend.Close();
            Debug.Log("关闭客户端连接");
        }
        catch (System.Exception)
        {
            Debug.LogError("未连接");
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    void Send(string msg)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(msg);
        socketSend.Send(buffer);
    }

    /// <summary>
    /// 修改鼠标位置
    /// </summary>
    /// <param name="data">接收到的数据</param>
    public void RetData(string data)
    {
        string[] retdata = data.Split('_');
        SetCursorPos(float.Parse(retdata[1]), float.Parse(retdata[2]));
    }

    IEnumerator SaveTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (wriet)
            {
                wriet = false;
            }

        }
    }
    int timer = 0;
    private int model;

    public string ReadData(string str)
    {
        string data = null;
        if (str != null)
        {
            string[] retdata = str.Split('_');
            if (retdata.Length > 2)
            {
                try
                {
                    data = retdata[1];
                    if (float.Parse(data) < 0)
                        data = Mathf.Abs(float.Parse(data)).ToString();
                    if (float.Parse(data) == 0)
                    {
                        data = "0";
                    }
                    else
                    {
                        UserInfoData.CurrentAngle = (float.Parse(data) / UserInfoData.Scale) % 360;
                    }
                }
                catch (Exception)
                {
                    Debug.Log("字符串传入错误");
                }
                timer += 10;
                UserInfoData.class_ag.Data_Input_Instead(UserInfoData.CurrentAngle, timer, model);
            }
            else
            {
                data = null;
            }
        }
        else
        {
            data = null;
        }
        return data;
    }
    private void OnDestroy()
    {
        thread.Abort();
    }
}

public struct TableName
{
    public const string tableName = "Temporary_Angle";

    public static string[] colName = new string[] { "Angle" };
}