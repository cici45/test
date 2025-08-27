using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class OptimizedKeyboardHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private InputField targetInputField;
    [SerializeField] private float preloadDelay = 2f;
    [SerializeField] private bool enablePreload = true;

    private Process keyboardProcess;
    private bool isKeyboardPreloaded = false;
    private bool isKeyboardActive = false;

    void Start()
    {
        if (targetInputField != null)
        {
            // 注册事件（通过接口实现替代直接引用）
        }

        if (enablePreload)
            StartCoroutine(PreloadKeyboard());
    }

    private System.Collections.IEnumerator PreloadKeyboard()
    {
        yield return new WaitForSeconds(preloadDelay);

       
        {
            UnityEngine.Debug.Log("开始预加载屏幕键盘...");
            OpenKeyboardInternal(true);

            yield return new WaitForSeconds(1f);

            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                isKeyboardPreloaded = true;
                SendMessageToKeyboard("WM_SYSCOMMAND", 0xF020); // SC_MINIMIZE
                UnityEngine.Debug.Log("屏幕键盘预加载完成");
            }
        }
       
    }

    // 实现ISelectHandler接口
    public void OnSelect(BaseEventData eventData)
    {
        if (targetInputField == null)
            targetInputField = GetComponent<InputField>();

        if (!isKeyboardActive && targetInputField.isFocused)
        {
            OpenKeyboard();
        }
    }

    // 实现IDeselectHandler接口
    public void OnDeselect(BaseEventData eventData)
    {
        // 可选：输入框失焦时不自动关闭键盘，提高响应速度
        // CloseKeyboard();
    }

    public async void OpenKeyboard()
    {
        try
        {
            if (isKeyboardPreloaded && keyboardProcess != null && !keyboardProcess.HasExited)
            {
                UnityEngine.Debug.Log("使用预加载的键盘实例");
                SendMessageToKeyboard("WM_SYSCOMMAND", 0xF120); // SC_RESTORE

                isKeyboardActive = true;
                return;
            }

            UnityEngine.Debug.Log("启动新的键盘实例");
            await Task.Run(() => OpenKeyboardInternal(false));
            isKeyboardActive = true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"打开键盘失败: {e.Message}");
            isKeyboardActive = false;
        }
    }

    private void OpenKeyboardInternal(bool isPreload)
    {
        try
        {
            CloseKeyboard();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "osk.exe",
                WindowStyle = isPreload ? ProcessWindowStyle.Minimized : ProcessWindowStyle.Normal,
                UseShellExecute = true
            };

            keyboardProcess = Process.Start(startInfo);

            if (!isPreload && keyboardProcess != null)
            {
                keyboardProcess.WaitForInputIdle(2000);
            }
        }
        catch (Exception e)
        {
            throw new Exception($"启动键盘进程失败: {e.Message}");
        }
    }

    public void CloseKeyboard()
    {
        try
        {
            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                keyboardProcess.Kill();
                keyboardProcess.WaitForExit(1000);
                keyboardProcess.Dispose();
            }

            keyboardProcess = null;
            isKeyboardActive = false;
            isKeyboardPreloaded = false;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"关闭键盘失败: {e.Message}");
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private void SendMessageToKeyboard(string messageName, int wParam)
    {
        if (keyboardProcess == null || keyboardProcess.HasExited)
            return;

        try
        {
            IntPtr hWnd = keyboardProcess.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                int msg = messageName switch
                {
                    "WM_SYSCOMMAND" => 0x0112,
                    _ => 0
                };

                if (msg != 0)
                    SendMessage(hWnd, msg, (IntPtr)wParam, IntPtr.Zero);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"发送消息到键盘失败: {e.Message}");
        }
    }

    void OnDestroy()
    {
        CloseKeyboard();
    }
}