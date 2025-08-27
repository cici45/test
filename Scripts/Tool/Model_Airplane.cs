using System;
using System.Collections.Generic;
using UnityEngine;

public class Model_Airplane     //�ɻ�������Ϸ���粿�Ͼ�/��������/�Ȳ����죩��1��
{

    List<Vector3> gold;
    double T_interval;
    int number;

    /// <summary>
    /// �ɻ�������Ϸ
    /// </summary>
    /// <param name="T_total">ѵ����ʱ��(s)</param>
    /// <param name="T_gap">�����ɽ�ҵ�ʱ��(s)</param>
    /// <param name="T_continuous">�����˶�ʱ��(s)</param>
    /// <param name="T_rest">��Ϣʱ��(s)</param>
    /// <param name="T_exercise">�˶�ʱ��(s)</param>
    /// <param name="T_keep">����ʱ��(s)</param>
    /// <param name="D">�Ѷȵȼ�ϵ��</param>
    /// <param name="ymin">���ɽ���ܴﵽ����С�����귶Χ</param>
    /// <param name="ymax">���ɽ���ܴﵽ����������귶Χ</param>
    /// <param name="modle_sin">����������� 0���Ϊsin������1���Ϊcos����</param>
    /// <param name="gold">���ص�λ�á��Ƿ��������Ƿ񲥷ŵ�Vector3����</param>
    /// <param name="number">����</param>
    /// <param name="T_interval">���ɼ��(s)</param>
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

        //��ô���ض��ֵ
        //Ҫ����������number�������ʱ�䣨T_interval��,����λ�ã�position����������������е�Ԫ��Ϊһ��vector2����,���vector2�ĵ�һ�������ǽ������������λ��(y),�ڶ����������ж��Ƿ����ɽ�ҵ��жϱ�����judge����1�����ɽ�ң�2�������ɽ�ң���Ϊ�г����˶�ʱ�����Ϣʱ�䣬���Ե�judge=1ʱ�������ɽ�ң�judge=0ʱ�����ɽ�ң�

    }
}