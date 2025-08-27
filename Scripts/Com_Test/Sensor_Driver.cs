using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com_Driver;
using static System.Net.Mime.MediaTypeNames;
namespace Senser_Driver_Space
{
    internal class Senser_Driver
    {
       
        
        public int Sensor_Type=-1;
       public PortControl Com_232;
        int First_Start = 1;
        double Angle_Old,Angle_New = 0;
        long Circle_Counter = 0;
        bool Data_Tansfer_Err =false;
        long[] Readed_AD;
        public double[] Force_Read_Data;
        byte[] Send_Buff = new byte[20];
        int[] Sensor_ID ;
        double[] Sensor_Offset ;
        double Force_Ratio;
        double[] Sensor_x;
        double[] Sensor_y;
        public double Center_x = 0, Center_y = 0;
        public Senser_Driver(string Path)
        {
            Com_232 = new PortControl(Path);
            ini();
        }

        public void ini()
        {
            Sensor_Type = Com_232.Paramters_Read.Senser_Type;
             Com_232.Start();
            First_Start = 1;
            Circle_Counter = 0;
            Angle_Old = 0;
            Readed_AD=new long[8];
            Force_Read_Data=new double[8];

             Sensor_ID = new int[8] { Com_232.Paramters_Read.Sensor_1_ID, Com_232.Paramters_Read.Sensor_2_ID,
                                        Com_232.Paramters_Read.Sensor_3_ID, Com_232.Paramters_Read.Sensor_4_ID,
                                        4,5,6,7};
             Sensor_Offset = new double[8] { Com_232.Paramters_Read.Sensor_1_Offset, Com_232.Paramters_Read.Sensor_2_Offset,
                                        Com_232.Paramters_Read.Sensor_3_Offset, Com_232.Paramters_Read.Sensor_4_Offset, 
                                            0,0,0,0};
            Force_Ratio = Com_232.Paramters_Read.Force_Ratio;
            Sensor_x = new double[4]{ Com_232.Paramters_Read.Sensor_1_x, Com_232.Paramters_Read.Sensor_2_x,
                                        Com_232.Paramters_Read.Sensor_3_x, Com_232.Paramters_Read.Sensor_4_x };
            Sensor_y = new double[4]{ Com_232.Paramters_Read.Sensor_1_y, Com_232.Paramters_Read.Sensor_2_y,
                                        Com_232.Paramters_Read.Sensor_3_y, Com_232.Paramters_Read.Sensor_4_y };
            Center_x = 0;

            Center_y = 0;

            if (Sensor_Type == 0)
            {
                Com_232.Com_Data_read_Length = 7;
            }

            else if (Sensor_Type == 1)
            {
                Com_232.Com_Data_read_Length = 9;

            }

            else if (Sensor_Type == 2)
            {
                Com_232.Com_Data_read_Length = 21;
            }


             else if (Sensor_Type == 3)
            {
                Com_232.Com_Data_read_Length = 0;
            }



    }

    public  double Read_Data(  double data)
        {
            double Data_Temp=0;
            if (Sensor_Type == 0)
            {
                Data_Temp= Get_Circle_Encorder_Data();
               
                if (!Data_Tansfer_Err)
                {
                    data = Data_Temp;
                    return data;
                }

            }

            else if (Sensor_Type == 1)
            {

                Data_Temp = Get_Cable_Encorder_Data();
                if (!Data_Tansfer_Err)
                {
                    data = Data_Temp;
                    return data;
                }

            }

            else if (Sensor_Type == 2)
            {
              
                    Calculate_AD_Parametwers();
                data= Data_Temp;
                return data;

            }

            //    data =0;

            return data;



        }

        public bool Read_ASII(out char [] ASII_Code)
        {

            if (Com_232.Com_Receieve_Length_Err == 0)
            {
                byte[] Read_Buff = new byte[Com_232.Data_Read_Length];
                for (int i = 0; i < Com_232.Data_Read_Length; i++)
                {
                    Read_Buff[i] = 0;
                }

               
                    Array.Copy(Com_232.Data_Com, Read_Buff, Com_232.Data_Read_Length);

                //char [] Temp=new char[Com_232.Data_Read_Length];

                //Array.Copy(Read_Buff, Temp, Com_232.Data_Read_Length);

                ASII_Code=new char[Com_232.Data_Read_Length];

                Array.Copy(Read_Buff, ASII_Code, Com_232.Data_Read_Length);


                //Console.WriteLine(ASII_Code);

                Com_232.Com_Receieve_Length_Err = 1;

               
                return true;
            }
            ASII_Code = null;
            return false;
        }

