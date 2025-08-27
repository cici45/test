using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetActionControl : MonoBehaviour
{
    public static Transform left_01;
    public static Transform left_02;
    public static Transform right_01;
    public static Transform right_02;

    public static Transform left_show01;
    public static Transform left_show02;
    public static Transform left_show03;
    public static Transform left_show04;
    public static Transform left_show05;
    public static Transform left_show06;
    public static Transform left_show07;
    public static Transform left_show08;
    public static Transform left_show09;


    public static Transform right_show01;
    public static Transform right_show02;
    public static Transform right_show03;
    public static Transform right_show04;
    public static Transform right_show05;
    public static Transform right_show06;
    public static Transform right_show07;
    public static Transform right_show08;
    public static Transform right_show09;


    public static Transform rightArrows;
    public static Transform leftArrows;
                            
    static bool isFlashing;
    static float showTime;
    static float lucencyValue;
    static bool isChange;
    private void Awake()
    {
        left_01 = transform.Find("Left/Image");
        left_02 = transform.Find("Left/Image/Image (1)");
        left_show01 = transform.Find("Left/Image/Image (1)/left_show01");
        left_show02 = transform.Find("Left/Image/Image (1)/left_show02");
        left_show03 = transform.Find("Left/Image/Image (1)/left_show03");
        left_show04 = transform.Find("Left/Image/Image (1)/left_show04");
        left_show05 = transform.Find("Left/Image/Image (1)/left_show05");
        left_show06 = transform.Find("Left/Image/left_show06");
        left_show07 = transform.Find("Left/Image/left_show07");
        left_show08 = transform.Find("Left/Image/left_show08");
        left_show09 = transform.Find("Left/Image/left_show09");



        right_01 = transform.Find("Right/Image");
        right_02 = transform.Find("Right/Image/Image (1)");
        Initialize();

        right_show01 = transform.Find("Right/Image/Image (1)/right_show01");
        right_show02 = transform.Find("Right/Image/Image (1)/right_show02");
        right_show03 = transform.Find("Right/Image/Image (1)/right_show03");
        right_show04 = transform.Find("Right/Image/Image (1)/right_show04");
        right_show05 = transform.Find("Right/Image/Image (1)/right_show05");
        right_show06 = transform.Find("Right/Image/right_show06");
        right_show07 = transform.Find("Right/Image/right_show07");
        right_show08 = transform.Find("Right/Image/right_show08");
        right_show09 = transform.Find("Right/Image/right_show09");

        rightArrows= transform.Find("RightArrows");
        leftArrows= transform.Find("LeftArrows");
    }

    public static void Initialize()
    {
        left_01.localEulerAngles = new Vector3(0,0,60);//-
        left_02.localEulerAngles = new Vector3(0, 0, -90);//+
        right_01.localEulerAngles = new Vector3(0, 0, -60);//+
        right_02.localEulerAngles = new Vector3(0, 0, 90);//-
    }
    public static void RefreshState(float num, float num1) 
    {
        rightArrows.gameObject.SetActive(num1>=1);
        leftArrows.gameObject.SetActive(num>=1);
    }
    public static void SetTargetAngle(float value)
    {
        
        left_01.localEulerAngles = new Vector3(0, 0, 60-(60* value));
        left_02.localEulerAngles = new Vector3(0, 0, -90+(90* value));

        right_01.localEulerAngles = new Vector3(0, 0, -60+ (60 * value));
        right_02.localEulerAngles = new Vector3(0, 0, 90 - (90 * value));

        isFlashing = true;
        showTime = 0;
        lucencyValue = 1;
        isChange = true;

        left_show01.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show02.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show03.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show04.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show05.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show06.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show07.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show08.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        left_show09.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        
        right_show01.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show02.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show03.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show04.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show05.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show06.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show07.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show08.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        right_show09.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (isFlashing)
        {
            if (showTime < 1)
            {
                showTime += 0.02f;
            }
            else
            {
                isFlashing = false;
                showTime = 0;
                lucencyValue = 1;
            }
        }

        if (isFlashing)
        {
            if (lucencyValue > 0f && isChange)
            {
                lucencyValue -= 0.05f;
            }
            else if (lucencyValue <= 0f && isChange)
            {
                isChange = false;
                Debug.Log("小于等于0");
            }
            else if (!isChange&& lucencyValue < 1)
            {
                lucencyValue += 0.05f;
                Debug.Log("lucencyValue:"+ lucencyValue);
            }
            else if(!isChange && lucencyValue >= 1)
            {
                isChange = true;
            }
            
            left_show01.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show02.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show03.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show04.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show05.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show06.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show07.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show08.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            left_show09.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);


            right_show01.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show02.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show03.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show04.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show05.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show06.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show07.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show08.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
            right_show09.GetComponent<Image>().color = new Color(1, 0, 0, lucencyValue);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
