using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ag_Class_Test
{
    public class AG
    {
        public class Max_Min
        {
            public static double Position_Dir;
            public double Max_Value;
            public double Min_Value;
            public void Ini()
            {
                Max_Value = -65000 * Position_Dir;
                Min_Value = 65000 * Position_Dir;
            }
            public void Cal_Max_Min(double Data)
            {
                if (Data < Min_Value)
                {
                    Min_Value = Data;

                }
                else if (Data > Max_Value)
                {
                    Max_Value = Data;
                }
            }
        }
        double[] Data_Filter;
        List<double>[] V = new List<double>[5];
        List<double>[] ACC = new List<double>[5];
        int[] N_Cal_SW;
        int Time_Old;
        double Data_Old;

        int First_Sign = 0;

        double P_Set;
        double N_Set;
        int Range_Div_n;

        Max_Min[] Data_Max_Min_Cal;

        int Model2_Cout_Start = 0;

        double[] Old_Avr = new double[5];
        double[] New_Avr = new double[5];

        int Muscle_Endurance_Sign = 0;
        int Muscle_Endurance_Count = 0;
        List<double> Muscle_Endurance_V = new List<double>();//存储评估段所有速度
        bool Muscle_Endurance_Calculated; //0 not ,1 ted
        int Muscle_Endurance_time;
        double Muscle_Endurance_Value;

        List<double> Muscle_Endurance = new List<double>();

        private bool Cal(double Data_1, double Data_2, double Time_1, double Time_2, out double Data_back)
        {
            Data_back = 0;
            if (Time_1 == Time_2)
            {
                return false;
            }
            Data_back = (Data_2 - Data_1) / (Time_2 - Time_1);
            return true;

        }
        public void Ini(double Limit_P, double Limit_N, int Limit_Div_n)
        {
            First_Sign = 0;
            P_Set = Limit_P;
            N_Set = Limit_N;
            Range_Div_n = Limit_Div_n;


            N_Cal_SW = new int[Range_Div_n];
            Array.Clear(N_Cal_SW, 0, 5);

            Model2_Cout_Start = 0;
            Muscle_Endurance_Sign = 0;
            Muscle_Endurance_Count = 0;
            Muscle_Endurance_Calculated = false; //0 not ,1 ted
            Data_Max_Min_Cal = new Max_Min[Range_Div_n];
            Data_Filter = new double[10];
            for (int i = 0; i < Range_Div_n; i++)
            {
                Max_Min obj = new Max_Min();
                Data_Max_Min_Cal[i] = obj;
                List<double> temp1 = new List<double>();
                List<double> temp2 = new List<double>();
                V[i] = temp1;
                ACC[i] = temp2;
            }
        }


        double[] V_Main;
        double[] V_Max;
        double[] V_Min;
        double[] ACC_Main;
        double[] ACC_Max;
        double[] ACC_Min;

        public void Out_Data(out double Total_Data_Max,
                             out double Total_Data_Min,
                             out double Total_V_Main,
                             out double Total_V_Max,
                             out double[] V_Main,
                             out double[] V_Max,
                             out double[] V_Min,
                             out double Total_ACC_Main,
                             out double Total_Acc_Max,
                             out double[] ACC_Main,
                             out double[] ACC_Max,
                             out double[] ACC_Min,
                             out int n_Feedback,
                             out double Muscle_State,
                             out double Endurance_time,
                             out double Endurance_Value
                                )
        {
            double[] Theta_Max_Save = new double[Range_Div_n];
            double[] Theta_Min_Save = new double[Range_Div_n];
            V_Main = new double[Range_Div_n];
            V_Max = new double[Range_Div_n];
            V_Min = new double[Range_Div_n];
            ACC_Main = new double[Range_Div_n];
            ACC_Max = new double[Range_Div_n];
            ACC_Min = new double[Range_Div_n];
            for (int i = 0; i < Range_Div_n; i++)
            {
                Theta_Max_Save[i] = Data_Max_Min_Cal[i].Max_Value;
                Theta_Min_Save[i] = Data_Max_Min_Cal[i].Min_Value;

                //Debug.Log("V[" + i + "].Count : " + V[i].Count);
                //---------20230705---修改 防止ACC与V的第i项没有数据
                if (V[i].Count > 0)
                {
                    V_Main[i] = V[i].Average();
                    V_Max[i] = V[i].Max();
                    V_Min[i] = V[i].Min();
                }
                if (ACC[i].Count > 0)
                {
                    ACC_Main[i] = ACC[i].Average();
                    ACC_Max[i] = ACC[i].Max();
                    ACC_Min[i] = ACC[i].Min();
                }
                //---------20230705---修改 防止ACC与V的第i项没有数据
                //V_Main[i] = V[i].Average();
                //V_Max[i] = V[i].Max();
                //V_Min[i] = V[i].Min();

                //ACC_Main[i] = ACC[i].Average();
                //ACC_Max[i] = ACC[i].Max();
                //ACC_Min[i] = ACC[i].Min();
            }

            Total_ACC_Main = ACC_Max.Average();
            double ACC_Var_Sum = 0;
            for (int i = 0; i < Range_Div_n; i++)
            {
                ACC_Var_Sum = System.Math.Pow(ACC_Max[i] - Total_ACC_Main, 2);

            }
            Muscle_State = System.Math.Sqrt(ACC_Var_Sum);

            Total_V_Main = V_Main.Average();

            n_Feedback = Range_Div_n;

            Total_Data_Max = Theta_Max_Save.Max();
            Total_Data_Min = Theta_Min_Save.Min();

            Total_V_Max = V_Max.Max(); ;
            Total_Acc_Max = ACC_Max.Max();

            Endurance_time = Muscle_Endurance_time;
            Endurance_Value = Muscle_Endurance_Value;
            //Debug.Log("Endurance_Value : " + Endurance_Value);
        }

        string[] _name;
        string[] _data;
        double v;
        double acc;

        //202307051632------------------------------------
        int Accumulate_Count = 0;//202307051632

        public void Data_Input_Instead(double Data_In, int Real_Time_ms, int model, int Accumulate_Times = 10)//202307051632
        {

            if (Accumulate_Count++ >= Accumulate_Times)
            {
                Data_Input(Data_In, Real_Time_ms, model);
            }
        }
        //=//202307051632==========================

        public void Data_Input(double Data_In, int Real_Time_ms, int model)//model 2 circl, 1 nagative, 0 positive  
        {
            int Time_t = Real_Time_ms;

            if (First_Sign == 0)
            {
                for (int i = 0; i < Range_Div_n; i++)
                {
                    Data_Max_Min_Cal[i].Ini();
                }

                for (int i = 0; i < 5; i++)
                {
                    Data_Filter[i] = Data_In;
                }

                Time_Old = Time_t;
                First_Sign = 1;

            }
            else
            {
                Data_Old = Data_Filter.Average();
                Array.Copy(Data_Filter, 0, Data_Filter, 1, 9);
                Data_Filter[0] = Data_In;
                double Data_Temp = Data_Filter.Average();
                if (model == 2)
                {

                    if (Model2_Cout_Start >= 4)
                    {
                        Model2_Cout_Start = 4;

                        if (Data_Temp > P_Set)
                        {
                            Data_Temp = P_Set;
                        }
                        if (Data_Temp < N_Set)
                        {
                            Data_Temp = N_Set;
                        }
                        double temp_data = (Data_Temp - N_Set) * Range_Div_n / ((P_Set - N_Set) * 1.1);
                        int K = (int)temp_data;
                        if (K < 0)
                        {
                            K = 0;
                        }
                        Data_Max_Min_Cal[K].Cal_Max_Min(Data_Temp);

                        double Tem_V_Cal = 0;
                        if (Cal(Data_Old, Data_Temp, Time_Old, Time_t, out Tem_V_Cal))//change V[K][N_Cal_SW[K]] ->Tem_V_Cal 202307051212
                        {
                            V[K].Add(Tem_V_Cal);
                            v = Tem_V_Cal;
                            if (N_Cal_SW[K] != 0)
                            {

                                Cal(V[K][N_Cal_SW[K] - 1], V[K][N_Cal_SW[K]], Time_Old, Time_t, out Tem_V_Cal);//change ACC[K][N_Cal_SW[K]] ->Tem_V_Cal 202307051212

                                ACC[K].Add(Tem_V_Cal);
                                acc = Tem_V_Cal;
                            }

                            if (K == (int)(Range_Div_n / 2) && Muscle_Endurance_Sign == 0)
                            {
                                Muscle_Endurance_V.Add(V[K][N_Cal_SW[K]]);
                            }
                            N_Cal_SW[K]++;

                        }

                        //calculate the muscle endurance
                        if (K < (int)(Range_Div_n / 2))
                        {
                            Muscle_Endurance_Sign = 0;

                        }
                        if (K > (Range_Div_n / 2 + 1) && Muscle_Endurance_Sign == 0 && Muscle_Endurance_V.Count > 0)
                        {
                            Muscle_Endurance_Sign = 1;
                            Muscle_Endurance.Add(Muscle_Endurance_V.Average());

                            if (Muscle_Endurance_Calculated == false && Muscle_Endurance_V.Average() < Muscle_Endurance_V.Max() * 0.75)
                            {
                                Muscle_Endurance_Calculated = true;
                                Muscle_Endurance_time = Time_t;
                                Muscle_Endurance_Value = Muscle_Endurance_V.Average();

                            }
                            if (Muscle_Endurance_V.Average() > Muscle_Endurance_V.Max() * 0.75)
                            {
                                Muscle_Endurance_Calculated = false;

                            }
                        }//-----------------------------

                        _name = new string[] { "实时角度", "速度", "加速度" };
                        _data = new string[] { Data_In.ToString(), v.ToString(), acc.ToString() };
                        List<string[]> _list = new List<string[]>() { _data };
                        string temp = "A" + System.Text.RegularExpressions.Regex.Replace(UserInfoData.StartTime, @"[^0-9]+", "");
                        TestSettle.AngleTableName = temp;
                        OperateUseSQL_H.Add_Data(temp, _name, _list);

                        Time_Old = Time_t;
                    }
                    else if (Model2_Cout_Start++ >= 3)
                    {
                        Time_Old = Time_t;
                    }
                }

                Array.Copy(Data_Filter, 0, Old_Avr, 0, 5);
                Array.Copy(Data_Filter, 0, New_Avr, 0, 5);


                //if (model == 1)
                //{

                //    if (New_Avr.Average() < Old_Avr.Average())
                //    {
                //        Model2_Cout_Start = 4;

                //        int K = (int)(Data_Temp * Range_Div_n / (P_Set - N_Set));

                //        Data_Max_Min_Cal[K].Cal_Max_Min(Data_Temp);

                //        if (Cal(Data_Old, Data_Temp, Time_Old, Time_t, V[K][N_Cal_SW[K]]))
                //        {
                //            Cal(V[K][N_Cal_SW[K] - 1], V[K][N_Cal_SW[K]], Time_Old, Time_t, ACC[K][N_Cal_SW[K]]);
                //            N_Cal_SW[K]++;

                //            if (K == (int)(Range_Div_n / 2) && Muscle_Endurance_Sign == 0)
                //            {
                //                Muscle_Endurance_V[Muscle_Endurance_Count] = V[K][N_Cal_SW[K]];
                //                Muscle_Endurance_Count++;

                //            }
                //        }

                //        //calculate the muscle endurance
                //        if (K < (int)(Range_Div_n / 2))
                //        {
                //            Muscle_Endurance_Sign = 0;

                //        }
                //        if (K > (Range_Div_n / 2 + 1) && Muscle_Endurance_Sign == 0)
                //        {
                //            Muscle_Endurance_Sign = 1;
                //            Muscle_Endurance.Add(Muscle_Endurance_V.Average());

                //            if (Muscle_Endurance_Calculated == false && Muscle_Endurance_V.Average() < Muscle_Endurance_V.Max() * 0.75)
                //            {
                //                Muscle_Endurance_Calculated = true;
                //                Muscle_Endurance_time = Time_t;
                //                Muscle_Endurance_Value = Muscle_Endurance_V.Average();

                //            }
                //            if (Muscle_Endurance_V.Average() > Muscle_Endurance_V.Max() * 0.75)
                //            {
                //                Muscle_Endurance_Calculated = false;

                //            }


                //        }//-----------------------------


                //        _name = new string[] { "实时角度", "速度", "加速度" };
                //        _data = new string[] { Data_In.ToString(), V[K].Max().ToString(), ACC[K].Max().ToString() };

                //        List<string[]> _list = new List<string[]>() { _data };
                //        string temp = "A" + UserInfoData.StartTime;
                //        OperateUseSQL_H.Add_Data(temp, _name, _list);

                //        Time_Old = Time_t;
                //    }
                //    else
                //    {
                //        Time_Old = Time_t;
                //    }

                //}
                //if (model == 0)
                //{

                //    if (New_Avr.Average() > Old_Avr.Average())
                //    {
                //        Model2_Cout_Start = 4;

                //        int K = (int)(Data_Temp * Range_Div_n / (P_Set - N_Set));

                //        Data_Max_Min_Cal[K].Cal_Max_Min(Data_Temp);

                //        if (Cal(Data_Old, Data_Temp, Time_Old, Time_t, V[K][N_Cal_SW[K]]))
                //        {
                //            Cal(V[K][N_Cal_SW[K] - 1], V[K][N_Cal_SW[K]], Time_Old, Time_t, ACC[K][N_Cal_SW[K]]);
                //            N_Cal_SW[K]++;

                //            if (K == (int)(Range_Div_n / 2) && Muscle_Endurance_Sign == 0)
                //            {
                //                Muscle_Endurance_V[Muscle_Endurance_Count] = V[K][N_Cal_SW[K]];
                //                Muscle_Endurance_Count++;

                //            }
                //        }

                //        //calculate the muscle endurance
                //        if (K < (int)(Range_Div_n / 2))
                //        {
                //            Muscle_Endurance_Sign = 0;

                //        }
                //        if (K > (Range_Div_n / 2 + 1) && Muscle_Endurance_Sign == 0)
                //        {
                //            Muscle_Endurance_Sign = 1;
                //            Muscle_Endurance.Add(Muscle_Endurance_V.Average());

                //            if (Muscle_Endurance_Calculated == false && Muscle_Endurance_V.Average() < Muscle_Endurance_V.Max() * 0.75)
                //            {
                //                Muscle_Endurance_Calculated = true;
                //                Muscle_Endurance_time = Time_t;
                //                Muscle_Endurance_Value = Muscle_Endurance_V.Average();

                //            }
                //            if (Muscle_Endurance_V.Average() > Muscle_Endurance_V.Max() * 0.75)
                //            {
                //                Muscle_Endurance_Calculated = false;

                //            }


                //        }//-----------------------------


                //        _name = new string[] { "实时角度", "速度", "加速度" };
                //        _data = new string[] { Data_In.ToString(), V[K].Max().ToString(), ACC[K].Max().ToString() };

                //        List<string[]> _list = new List<string[]>() { _data };
                //        string temp = "A" + UserInfoData.StartTime;
                //        OperateUseSQL_H.Add_Data(temp, _name, _list);

                //        Time_Old = Time_t;
                //    }
                //    else
                //    {
                //        Time_Old = Time_t;
                //    }
                //}
            }
        }
    }
}
