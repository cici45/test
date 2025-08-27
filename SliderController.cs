using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ��Ҫ�����������ռ�

public class SliderController : MonoBehaviour
{
    // �����ĸ����������
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;

    // ��ǵ�ǰ�����϶��Ļ�����
    private Slider activeSlider;

    void Start()
    {
        // ��ʼ����������ֵ
        slider1.value = 0.25f;
        slider2.value = 0.25f;
        slider3.value = 0.25f;
        slider4.value = 0.25f;

        // Ϊÿ������������϶���ʼ��������ֵ�仯���¼�����
        AddSliderListeners(slider1);
        AddSliderListeners(slider2);
        AddSliderListeners(slider3);
        AddSliderListeners(slider4);
    }

    // ��ӻ������¼�����
    void AddSliderListeners(Slider slider)
    {
        // �����¼����������
        EventTrigger trigger = slider.gameObject.AddComponent<EventTrigger>();

        // ��ӿ�ʼ�϶��¼�
        AddEventTrigger(trigger, EventTriggerType.BeginDrag, (data) => {
            activeSlider = slider;
        });

        // ��ӽ����϶��¼�
        AddEventTrigger(trigger, EventTriggerType.EndDrag, (data) => {
            activeSlider = null;
        });

        // ���ֵ�仯�¼�
        slider.onValueChanged.AddListener((value) => {
            OnSliderValueChanged(slider);
        });
    }

    // ��������������¼�������
    void AddEventTrigger(EventTrigger trigger, EventTriggerType type, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((data) => action(data));
        trigger.triggers.Add(entry);
    }

    // ��������ֵ�ı�ʱ����
    void OnSliderValueChanged(Slider changedSlider)
    {
        // ���û�л�Ļ���������ִ���κβ���
        if (activeSlider == null || activeSlider != changedSlider)
            return;

        // ��ȡ��ǰ�ĸ���������ֵ
        float value1 = slider1.value;
        float value2 = slider2.value;
        float value3 = slider3.value;
        float value4 = slider4.value;

        // ������ֵ
        float total = value1 + value2 + value3 + value4;

        // �����ֵ������ 1����Ҫ���·���������������ֵ
        if (Mathf.Abs(total - 1f) > 0.001f)
        {
            // ������Ҫ����������
            float adjustment = 1f - total;

            // ���ݵ�ǰ��Ļ���������������������������ֵ
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

    // ��������������������ֵ
    void AdjustOtherSliders(Slider s1, Slider s2, Slider s3, float adjustment)
    {
        // �������������������ĵ�ǰ��ֵ
        float otherTotal = s1.value + s2.value + s3.value;
        Debug.Log(otherTotal+"total");
        Debug.Log(adjustment + "adju11");
        // ���������������������ֵΪ 0����ƽ�����������
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
            // ���ձ������������
            float ratio1 = s1.value / otherTotal;
            float ratio2 = s2.value / otherTotal;
            float ratio3 = s3.value / otherTotal;

            s1.value += adjustment * ratio1;
            s2.value += adjustment * ratio2;
            s3.value += adjustment * ratio3;
        }

        // ȷ�����л�������ֵ����Ч��Χ��
        ClampSliderValues();
    }

    // ȷ�����л�������ֵ�� 0 �� 1 ֮��
    void ClampSliderValues()
    {
        slider1.value = Mathf.Clamp(slider1.value, 0f, 1f);
        slider2.value = Mathf.Clamp(slider2.value, 0f, 1f);
        slider3.value = Mathf.Clamp(slider3.value, 0f, 1f);
        slider4.value = Mathf.Clamp(slider4.value, 0f, 1f);
    }
}