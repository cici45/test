using System;
using System.Collections.Generic;
using UnityEngine;

public class Model_Airplane     //飞机捡金币游戏（肩部上举/推胸拉背/腿部屈伸）（1）
{

    List<Vector3> gold;
    double T_interval;
    int number;

    /// <summary>
    /// 飞机捡金币游戏
    /// </summary>
    /// <param name="T_total">训练总时长(s)</param>
    /// <param name="T_gap">不生成金币的时间(s)</param>
    /// <param name="T_continuous">连续运动时间(s)</param>
    /// <param name="T_rest">休息时间(s)</param>
    /// <param name="T_exercise">运动时间(s)</param>
    /// <param name="T_keep">保持时间(s)</param>
    /// <param name="D">难度等级系数</param>
    /// <param name="ymin">生成金币能达到的最小纵坐标范围</param>
    /// <param name="ymax">生成金币能达到的最大纵坐标范围</param>
    /// <param name="modle_sin">金币生成曲线 0金币为sin函数，1金币为cos函数</param>
    /// <param name="gold">返回的位置、是否生成与是否播放的Vector3数组</param>
    /// <param name="number">个数</param>
    /// <param name="T_interval">生成间隔(s)</param>
    public void Input_Ini(double T_total, double T_gap, double T_continuous, double T_rest, double T_exercise, double T_keep, int D, double ymin, double ymax, int modle_sin, out List<Vector3> gold, out int number, out double T_interval)
    {
        double v0 = 20;
        gold = new List<Vector3>();
        number = 0;
        //double D_factor = 0.5;
        int[] L = { 5, 6, 7 };
        int judge, video_play;
        T_interval = (T_exercise + T_keep) / L[D];
        int unit_number_continuous = (int)((T_continuous / T_interval) + 1);
        int unit_number_continuous_rest = (int)(((T_continuous + T_rest) / T_interval) + 1);
        int total_number = (int)(T_total / T_interval) + 1;
        int number_gap = (int)((T_exercise + T_keep + T_gap) / T_interval) + 1;
        //double v = D * v_factor * v0;
        double position;
        int odd_judge = L[D] % 2;
        int odd_mod = L[D] / 2;
        for (int i = 0; i < total_number; i++)
        {
            int i_continuous_rest_rem = i % (unit_number_continuous_rest - 1);
            int i_gap_rem = i_continuous_rest_rem % (number_gap - 1);

            if (i_continuous_rest_rem < unit_number_continuous - 1)
            {
                if (i_gap_rem < L[D])
                {
                    if (odd_judge == 0)
                    {
                        position = (ymax - ymin) * Math.Sin(i_gap_rem * Math.PI / L[D] + modle_sin * Math.PI) + (1 - modle_sin) * ymin + modle_sin * ymax;
                    }
                    else
                    {
                        if (i_gap_rem <= odd_mod)
                        {
                            position = (ymax - ymin) * Math.Sin(0.5 * i_gap_rem * Math.PI / odd_mod + modle_sin * Math.PI) + (1 - modle_sin) * ymin + modle_sin * ymax;
                        }
                        else
                        {
                            position = (ymax - ymin) * Math.Sin(0.5 * (i_gap_rem + 1) * Math.PI / (L[D] - odd_mod) + modle_sin * Math.PI) + (1 - modle_sin) * ymin + modle_sin * ymax;
                        }

                    }
                    judge = 1;
                    video_play = 0;
                    Vector3 arr = new Vector3((float)position, judge, video_play);
                    gold.Add(arr);
                    number++;
                }
                else
                {
                    position = 0;
                    judge = 0;
                    video_play = 0;
                    Vector3 arr = new Vector3((float)position, judge, video_play);
                    gold.Add(arr);
                }
            }
            else
            {
                position = 0;
                judge = 0;
                video_play = 1;
                Vector3 arr = new Vector3((float)position, judge, video_play);
                gold.Add(arr);
            }
            Console.WriteLine(gold[i]);
        }

        //怎么返回多个值
        //要返回数量（number），间隔时间（T_interval）,生成位置（position）（其中这个数组中的元素为一个vector2类型,这个vector2的第一个数字是金币纵坐标生成位置(y),第二个数字是判断是否生成金币的判断变量（judge），1：生成金币，2：不生成金币，因为有持续运动时间和休息时间，所以当judge=1时正常生成金币，judge=0时不生成金币）

    }
}