        public bool Read_Buff(out byte[] buff)
        {
            buff = null;
            if (Com_232.Com_Receieve_Length_Err == 0)
            {
                byte[] Read_Buff = new byte[Com_232.Data_Read_Length];
                for (int i = 0; i < Com_232.Data_Read_Length; i++)
                {
                    Read_Buff[i] = 0;
                }


                Array.Copy(Com_232.Data_Com, Read_Buff, Com_232.Data_Read_Length);

                Com_232.Com_Receieve_Length_Err = 1;
                buff = Read_Buff;
                return true;
            }
            return false;
        }
        double Get_Cable_Encorder_Data()
        {
            double Data_Return = 0;

            Send_CMD(Sensor_Type);

            Data_Return = Read_Encorder_Cable_Angle();

           
            if (!Data_Tansfer_Err)
            {
                return Data_Return;
            }
            else
            {

                return -9999999999;
            }


        }
        double Get_Circle_Encorder_Data()
        {
            double Data_Return=0;

            Send_CMD(Sensor_Type);

            Data_Return = Read_Encorder_Circle_Angle();
            if (!Data_Tansfer_Err)
            {
                return Data_Return;
            }
            else
            {

                return -9999999999;
            }

  
        }
        uint Crc_Count(byte []pbuf, uint num)
        {
            int i, j; uint wcrc = 0xffff;
            for (i = 0; i < num; i++)
            {
                wcrc ^= (uint)(pbuf[i]);
            for (j = 0; j < 8; j++)
            {

                if ((wcrc & 0x0001)==1)
                {
                    wcrc >>= 1; wcrc ^= 0xa001;
                }
                else
                {
                    wcrc >>= 1;
                }
            }
        }
return wcrc;
}

