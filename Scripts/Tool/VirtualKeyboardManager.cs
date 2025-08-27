using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;

public class VirtualKeyboardManager: MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private InputField targetInputField;
    private float preloadDelay = 2f;
    private bool enablePreload = true;
    private bool debugMode = true;

    private Process keyboardProcess;
    private IntPtr keyboardWindowHandle = IntPtr.Zero;
    private bool isKeyboardActive = false;
    private bool isKeyboardPreloaded = false;

    // Windows API 声明
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);

    // 窗口状态常量
    private const int SW_SHOW = 5;
    private const int SW_MINIMIZE = 6;
    private const int SW_RESTORE = 9;

    // 委托类型
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    void Start()
    {
        if (targetInputField == null)
            targetInputField = GetComponent<InputField>();

        if (targetInputField == null)
        {
            LogError("未找到InputField组件，请在Inspector中指定");
            return;
        }

     
            StartCoroutine(PreloadKeyboard());
    }

    // 预加载键盘
    private IEnumerator PreloadKeyboard()
    {


        // 尝试查找现有键盘进程
        if (FindKeyboardWindow())
        {
            isKeyboardPreloaded = true;
            MinimizeKeyboard();
            LogDebug("找到并预加载现有键盘进程");
        }
        else
        {
            // 启动新的键盘进程
            if (LaunchKeyboard(true))
            {
                // 等待键盘窗口初始化
                float startTime = Time.time;
                while (Time.time - startTime < 3f)
                {
                    yield return null;

                    if (FindKeyboardWindow())
                    {
                        isKeyboardPreloaded = true;
                        MinimizeKeyboard();
                        LogDebug("成功预加载新键盘进程");
                        yield break;
                    }
                }

                LogError("键盘预加载超时，未能找到键盘窗口");
            }
        }
    }

    // 实现接口方法
    public void OnSelect(BaseEventData eventData)
    {
            ShowKeyboard();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideKeyboard();
    }

    // 显示键盘
    public void ShowKeyboard()
    {
        ShowWindow(keyboardWindowHandle, SW_RESTORE);
       // SetForegroundWindow(keyboardWindowHandle);
        //ShowWindow(keyboardWindowHandle, SW_RESTORE);
        //try
        //{
        //    // 检查键盘是否已运行
        //    if (!FindKeyboardWindow())
        //    {
        //        LogDebug("启动新的键盘进程");

        //        if (!LaunchKeyboard(false))
        //        {
        //            LogError("无法启动屏幕键盘");
        //            return;
        //        }

        //        // 等待键盘窗口初始化
        //        float startTime = Time.time;
        //        while (Time.time - startTime < 3f)
        //        {
        //            if (FindKeyboardWindow())
        //                break;

        //            System.Threading.Thread.Sleep(100);
        //        }
        //    }

        //    if (keyboardWindowHandle != IntPtr.Zero)
        //    {
        //        // 恢复并激活键盘窗口
        //        if (IsIconic(keyboardWindowHandle))


        //        SetForegroundWindow(keyboardWindowHandle);
        //        isKeyboardActive = true;
        //        LogDebug("键盘已显示");
        //    }
        //    else
        //    {
        //        LogError("无法找到键盘窗口");
        //    }
        //}
        //catch (Exception e)
        //{
        //    LogError($"显示键盘失败: {e.Message}");
        //}
    }

    // 隐藏键盘
    public void HideKeyboard()
    {
        if (isKeyboardActive && keyboardWindowHandle != IntPtr.Zero)
        {
            MinimizeKeyboard();
            isKeyboardActive = false;
            LogDebug("键盘已隐藏");
        }
    }

    // 关闭键盘进程
    public void CloseKeyboard()
    {
        try
        {
            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                keyboardProcess.Kill();
                keyboardProcess.WaitForExit(1000);
            }

            keyboardProcess = null;
            keyboardWindowHandle = IntPtr.Zero;
            isKeyboardActive = false;
            isKeyboardPreloaded = false;

            LogDebug("键盘进程已关闭");
        }
        catch (Exception e)
        {
            LogError($"关闭键盘失败: {e.Message}");
        }
    }

    // 启动键盘进程
    private bool LaunchKeyboard(bool isPreload)
    {
        try
        {
            // 先尝试关闭现有进程
            CloseKeyboard();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "osk.exe",
                WindowStyle = isPreload ? ProcessWindowStyle.Minimized : ProcessWindowStyle.Normal,
                UseShellExecute = true
            };

            keyboardProcess = Process.Start(startInfo);

            if (keyboardProcess != null)
            {
                LogDebug($"键盘进程已启动，ID: {keyboardProcess.Id}");
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            LogError($"启动键盘进程失败: {e.Message}");
            return false;
        }
    }

    // 查找键盘窗口
    private bool FindKeyboardWindow()
    {
        keyboardWindowHandle = IntPtr.Zero;

        try
        {
            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                System.Text.StringBuilder className = new System.Text.StringBuilder(256);
                GetClassName(hWnd, className, 256);

                // 检查窗口类名是否为OSKMainClass（屏幕键盘的类名）
                if (className.ToString() == "OSKMainClass")
                {
                    keyboardWindowHandle = hWnd;
                    return false; // 停止枚举
                }

                return true; // 继续枚举
            }, IntPtr.Zero);

            if (keyboardWindowHandle != IntPtr.Zero)
            {
                // 获取窗口关联的进程ID
                int processId;
                GetWindowThreadProcessId(keyboardWindowHandle, out processId);

                // 验证进程是否存在且是osk.exe
                try
                {
                    Process process = Process.GetProcessById(processId);
                    if (!process.HasExited && process.ProcessName.ToLower().Contains("osk"))
                    {
                        keyboardProcess = process;
                        LogDebug($"找到键盘窗口，句柄: {keyboardWindowHandle.ToInt64()}");
                        return true;
                    }
                }
                catch (ArgumentException)
                {
                    // 进程不存在
                }
            }

            return false;
        }
        catch (Exception e)
        {
            LogError($"查找键盘窗口失败: {e.Message}");
            return false;
        }
    }

    // 最小化键盘窗口
    private void MinimizeKeyboard()
    {
        if (keyboardWindowHandle != IntPtr.Zero)
        {
            ShowWindow(keyboardWindowHandle, SW_MINIMIZE);
        }
    }

    // 调试日志
    private void LogDebug(string message)
    {
        if (debugMode)
            UnityEngine.Debug.Log($"[VirtualKeyboardManager] {message}");
    }

    // 错误日志
    private void LogError(string message)
    {
        UnityEngine.Debug.LogError($"[VirtualKeyboardManager] {message}");
    }

    void OnDestroy()
    {
        CloseKeyboard();
    }
}