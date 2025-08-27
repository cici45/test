using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Diagnostics; // �����������ڽ��̹���
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;

public class TouchscreenKeyboardHandler : MonoBehaviour, ISelectHandler, IDeselectHandler // ����IDeselectHandler�ӿ�
{[HideInInspector]
    [SerializeField] 
    private InputField targetInputField;
    private Process keyboardProcess; // �������洢���̽�������
    private TouchScreenKeyboard mobileKeyboard; // �������洢�ƶ���������

    void Start()
    {
        // ���δ�ֶ�ָ��InputField�����Զ�����
        if (targetInputField == null)
            targetInputField = GetComponent<InputField>();

        if (targetInputField == null)
        {
            Debug.LogError("δ�ҵ�InputField�����");
            enabled = false;
        }

        // ������������������¼�
        if (targetInputField != null)
        {
            targetInputField.onEndEdit.AddListener(OnEndEdit);
        }
    }

    // ʵ��ISelectHandler�ӿڷ���
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("InputField��ѡ�У����Ե���ϵͳ����");
        OpenKeyboard();
    }

    // ������ʵ��IDeselectHandler�ӿڷ���
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("InputFieldʧȥ���㣬���Թر�ϵͳ����");
        CloseKeyboard();
    }

    // �������������ʱ�رռ���
    private void OnEndEdit(string text)
    {
        CloseKeyboard();
    }

    // ���������������������ⲿ���ùرռ���
    public void ForceCloseKeyboard()
    {
        CloseKeyboard();
    }

    private void OpenKeyboard()
    {
        if (IsWindowsPlatform())
        {
            OpenWindowsKeyboard();
        }
        else
        {
            // ����ʹ��Unityԭ���������������ƶ��豸��
            OpenUnityKeyboard();
        }
    }

    private void CloseKeyboard()
    {
        if (IsWindowsPlatform())
        {
            CloseWindowsKeyboard();
        }
        else
        {
            CloseUnityKeyboard();
        }
    }

    private bool IsWindowsPlatform()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer ||
               Application.platform == RuntimePlatform.WindowsEditor;
    }

    private void OpenWindowsKeyboard()
    {
        try
        {
            // ��������Windows��Ļ����
            // �ȳ��Թر����м��̽��̣�������ڣ�
            CloseWindowsKeyboard();

            // �����µļ��̽��̲���������
            keyboardProcess = Process.Start("osk.exe");
            Debug.Log("������Windows��Ļ����");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"������Ļ����ʧ��: {e.Message}");

            // ���÷���������ͨ������·������
            TryAlternativePath();
        }
    }

    // �������ر�Windows���̷���
    private void CloseWindowsKeyboard()
    {
        try
        {
            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                keyboardProcess.Kill();
                keyboardProcess.Dispose();
                keyboardProcess = null;
                Debug.Log("�ѹر�Windows��Ļ����");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"�ر���Ļ����ʧ��: {e.Message}");
        }
    }

    private void TryAlternativePath()
    {
        try
        {
            // ����ͨ������·������
            keyboardProcess = Process.Start(@"C:\Windows\System32\osk.exe");
            Debug.Log("��ͨ������·������Windows��Ļ����");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"����·������ʧ��: {ex.Message}");

            // �����ֶΣ�����ʹ��osk��һ��������ʽ
            TrySendKeys();
        }
    }

    private void TrySendKeys()
    {
        try
        {
            // ģ�ⰴ��Win+Ctrl+O��ϼ���ĳЩϵͳ����Ļ���̿�ݼ���
            // ע�⣺��Ҫ���System.Windows.Forms����
            // ������Ŀ����������"Allow 'unsafe' code"
            // SendKeys.SendWait("^{WIN}o");
            Debug.Log("�ѳ���ͨ����ݼ�������Ļ����");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"��ݼ�����ʧ��: {ex.Message}");
        }
    }

    private void OpenUnityKeyboard()
    {
        if (targetInputField != null)
        {
            // �򿪼��̲���������
            mobileKeyboard = TouchScreenKeyboard.Open(targetInputField.text);
            Debug.Log("������Unityԭ������");
        }
    }

    // �������ر��ƶ����̷���
    private void CloseUnityKeyboard()
    {
        if (mobileKeyboard != null && mobileKeyboard.active)
        {
            mobileKeyboard.active = false;
            Debug.Log("�ѹر�Unityԭ������");
        }
    }
}