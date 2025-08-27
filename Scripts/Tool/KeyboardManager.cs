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
    public bool debugMode = true;          // �Ƿ���ʾ������־
    public float hideDelay = 0.1f;         // ���ؼ��̵��ӳ�ʱ��
    public bool useTabTip = true;          // �Ƿ�ʹ�� Windows �������̣�TabTip.exe��

    private InputField targetInputField;   // Ŀ�������
    private Process keyboardProcess;       // ���̽���
    private bool isKeyboardActive = false; // �����Ƿ�����ʾ

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

    // ��ʾ���̣�����Ȩ�����⣩
    public void ShowKeyboard()
    {
        if (isKeyboardActive) return;

        if (useTabTip)
        {
            // ����1��ֱ�ӵ��� TabTip.exe����Ȩ�޴���
            LaunchTabTipKeyboard();
        }
        else
        {
            // ����2������ osk.exe����ͳ���̣�
            LaunchOSKKeyboard();
        }

        isKeyboardActive = true;
        LogDebug("Keyboard shown");
    }

    // ���� TabTip.exe���Ż�Ȩ�����⣩
    private void LaunchTabTipKeyboard()
    {
        try
        {
            CloseKeyboard();

            // ����1��ʹ�� ShellExecute ��Ĭ��Ȩ������
            string tabTipPath = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";
            ShellExecute(IntPtr.Zero, "open", tabTipPath, null, null, SW_SHOW);

            // ����2�����÷�����ͨ�� explorer.exe ���ã�
            if (!IsProcessRunning("TabTip"))
            {
                Process.Start("explorer.exe", "shell:appsFolder\\Microsoft.Windows.ScreenKeyboard_8wekyb3d8bbwe!App");
            }

            LogDebug("TabTip keyboard launched");
        }
        catch (Exception e)
        {
            LogError($"TabTip launch failed: {e.Message}");
            // ���˵� OSK
            LaunchOSKKeyboard();
        }
    }

    // ���� OSK.exe����ͳ���̣�
    private void LaunchOSKKeyboard()
    {
        try
        {
            CloseKeyboard();
            keyboardProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "osk.exe",
                UseShellExecute = true, // ����Ϊ true ������Ȩ��
                Verb = "runas"          // �������ԱȨ�ޣ���ѡ��
            });
            LogDebug("OSK keyboard launched");
        }
        catch (Exception e)
        {
            LogError($"OSK launch failed: {e.Message}");
        }
    }

    // �������Ƿ�����
    private bool IsProcessRunning(string name)
    {
        Process[] processes = Process.GetProcessesByName(name);
        return processes.Length > 0;
    }

    // ���ؼ���
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

    // ���Ҽ��̴���
    private IntPtr FindKeyboardWindow()
    {
        IntPtr hWnd = FindWindow("IPTip_Main_Window", null); // TabTip
        if (hWnd != IntPtr.Zero) return hWnd;

        hWnd = FindWindow("OSKMainClass", null); // OSK
        return hWnd;
    }

    // �رռ��̽���
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

    // ������¼�
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

    // ��־���
    private void LogDebug(string message)
    {
        if (debugMode) Debug.Log($"[Keyboard] {message}");
    }

    private void LogError(string message)
    {
        Debug.LogError($"[Keyboard] {message}");
    }
}