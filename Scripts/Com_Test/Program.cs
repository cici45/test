using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Senser_Driver_Space;
using UnityEngine;

namespace Com_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("hello world");
            Senser_Driver Sensors= new Senser_Driver(Application.streamingAssetsPath + "/Com_XML.xml");
            double Angle_Feed = 0;
            while (true)
            {
                #region 读取编码器或拉线传感器

                if (Sensors.Sensor_Type == 0)
                { 
                Angle_Feed = Sensors.Read_Data(Angle_Feed);
                Console.WriteLine("Circle_Sensr"+Angle_Feed);
                Sensors.Delay(100);
                }
                if (Sensors.Sensor_Type == 1)
                {
                    Angle_Feed = Sensors.Read_Data(Angle_Feed);
                    Console.WriteLine("Cable_Sensr" + Angle_Feed);
                    Sensors.Delay(25);//发送指令中默认5ms延时
                }

                #endregion

                #region 读取AD值，最大值4095，需要设置force radio 进行转换为力值
                if (Sensors.Sensor_Type == 2)
                {
                    Angle_Feed = Sensors.Read_Data(Angle_Feed);
                    double AA = Sensors.Center_x;
                    double BB = Sensors.Center_y;
                    double Force1 = Sensors.Force_Read_Data[0];
                    double Force2 = Sensors.Force_Read_Data[1];
                    double Force3 = Sensors.Force_Read_Data[2];
                    double Force4 = Sensors.Force_Read_Data[3];

                    for(int i=0;i<4;i++)
                    { 
                    string Out_Inf = "ADsensor_";
                    Console.WriteLine(Out_Inf + (i+1) + "=" + Sensors.Force_Read_Data[i]);
                    }
                    Sensors.Delay(25);//发送指令中默认5ms延时
                }
                #endregion


                #region //Read_Scannner 读取扫码枪结果

                if (Sensors.Sensor_Type == 3)
                {
                    char[] Scannner_Resder;//= new char[1];

                  bool Succ_Sign= Sensors.Read_ASII(out Scannner_Resder);
                    if (Succ_Sign)
                    {
                        string Temp = new string (Scannner_Resder);
                        //Console.WriteLine( Scannner_Resder);
                        Console.WriteLine("Scanner_  "+Temp);
                    }
                  
                    Sensors.Delay(25);//发送指令中默认5ms延时
                }


                #endregion

            }

        }
    }
}
