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
            // ע���¼���ͨ���ӿ�ʵ�����ֱ�����ã�
        }

        if (enablePreload)
            StartCoroutine(PreloadKeyboard());
    }

    private System.Collections.IEnumerator PreloadKeyboard()
    {
        yield return new WaitForSeconds(preloadDelay);

       
        {
            UnityEngine.Debug.Log("��ʼԤ������Ļ����...");
            OpenKeyboardInternal(true);

            yield return new WaitForSeconds(1f);

            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                isKeyboardPreloaded = true;
                SendMessageToKeyboard("WM_SYSCOMMAND", 0xF020); // SC_MINIMIZE
                UnityEngine.Debug.Log("��Ļ����Ԥ�������");
            }
        }
       
    }

    // ʵ��ISelectHandler�ӿ�
    public void OnSelect(BaseEventData eventData)
    {
        if (targetInputField == null)
            targetInputField = GetComponent<InputField>();

        if (!isKeyboardActive && targetInputField.isFocused)
        {
            OpenKeyboard();
        }
    }

    // ʵ��IDeselectHandler�ӿ�
    public void OnDeselect(BaseEventData eventData)
    {
        // ��ѡ�������ʧ��ʱ���Զ��رռ��̣������Ӧ�ٶ�
        // CloseKeyboard();
    }

    public async void OpenKeyboard()
    {
        try
        {
            if (isKeyboardPreloaded && keyboardProcess != null && !keyboardProcess.HasExited)
            {
                UnityEngine.Debug.Log("ʹ��Ԥ���صļ���ʵ��");
                SendMessageToKeyboard("WM_SYSCOMMAND", 0xF120); // SC_RESTORE

                isKeyboardActive = true;
                return;
            }

            UnityEngine.Debug.Log("�����µļ���ʵ��");
            await Task.Run(() => OpenKeyboardInternal(false));
            isKeyboardActive = true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"�򿪼���ʧ��: {e.Message}");
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
            throw new Exception($"�������̽���ʧ��: {e.Message}");
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
            UnityEngine.Debug.LogError($"�رռ���ʧ��: {e.Message}");
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
            UnityEngine.Debug.LogError($"������Ϣ������ʧ��: {e.Message}");
        }
    }

    void OnDestroy()
    {
        CloseKeyboard();
    }
}