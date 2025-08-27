using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCPrinter;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathRSA : Singleton<MathRSA>
{
    private MathRSA()
    {
        count = 10;
        flag = true;
    }
    private List<float> initialListData = new List<float>();
    private List<float> filterListData = new List<float>();
    private int Dicindex = 1;
    
    public float d;
    public int GradeNum;
    private bool flag = true;
    private int count = 10;

    /// <summary>
    /// 添加原始的数据 0.25f添加一次
    /// </summary>
    /// <param name="value"></param>
    public void initial(float value)
    {
        if (initialListData.Count <  count)
        {
            initialListData.Add(value);
        }
        else
        {
            for (int i = 0; i < initialListData.Count-1; i++)
            {
                initialListData[i] = initialListData[i + 1];
            }

            initialListData[count - 1] = value;
            filterFun();
        }
    }

    private float tempValue = 0;
    /// <summary>
    /// 数据滤波算法
    /// </summary>
    public void filterFun()
    {
        //满10条数据调用， 
        filterListData.Add(initialListData.Sum() / count);
        d = filterListData.Max();
    }
    /// <summary>
    /// 这个方法是游戏结束的时候调用，所使用的数据均位数据滤波得出的
    /// </summary>
    public void GameOverCallback()
    {
        try
        {
            EndFuntion();
        }
        catch (Exception)
        {
            Debug.Log("数据为空！！！！！！！");
        }
    }
    /// <summary>
    /// 画图 以及计算肌力稳定性的函数
    /// </summary>
    private void Ptheta(List<float> b, List<int> v, int k, int A, int B, ref float[] X, ref float[] Y)
    {
        int bm = A - 3;
        int BM = B + 3;
        int db = (BM - bm) / k;
        List<int> x = new List<int>();
        for (int i = bm; i < k * db + bm; i += db)
        {
            x.Add(i);
        }
        float[] y = new float[k];
        float[] nb = new float[k];
        if (b.Count > 0 && v.Count > 0)
        {
            for (int i = 0; i < b.Count; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (j < b.Count && j < x.Count - 1)
                    {
                        if (b[i] >= x[j] && b[i] < x[j + 1])
                        {
                            y[j] += v[i];
                            nb[j] += 1;
                        }
                    }
                }
                if (b[i] == x[x.Count - 1])
                {
                    y[y.Length - 1] += v[i];
                    nb[nb.Length - 1] += 1;
                }
            }
            int index = 0;
            for (int i = bm + db / 2; i < BM - db / 2; i += db)
            {
                if(index<X.Length)
                {
                    X[index] = i;
                    index++;
                }
            }
            for (int i = 0; i < X.Length; i++)
            {
                if (i < nb.Length)
                {
                    if (double.IsNaN(y[i] / nb[i]))
                        Y[i] = 0;
                    else
                        Y[i] = (y[i] / nb[i]);
                }
            }
        }
    }
    /// <summary>
    /// 计算速度的方差函数，用以计算肌力稳定性
    /// </summary>
    /// <returns></returns>
    private float[] Muscle(List<float> b, List<int> v, int k, int A, int B, ref float[] X, ref float[] Y,ref float[] X1,ref float[] Y1)
    {
        Ptheta(b, v, k, A, B,ref X,ref Y);
        float[] v0 = Y;
        int bm = A - 3;
        int BM = B + 3;
        int db = (BM - bm) / k;
        List<int> x = new List<int>();
        for (int i = bm; i <k*db+bm ; i+=db)
        {
            x.Add(i);
        }
        float[] y = new float[k];
        float[] nb = new float[k];
        if(b.Count > 0&&v.Count>0)
        {
            for (int i = 0; i < b.Count; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (j < b.Count && j < x.Count - 1)
                    {
                        if (b[i] >= x[j] && b[i] < x[j + 1])
                        {
                            y[j] += (int)Mathf.Pow((v[i] - v0[j]), 2);
                            nb[j] += 1;
                        }
                    }
                }

                if (b[i] == x[x.Count - 1])
                {
                    y[y.Length - 1] += (int)Mathf.Pow((v[i] - v0[v0.Length - 1]), 2);
                    nb[nb.Length - 1] += 1;
                }
            }
            int index = 0;
            for (int i = bm + db / 2; i < BM - db / 2; i += db)
            {
                if (index < X1.Length)
                {
                    X1[index] = i;
                    index++;
                }
            }

            for (int i = 0; i < X1.Length; i++)
            {
                if (i < nb.Length && i < y.Length)
                    if (double.IsNaN(y[i] / nb[i]))
                        Y1[i] = 0;
                    else
                        Y1[i] = (y[i] / nb[i]);
            }
        }
       
        return nb;
    }
    public float[] Xend = new float[5];
    public float[] Yend = new float[5];
    public float[] Xend1 = new float[5];
    public float[] Yend1 = new float[5];
    private float ts = 0.2f;
    private float d1 ;
    public void EndFuntion()
    {
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data == null && data.Count == 0)
        {
            Debug.LogWarning("数据为空");
            return;
        }
        d1 = filterListData.Max();
        int hv = 1;
        int mv = filterListData.Count;
        List<int> v = new List<int>();
        for (int i = 0; i < mv-hv; i++)
        {
            v.Add((int)((filterListData[i+hv]-filterListData[i])/(hv*ts)));
        }
        int Vmax = v.Max();
        int VMin = v.Min();
        List<int> vAbs = new List<int>();
        for (int i = 0; i < v.Count; i++)
        {
            vAbs.Add(Mathf.Abs(v[i]));
        }
        int vp = vAbs.Sum() / v.Count;
        int hr = 1;
        int mr = v.Count;
        List<int> r = new List<int>();
        for (int i = 0; i < mr-hr; i++)
        {
            r.Add((int)((v[i+hr]-v[i])/(hr*ts)));
        }
        int rMax = r.Max();
        int rMin = r.Min();
        List<int> rps = new List<int>();
        for (int i = 0; i < r.Count; i++)
        {
            rps.Add(Mathf.Abs(r[i]));
        }
        int rp = rps.Sum() / r.Count;
        int k = 5;
        List<float> b = new List<float>();
        for (int i = 1; i < filterListData.Count; i++)
        {
            b.Add(filterListData[i]);
        }
        float[] X = new float[5];
        float[] Y = new float[5];
        /*pt*/ 
        Ptheta(b, v, k, int.Parse(data[0][4]), int.Parse(data[0][3]), ref X, ref Y);
        float[] X1 = new float[5];
        float[] Y1 = new float[5];
        float[] X2 = new float[5];
        float[] Y2 = new float[5];
        /*mu*/  
        Muscle(b, v, k, int.Parse(data[0][4]), int.Parse(data[0][3]), ref X1,ref Y1,ref X2, ref Y2);
        /*mus*/ 
        float[] mus = Muscle(b, v, k, int.Parse(data[0][4]), int.Parse(data[0][3]), ref X,ref Y,ref X1, ref Y1);
        int mup = (int)((mus[1] + mus[mus.Length - 1]) / mus.Sum());
        List<int> p1 = new List<int>();
        for (int i = 0; i < k; i++)
        {
            p1.Add((1/(2*k))*1);
        }
        float p2 = 0.1f;
        float p3 = 0.3f;
        float w = 0;
        float h = 0;
        if (h == 0)
        {
            for (int i = 0; i < Y.Length; i++)
            {
                if(i<p1.Count)
                w += p1[i] * (int)Mathf.Exp(-1 * Y2[i]);
            }

            var exp = p2 * (int)Mathf.Exp(-1 * Y2[0]) + p2 * Mathf.Exp(-1 * Y2[Y2.Length - 1]) + p3 * mup;
            w += (int)exp;
        }
        else
        {
            for (int i = 0; i < Y2.Length; i++)
            {
                if (i < p1.Count)
                    w += p1[i] * (int)Mathf.Exp(-1 * Y2[i]);
            }
            for (int i = 0; i < Y.Length - 1; i++)
            {
                if (i < p1.Count)
                    w += p1[i] * (Y[i] / (Y[i] + 1));
            }
        }
        float s = (1 / 9) * d + (1 / 9) * Mathf.Abs(VMin) + (1 / 9) * Vmax + (1 / 9) * vp + (1 / 9) * Mathf.Abs(rMin) +
                  (1 / 9) * rMax + (1 / 9) * rp + (1 / 9) * w + (1 / 9) * g;
        Ptheta(b,vAbs,k, int.Parse(data[0][4]), int.Parse(data[0][3]), ref Xend,ref Yend);
        
        Xend = new float[5];
        Yend = new float[5];
        Muscle(b,vAbs,k, int.Parse(data[0][4]), int.Parse(data[0][3]), ref Xend,ref Yend,ref Xend1,ref Yend1);
        
        OperateUseSQL_H.Delete_DataAllValues("Temporary_resultValues_TrainMode");
        List<string[]> data1 = new List<string[]>();
        string[] col = new string[]
        {
            "最大运动角度", "正向运动最大角速度", "负向运动最大角速度", "平均运动角速度", "正向最大运动角加速度", "负向最大运动角加速度",
            "平均运动角加速度", "肌力稳定性","任务完成度","综合评定值"
        };
        string[] values = new string[]
        {
            d.ToString(), Vmax.ToString(), VMin.ToString(), vp.ToString(), rMax.ToString(),
            rMin.ToString(), rp.ToString(), w.ToString(), g.ToString(), s.ToString()
        };
        data1.Add(values);
        OperateUseSQL_H.Add_Data("Temporary_resultValues_TrainMode", col, data1);
    }
    
    private List<float> VKfilterList = new List<float>();
    private float time = 0.25f;
    private float VKMax, VKMin;
    /// <summary>
    /// vk（运动角速度）
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="emphasis"></param>
    public void VKfilter(int origin,int emphasis)
    {
        while (emphasis<=filterListData.Count-1)
        {
            float temp=filterListData[emphasis - 1] - filterListData[origin - 1];
            VKfilterList.Add(temp/ ((emphasis - origin)*time));
            emphasis++;
            origin++;
        }

        if (VKfilterList != null && VKfilterList.Count > 0)
        {
            VKMax = VKfilterList.Max();
            VKMin = VKfilterList.Min();
        }
        else
        {
            Debug.Log("VKfilterList集合为空");
            VKMax = 0;
            VKMin = 0;
        }
    }

    private float VpAverageValue;
    /// <summary>
    /// vp（平均运动角速度）
    /// </summary>
    public void VPAverage()
    {
        float tepmValue = 0;
        for (int i = 0; i < VKfilterList.Count; i++)
        {
            tepmValue += Mathf.Abs(VKfilterList[i]);
        }
        VpAverageValue = tempValue / VKfilterList.Count;
    }

    private List<float> RKList = new List<float>();
    private float RkMax, RKMin;
    /// <summary>
    /// 运动加角度
    /// </summary>
    public void RkAngleAddSpeed()
    {
        int index = VKfilterList.Count - 1;
        while (index>0)
        {
            RKList.Add((VKfilterList[index] - VKfilterList[index - 1])/time);
            index--;
        }
        if (RKList != null && RKList.Count > 0)
        {
            RkMax = RKList.Max();
            RKMin = RKList.Min();
        }
        else
        {
            Debug.Log("VKfilterList集合为空");
            RkMax = 0;
            RKMin = 0;
        }
    }

    private float RPAverageValue;
    /// <summary>
    /// 平均运动加角度
    /// </summary>
    public void RPAverage()
    {
        float tepmValue = 0;
        for (int i = 0; i < RKList.Count; i++)
        {
            tepmValue += Mathf.Abs(RKList[i]);
        }
        RPAverageValue = tempValue / RKList.Count;
    }

    private float W;
    /// <summary>
    /// 肌肉稳定性
    /// </summary>
    /// <param name="B0">需要达到的目标角度</param>
    /// <param name="E0">B0的波动范围</param>
    /// <param name="E1">E1为VK的波动范围</param>
    /// <param name="f">f为0，是等张，f为1，是等长</param>
    /// <returns></returns>
    public float MuscleStabilization()
    {
        int B0;
        int E0;
        int E1;
        int f;
        /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", //保持时间"HoldTime", 间隔时间"ExerciseTime", "TrainingType" */
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data != null && data.Count > 0)
        {
            B0 = int.Parse(data[0][3]);
            E0 = 10;
            E1 = 10;
            f = int.Parse(data[0][7]);
            List<float> tempList = new List<float>();
            List<float> tempList1 = new List<float>();
            for (int i = 0; i < filterListData.Count; i++)
            {
                float tempValue = Mathf.Pow(filterListData[i] - B0, 2) - Mathf.Pow(E0, 2);
                //[(bi-b0)平方-E0平方]
                if (tempValue > 0)
                {
                    tempList.Add(Mathf.Pow(tempValue, 2));
                }
            }

            var tempSum = tempList.Sum() / filterListData.Count;

            for (int i = 0; i < VKfilterList.Count; i++)
            {
                float tempValue = Mathf.Pow(VKfilterList[i] - E1, 2) - Mathf.Pow(E1, 2);
                if (tempValue > 0)
                {
                    tempList1.Add(Mathf.Pow(tempValue, 2));
                }
            }
            var tempSum1 = tempList1.Sum() / VKfilterList.Count;
            W = (1 - f) * tempSum + f * tempSum1;
            return W;
        }
        else
        {
            Debug.Log("没有读到数据！！！！！！！！");
            return 0;
        }
    }

    private float g=0; // todo 待计算
    private float p = 0.11f;
    private float S = 0;
    /// <summary>
    /// 计算综合评定值
    /// </summary>
    /// <returns>返回计算的值</returns>
    public float CalculateS()
    {
        S=d * p + Math.Abs(VKMin) * p + VKMax * p + VpAverageValue * p + Math.Abs(RKMin) * p + RkMax * p +
          RPAverageValue * p + W * p + g * p;
        return S;
    }
    
    

    #region 等长与等张的算法
    /// <summary>
    /// 
    /// </summary>
    /// <param name="intType">用来判断调用等长还是等张，0为等长，1是等张</param>
    /// <param name="TotalTime">训练时间</param>
    /// <param name="level">选择的等级</param>
    /// <param name="T">等长公式里面用的T</param>
    /// <param name="MaxValue">传入正极限的值</param>
    /// <param name="MinValue">传入负极限的值</param>
    string tableName;
    public float IsotonicOrEquilongFunc()
    {
        int intType;
        int TotalTime;
        int level;
        int T;
        float MaxValue;
        float MinValue;
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data != null && data.Count > 0)
        {
            intType = int.Parse(data[0][7]);
            TotalTime = int.Parse(data[0][1]);
            level = int.Parse(data[0][2]);
            T = int.Parse(data[0][5]);
            Debug.Log(data[0][3]);
            MaxValue = float.Parse(data[0][3]);
            MinValue = float.Parse(data[0][4]);
            
            int K;
            int T0 = 1;
            int T1 = 1;
            if (intType == 0){
                K = 0;
                T0 = EquilongFunc(level, T);
            }
            else{
                K = 1;
                T1 = IsotonicgFunc(level);
            }
            float t = K * Mathf.Sin(2 * Mathf.PI * TotalTime / T0) + (1 - K) * Mathf.Sin(2 * Mathf.PI * TotalTime / T1);
            /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", //保持时间"HoldTime", 间隔时间"ExerciseTime", "TrainingType" */
            List<string> col = new List<string>
                {"KeepTime","IntervalTime" ,"MaxValue","MinValue","num" };
            int tempnum = (int.Parse(data[0][5]) + int.Parse(data[0][6]));
            int tempValue = int.Parse(data[0][1])*60/tempnum;
            List<string> values = new List<string>
                { data[0][5], data[0][6],data[0][3], data[0][4],tempValue.ToString() };
            OperateUseSQL_H.Delete_DataAllValues("Temporary_GeneralTab_TrainMode");
            List<string[]> data1 = new List<string[]>();
            data1.Add(values.ToArray());
            OperateUseSQL_H.Add_Data("Temporary_GeneralTab_TrainMode", col.ToArray(), data1);
            
            if (t >= 0) {
                //返回正极限
                return MaxValue;
            }
            else {
                //返回负极限
                return MinValue;
            }
        }
        else
        {
            Debug.Log("data 为 null");
            return 0f;
        }
        
    }

    private int K = -1;

    /// <summary>
    /// 等长
    /// </summary>
    /// <returns></returns>
    public int EquilongFunc(int level,int T)
    {
        int T0 = 0;
        switch (level)
        {
            case 10:
                T0 = 10 + T;
                break;
            case 7:
                T0 = 7 + T;
                break;
            case 5:
                T0 = 5 + T;
                break;
            default:
                T0 = -1;
                break;
        }
        K = 0;
        return T0;
    }
    /// <summary>
    /// 等张
    /// </summary>
    /// <param name="level">游戏的难度等级</param>
    /// <returns></returns>
    public int IsotonicgFunc(int level)
    {
        int T1 = 0;
        switch (level)
        {
            case 5:
                T1 = 5;
                break;
            case 7:
                T1 = 7;
                break;
            case 10:
                T1 = 10;
                break;
            default:
                T1 = -1;
                break;
        }
        K = 1;
        return T1;
    }

    #endregion

    /// <summary>
    /// 积分模型 调用的时候需要做归位处理,调用时机在用户达到规定的角度就要调用
    /// </summary>
    /// <param name="a">给定的标准位置</param>
    /// <param name="a0">得到的实时位置</param>
    /// <param name="T0">等长或等张运动的周期时间</param>
    /// <param name="t0">用户界面设置的训练总时长</param>
    /// <param name="e0">等长训练或等张训练的标准位置的阈值</param>
    /// <returns></returns>
    private int StandardNum = 0;
    // ReSharper disable Unity.PerformanceAnalysis
    public void Scores(float a0)
    {
        float a;  //MaxAugle
        float T0; //HoldTime", "ExerciseTime"
        float t0; //GameTime
        float e0 =5;
        
        /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" */
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data != null && data.Count > 0)
        {
            a = float.Parse(data[0][3]);
            T0 = float.Parse(data[0][5])+float.Parse(data[0][6]);
            t0 = float.Parse(data[0][1]) * 60;

            float tempScores = 0;
            if (Math.Pow(a - a0, 2) - Math.Pow(e0, 2) <= 0)
            {
                tempScores = -100/(t0/T0);
            }
            else
            {
                tempScores = 100/(t0/T0);
                StandardNum++;
            }
            g = StandardNum / (t0/T0);
            GradeNum += Mathf.CeilToInt(tempScores);
            GradeNum = Mathf.Clamp(GradeNum, 0, 100);
        }
        else
        {
            Debug.Log("当前数据为空 Is 386");
        }
    }

    public void Scores(bool flag)
    {
        float T0; //HoldTime", "ExerciseTime"
        float t0; //GameTime
        float e0 =5;
        
        /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" */
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data != null && data.Count > 0)
        {
            T0 = float.Parse(data[0][5])+float.Parse(data[0][6]);
            t0 = float.Parse(data[0][1])*60;
        
            float tempScores = 0;
            if (flag)
            {
                tempScores = 100/(t0/T0);
            }
            else
            {
                tempScores = -100/ (t0/T0);
            }
            Debug.Log(tempScores);
            g = StandardNum / (t0/T0);
            GradeNum += Mathf.CeilToInt(tempScores);
            GradeNum = Mathf.Clamp(GradeNum, 0, 100);
        }
        else
        {
            Debug.Log("当前数据为空 Is 386");
        }
        
    }
    private List<int[]> levelList = new List<int[]>() { new int[]{3,8} ,new int[]{5,12},new int[]{9,15} };
    private List<int[]> _intslist = new List<int[]>();
    /// <summary>
    /// 拧密码的训练模型
    /// </summary>
    /// <param name="T0">更换密码的时间间隔</param>
    /// <param name="T">游戏总时间</param>
    /// <param name="D">游戏等级，共3个等级</param>
    public void TweakPassWork()
    {
        int T0;
        int T;
        int D;
        
        /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" */
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data == null && data.Count == 0)
        {
            Debug.LogWarning("数据为空");
            return;
        }
        T0 = int.Parse(data[0][6]) + int.Parse(data[0][5]);
        T = int.Parse(data[0][1])*60;
        D = GetGameLevel();
        int n = Mathf.CeilToInt(T / T0);
        int a = levelList[D-1][0];
        int b = levelList[D-1][1];
        int y1, y2, y3;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            if (b >= 11)
                y1 = Random.Range(1, 9);
            else
                y1 = Random.Range(1, b - 2);
            if ((b - 1 - y1) <= 9)
                y2 = Random.Range(1, (b - 1 - y1));
            else
                y2 = Random.Range(1, 9);
            if ((a - y1 - y2) <= 0)
            {
                if ((b - y1 - y2) <= 9)
                {
                    y3 = Random.Range(1, (b - y1 - y2));
                }
                else
                {
                    y3 = Random.Range(1, 9);
                }
            }
            else
            {
                if ((b - y1 - y2) <= 9)
                {
                    y3 = Random.Range((a - y1 - y2), (b - y1 - y2));
                }
                else
                {
                    y3 = Random.Range((a - y1 - y2), 9);
                }
            }
            sb.Append(y1 + "" + y2 + "" + y3 + "_");
        }
        OperateUseSQL_H.Delete_DataAllValues("Temporary_SafeBoxData_TrainMode");
        List<string[]> data1 = new List<string[]>();
        OperateUseSQL_H.Add_Data("Temporary_SafeBoxData_TrainMode",
            new string[] {"MaxAngle", "MinAngle", "KeepTime", "Data"},
            new List<string[]>() {new string[] {data[0][3],data[0][4],T0.ToString(),sb.ToString()}});
    }

    public int GetGameLevel()
    {
        int D;
        int type;
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data == null && data.Count == 0)
        {
            Debug.LogWarning("数据为空");
            return 0;
        }
        D = int.Parse(data[0][2]);
        type = int.Parse(data[0][7]);
        int level=-1;
        if (type == 0)
        {
            switch (D)
            {
                case 10:
                    level = 1;
                    break;
                case 7:
                    level = 2;
                    break;
                case 5:
                    level = 3;
                    break;
            }
        }
        else
        {
            switch (D)
            {
                case 5:
                    level = 1;
                    break;
                case 7:
                    level = 2;
                    break;
                case 10:
                    level = 3;
                    break;
            }
        }
        return level;
    }

    /// <summary>
    /// 赛车游戏车辆生成车道的方法
    /// </summary>
    /// <param name="totalTime">游戏总时长</param>
    /// <param name="timer">生成车辆的间隔时间</param>
    public void Racing()
    {
        int totalTime;
        int timer;
        /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" */
        tableName = "Temporary_TrainMode";
        OperateUseSQL_H.Read_Data(tableName, out List<string[]> data, out int Read_Data_number);
        if (data == null && data.Count == 0)
        {
            Debug.LogWarning("当前数据为空");
            return;
        }
        totalTime = int.Parse(data[0][1])*60;
        timer = int.Parse(data[0][6])+int.Parse(data[0][5]);
        
        StringBuilder sb= new StringBuilder();
        int num = totalTime / timer;
        for (int i = 0; i <= num; i++)
        {
            sb.Append((int)Mathf.Pow(-1, i)+"|");
        }
        OperateUseSQL_H.Delete_DataAllValues("Temporary_RacingGames_TrainMode");
        List<string[]> data1 = new List<string[]>();
        string[] col = new string[]
        {"MaxAugle","MinAugle","KeepTime","pos"};
        string[] values = new string[]{ data[0][3],data[0][4],timer.ToString(),sb.ToString()};
        data1.Add(values);
        OperateUseSQL_H.Add_Data("Temporary_RacingGames_TrainMode", col, data1);
    }

   
    private void Ptheta(List<float> b, List<float> v, int k,ref StringBuilder Xlist,ref StringBuilder Ylist)
    {

        float bMax = b.Max();
        float bMin = b.Min();
        // ReSharper disable once PossibleLossOfFraction
        float db = (bMax - bMin) / k;
        List<float> xlist = new List<float>();
        for (float i = bMin; i < bMax; i += db)
        {
            xlist.Add(i);
        }
        float[] yList = new float[k];
        float[] nbList = new float[k];
        if (xlist.Count > 0)
        {
            for (int i = 0; i < b.Count; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i <= b.Count - 1 && j <= xlist.Count - 2 && i <= v.Count - 1)
                    {
                        if (b[i] >= xlist[j] && b[i] < xlist[(j + 1) == K - 1 ? K - 1 : j + 1])
                        {
                            yList[j] += v[i];
                            nbList[j] += 1;
                        }
                    }
                }

                if (b[i] == xlist[xlist.Count - 1])
                {
                    yList[yList.Length - 1] = yList[yList.Length - 1] + v[i];
                    nbList[nbList.Length - 1] = nbList[nbList.Length - 1] + 1;
                }
            }
            for (float i = bMin + db / 2; i < bMax - db / 2; i += db)
            {
                Xlist.Append(i + "_");
            }
            for (int i = 0; i < yList.Length; i++)
            {
                Ylist.Append(yList[i] / nbList[i] + "_");
            }
        }

    }

    private StringBuilder VKsb;
    private StringBuilder VKsb1;
    /// <summary>
    /// 绘制运动角速度vk关于运动角度d的折线图
    /// </summary>
    /// <returns></returns>
    public void VKFunc()
    {
        VKsb = new StringBuilder();
        VKsb1 = new StringBuilder();
        if (filterListData.Count > 0 && VKfilterList.Count > 0)
        {
            Ptheta(filterListData, VKfilterList, 20, ref VKsb, ref VKsb1);
        }
        else
        {
            Debug.Log("filterListData 或 VKfilterList 数据为空");
        }
    }
    private StringBuilder RKsb;
    private StringBuilder RKsb1;
    /// <summary>
    /// 绘制运动角加速度rk关于运动角度d的折线图，r为以前评估指标中的角加速度
    /// </summary>
    /// <returns></returns>
    public void RKFunc()
    {
        RKsb = new StringBuilder();
        RKsb1 = new StringBuilder();
        if (filterListData.Count > 0 && RKList.Count > 0)
        {
            Ptheta(filterListData, RKList, 20, ref RKsb, ref RKsb1);
        }
        else
        {
            Debug.Log("filterListData 或 RKList 数据为空");
        }
    }

    /// <summary>
    /// 项目二飞机大战算法
    /// </summary>
    /// <param name="x"></param>
    /// <param name="timer"></param>
    /// <param name="T0">更换密码的时间间隔</param>
    /// <param name="T">游戏总时间</param>
    /// <param name="D">游戏等级，共3个等级</param>
    public List<int[]> PlaneArithmetic(int x, ref int timer, ref float max,ref float min)
    {
        int D;
        int T0;
        int T;
        int D0 = 2;
        int H1 = 5;
        int H2 = 10;
        List<int[]> list = new List<int[]>() {new int[] {4, 8}, new int[] {2, 6}, new int[] {0, 4}};
        List<int[]> p = new List<int[]>();
        /*"ProgrammeName", "GameTime", "Difficulty", "MaxAugle", "MinAugle", "HoldTime", "ExerciseTime", "TrainingType" */
        OperateUseSQL_H.Read_Data("Temporary_TrainMode", out List<string[]> data, out int Read_Data_number);
        if (data == null && data.Count == 0)
        {
            Debug.LogWarning("数据为空");
            return null;
        }
        T0 = int.Parse(data[0][6]) + int.Parse(data[0][5]);
        timer = T0;
        max = float.Parse(data[0][3]);
        min = float.Parse(data[0][4]);
        T = int.Parse(data[0][1])*60;
        D = GetGameLevel();
        int n = Mathf.CeilToInt(T / T0);
        int a = list[D-1][0];
        int b = list[D-1][1];
        int h;
        int d;
        for (int i = 0; i < n; i++)
        {
            int k = (int)Mathf.Pow(-1, i);
            if (k <= 0)
            {
                h = H1;
                d = x + D0 + Random.Range(a, b);
            }
            else
            {
                h = H2;
                d = x + D0 + Random.Range(a, b);
            }
            p.Add(new int[]{h,d});
        }
        return p;
    }
}