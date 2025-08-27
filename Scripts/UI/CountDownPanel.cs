using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownPanel: MonoBehaviour
{
    public static   CountDownPanel downPanel;
    [HideInInspector]
    public Text NumTip;
    // Start is called before the first frame update
    private void Awake()
    {
        downPanel = this;
        NumTip = transform.Find("AlarmClockImg/NumTip").GetComponent<Text>();
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
