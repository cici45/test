using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 需要添加这个命名空间

public class SliderController : MonoBehaviour
{
    // 引用四个滑动条组件
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;

    // 标记当前正在拖动的滑动条
    private Slider activeSlider;

    void Start()
    {
        // 初始化滑动条的值
        slider1.value = 0.25f;
        slider2.value = 0.25f;
        slider3.value = 0.25f;
        slider4.value = 0.25f;

        // 为每个滑动条添加拖动开始、结束和值变化的事件监听
        AddSliderListeners(slider1);
        AddSliderListeners(slider2);
        AddSliderListeners(slider3);
        AddSliderListeners(slider4);
    }

    // 添加滑动条事件监听
    void AddSliderListeners(Slider slider)
    {
        // 创建事件触发器组件
        EventTrigger trigger = slider.gameObject.AddComponent<EventTrigger>();

        // 添加开始拖动事件
        AddEventTrigger(trigger, EventTriggerType.BeginDrag, (data) => {
            activeSlider = slider;
        });

        // 添加结束拖动事件
        AddEventTrigger(trigger, EventTriggerType.EndDrag, (data) => {
            activeSlider = null;
        });

        // 添加值变化事件
        slider.onValueChanged.AddListener((value) => {
            OnSliderValueChanged(slider);
        });
    }

    // 辅助方法：添加事件触发器
    void AddEventTrigger(EventTrigger trigger, EventTriggerType type, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => action(data));
        trigger.triggers.Add(entry);
    }

    // 当滑动条值改变时调用
    void OnSliderValueChanged(Slider changedSlider)
    {
        // 如果没有活动的滑动条，则不执行任何操作
        if (activeSlider == null || activeSlider != changedSlider)
            return;

        // 获取当前四个滑动条的值
        float value1 = slider1.value;
        float value2 = slider2.value;
        float value3 = slider3.value;
        float value4 = slider4.value;

        // 计算总值
        float total = value1 + value2 + value3 + value4;

        // 如果总值不等于 1，需要重新分配其他滑动条的值
        if (Mathf.Abs(total - 1f) > 0.001f)
        {
            // 计算需要调整的总量
            float adjustment = 1f - total;

            // 根据当前活动的滑动条，调整其他三个滑动条的值
            if (changedSlider == slider1)
            {
                AdjustOtherSliders(slider2, slider3, slider4, adjustment);
            }
            else if (changedSlider == slider2)
            {
                AdjustOtherSliders(slider1, slider3, slider4, adjustment);
            }
            else if (changedSlider == slider3)
            {
                AdjustOtherSliders(slider1, slider2, slider4, adjustment);
            }
            else if (changedSlider == slider4)
            {
                AdjustOtherSliders(slider1, slider2, slider3, adjustment);
            }
        }
    }

    // 调整其他三个滑动条的值
    void AdjustOtherSliders(Slider s1, Slider s2, Slider s3, float adjustment)
    {
        // 计算其他三个滑动条的当前总值
        float otherTotal = s1.value + s2.value + s3.value;
        Debug.Log(otherTotal+"total");
        Debug.Log(adjustment + "adju11");
        // 如果其他三个滑动条的总值为 0，则平均分配调整量
        if (otherTotal < 0.001f)
        {
            Debug.Log(adjustment+"adju");
            s1.value += adjustment / 3f;
            s2.value += adjustment / 3f;
            s3.value += adjustment / 3f;
        }
        else
        {
            Debug.Log(s1.value+" "+s2.value+" "+s3.value);
            // 按照比例分配调整量
            float ratio1 = s1.value / otherTotal;
            float ratio2 = s2.value / otherTotal;
            float ratio3 = s3.value / otherTotal;

            s1.value += adjustment * ratio1;
            s2.value += adjustment * ratio2;
            s3.value += adjustment * ratio3;
        }

        // 确保所有滑动条的值在有效范围内
        ClampSliderValues();
    }

    // 确保所有滑动条的值在 0 到 1 之间
    void ClampSliderValues()
    {
        slider1.value = Mathf.Clamp(slider1.value, 0f, 1f);
        slider2.value = Mathf.Clamp(slider2.value, 0f, 1f);
        slider3.value = Mathf.Clamp(slider3.value, 0f, 1f);
        slider4.value = Mathf.Clamp(slider4.value, 0f, 1f);
    }
}