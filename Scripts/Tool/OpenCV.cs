using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class OpenCV : MonoBehaviour
{
    static string exepath = Application.streamingAssetsPath + "/Encorder_Reading_Singl_Release/Project1";
    void Start()
    {
        
    }

    public static void OpenExe()
    {
        Process.Start(exepath);
        UnityEngine.Debug.Log("打开进程");
    }

    public static void Offexe()
    {
        OffExe();
    }

    public static void OffExe()
    {
        Process[] processes = Process.GetProcesses();
        foreach(Process process in processes)
        {
            try
            {
                if (!process.HasExited)
                {
                    if(process.ProcessName== "Project1")
                    {
                        process.Kill();
                        UnityEngine.Debug.Log("进程已关闭！！");
                    }
                }
            }
            catch(System.InvalidOperationException ex)
            {
                UnityEngine.Debug.Log(ex);
            }
        }
    }
    //void OnDeisable()
    //{
    //    OffExe();
    //}
}
