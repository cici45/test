using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Security.Principal;
using Debug = UnityEngine.Debug;

public class KeyboardManager : MonoBehaviour, ISelectHandler//IDeselectHandler, IPointerDownHandler
{
    public static KeyboardManager key;

    [Header("Settings")]
    public bool debugMode = true;          // 是否显示调试日志
    public float hideDelay = 0.1f;         // 隐藏键盘的延迟时间
    public bool useTabTip = true;          // 是否使用 Windows 触屏键盘（TabTip.exe）

    private InputField targetInputField;   // 目标输入框
    private Process keyboardProcess;       // 键盘进程
    private bool isKeyboardActive = false; // 键盘是否已显示

    // Windows API
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("shell32.dll")]
    private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

    // Window commands
    private const int SW_SHOW = 5;
    private const int SW_HIDE = 0;
    private const int SW_MINIMIZE = 6;

    void Awake()
    {
        key = this;
        targetInputField = GetComponent<InputField>();
        if (targetInputField == null)
        {
            LogError("No InputField component found!");
            enabled = false;
            return;
        }
    }

    void OnDestroy()
    {
        CloseKeyboard();
    }

    // 显示键盘（兼容权限问题）
    public void ShowKeyboard()
    {
        if (isKeyboardActive) return;

        if (useTabTip)
        {
            // 方法1：直接调用 TabTip.exe（带权限处理）
            LaunchTabTipKeyboard();
        }
        else
        {
            // 方法2：调用 osk.exe（传统键盘）
            LaunchOSKKeyboard();
        }

        isKeyboardActive = true;
        LogDebug("Keyboard shown");
    }

    // 启动 TabTip.exe（优化权限问题）
    private void LaunchTabTipKeyboard()
    {
        try
        {
            CloseKeyboard();

            // 方法1：使用 ShellExecute 以默认权限启动
            string tabTipPath = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";
            ShellExecute(IntPtr.Zero, "open", tabTipPath, null, null, SW_SHOW);

            // 方法2：备用方案（通过 explorer.exe 调用）
            if (!IsProcessRunning("TabTip"))
            {
                Process.Start("explorer.exe", "shell:appsFolder\\Microsoft.Windows.ScreenKeyboard_8wekyb3d8bbwe!App");
            }

            LogDebug("TabTip keyboard launched");
        }
        catch (Exception e)
        {
            LogError($"TabTip launch failed: {e.Message}");
            // 回退到 OSK
            LaunchOSKKeyboard();
        }
    }

    // 启动 OSK.exe（传统键盘）
    private void LaunchOSKKeyboard()
    {
        try
        {
            CloseKeyboard();
            keyboardProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "osk.exe",
                UseShellExecute = true, // 必须为 true 以提升权限
                Verb = "runas"          // 请求管理员权限（可选）
            });
            LogDebug("OSK keyboard launched");
        }
        catch (Exception e)
        {
            LogError($"OSK launch failed: {e.Message}");
        }
    }

    // 检查进程是否运行
    private bool IsProcessRunning(string name)
    {
        Process[] processes = Process.GetProcessesByName(name);
        return processes.Length > 0;
    }

    // 隐藏键盘
    public void HideKeyboard()
    {
        if (!isKeyboardActive) return;

        IntPtr keyboardWindow = FindKeyboardWindow();
        if (keyboardWindow != IntPtr.Zero)
        {
            ShowWindow(keyboardWindow, SW_HIDE);
        }

        isKeyboardActive = false;
        LogDebug("Keyboard hidden");
    }

    // 查找键盘窗口
    private IntPtr FindKeyboardWindow()
    {
        IntPtr hWnd = FindWindow("IPTip_Main_Window", null); // TabTip
        if (hWnd != IntPtr.Zero) return hWnd;

        hWnd = FindWindow("OSKMainClass", null); // OSK
        return hWnd;
    }

    // 关闭键盘进程
    private void CloseKeyboard()
    {
        try
        {
            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                keyboardProcess.Kill();
                keyboardProcess.WaitForExit(1000);
            }
        }
        catch (Exception e)
        {
            LogError($"Error closing keyboard: {e.Message}");
        }
        finally
        {
            keyboardProcess = null;
        }
    }

    // 输入框事件
    public void OnSelect(BaseEventData eventData) => ShowKeyboard();
   // public void OnDeselect(BaseEventData eventData) => StartCoroutine(DelayedHideKeyboard());
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (eventData.pointerPress == gameObject) return;
    //    StartCoroutine(DelayedHideKeyboard());
    //}

    private IEnumerator DelayedHideKeyboard()
    {
        yield return new WaitForSeconds(hideDelay);
        HideKeyboard();
    }

    // 日志输出
    private void LogDebug(string message)
    {
        if (debugMode) Debug.Log($"[Keyboard] {message}");
    }

    private void LogError(string message)
    {
        Debug.LogError($"[Keyboard] {message}");
    }
}