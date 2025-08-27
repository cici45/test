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

    // Windows API ����
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

    // ����״̬����
    private const int SW_SHOW = 5;
    private const int SW_MINIMIZE = 6;
    private const int SW_RESTORE = 9;

    // ί������
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    void Start()
    {
        if (targetInputField == null)
            targetInputField = GetComponent<InputField>();

        if (targetInputField == null)
        {
            LogError("δ�ҵ�InputField���������Inspector��ָ��");
            return;
        }

     
            StartCoroutine(PreloadKeyboard());
    }

    // Ԥ���ؼ���
    private IEnumerator PreloadKeyboard()
    {


        // ���Բ������м��̽���
        if (FindKeyboardWindow())
        {
            isKeyboardPreloaded = true;
            MinimizeKeyboard();
            LogDebug("�ҵ���Ԥ�������м��̽���");
        }
        else
        {
            // �����µļ��̽���
            if (LaunchKeyboard(true))
            {
                // �ȴ����̴��ڳ�ʼ��
                float startTime = Time.time;
                while (Time.time - startTime < 3f)
                {
                    yield return null;

                    if (FindKeyboardWindow())
                    {
                        isKeyboardPreloaded = true;
                        MinimizeKeyboard();
                        LogDebug("�ɹ�Ԥ�����¼��̽���");
                        yield break;
                    }
                }

                LogError("����Ԥ���س�ʱ��δ���ҵ����̴���");
            }
        }
    }

    // ʵ�ֽӿڷ���
    public void OnSelect(BaseEventData eventData)
    {
            ShowKeyboard();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideKeyboard();
    }

    // ��ʾ����
    public void ShowKeyboard()
    {
        ShowWindow(keyboardWindowHandle, SW_RESTORE);
       // SetForegroundWindow(keyboardWindowHandle);
        //ShowWindow(keyboardWindowHandle, SW_RESTORE);
        //try
        //{
        //    // �������Ƿ�������
        //    if (!FindKeyboardWindow())
        //    {
        //        LogDebug("�����µļ��̽���");

        //        if (!LaunchKeyboard(false))
        //        {
        //            LogError("�޷�������Ļ����");
        //            return;
        //        }

        //        // �ȴ����̴��ڳ�ʼ��
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
        //        // �ָ���������̴���
        //        if (IsIconic(keyboardWindowHandle))


        //        SetForegroundWindow(keyboardWindowHandle);
        //        isKeyboardActive = true;
        //        LogDebug("��������ʾ");
        //    }
        //    else
        //    {
        //        LogError("�޷��ҵ����̴���");
        //    }
        //}
        //catch (Exception e)
        //{
        //    LogError($"��ʾ����ʧ��: {e.Message}");
        //}
    }

    // ���ؼ���
    public void HideKeyboard()
    {
        if (isKeyboardActive && keyboardWindowHandle != IntPtr.Zero)
        {
            MinimizeKeyboard();
            isKeyboardActive = false;
            LogDebug("����������");
        }
    }

    // �رռ��̽���
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

            LogDebug("���̽����ѹر�");
        }
        catch (Exception e)
        {
            LogError($"�رռ���ʧ��: {e.Message}");
        }
    }

    // �������̽���
    private bool LaunchKeyboard(bool isPreload)
    {
        try
        {
            // �ȳ��Թر����н���
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
                LogDebug($"���̽�����������ID: {keyboardProcess.Id}");
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            LogError($"�������̽���ʧ��: {e.Message}");
            return false;
        }
    }

    // ���Ҽ��̴���
    private bool FindKeyboardWindow()
    {
        keyboardWindowHandle = IntPtr.Zero;

        try
        {
            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                System.Text.StringBuilder className = new System.Text.StringBuilder(256);
                GetClassName(hWnd, className, 256);

                // ��鴰�������Ƿ�ΪOSKMainClass����Ļ���̵�������
                if (className.ToString() == "OSKMainClass")
                {
                    keyboardWindowHandle = hWnd;
                    return false; // ֹͣö��
                }

                return true; // ����ö��
            }, IntPtr.Zero);

            if (keyboardWindowHandle != IntPtr.Zero)
            {
                // ��ȡ���ڹ����Ľ���ID
                int processId;
                GetWindowThreadProcessId(keyboardWindowHandle, out processId);

                // ��֤�����Ƿ��������osk.exe
                try
                {
                    Process process = Process.GetProcessById(processId);
                    if (!process.HasExited && process.ProcessName.ToLower().Contains("osk"))
                    {
                        keyboardProcess = process;
                        LogDebug($"�ҵ����̴��ڣ����: {keyboardWindowHandle.ToInt64()}");
                        return true;
                    }
                }
                catch (ArgumentException)
                {
                    // ���̲�����
                }
            }

            return false;
        }
        catch (Exception e)
        {
            LogError($"���Ҽ��̴���ʧ��: {e.Message}");
            return false;
        }
    }

    // ��С�����̴���
    private void MinimizeKeyboard()
    {
        if (keyboardWindowHandle != IntPtr.Zero)
        {
            ShowWindow(keyboardWindowHandle, SW_MINIMIZE);
        }
    }

    // ������־
    private void LogDebug(string message)
    {
        if (debugMode)
            UnityEngine.Debug.Log($"[VirtualKeyboardManager] {message}");
    }

    // ������־
    private void LogError(string message)
    {
        UnityEngine.Debug.LogError($"[VirtualKeyboardManager] {message}");
    }

    void OnDestroy()
    {
        CloseKeyboard();
    }
}