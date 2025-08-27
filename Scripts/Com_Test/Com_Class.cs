//using UnityEngine;
/*https://blog.csdn.net/weixin_45375968/article/details/121760710 */

using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.CodeDom;
using UnityEngine;

namespace Com_Driver
{
public class PortControl 
{
    #region 定义串口属性
   
    //public GUIText Test;
    //定义基本信息
    public string portName = "COM3";//串口名
    public int baudRate = 9600;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
    public StopBits stopBits = StopBits.One;//停止位
    SerialPort sp = null;
    Thread dataReceiveThread;
    //发送的消息
    string message = "";
    public List<byte> listReceive = new List<byte>();
    char[] strchar = new char[100];//接收的字符信息转换为字符数组信息
    string str;
    #endregion
    public Xml_Read Paramters_Read;
    public int Com_Data_read_Length=0;
    public  byte[] Data_Com=new byte[1024];
        public int Com_Receieve_Length_Err = 1;
        public int Data_Read_Length=0;

        public PortControl(string Path)
        {
            Paramters_Read = new Xml_Read(Path);// Application.streamingAssetsPath + "/Com_XML.xml")
        }

        public  void Start()
    {
         

            OpenPort();
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();
    }
    void Update()
    {

    }

        #region 创建串口，并打开串口
        public void OpenPort()
        {


            portName = Paramters_Read.Com_ID;
            baudRate = Paramters_Read.Baudrate;
            // parity = Paramters_Read.Parity_Set;
            switch (Paramters_Read.Parity_Set)
            {
                case 0:
                    parity = Parity.None;
                    break;

                case 1:
                    parity = Parity.Odd;
                    break;
                case 2:
                    parity = Parity.Even;
                    break;
                default:
                    parity = Parity.None;
                    break;

            }
            dataBits = Paramters_Read.Data_Bits;
            //stopBits = Paramters_Read.Stop_Bits;
            switch (Paramters_Read.Stop_Bits)
            {
                case 0:
                    stopBits = StopBits.None;
                    break;

                case 1:
                    stopBits = StopBits.One;
                    break;
                case 2:
                    stopBits = StopBits.Two;
                    break;
                default:
                    stopBits = StopBits.OnePointFive;
                    break;

            }



            //创建串口
            sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            sp.ReadTimeout = 400;
            try
            {
                sp.Open();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    #endregion



    #region 程序退出时关闭串口
    void OnApplicationQuit()
    {
        ClosePort();
    }
    public void ClosePort()
    {
        try
        {
            sp.Close();
            dataReceiveThread.Abort();
        }
        catch (Exception ex)
        {
          //  Debug.Log(ex.Message);
        }
    }
    #endregion


    /// <summary>
    /// 打印接收的信息
    /// </summary>
    void PrintData()
    {
        for (int i = 0; i < listReceive.Count; i++)
        {
            strchar[i] = (char)(listReceive[i]);
            str = new string(strchar);
        }
      //  Debug.Log(str);
    }
    void Clear_Receive_Buffer()
    {
        if(sp.BytesToRead>0)//如果 有缓存数据
        {
        byte[] Buffer_Read = new byte[sp.BytesToRead];
        sp.Read(Buffer_Read, 0, sp.BytesToRead);
        }


    }

    //void Receive_Byte_Length()
    //{

    //    try
    //    {
    //        if (Com_Data_read_Length == 0)
    //        {


    //            Com_Data_read_Length = sp.BytesToRead;//获取所有缓存数据
    //            byte[] Buffer_Read = new byte[Com_Data_read_Length];
    //            Data_Com = new byte[Com_Data_read_Length];
    //            sp.Read(Buffer_Read, 0, Com_Data_read_Length);

    //            Data_Com = (byte[])Buffer_Read.Clone();

    //            // Buffer_Read.CopyTo(Data_Com,0);
    //            //Array.Copy(Buffer_Read,Data_Com,Buffer_Read.Length);

    //            Com_Data_read_Length = 0;

    //        }
    //        else
    //        {

    //            //获取定长数据；       //Com_Data_read_Length = sp.BytesToRead;//获取所有缓存数据
    //            byte[] Buffer_Read = new byte[Com_Data_read_Length];
        

    //            if (sp.BytesToRead >= Com_Data_read_Length)//判断内存长度
    //            {
    //                sp.Read(Buffer_Read, 0, Com_Data_read_Length);

    //              //  Data_Com = (byte[])Buffer_Read.Clone();
    //               Array.Copy(Buffer_Read,Data_Com,Buffer_Read.Length);



    //                }
    //            }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.Message);

    //    }



    //}

    #region 接收数据
    void DataReceiveFunction()
    {
            #region 按单个字节发送处理信息，不能接收中文
            //while (sp != null && sp.IsOpen)
            //{
            //    Thread.Sleep(Paramters_Read.Sampling_Time_ms/2);
            //    try
            //    {
            //        byte addr = Convert.ToByte(sp.ReadByte());
            //        sp.DiscardInBuffer();
            //        listReceive.Add(addr);
            //        PrintData();
            //    }
            //    catch
            //    {
            //        //listReceive.Clear();
            //    }
            //}
            #endregion


            #region 按字节数组发送处理信息，信息缺失
            //byte[] buffer = new byte[1024];
            //int bytes = 0;
            //while (true)
            //{
            //    if (sp != null && sp.IsOpen)
            //    {
            //        try
            //        {
            //            bytes = sp.Read(buffer, 0, buffer.Length);//接收字节
            //            if (bytes == 0)
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                string strbytes = Encoding.Default.GetString(buffer);
            //                //Debug.Log(strbytes);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            if (ex.GetType() != typeof(ThreadAbortException))
            //            {
            //            }
            //        }
            //    }
            //    Thread.Sleep(Paramters_Read.Sampling_Time_ms/2);
            //}
            #endregion

            #region 按设定长度接收Byte数据
            while (true)
            {
                try
                {
                    if (Com_Data_read_Length == 0)
                    {


                        Com_Data_read_Length = sp.BytesToRead;//获取所有缓存数据

                        if (Com_Data_read_Length!=0)
                        {
                            Data_Read_Length = Com_Data_read_Length;
                            Com_Receieve_Length_Err = 0;//有数据
                        byte[] Buffer_Read = new byte[Com_Data_read_Length];
                        // Data_Com = new byte[Com_Data_read_Length];
                        sp.Read(Buffer_Read, 0, Com_Data_read_Length);
                        Array.Copy(Buffer_Read, Data_Com, Buffer_Read.Length);

                        }
              


                        //Data_Com = (byte[])Buffer_Read.Clone();

                        // Buffer_Read.CopyTo(Data_Com,0);
                      //  Array.Copy(Buffer_Read,Data_Com,Buffer_Read.Length);

                        Com_Data_read_Length = 0;

                    }
                    else
                    {

                        //获取定长数据；       //Com_Data_read_Length = sp.BytesToRead;//获取所有缓存数据
                        byte[] Buffer_Read = new byte[Com_Data_read_Length];
                        //Data_Com = new byte[Com_Data_read_Length];

                      // Console.WriteLine("长度"+sp.BytesToRead+"接收长度"+ Com_Data_read_Length+"结束");


                        if (sp.BytesToRead >= Com_Data_read_Length)//判断内存长度
                        {
                            Com_Receieve_Length_Err = 0; //有数据
                            // sp.Read(Buffer_Read, 0, Com_Data_read_Length);
                            sp.Read(Data_Com, 0, Com_Data_read_Length);

                            //Data_Com = (byte[])Buffer_Read.Clone();


                            //String AA = "AA_";
                            //for (int j = 0; j < Com_Data_read_Length; j++)
                            //{
                            //    AA = AA + Data_Com[j] + " ";
                            //}
                            //Console.WriteLine(AA);


                            Clear_Receive_Buffer();//清除接收缓存

                        }
                        else
                        {
                           // Com_Receieve_Length_Err = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                Delay(5);
            }
        #endregion
    }
        #endregion

        public void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                //Application.DoEvents();
                //  Application.Rtf();
            }
        }

        #region 发送数据
        public void WriteData(string dataStr)
    {
        if (sp.IsOpen)
        {
            sp.Write(dataStr);

            //sp.Write(byte[] buffer, int offset, int count);
        }
    }
        /// <summary>
        /// 将String类型转为16进制Byte数组
        /// </summary>
        /// <param name="hexStrContent">16进制表示的字符串</param>
        /// <returns></returns>
        public byte[] ConvertStringToByteArray(string hexStrContent)
        {
            //if ((hexStrContent.Length % 2) != 0)
            //{
            //    hexStrContent = hexStrContent.Insert(0, 0.ToString()); //如果长度为奇，在开头补零
            //}
            hexStrContent = hexStrContent.Replace(" ", "");
            byte[] bText = new byte[hexStrContent.Length / 2];
            for (int i = 0; i < hexStrContent.Length / 2; i++)
            {
                bText[i] = Convert.ToByte(Convert.ToInt32(hexStrContent.Substring(i * 2, 2), 16));
                Debug.Log(bText[i] + "byte");
            }
            return bText;
        }

        public void Write_Byte_Data(byte[] buffer, int offset, int count)
        {
            if (sp.IsOpen)
            {
                sp.Write(buffer, offset, count);

                //sp.Write(byte[] buffer, int offset, int count);
            }
        }
        //void OnGUI()
        //{
        //    message = GUILayout.TextField(message);
        //    if (GUILayout.Button("Send Input"))
        //    {
        //        WriteData(message);
        //    }
        //    string test = "AA BB 01 12345 01AB 0@ab 发送";//测试字符串
        //    if (GUILayout.Button("Send Test"))
        //    {
        //        WriteData(test);
        //    }
        //}
        #endregion
    }
   

}
