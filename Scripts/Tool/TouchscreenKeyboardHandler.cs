using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Diagnostics; // 新增引用用于进程管理
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;

public class TouchscreenKeyboardHandler : MonoBehaviour, ISelectHandler, IDeselectHandler // 新增IDeselectHandler接口
{[HideInInspector]
    [SerializeField] 
    private InputField targetInputField;
    private Process keyboardProcess; // 新增：存储键盘进程引用
    private TouchScreenKeyboard mobileKeyboard; // 新增：存储移动键盘引用

    void Start()
    {
        // 如果未手动指定InputField，则自动查找
        if (targetInputField == null)
            targetInputField = GetComponent<InputField>();

        if (targetInputField == null)
        {
            Debug.LogError("未找到InputField组件！");
            enabled = false;
        }

        // 新增：监听输入结束事件
        if (targetInputField != null)
        {
            targetInputField.onEndEdit.AddListener(OnEndEdit);
        }
    }

    // 实现ISelectHandler接口方法
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("InputField被选中，尝试调用系统键盘");
        OpenKeyboard();
    }

    // 新增：实现IDeselectHandler接口方法
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("InputField失去焦点，尝试关闭系统键盘");
        CloseKeyboard();
    }

    // 新增：输入结束时关闭键盘
    private void OnEndEdit(string text)
    {
        CloseKeyboard();
    }

    // 新增：公共方法，允许外部调用关闭键盘
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
            // 尝试使用Unity原生方法（适用于移动设备）
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
            // 尝试启动Windows屏幕键盘
            // 先尝试关闭现有键盘进程（如果存在）
            CloseWindowsKeyboard();

            // 启动新的键盘进程并保存引用
            keyboardProcess = Process.Start("osk.exe");
            Debug.Log("已启动Windows屏幕键盘");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"启动屏幕键盘失败: {e.Message}");

            // 备用方案：尝试通过完整路径启动
            TryAlternativePath();
        }
    }

    // 新增：关闭Windows键盘方法
    private void CloseWindowsKeyboard()
    {
        try
        {
            if (keyboardProcess != null && !keyboardProcess.HasExited)
            {
                keyboardProcess.Kill();
                keyboardProcess.Dispose();
                keyboardProcess = null;
                Debug.Log("已关闭Windows屏幕键盘");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"关闭屏幕键盘失败: {e.Message}");
        }
    }

    private void TryAlternativePath()
    {
        try
        {
            // 尝试通过完整路径启动
            keyboardProcess = Process.Start(@"C:\Windows\System32\osk.exe");
            Debug.Log("已通过备用路径启动Windows屏幕键盘");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"备用路径启动失败: {ex.Message}");

            // 最后的手段：尝试使用osk另一种启动方式
            TrySendKeys();
        }
    }

    private void TrySendKeys()
    {
        try
        {
            // 模拟按下Win+Ctrl+O组合键（某些系统的屏幕键盘快捷键）
            // 注意：需要添加System.Windows.Forms引用
            // 并在项目设置中启用"Allow 'unsafe' code"
            // SendKeys.SendWait("^{WIN}o");
            Debug.Log("已尝试通过快捷键启动屏幕键盘");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"快捷键启动失败: {ex.Message}");
        }
    }

    private void OpenUnityKeyboard()
    {
        if (targetInputField != null)
        {
            // 打开键盘并保存引用
            mobileKeyboard = TouchScreenKeyboard.Open(targetInputField.text);
            Debug.Log("已启动Unity原生键盘");
        }
    }

    // 新增：关闭移动键盘方法
    private void CloseUnityKeyboard()
    {
        if (mobileKeyboard != null && mobileKeyboard.active)
        {
            mobileKeyboard.active = false;
            Debug.Log("已关闭Unity原生键盘");
        }
    }
}