      public  void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                //Application.DoEvents();
                //  Application.Rtf();
            }
        }
     void Send_CMD (int SensorType_Seted)
        {
        
            if(SensorType_Seted==0)
            { 
            Send_Buff[0] = (byte)Com_232.Paramters_Read.Encorder_ID;
            Send_Buff[1] = 0x03;
            Send_Buff[2] = 0x00;
            Send_Buff[3] = 0x00;
            Send_Buff[4] = 0x00;
            Send_Buff[5] = 0x01;

            uint Data_Read = Crc_Count(Send_Buff, 6);



            Send_Buff[7] = (byte)((Data_Read & 0xFF00) >> 8);
            Send_Buff[6] = (byte)(Data_Read & 0xFF);

            // Com_232->write(Send_Buff, 8);
            Com_232.Write_Byte_Data(Send_Buff, 0, 8);
             }

          
            if (SensorType_Seted == 1)
            {
                Send_Buff[0] = (byte)Com_232.Paramters_Read.Encorder_ID;
                Send_Buff[1] = 0x03;
                Send_Buff[2] = 0x00;
                Send_Buff[3] = 0x00;
                Send_Buff[4] = 0x00;
                Send_Buff[5] = 0x02;

                uint Data_Read = Crc_Count(Send_Buff, 6);



                Send_Buff[7] = (byte)((Data_Read & 0xFF00) >> 8);
                Send_Buff[6] = (byte)(Data_Read & 0xFF);

                // Com_232->write(Send_Buff, 8);
                Com_232.Write_Byte_Data(Send_Buff, 0, 8);
            }



            if (SensorType_Seted == 2)
            {
                Send_Buff[0] = (byte)Com_232.Paramters_Read.Encorder_ID;
                Send_Buff[1] = 0x03;
                Send_Buff[2] = 0x00;
                Send_Buff[3] = 0x00;
                Send_Buff[4] = 0x00;
                Send_Buff[5] = 0x08;

                uint Data_Read = Crc_Count(Send_Buff, 6);



                Send_Buff[7] = (byte)((Data_Read & 0xFF00) >> 8);
                Send_Buff[6] = (byte)(Data_Read & 0xFF);

                // Com_232->write(Send_Buff, 8);
                Com_232.Write_Byte_Data(Send_Buff, 0, 8);
            }


            //Sleep(10);
            Delay(5);

        }
    long Read_Encorder_Circle_Puls(int read_length)
            {
            Data_Tansfer_Err = false;
            long Encorder_Puls_Read=0;
            byte [] Read_Buff=new byte[read_length];
            for (int i = 0; i < read_length; i++)
            {
                Read_Buff[i] = 0;
            }

         if(Com_232.Com_Receieve_Length_Err==0)
            {
                //   Read_Buff = (byte[])Com_232.Data_Com.Clone();

                Array.Copy(Com_232.Data_Com, Read_Buff, read_length);

                Com_232.Com_Receieve_Length_Err = 1;
                // Com_232->read(Read_Buff,7,NULL);
                String AA = "BB_";

                //AA+=B.ToString;

                //for (int j = 0; j < read_length; j++)
                //{
                //    AA = AA + Com_232.Data_Com[j] + " ";


                //}


                //Console.WriteLine(AA);


                int i = 0;
               // for (int i = 0; i < read_length; i++)
                {
                    if (Read_Buff[i + 0] == Com_232.Paramters_Read.Encorder_ID && Read_Buff[i + 1] == 0x03)
                    {
                        long [] Data= { 0, 0, 0, 0 };
                        Data[2] = (Read_Buff[i + 3] & 0xFF) << 8;
                        Data[3] = (Read_Buff[i + 4] & 0xFF);
                       uint CRC_Sum = 0;
                        CRC_Sum = Crc_Count(Read_Buff, 5);
                        long Tem_CRC_H = (Read_Buff[i + 6] & 0xff) << 8;
                        long  Tem_CRC_L = Read_Buff[i + 5] & 0xff;

                        if (CRC_Sum == (Tem_CRC_H + Tem_CRC_L))
                        {

                       
                            Encorder_Puls_Read = Data[2] + Data[3];
                     
                            return Encorder_Puls_Read;
                        }
                      
                    }

                }
              }
            Data_Tansfer_Err = true;

            return -1;
        }

        long Read_Encorder_Cable_Puls(int read_length)
        {
            Data_Tansfer_Err = false;
            long Encorder_Puls_Read = 0;
            byte[] Read_Buff = new byte[read_length];
            for (int i = 0; i < read_length; i++)
            {
                Read_Buff[i] = 0;
            }
            if (Com_232.Com_Receieve_Length_Err == 0)
            {
                Read_Buff = (byte[])Com_232.Data_Com.Clone();
                // Com_232->read(Read_Buff,7,NULL);

                Com_232.Com_Receieve_Length_Err = 1;
                int i = 0;
               // for (int i = 0; i < read_length; i++)
                {
                    if (Read_Buff[i + 0] == Com_232.Paramters_Read.Encorder_ID && Read_Buff[i + 1] == 0x03)
                    {
                        long[] Data = { 0, 0, 0, 0 };
                        Data[0] = (Read_Buff[i + 3] & 0xFF) << 24;
                        Data[1] = (Read_Buff[i + 4] & 0xFF) << 16;
                        Data[2] = (Read_Buff[i + 5] & 0xFF) << 8;
                        Data[3] = (Read_Buff[i + 6] & 0xFF);
                        uint CRC_Sum = 0;
                        CRC_Sum = Crc_Count(Read_Buff, (uint)(read_length - 2));
                        long Tem_CRC_H = (Read_Buff[i + read_length - 1] & 0xff) << 8;
                        long Tem_CRC_L = Read_Buff[i + read_length - 2] & 0xff;

                        if (CRC_Sum == (Tem_CRC_H + Tem_CRC_L))
                        {

                      
                            Encorder_Puls_Read = Data[0] + Data[1] + Data[2] + Data[3]; ;
                            return Encorder_Puls_Read;
                        }
                    
                    }

                }
            }

            Data_Tansfer_Err = true;
            return -9999999999;
        }


        double Read_Encorder_Circle_Angle()
        {
            int CMD_Feed_Length = 7;
            double Temp = Read_Encorder_Circle_Puls(CMD_Feed_Length) ;

               Temp = (Temp * 360) / Com_232.Paramters_Read.Encorder_Puls;
            //string Test = "Test_"+ Temp;
            //Console.WriteLine(Test);

            Temp = (double)((int)(Temp * 1000 + 0.5)) / 1000 - Com_232.Paramters_Read.Position_Offset;
           double  Angle_New = Temp;
            if (First_Start==1)
            {
                First_Start = 0;
            }
            else
            {
                if (Angle_New - Angle_Old < -180)
                {

                    Circle_Counter++;
                }

                if (Angle_New - Angle_Old > 180)
                {
                    Circle_Counter--;
                }
            }
            Angle_Old = Angle_New;

            Temp = Temp + Com_232.Paramters_Read.MutiCircle_Select * Circle_Counter * 360;

            Temp = Com_232.Paramters_Read.Position_Dir * Temp;



            return Temp;
        }


        double Read_Encorder_Cable_Angle()
        {
            int CMD_Feed_Length = 9;
            double Temp = Read_Encorder_Cable_Puls(CMD_Feed_Length);
                  Temp = Temp * 360 / Com_232.Paramters_Read.Encorder_Puls;
            Temp = (double)((int)(Temp * 1000 + 0.5)) / 1000 - Com_232.Paramters_Read.Position_Offset;
            double Angle_New = Temp;
            //if (First_Start == 1)
            //{
            //    First_Start = 0;
            //}
            //else
            //{
            //    if (Angle_New - Angle_Old < -180)
            //    {

            //        Circle_Counter++;
            //    }

            //    if (Angle_New - Angle_Old > 180)
            //    {
            //        Circle_Counter--;
            //    }
            //}
            //Angle_Old = Angle_New;

           // Temp = Temp + Com_232.Paramters_Read.MutiCircle_Select * Circle_Counter * 360;

            Temp = Com_232.Paramters_Read.Position_Dir * Temp;



            return Temp;
        }
      

     void Read_AD_Value( int read_length)
        {
            Send_CMD(Sensor_Type);
            Data_Tansfer_Err = false;
            if (Com_232.Com_Receieve_Length_Err == 0)
            {
                byte[] Read_Buff = new byte[read_length];
                Com_232.Com_Receieve_Length_Err = 1;

                Read_Buff = (byte[])Com_232.Data_Com.Clone();

                int i = 0;
                // for (int i = 0; i < 7; i++)
                {
                    if (Read_Buff[i + 0] == Com_232.Paramters_Read.Encorder_ID && Read_Buff[i + 1] == 0x03)
                    {
                        long[] Data ={0,0,0,0,
                                        0,0,0,0,
                              0,0,0,0,
                              0,0,0,0   };


                        for (int j = 0; j < 16; j += 2)
                        {
                            Data[j + 0] = (Read_Buff[i + j + 3] & 0xFF) << 8;
                            Data[j + 1] = (Read_Buff[i + j + 4] & 0xFF);
                        }


                        uint CRC_Sum = 0;
                        CRC_Sum = Crc_Count(Read_Buff, 19);
                        long Tem_CRC_H = (Read_Buff[i + 20] & 0xff) << 8;
                        long Tem_CRC_L = Read_Buff[i + 19] & 0xff;

                        if (CRC_Sum == (Tem_CRC_H + Tem_CRC_L))
                        {

                            for (int j = 0; j < 16; j += 2)
                            {
                                Readed_AD[j / 2] = Data[j + 0] + Data[j + 1];
                            }

                        }

                    }

                }
                Data_Tansfer_Err = true;
               //delete [] Read_Buff; 释放发送内存
                
            }




        }



     void Calculate_AD_Parametwers()
        {
            

            Read_AD_Value(21);


            if (!Data_Tansfer_Err)
            {

                double Force_Sum = 0;
                for (int i = 0; i < 8; i++)
                {
                    Force_Read_Data[i] = (Readed_AD[Sensor_ID[i]] - Sensor_Offset[i]) * Force_Ratio;
                    Force_Sum += Force_Read_Data[i];
                }




                if (Force_Sum != 0)
                {
                    Center_x = (Force_Read_Data[0] * Sensor_x[0] + Force_Read_Data[1] * Sensor_x[1] + Force_Read_Data[2] * Sensor_x[2]) / Force_Sum;
                    Center_y = (Force_Read_Data[0] * Sensor_y[0] + Force_Read_Data[1] * Sensor_y[1] + Force_Read_Data[2] * Sensor_y[2]) / Force_Sum;
                }



            }

        }


    }

   

   

}
