using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hanger : MonoBehaviour
{
    float KeepTime;
    float time = 0;
    public Slider slider;
    private void Start()
    {
        KeepTime = GoldenMiner.sportTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!GoldenMiner.isPause)
        {
            time += Time.deltaTime;
            float value = time / KeepTime;
            slider.value = value;
            //Debug.Log("Åö×²´¥·¢ £º " + slider.value);
            if (time > KeepTime)
            {
                other.transform.SetParent(transform);
                time = 0;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GoldenMiner.isPause)
        {
            time += Time.deltaTime;
            float value = time / KeepTime;
            slider.value = value;
            //Debug.Log("Åö×²´¥·¢ £º " + slider.value);
            if (time > KeepTime)
            {
                collision.transform.SetParent(transform);
                time = 0;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        time = 0;
        slider.value = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        time = 0;
        slider.value = 0;
    }

}
